using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ASummonersTale.Components.Clouds
{
    internal class CloudLayer
    {
            internal List<Cloud> Clouds;

            private readonly int cloudX;

            public float CurrentCloudSpawn = 6;

            private readonly Random random;

            private readonly int MaxClouds = 5;

            private readonly int CloudSpawnTimer = 7;

            private int layerDepth, seed;

            public CloudLayer(int layerDepth)
            {
                Clouds = new List<Cloud>();
                random = new Random();

                seed = random.Next(0, 10);



                this.layerDepth = layerDepth;

                cloudX = -Cloud.Texture.Width;
            }

            public void Update(float delta)
            {
                CurrentCloudSpawn += delta;

                if (Clouds.Count < MaxClouds && ((int)CurrentCloudSpawn == CloudSpawnTimer))
                {
                    SpawnCloud();

                    CurrentCloudSpawn = 0;
                }

                foreach (Cloud cloud in Clouds)
                {
                    float newX = cloud.Position.X + (Cloud.Speed * delta);

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
                int cloudY = random.Next(CloudComponent.MinHeight, CloudComponent.MaxHeight);

                Cloud cloud = new Cloud
                {
                    Position = new Vector2(cloudX, cloudY),
                    ShouldBeDestroyed = false
                };

                Clouds.Add(cloud);
            }
        }

    }
