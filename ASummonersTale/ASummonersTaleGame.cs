using ASummonersTale.Components.Input;
using ASummonersTale.GameStates;
using ASummonersTale.GameStates.Interfaces;
using ASummonersTale.StateManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ASummonersTale
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class ASummonersTaleGame : Game
    {
        GraphicsDeviceManager graphics;
        
        private static Rectangle screenRectangle;
        public static Rectangle ScreenRectangle => screenRectangle;

        private SpriteBatch spriteBatch;
        public SpriteBatch SpriteBatch => spriteBatch;

        private GameStateManager stateManager;
        private ISplashScreenState splashScreenState;
        public ISplashScreenState SplashScreenState => splashScreenState;

        private IMenuState startMenuState;
        public IMenuState StartMenuState => startMenuState;

        public ASummonersTaleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            screenRectangle = new Rectangle(0, 0, 1280, 720);

            graphics.PreferredBackBufferHeight = screenRectangle.Height;
            graphics.PreferredBackBufferWidth = screenRectangle.Width;

            stateManager = new GameStateManager(this);
            Components.Add(stateManager);

            this.IsMouseVisible = true;

            splashScreenState = new SplashScreenState(this);
            startMenuState = new MenuState(this);

            stateManager.ChangeState((SplashScreenState)splashScreenState, PlayerIndex.One);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Components.Add(new InputHandler(this));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
