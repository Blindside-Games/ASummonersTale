using Microsoft.Xna.Framework;
using System;

namespace ASummonersTale.TileEngine.Animations
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
            this.frameWidth = frameWidth;

            for (int i = 0; i < frameCount; i++)
            {
                frames[i] = new Rectangle(
                    xOffset + (i * frameWidth), yOffset,
                    frameWidth, frameHeight);
            }

            FramesPerSecond = 5;
            Reset();
        }

        private Animation(Animation animation)
        {
            frames = animation.frames;
            FramesPerSecond = 5;
        }

        internal void Reset()
        {
            currentFrame = 0;
            frameTimer = TimeSpan.Zero;
        }

        public void Update(GameTime gameTime)
        {
            frameTimer += gameTime.ElapsedGameTime;

            if (frameTimer >= frameLength)
            {
                frameTimer = TimeSpan.Zero;
                currentFrame = (currentFrame + 1) % frames.Length;
            }
        }

        public object Clone()
        {
            Animation animationClone = new Animation(this);

            animationClone.frameHeight = frameHeight;
            animationClone.frameWidth = frameWidth;

            return animationClone;
        }
    }
}
