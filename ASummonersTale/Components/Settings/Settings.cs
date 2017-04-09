using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASummonersTale.Components.Settings
{
    internal class Settings
    {
        [IniSection("Video", true, typeof(bool))]
        public bool AntiAliasingOn { get; set; }

        [IniSection("Video", false, typeof(bool))]
        public bool Subtitles { get; set; } = false;

        [IniSection("Audio", 1.0f, typeof(float), MinimumValue = 0.0f, MaximumValue = 1.0f)]
        public float MasterVolume { get; set; }

        [IniSection("Audio", 1.0f, typeof(float), MinimumValue = 0.0f, MaximumValue = 1.0f)]
        public float MusicVolume { get; set; }

        [IniSection("Audio", 1.0f, typeof(float), MinimumValue = 0.0f, MaximumValue = 1.0f)]
        public float SoundEffectsVolume { get; set; }

        private readonly IniFile settingsIniFile;

        [IniSection("KeyBindings", null, typeof(Dictionary<Action, Microsoft.Xna.Framework.Input.Keys[]>))]
        internal KeyBindings Bindings { get; set; }

        bool AllCategoriesPresent
        {
            get
            {
                foreach (var cat in GetSettingsCategories())
                    if (!settingsIniFile.GetSectionNames().Contains(cat))
                    {
                        log.Warn($"{cat} not in settings file. Settings will be reset");
                        return false;
                    }

                log.Debug("All settings categories present");
                return true;
            }
        }

        private readonly ILog log;

        public Settings()
        {
            log = LogManager.GetLogger(nameof(Settings));

            log.Debug("Creating or opening ini file");
            settingsIniFile = new IniFile("settings.ini");

            Bindings = new KeyBindings();

            if (!AllCategoriesPresent)
                Task.Run(async () => await Reset());
        }

        public async Task<bool> ReadSettings()
        {
            bool valid = AllCategoriesPresent;

            IEnumerable<PropertyInfo> properties = typeof(Settings).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(prop => prop.CustomAttributes.Any());

            log.Info("Reading keybindings");
            Bindings.ReadKeybindings(settingsIniFile);

            // If valid is true here all sections must be present
            if (valid)
                foreach (var propertyInfo in properties)
                {
                    IniSection attribute = (IniSection)propertyInfo.GetCustomAttribute(typeof(IniSection), false);

                    log.Debug($"Reading settings for {propertyInfo.Name}");

                    string raw = settingsIniFile.Read(propertyInfo.Name, attribute.Section);

                    if (propertyInfo.PropertyType.Name == "KeyBindings")
                        continue;

                    switch (attribute.Type.Name)
                    {

                        case "Single":
                            {
                                float value;

                                if (!float.TryParse(raw, out value))
                                {
                                    log.Warn($"Invalid value '{raw}' for {propertyInfo.Name}");

                                    valid = false;
                                    break;
                                }

                                if (!(value >= (float)attribute.MinimumValue && value <= (float)attribute.MaximumValue))
                                    valid = false;
                                else
                                    propertyInfo.SetValue(this, value);


                                break;
                            }

                        case "Boolean":
                            {
                                bool value;

                                if (!bool.TryParse(raw, out value))
                                {
                                    log.Warn($"Invalid value '{raw}' for {propertyInfo.Name}");
                                    valid = false;
                                }
                                else
                                    propertyInfo.SetValue(this, value);

                                break;
                            }

                        default:
                            {
                                valid = false;
                                break;
                            }
                    }

                    if (!valid)
                        break;

                }

            if (!valid && MessageBox.Show("Invalid settings file. Press OK to reset to default settings.",
                    "Settings Invalid",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.OK)
            {
                await Reset();

                valid = true;
            }

            return valid;
        }

        public async Task Reset()
        {
            log.Info("Writing default settings");

            IEnumerable<PropertyInfo> properties = typeof(Settings).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(prop => prop.CustomAttributes.Any());

            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.PropertyType.Name == "KeyBindings")
                    continue;

                IniSection attribute = (IniSection)propertyInfo.GetCustomAttribute(typeof(IniSection), false);

                settingsIniFile.Write(propertyInfo.Name, attribute.DefaultValue.ToString(), attribute.Section);
            }

            log.Info("Resetting keybindings");
            Bindings.ResetKeybindings(settingsIniFile);
        }

        private IEnumerable<string> GetSettingsCategories()
        {
            List<string> results = new List<string>();

            IEnumerable<PropertyInfo> properties = typeof(Settings).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(prop => prop.CustomAttributes.Any());

            foreach (var propertyInfo in properties)
            {
                IniSection attribute = (IniSection)propertyInfo.GetCustomAttribute(typeof(IniSection), false);

                if (!results.Contains($"[{attribute.Section}]"))
                    results.Add($"[{attribute.Section}]");
            }

            return results;
        }

        internal class KeyBindings
        {
            Dictionary<Action, List<Microsoft.Xna.Framework.Input.Keys>> keyBindings;

            internal List<Microsoft.Xna.Framework.Input.Keys> this[Action action]
            {
                get
                {
                    try
                    {
                        return keyBindings[action];
                    }
                    catch
                    {
                        log.Fatal($"Unable to get keybind for {action.ToString()}");

                        return null;
                    }

                }
            }

            internal KeyValuePair<Action, List<Microsoft.Xna.Framework.Input.Keys>> this[Action a, List<Microsoft.Xna.Framework.Input.Keys> k]
            {
                set => keyBindings.Add(a, k);
            }

            private readonly ILog log;

            public KeyBindings()
            {
                log = LogManager.GetLogger(nameof(KeyBindings));

                keyBindings = new Dictionary<Action, List<Microsoft.Xna.Framework.Input.Keys>>();
            }

            internal void ResetKeybindings(IniFile ini)
            {
                foreach (KeyValuePair<Action, List<Microsoft.Xna.Framework.Input.Keys>> kvp in defaultKeys)
                {
                    StringBuilder keys = new StringBuilder();

                    int count = kvp.Value.Count();

                    log.Debug($"Writing keybinds for {kvp.Key}. There are {count} keybindings for this key");

                    for (int i = 0; i < count; i++)
                    {
                        log.Debug($"Writing {i} keybind for {kvp.Key}");

                        if (i == count - 1)
                            keys.Append(kvp.Value[i]);
                        else
                            keys.Append($"{kvp.Value[i]},");
                    }

                    ini.Write(kvp.Key.ToString(), keys.ToString(), nameof(KeyBindings));
                }

                //ReadKeybindings(ini);
            }

            internal void ReadKeybindings(IniFile ini)
            {
                log.Info("Reading keybindings");

                string[] keyBindingsFromSettingsFile = ini.GetAllKeysInSection(nameof(KeyBindings));

                Debug.Assert(keyBindingsFromSettingsFile.Length == (int)Action.Count, "Keybind missing");

                if (keyBindingsFromSettingsFile.Length != (int)Action.Count)
                    log.Warn("Invalid keybind or keybind missing");

                Regex actionNameRegex = new Regex("^(.*?)="), keyBindingsRegex = new Regex("=(.*)");

                foreach (var keybinding in keyBindingsFromSettingsFile)
                {
                    string actionName = actionNameRegex.Match(keybinding).ToString().TrimEnd('=');
                    log.Debug($"Reading key bindings for {actionName}");


                    string[] keys = keyBindingsRegex.Match(keybinding).ToString().TrimStart('=').Split(',');

                    if (!Enum.TryParse(actionName, out Action action))
                        log.Warn($"Invalid keybind '{actionName}'");

                    List<Microsoft.Xna.Framework.Input.Keys> keyEnums = new List<Microsoft.Xna.Framework.Input.Keys>();

                    foreach (var key in keys)
                    {
                        log.Debug($"Found key {key}");
                        keyEnums.Add((Microsoft.Xna.Framework.Input.Keys)Enum.Parse(typeof(Microsoft.Xna.Framework.Input.Keys), key));
                    }

                    keyBindings.Add(action, keyEnums);
                }
            }

            static Dictionary<Action, List<Microsoft.Xna.Framework.Input.Keys>> defaultKeys = new Dictionary<Action, List<Microsoft.Xna.Framework.Input.Keys>>
            {
                { Action.MoveUp, new List<Microsoft.Xna.Framework.Input.Keys> { Microsoft.Xna.Framework.Input.Keys.W, Microsoft.Xna.Framework.Input.Keys.Up } },
                { Action.MoveDown, new List<Microsoft.Xna.Framework.Input.Keys> { Microsoft.Xna.Framework.Input.Keys.S, Microsoft.Xna.Framework.Input.Keys.Down } },
                { Action.MoveLeft, new List<Microsoft.Xna.Framework.Input.Keys> { Microsoft.Xna.Framework.Input.Keys.A, Microsoft.Xna.Framework.Input.Keys.Left } },
                { Action.MoveRight, new List<Microsoft.Xna.Framework.Input.Keys> { Microsoft.Xna.Framework.Input.Keys.D, Microsoft.Xna.Framework.Input.Keys.Right } },
                { Action.MoveMapUp, new List<Microsoft.Xna.Framework.Input.Keys> { Microsoft.Xna.Framework.Input.Keys.Up } },
                { Action.MoveMapRight, new List<Microsoft.Xna.Framework.Input.Keys> { Microsoft.Xna.Framework.Input.Keys.Right } },
                { Action.MoveMapDown, new List<Microsoft.Xna.Framework.Input.Keys> { Microsoft.Xna.Framework.Input.Keys.Down } },
                { Action.MoveMapLeft, new List<Microsoft.Xna.Framework.Input.Keys> { Microsoft.Xna.Framework.Input.Keys.Left } },
                { Action.ToggleMap, new List<Microsoft.Xna.Framework.Input.Keys> { Microsoft.Xna.Framework.Input.Keys.M } }
            };
        }

        internal enum Action
        {
            MoveUp,
            MoveDown,
            MoveLeft,
            MoveRight,
            MoveMapUp,
            MoveMapDown,
            MoveMapLeft,
            MoveMapRight,
            ToggleMap,
            Count
        }
    }
}
