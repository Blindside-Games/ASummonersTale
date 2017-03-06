using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ASummonersTale.TileEngine
{
    public class TileMap
    {
        [ContentSerializer]
        int width, height;

        [ContentSerializer]
        public string Name { get; private set; }

        [ContentSerializer]
        public TileLayer GroundLayer { get; set; }

        [ContentSerializer]
        public TileLayer EdgeLayer { get; set; }

        [ContentSerializer]
        public TileLayer BuildingLayer { get; set; }

        [ContentSerializer]
        public TileLayer DecorationLayer { get; set; }

        [ContentSerializer]
        public Dictionary<string, Point> Characters { get; private set; }

        [ContentSerializer]
        internal TileSet TileSet { get; set; }

        int Width => width;
        int Height => height;

        public int PixelWidth => width * Engine.TileWidth;
        public int PixelHeight => height * Engine.TileHeight;

        public TileMap(TileSet tileSet, string mapName)
        {
            Characters = new Dictionary<string, Point>();
            TileSet = tileSet;
            Name = mapName;
        }

        public TileMap(TileSet tileSet, string mapName, TileLayer ground, TileLayer edge, TileLayer buildings, TileLayer Decoration)
        {

        }

        public void FillEdges()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    EdgeLayer[x, y] = -1;
                }
            }
        }

        public void FillDecoration()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    DecorationLayer[x, y] = -1;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (GroundLayer != null)
                GroundLayer.Update(gameTime);

            if (EdgeLayer != null)
                EdgeLayer.Update(gameTime);

            if (BuildingLayer != null)
                BuildingLayer.Update(gameTime);

            if (DecorationLayer != null)
                DecorationLayer.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
        {
            if (GroundLayer != null)
                GroundLayer.Draw(gameTime, spriteBatch, TileSet, camera);

            if (EdgeLayer != null)
                EdgeLayer.Draw(gameTime, spriteBatch, TileSet, camera);

            if (BuildingLayer != null)
                BuildingLayer.Draw(gameTime, spriteBatch, TileSet, camera);

            if (DecorationLayer != null)
                DecorationLayer.Draw(gameTime, spriteBatch, TileSet, camera);
        }
    }
}