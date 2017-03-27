using ASummonersTale.Components.Settings;
using log4net.Config;
using System;
using System.Linq;

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

            XmlConfigurator.Configure();

            using (var game = new ASummonersTaleGame())
            {
                if (!game.Components.Any())
                    return;


                game.Run();
            }
        }
    }
#endif
}
