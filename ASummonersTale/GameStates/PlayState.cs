using ASummonersTale.Components;
using ASummonersTale.Components.Input;
using ASummonersTale.GameStates.Interfaces;
using ASummonersTale.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ASummonersTale.GameStates
{
    public class PlayState : GameState, IPlayState
    {
        Engine tileEngine = new Engine(ASummonersTaleGame.ScreenRectangle, 64, 64);
        TileMap currentMap;
        Camera camera;

        LevelManager levelManager;

        public PlayState(Game game) : base(game)
        {
            game.Services.AddService(typeof(IPlayState), this);
            levelManager = new LevelManager();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            Vector2 motion = Vector2.Zero;

            bool s = InputHandler.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S),
             a = InputHandler.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A),
             d = InputHandler.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D),
             w = InputHandler.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W);

            // TODO: Continue
            if (w && a)
            {
                motion.X = motion.Y = -1;
            }
            else if (w && d)
            {
                motion.X = 1;
                motion.Y = -1;
            }
            else if (s && a)
            {
                motion.X = -1;
                motion.Y = 1;
            }
            else if (s && d)
            {
                motion.X = 1;
                motion.Y = 1;
            }
            else if (w)
                motion.Y = -1;
            else if (a)
                motion.X = -1;
            else if (s)
                motion.Y = 1;
            else if (d)
                motion.X = 1;
            else if (InputHandler.KeyReleased(Microsoft.Xna.Framework.Input.Keys.M))
            {
                GameReference.WorldMapState.ConstructMap(currentMap, camera);

                manager.PushState((WorldMapState)GameReference.WorldMapState, PlayerIndex.One);
            }

            if (motion != Vector2.Zero)
            {
                motion.Normalize();

                motion *= camera.Speed;

                camera.Position += motion;

                camera.LockCamera(currentMap, ASummonersTaleGame.ScreenRectangle);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (currentMap != null && camera != null)
                currentMap.Draw(gameTime, GameReference.SpriteBatch, camera);
        }

        public void NewGame()
        {
            Texture2D tiles = GameReference.Content.Load<Texture2D>(@"Images\Tilesets\town");
            TileSet townTileset = new TileSet(8, 8, 32, 32);
            townTileset.Texture = tiles;

            TileLayer background = new TileLayer(200, 200), edges = new TileLayer(200, 200),
                building = new TileLayer(200, 200), decorations = new TileLayer(200, 200);

            levelManager[1] = new TileMap(townTileset, "test-map", background, edges, building, decorations);

            currentMap = levelManager[1];

            currentMap.FillEdges();
            currentMap.FillBuildings();
            currentMap.FillDecoration();

            camera = new Camera();
        }

        public void LoadGame()
        {

        }

        public void StartGame()
        {
        }


    }
}
