using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ASummonersTale.TileEngine.Animations
{
    // Potentially refactor to inherit from base class Entity
    internal class AnimatedSprite
    {
        Dictionary<AnimationKey, Animation> animations;

        AnimationKey currentAnimation;
        bool isAnimating;

        Texture2D texture;

        private Vector2 position;
        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        Vector2 velocity;

        float speed = 200f;

        public bool Active { get; set; }

        public AnimationKey CurrentAnimation
        {
            get => currentAnimation;
            set => currentAnimation = value;
        }

        public bool IsAnimating
        {
            get => isAnimating;
            set => isAnimating = value;
        }

        public int Width => animations[currentAnimation].FrameWidth;
        public int Height => animations[currentAnimation].FrameHeight;

        public float Speed
        {
            get => speed;
            set => MathHelper.Clamp(value, 1f, 400f);
        }

        public Vector2 Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        public AnimatedSprite(Texture2D sprite, Dictionary<AnimationKey, Animation> animation)
        {
            texture = sprite;
            animations = new Dictionary<AnimationKey, Animation>();

            foreach (var key in animation.Keys)
                animations.Add(key, (Animation)animation[key].Clone());
        }

        public void ResetAnimation() => animations[currentAnimation].Reset();

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, animations[currentAnimation].CurrentFrameRectangle, Color.White);
        }

        public void LockToMap(Point mapSize)
        {
            position.X = MathHelper.Clamp(position.X, 0, mapSize.X - Width);
            position.Y = MathHelper.Clamp(position.Y, 0, mapSize.Y - Height);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (isAnimating)
                animations[currentAnimation].Update(gameTime);
        }
    }
}
