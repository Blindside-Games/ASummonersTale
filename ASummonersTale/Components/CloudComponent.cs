using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace ASummonersTale.Components
{
    internal class Cloud
    {
        public static Texture2D Texture;
        public Vector2 Position;

        public static readonly int Speed = 50;

        public bool ShouldBeDestroyed;
    }

    internal class CloudLayer
    {
        internal List<Cloud> Clouds;

        private readonly int cloudX;
        
        public float CurrentCloudSpawn = 6;

        private readonly Random random;

        public CloudLayer()
        {
            Clouds = new List<Cloud>();
            random = new Random();

            cloudX = -Cloud.Texture.Width;
        }

        public void RemoveInactiveClouds()
        {
            Clouds.RemoveAll(c => c.ShouldBeDestroyed);
        }

        public void SpawnCloud()
        {
            int cloudY = random.Next(CloudComponent.MinHeight, CloudComponent.MaxHeight);

            Cloud cloud = new Cloud
            {
                Position = new Vector2(cloudX, cloudY),
                ShouldBeDestroyed = false
            };

            Clouds.Add(cloud);
        }
    }

    class CloudComponent
    {
        private List<CloudLayer> layers;

        private const int MaxClouds = 5;

        private static int minHeight, maxHeight, screenWidth;

        public static int MinHeight => minHeight;

        public static int MaxHeight => maxHeight;

        public static int ScreenWidth => screenWidth;

        public const float CloudSpawnTimer = 7;

        public CloudComponent(int layers, Texture2D texture, ASummonersTaleGame game)
        {
            Cloud.Texture = texture;

            this.layers = new List<CloudLayer>();

            maxHeight = ASummonersTaleGame.ScreenRectangle.X + (ASummonersTaleGame.ScreenRectangle.Height / 6);
            minHeight = 0;

            screenWidth = game.GraphicsDevice.Adapter.CurrentDisplayMode.Width;

            for (int i = 0; i < layers; i++)
                this.layers.Add(new CloudLayer());
        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            layers.ForEach(layer => layer.CurrentCloudSpawn += delta); 

            foreach (var cloudLayer in layers)
            {
                if (cloudLayer.Clouds.Count < MaxClouds && ((int)cloudLayer.CurrentCloudSpawn == CloudSpawnTimer))
                {
                    cloudLayer.SpawnCloud();
                    cloudLayer.CurrentCloudSpawn = 0;
                }

                foreach (Cloud cloud in cloudLayer.Clouds)
                {
                    float newX = cloud.Position.X + (Cloud.Speed * delta);

                    cloud.Position = new Vector2(newX, cloud.Position.Y);

                    if (cloud.Position.X >= ASummonersTaleGame.ScreenRectangle.Width)
                        cloud.ShouldBeDestroyed = true;
                }

                cloudLayer.RemoveInactiveClouds();
            }
        }

        public void Draw(int layer, GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var cloud in layers[layer-1].Clouds)
            {
                spriteBatch.Draw(Cloud.Texture, cloud.Position, Color.White);
            }
        }
    }
}
