using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ASummonersTale.TileEngine
{
    class TileSet
    {
        public int HorizontalTileCount { get; } = 8;
        public int VerticalTileCount { get; } = 8;
        public int TileWidth { get; } = 64;
        public int TileHeight { get; } = 64;

        private readonly Rectangle[] sourceRectangles;

        [ContentSerializerIgnore]
        public Texture2D TileSetTexture { get; set; }

        [ContentSerializer]
        public string TileSetTextureName { get; set; }

        [ContentSerializerIgnore]
        public Rectangle[] SourceRectangles => (Rectangle[])sourceRectangles.Clone();

        public TileSet()
        {
            sourceRectangles = new Rectangle[HorizontalTileCount * VerticalTileCount];

            int tile = 0;

            for (int y = 0; y < VerticalTileCount; y++)
                for (int x = 0; x < HorizontalTileCount; x++)
                {
                    sourceRectangles[tile] = new Rectangle(x * TileWidth, x * TileHeight, TileWidth, TileHeight);
                    ++tile;
                }

        }

        public TileSet(int horizontalTiles, int verticalTiles, int tileWidth, int tileHeight)
        {
            HorizontalTileCount = horizontalTiles;
            VerticalTileCount = verticalTiles;
            TileHeight = tileHeight;
            TileWidth = tileWidth;

            sourceRectangles = new Rectangle[HorizontalTileCount * VerticalTileCount];

            int tile = 0;

            for (int y = 0; y < VerticalTileCount; y++)
                for (int x = 0; x < HorizontalTileCount; x++)
                {
                    sourceRectangles[tile] = new Rectangle(x * TileWidth, x * TileHeight, TileWidth, TileHeight);
                    ++tile;
                }
        }

    }
}
