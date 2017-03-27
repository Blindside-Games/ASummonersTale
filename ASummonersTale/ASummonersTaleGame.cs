using ASummonersTale.Components.Input;
using ASummonersTale.Components.Settings;
using ASummonersTale.GameStates;
using ASummonersTale.GameStates.Interfaces;
using ASummonersTale.StateManager;
using ASummonersTale.TileEngine.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASummonersTale
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class ASummonersTaleGame : Game
    {
        GraphicsDeviceManager graphics;

        internal Settings Settings;
        public static bool SavedGamePresent = false;

        private static Rectangle screenRectangle;
        public static Rectangle ScreenRectangle => screenRectangle;

        private SpriteBatch spriteBatch;
        public SpriteBatch SpriteBatch => spriteBatch;

        #region Game States
        private GameStateManager stateManager;
        private ISplashScreenState splashScreenState;
        public ISplashScreenState SplashScreenState => splashScreenState;

        private IMenuState startMenuState;
        public IMenuState StartMenuState => startMenuState;

        private IPlayState playState;
        public IPlayState PlayState => playState;

        private IWorldMapState worldMapState;
        internal IWorldMapState WorldMapState => worldMapState;

        public RasterizerState RasterizerState;
        #endregion

        #region Player

        Dictionary<AnimationKey, Animation> playerAnimations = new Dictionary<AnimationKey, Animation>();

        internal Dictionary<AnimationKey, Animation> PlayerAnimations => playerAnimations;

        #endregion

        public ASummonersTaleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            #region Set up settings
            Settings = new Settings();

            if (!(Task.Run(async () => await Settings.ReadSettings()).Result))
            {
                Exit();

                return;
            }

            if (Settings.AntiAliasingOn)
                EnableAntiAliasing();
            #endregion

            screenRectangle = new Rectangle(0, 0, 1280, 720);

            graphics.PreferredBackBufferHeight = screenRectangle.Height;
            graphics.PreferredBackBufferWidth = screenRectangle.Width;

            stateManager = new GameStateManager(this);
            Components.Add(stateManager);

            this.IsMouseVisible = true;

            #region Create game states
            splashScreenState = new SplashScreenState(this);
            startMenuState = new MenuState(this);
            playState = new PlayState(this);
            worldMapState = new WorldMapState(this);
            #endregion

            stateManager.ChangeState((SplashScreenState)splashScreenState, PlayerIndex.One);
        }

        private void EnableAntiAliasing()
        {
            graphics.PreferMultiSampling = true;

            RasterizerState = new RasterizerState { MultiSampleAntiAlias = true };
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Components.Add(new InputHandler(this));

            #region Initialise player animations

            playerAnimations.Add(AnimationKey.WalkDown, new Animation(3, 32, 32, 0, 0));
            playerAnimations.Add(AnimationKey.WalkLeft, new Animation(3, 32, 32, 0, 32));
            playerAnimations.Add(AnimationKey.WalkRight, new Animation(3, 32, 32, 0, 64));
            playerAnimations.Add(AnimationKey.WalkUp, new Animation(3, 32, 32, 0, 96));

            #endregion

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

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

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
