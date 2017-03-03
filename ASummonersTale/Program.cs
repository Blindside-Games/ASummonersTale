using ASummonersTale.Components.Settings;
using Microsoft.Win32;
using System;

namespace ASummonersTale
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            RegistryConfiguration.ConfigureRegistry();

            using (var game = new ASummonersTaleGame())
                game.Run();
        }
    }
#endif
}
