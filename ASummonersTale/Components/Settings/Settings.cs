using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;

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

        bool AllCategoriesPresent
        {
            get
            {
                foreach (var cat in GetSettingsCategories())
                    if (!settingsIniFile.GetSectionNames().Contains(cat))
                        return false;

                return true;
            }
        }

        public Settings()
        {
            settingsIniFile = new IniFile("settings.ini");
        }

        public bool ReadSettings()
        {
            bool valid = settingsIniFile.SectionExists("Audio") && settingsIniFile.SectionExists("Video");

            PropertyInfo[] properties = typeof(Settings).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // If valid is true here all sections must be present
            if (valid)
                foreach (var propertyInfo in properties)
                {
                    IniSection attribute = (IniSection)propertyInfo.GetCustomAttribute(typeof(IniSection), false);

                    switch (attribute.Type.Name)
                    {
                        case "Single":
                            {
                                float value;

                                if (!float.TryParse(settingsIniFile.Read(propertyInfo.Name, attribute.Section), out value))
                                {
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

                                if (!bool.TryParse(settingsIniFile.Read(propertyInfo.Name, attribute.Section), out value))
                                    valid = false;
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
                Reset();

                valid = true;
            }

            return valid;
        }

        public void Reset()
        {
            PropertyInfo[] properties = typeof(Settings).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var propertyInfo in properties)
            {
                IniSection attribute = (IniSection)propertyInfo.GetCustomAttribute(typeof(IniSection), false);

                settingsIniFile.Write(propertyInfo.Name, attribute.DefaultValue.ToString(), attribute.Section);
            }
        }

        private IEnumerable<string> GetSettingsCategories()
        {
            List<string> results = new List<string>();

            PropertyInfo[] properties = typeof(Settings).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var propertyInfo in properties)
            {
                IniSection attribute = (IniSection)propertyInfo.GetCustomAttribute(typeof(IniSection), false);

                if (!results.Contains($"[{attribute.Section}]"))
                    results.Add($"[{attribute.Section}]");
            }

            return results;
        }
    }
}
