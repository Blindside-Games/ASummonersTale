using System;
using Microsoft.Xna.Framework;

namespace ASummonersTale.TileEngine
{
    public class Camera
    {
        private float speed;

        public float Speed
        {
            get { return speed; }
            set { speed = (float)MathHelper.Clamp(value, 1f, 16f); }
        }


        private Vector2 position;
        public Vector2 Position { get { return position; } set { position = value; } }

        public Matrix Transformation => Matrix.CreateTranslation(new Vector3(-position, 0f));

        public Camera()
        {
            speed = 4f;
        }

        public Camera(Vector2 position) : this()
        {
            this.position = position;
        }

        public void LockCamera(TileMap map, Rectangle viewport)
        {
            position.X = MathHelper.Clamp(position.X, 0, map.PixelWidth - viewport.Width);
            position.Y = MathHelper.Clamp(position.Y, 0, map.PixelHeight - viewport.Height);
        }
    }
}