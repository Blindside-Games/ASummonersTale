using ASummonersTale.Components.Input;
using ASummonersTale.GameStates.Interfaces;
using ASummonersTale.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ASummonersTale.GameStates
{
    internal class WorldMapState : GameState, IWorldMapState
    {
        private TileMap tileMap;
        private Camera camera;

        public WorldMapState(ASummonersTaleGame game) : base(game)
        {
            Game.Services.AddService(typeof(IWorldMapState), this);
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 motion = Vector2.Zero;

            bool s = InputHandler.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S),
             a = InputHandler.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A),
             d = InputHandler.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D),
             w = InputHandler.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W);

            if (InputHandler.KeyReleased(Keys.M))
            {
                Engine.ScaleFactor = 1;

                // Not sure this is correct, this will leave 2 play states on the stack. if you were to repeatedly 
                // close and reopen the map, it would leave lots of copies of the play state on the state stack      
                manager.ChangeState((PlayState)GameReference.PlayState, PlayerIndex.One);
            }
            else if (w && a)
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

            if (InputHandler.MousewheelUp)
                Engine.ScaleFactor -= 1;
            else if (InputHandler.MousewheelDown)
                Engine.ScaleFactor += 1;

            if (motion != Vector2.Zero)
            {
                motion.Normalize();

                motion *= camera.Speed;

                camera.Position += motion;

                camera.LockCamera(tileMap, ASummonersTaleGame.ScreenRectangle);
            }

            tileMap.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            tileMap.Draw(gameTime, GameReference.SpriteBatch, camera);

            base.Draw(gameTime);
        }

        public void ConstructMap(TileMap tileMap, Camera camera)
        {
            this.tileMap = tileMap;
            this.camera = camera;

            Engine.ScaleFactor = 2;
        }
    }
}
