using ASummonersTale.Components;
using ASummonersTale.Components.Input;
using ASummonersTale.GameStates.Interfaces;
using ASummonersTale.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Windows.Forms;

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

            // TODO: Continue
            if (InputHandler.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))

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
            MessageBox.Show("Not ready");
        }

        public void StartGame()
        {
            MessageBox.Show("Not ready");
        }

 
    }
}
