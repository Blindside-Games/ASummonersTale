using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASummonersTale.Components.Settings
{
    internal class Settings
    {
        internal bool AntiAliasingOn { get; set; } = true;
        internal bool Subtitles { get; set; } = false;

        internal float MasterVolume { get; set; } = 1.0f;
        internal float MusicVolume { get; set; } = 1.0f;
        internal float SoundEffectsVolume { get; set; } = 1.0f;
    }
}
