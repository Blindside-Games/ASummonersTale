using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public Engine(Rectangle viewPort, int tileWidth, int tileHeight) : this(viewPort)
        {
            TileWidth = tileWidth;
            TileHeight = tileHeight;
        }


        internal static Point VectorToCell(Vector2 position)
        {
            return new Point((int)position.X / TileWidth, (int)position.Y / TileHeight);
        }

        public void SetMap(TileMap map)
        {
            if (map == null)
                throw new ArgumentException(nameof(map));

            this.map = map;
        }

        public void Update(GameTime gameTime)
        {
            map.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            map.Draw(gameTime, spriteBatch, camera);
        }
    }
}