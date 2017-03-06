using Microsoft.Xna.Framework;
using System;

namespace ASummonersTale.TileEngine
{
    internal class Engine
    {
        public static Rectangle ViewportRectangle { get; private set; }
        public static int TileWidth { get; internal set; } = 32;
        public static int TileHeight { get; internal set; } = 32;

        private TileMap map;

        private static float scrollSpeed = 500f;

        private static Camera camera;

        public static Camera Camera => camera;

        public TileMap Map => map;
        
        public Engine(Rectangle viewPort)
        {
            ViewportRectangle = viewPort;

            camera = new Camera();

            TileHeight = TileWidth = 64;
        }

        public Engine (Rectangle viewPort, int tileWidth, int tileHeight) : this(viewPort)
        {
            TileWidth = tileWidth;
            TileHeight = tileHeight;
        }   





        internal static Point VectorToCell(Vector2 position)
        {
            throw new NotImplementedException();
        }


    }
}