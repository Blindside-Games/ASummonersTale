using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ASummonersTale.TileEngine
{
    internal class Engine
    {
        public static Rectangle ViewportRectangle { get; private set; }

        private static int tileWidth = 32;
        public static int TileWidth
        {
            get => tileWidth / ScaleFactor;
            internal set => tileWidth = value;
        }

        private static int tileHeight = 32;
        public static int TileHeight
        {
            get => tileHeight / ScaleFactor;
            private set => tileHeight = value;
        }

        private static int minScaleFactor = 1, maxScaleFactor = 4;

        private static int scaleFactor;
        public static int ScaleFactor
        {
            get => scaleFactor;
            set => scaleFactor = MathHelper.Clamp(value, minScaleFactor, maxScaleFactor);
        }

        private TileMap map;

        private static float scrollSpeed = 500f;

        private static Camera camera;

        public static Camera Camera => camera;

        public TileMap Map => map;

        public Engine(Rectangle viewPort)
        {
            scaleFactor = minScaleFactor;

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
            this.map = map ?? throw new ArgumentException(nameof(map));
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