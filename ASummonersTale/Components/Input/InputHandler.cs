using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ASummonersTale.Components.Input
{
    public class InputHandler : GameComponent
    {
        private static KeyboardState currentKeyboardState = Keyboard.GetState(),
            previousKeyboardState = Keyboard.GetState();

        private static MouseState currentMouseState = Mouse.GetState(),
            previousMouseState = Mouse.GetState();

        public static KeyboardState KeyboardState => currentKeyboardState;
        public static KeyboardState PreviousKeyboardState => previousKeyboardState;

        public static MouseState MouseState => currentMouseState;
        public static MouseState PreviousMouseState => previousMouseState;

        public InputHandler(Game game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            base.Update(gameTime);
        }

        public static void Flush()
        {
            currentMouseState = previousMouseState;
            currentKeyboardState = previousKeyboardState;
        }

        public static bool KeyReleased(Keys key)
            => currentKeyboardState.IsKeyUp(key) && previousKeyboardState.IsKeyDown(key);

        public static bool MouseReleased(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return currentMouseState.LeftButton == ButtonState.Released &&
                           previousMouseState.LeftButton == ButtonState.Pressed;

                case MouseButton.Centre:
                    return currentMouseState.MiddleButton == ButtonState.Released &&
                           previousMouseState.MiddleButton == ButtonState.Pressed;

                case MouseButton.Right:
                    return currentMouseState.RightButton == ButtonState.Released &&
                           previousMouseState.RightButton == ButtonState.Pressed;

                default:
                    return false;
            }
        }
    }
}
