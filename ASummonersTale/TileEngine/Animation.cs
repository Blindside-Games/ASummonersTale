using System;
using Microsoft.Xna.Framework;

namespace ASummonersTale.TileEngine
{
    internal class Animation
    {
        Rectangle[] frames;
        int framesPerSecond;
        TimeSpan frameLength, frameTimer;
        int currentFrame;
        int frameWidth, frameHeight;

        internal int FramesPerSecond
        {
            get => framesPerSecond;

            set
            {
                if (value < 1)
                    framesPerSecond = 1;
                else if (value > 60)
                    framesPerSecond = 60;
                else
                    framesPerSecond = value;

                frameLength = TimeSpan.FromSeconds(1 / (double)framesPerSecond);
                    
            }
        }

        internal Rectangle CurrentFrameRectangle => frames[currentFrame];

        internal int CurrentFrame
        {
            get => currentFrame;

            set => currentFrame = MathHelper.Clamp(value, 0, frames.Length - 1);
        }

        internal int FrameWidth => frameWidth;
        internal int FrameHeight => frameHeight;

        public Animation(int frameCount, int frameWidth, int frameHeight, int xOffset, int yOffset)
        {
            frames = new Rectangle[frameCount];

            this.frameHeight = frameHeight;
        }
    }
}
