using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ASummonersTale.Components.Clouds
{
    internal class CloudLayer
    {
        private readonly int cloudX;

        private readonly int MaxClouds = 5;

        private readonly Random random;

        private readonly int speed;
        internal List<Cloud> Clouds;

        private int CloudSpawnTimer = 7;

        public float CurrentCloudSpawn = 6;

        public CloudLayer(int layerDepth, int layerSpeed)
        {
            Clouds = new List<Cloud>();
            random = new Random(layerDepth);

            LayerDepth = layerDepth;
            speed = layerSpeed;

            cloudX = -Cloud.Texture.Width;
        }

        internal int LayerDepth { get; }

        public void Update(float delta)
        {
            CurrentCloudSpawn += delta;

            if (Clouds.Count < MaxClouds && (int) CurrentCloudSpawn >= CloudSpawnTimer)
            {
                SpawnCloud();

                CloudSpawnTimer = random.Next(4, 7);

                CurrentCloudSpawn = 0;
            }

            foreach (var cloud in Clouds)
            {
                var newX = cloud.Position.X + speed * delta;

                cloud.Position = new Vector2(newX, cloud.Position.Y);

                if (cloud.Position.X >= ASummonersTaleGame.ScreenRectangle.Width)
                    cloud.ShouldBeDestroyed = true;
            }
        }

        public void RemoveInactiveClouds()
        {
            Clouds.RemoveAll(c => c.ShouldBeDestroyed);
        }

        public void SpawnCloud()
        {
            var cloudY = random.Next(CloudComponent.MinHeight, CloudComponent.MaxHeight);

            var cloud = new Cloud
            {
                Position = new Vector2(cloudX, cloudY),
                ShouldBeDestroyed = false
            };

            Clouds.Add(cloud);
        }
    }
}