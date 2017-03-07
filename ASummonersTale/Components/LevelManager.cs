using ASummonersTale.TileEngine;
using System.Collections.Generic;

namespace ASummonersTale.Components
{
    internal class LevelManager
    {
        private Dictionary<int, TileMap> levels;

        public LevelManager()
        {
            levels = new Dictionary<int, TileMap>();
        }

        public TileMap this[int level]
        {
            get
            {
                return levels[level];
            }

            set
            {
                levels.Add(level, value);
            }
        }
    }
}
