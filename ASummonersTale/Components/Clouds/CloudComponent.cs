using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace ASummonersTale.Components.Clouds
{
    
    class CloudComponent
    {
        private List<CloudLayer> layers;

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

            for (int i = 0, layerDepth = 1; i < layers; i++, layerDepth++)
                this.layers.Add(new CloudLayer(layerDepth));
        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

           

            foreach (var cloudLayer in layers)
            {
                cloudLayer.Update(delta);  

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
