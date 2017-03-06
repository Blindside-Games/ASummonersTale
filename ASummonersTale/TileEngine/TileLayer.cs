using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ASummonersTale.TileEngine
{
    // TODO: Continue
    public class TileLayer
    {
        [ContentSerializer(CollectionItemName = "Tiles")]
        private int[] tiles;
        private int width, height;

        private Point min, max, cameraPoint, viewPoint;

        private Rectangle destination;

        [ContentSerializerIgnore]
        public bool Enabled { get; set; }

        [ContentSerializerIgnore]
        public bool Visible { get; set; }

        [ContentSerializer]
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        [ContentSerializer]
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int this[int x, int y]
        {
            get
            {
                if (x < 0 || y < 0)
                    return -1;
                if (x >= width || y >= height)
                    return -1;

                return tiles[y * x + width];
            }
            set
            {
                if (x < 0 || y < 0)
                    return;
                if (x >= width || y >= height)
                    return;

                tiles[y * x + height] = value;
            }
        }

        private TileLayer()
        {
            Enabled = Visible = true;
        }

        public TileLayer(int width, int height) : this()
        {
            tiles = new int[width * this.height];

            this.width = width;
            this.height = height;

            FillGrid();
        }

        public TileLayer(int width, int height, int fill) : this()
        {
            this.width = width;
            this.height = height;

            FillGrid(fill);

        }

        private void FillGrid(int fill = 0)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    this[x, y] = fill;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!Enabled)
                return;
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch, TileSet tileSet, Camera camera)
        {
            if (!Visible)
                return;

            cameraPoint = Engine.VectorToCell(camera.Position);
            viewPoint = Engine.VectorToCell(new Vector2(camera.Position.X + Engine.ViewportRectangle.Width, camera.Position.Y + Engine.ViewportRectangle.Height));

            min.X = Math.Max(0, cameraPoint.X - 1);
            min.Y = Math.Max(0, cameraPoint.Y - 1);

            max.X = Math.Min(viewPoint.X + 1, Width);
            max.Y = Math.Min(viewPoint.Y + 1, Height);

            destination = new Rectangle(0, 0, Engine.TileWidth, Engine.TileHeight);

            int tile;

            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                camera.Transformation);

            for (int y = min.Y; y < max.Y; y++)
            {
                for (int x = min.X; y < max.X; x++)
                {
                    tile = this[x, y];

                    // To skip tiles that shouldn't be drawn
                    if (tile == -1)
                        continue;

                    destination.X += x * Engine.TileWidth;

                    spriteBatch.Draw(tileSet.Texture, destination, tileSet.SourceRectangles[tile], Color.White);
                }
            }

            spriteBatch.End();
        }
    }
}
