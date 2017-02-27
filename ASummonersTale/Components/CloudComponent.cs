using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ASummonersTale.Components
{
    internal class Cloud
    {
        public static Texture2D Texture;
        public Vector2 Position;

        public static readonly int Speed = 50;

        public bool ShouldBeDestroyed;
    }

    class CloudComponent
    {
        private List<Cloud> clouds;

        private readonly Random random;

        private const int MaxClouds = 5;

        private readonly int MinHeight, MaxHeight, ScreenWidth;

        private readonly int cloudX;

        private readonly float CloudSpawnTimer = 7;
        private float currentCloudSpawn = 6;

        public CloudComponent(Texture2D texture, ASummonersTaleGame game)
        {
            Cloud.Texture = texture;

            MaxHeight = ASummonersTaleGame.ScreenRectangle.X + (ASummonersTaleGame.ScreenRectangle.Height / 4);
            MinHeight = 0;

            cloudX = -Cloud.Texture.Width;

            ScreenWidth = game.GraphicsDevice.Adapter.CurrentDisplayMode.Width;

            clouds = new List<Cloud>();

            random = new Random();
        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            currentCloudSpawn += delta;

            if (clouds.Count < MaxClouds && ((int)currentCloudSpawn == CloudSpawnTimer))
            {
                SpawnCloud();
                currentCloudSpawn = 0;
            }

            foreach (Cloud cloud in clouds)
            {
                float newX = cloud.Position.X + (Cloud.Speed * delta);

                cloud.Position = new Vector2(newX, cloud.Position.Y);

                if (cloud.Position.X >= ASummonersTaleGame.ScreenRectangle.Width)
                    cloud.ShouldBeDestroyed = true;
            }

            clouds.RemoveAll(c => c.ShouldBeDestroyed);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var cloud in clouds)
            {
                spriteBatch.Draw(Cloud.Texture, cloud.Position, Color.White);
            }
        }

        private void SpawnCloud()
        {
            int cloudY = random.Next(MinHeight, MaxHeight);

            Cloud cloud = new Cloud
            {
                Position = new Vector2(cloudX, cloudY),
                ShouldBeDestroyed = false
            };

            clouds.Add(cloud);
        }
    }
}
