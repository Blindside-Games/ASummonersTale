using ASummonersTale.Components;
using ASummonersTale.Components.Input;
using ASummonersTale.GameStates.Interfaces;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;
using System;

namespace ASummonersTale.GameStates
{
    public class MenuState : GameState, IMenuState
    {
        private Texture2D background;
        private SpriteFont font;
        private MenuComponent menuComponent;

        PenumbraComponent penumbra;

        float staffLightIntensity = 1f;

        Random r = new Random();

        Light staffLight, doorLight;

        public MenuState(Game game) : base(game)
        {
            log = LogManager.GetLogger(nameof(MenuState));


            log.Info("Adding menu service");
            Game.Services.AddService(typeof(IMenuState), this);

            
            penumbra = new PenumbraComponent(GameReference)
            {
                AmbientColor = Color.Gray
            };

            log.Debug("Adding staff light");
            staffLight = new PointLight
            {
                Scale = new Vector2(staffLightIntensity),
                Position = new Vector2(413, 375),
                Color = Color.Yellow
            };

            log.Debug("Adding door light");
            doorLight = new PointLight()
            {
                Scale = new Vector2(250),
                Position = new Vector2(213, 248),
                Color = Color.Yellow
            };

            penumbra.Lights.Add(staffLight);
            penumbra.Lights.Add(doorLight);
        }

        public override void Initialize()
        {
            log.Info("Intitialising Penumbra");
            penumbra.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            log.Debug("Loading font");
            font = Game.Content.Load<SpriteFont>(@"Fonts\trajan");
            log.Debug("Loading background");
            background = Game.Content.Load<Texture2D>(@"Images\Menu Screens\menuscreen");

            string[] menuItems = { "CONTINUE", "NEW GAME", "OPTIONS", "EXIT" };

            menuComponent = new MenuComponent(font, menuItems, GameReference);

            menuComponent.Position = new Vector2(1350 - menuComponent.Width, 90);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            menuComponent.Update(gameTime, null);

            if (staffLightIntensity <= 250)
                staffLightIntensity += (700f * (float)gameTime.ElapsedGameTime.TotalSeconds);

            staffLight.Scale = new Vector2(staffLightIntensity);

            if (InputHandler.KeyReleased(Keys.Space) || InputHandler.KeyReleased(Keys.Enter) || InputHandler.MouseReleased(MouseButton.Left))
            {
                // TODO: Implement next states
                switch (menuComponent.SelectedIndex)
                {
                    case 0:
                        {
                            try
                            {
                                GameReference.PlayState.LoadGame();
                                GameReference.PlayState.StartGame();

                                // manager.PushState((PlayState)GameReference.PlayState, playerInControl);
                            }
                            catch
                            {

                            }
                            break;
                        }
                    case 1:
                        {
                            log.Info("Starting game");
                            GameReference.PlayState.NewGame();
                            GameReference.PlayState.StartGame();

                            log.Debug("Pushing play state");
                            manager.PushState((PlayState)GameReference.PlayState, playerInControl);
                            break;
                        }
                    case 2:
                        break;

                    case 3:
                        log.Info("Exiting game");
                        Game.Exit();
                        break;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            penumbra.BeginDraw();
            GameReference.SpriteBatch.Begin(rasterizerState: GameReference.RasterizerState);
            GameReference.SpriteBatch.Draw(background, Vector2.Zero, Color.White);
            GameReference.SpriteBatch.End();
            penumbra.Draw(gameTime);

            base.Draw(gameTime);

            GameReference.SpriteBatch.Begin(rasterizerState: GameReference.RasterizerState);
            menuComponent.Draw(gameTime, GameReference.SpriteBatch);
            GameReference.SpriteBatch.End();
        }
    }
}
