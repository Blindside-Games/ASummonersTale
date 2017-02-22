using Microsoft.Xna.Framework;

namespace ASummonersTale.TileEngine
{
    public class Camera
    {
        private float speed;

        public float Speed
        {
            get { return speed; }
            set { speed = (float)MathHelper.Clamp(speed, 1f, 16f); }
        }

        /// <summary>
        ///  TODO: Continue
        /// </summary>
        public Vector2 Position { get; set; }

    }
}