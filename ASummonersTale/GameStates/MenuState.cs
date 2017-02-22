using ASummonersTale.Components;
using ASummonersTale.Components.Input;
using ASummonersTale.GameStates.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ASummonersTale.GameStates
{
    public class MenuState : GameState, IMenuState
    {
        private Texture2D background;
        private SpriteFont font;
        private MenuComponent menuComponent;

        public MenuState(Game game) : base(game)
        {
            Game.Services.AddService(typeof(IMenuState), this);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>(@"Fonts\InterfaceFont");
            background = Game.Content.Load<Texture2D>(@"Images\Menu Screens\menuscreen");

            Texture2D buttonTexture = Game.Content.Load<Texture2D>(@"Images\Miscellaneous\woodenbutton");

            string[] menuItems = {"NEW GAME", "CONTINUE", "OPTIONS", "EXIT"};

            menuComponent = new MenuComponent(font, buttonTexture, menuItems);

            menuComponent.Position = new Vector2(1200 - menuComponent.Width, 90);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            menuComponent.Update(gameTime, null);

            if (InputHandler.KeyReleased(Keys.Space) || InputHandler.KeyReleased(Keys.Enter))
            {
                if (menuComponent.SelectedIndex < 3)
                    InputHandler.Flush();
                else
                    Game.Exit();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameReference.SpriteBatch.Begin();

            GameReference.SpriteBatch.Draw(background, Vector2.Zero, Color.White);

            GameReference.SpriteBatch.End();

            base.Draw(gameTime);

            GameReference.SpriteBatch.Begin();

            menuComponent.Draw(gameTime, GameReference.SpriteBatch);

            GameReference.SpriteBatch.End();
        }
    }
}