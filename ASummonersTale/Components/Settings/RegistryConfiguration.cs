using Microsoft.Win32;
using System;
using System.IO;

namespace ASummonersTale.Components.Settings
{
    internal static class RegistryConfiguration
    {
        private static RegistryKey rootKey;

        static RegistryConfiguration()
        {
            rootKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Blindside Games\A Summoner's Tale", true);
        }

        public static void ConfigureRegistry()
        {
            ConfigureRootKey();
            ConfigureAppKeys();
            ConfigureSavedGameKey();
            ReadSavedGameKey();
        }

        private static void ConfigureRootKey()
        {

            if (rootKey == null)
            {
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Blindside Games\A Summoner's Tale");
                rootKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Blindside Games\A Summoner's Tale", true);
            }
        }

        private static void ConfigureAppKeys()
        {
            if (rootKey.GetValue("AppVersion") == null)
                rootKey.SetValue("AppVersion", "0.2.0.83", RegistryValueKind.String);

            if (rootKey.GetValue("AppName") == null)
                rootKey.SetValue("AppName", "A Summoner's Tale");
        }


        private static void ConfigureSavedGameKey()
        {

            if (!File.Exists($@"{Environment.CurrentDirectory}\\savegame.sts"))
                rootKey.SetValue("SavedGamePresent", false, RegistryValueKind.DWord);
            else
                rootKey.SetValue("SavedGamePresent", true, RegistryValueKind.DWord);

        }

        private static void ReadSavedGameKey()
        {
            int value = (int)rootKey.GetValue("SavedGamePresent");

            ASummonersTaleGame.SavedGamePresent = Convert.ToBoolean(value);
        }
    }
}
