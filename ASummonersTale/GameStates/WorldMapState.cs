using ASummonersTale.Components.Input;
using ASummonersTale.GameStates.Interfaces;
using ASummonersTale.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static ASummonersTale.Components.Settings.Settings;

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

            bool down = InputHandler.IsKeybindDown(GameReference.Settings.Bindings[Action.MoveMapDown]),
             left = InputHandler.IsKeybindDown(GameReference.Settings.Bindings[Action.MoveMapLeft]),
             right = InputHandler.IsKeybindDown(GameReference.Settings.Bindings[Action.MoveMapRight]),
             up = InputHandler.IsKeybindDown(GameReference.Settings.Bindings[Action.MoveMapUp]);

            if (InputHandler.IsKeybindReleased(GameReference.Settings.Bindings[Action.ToggleMap]))
            {
                Engine.ScaleFactor = 1;

                // Not sure this is correct, this will leave 2 play states on the stack. if you were to repeatedly 
                // close and reopen the map, it would leave lots of copies of the play state on the state stack      
                manager.ChangeState((PlayState)GameReference.PlayState, PlayerIndex.One);
            }
            else if (up && left)
            {
                motion.X = motion.Y = -1;
            }
            else if (up && right)
            {
                motion.X = 1;
                motion.Y = -1;
            }
            else if (down && left)
            {
                motion.X = -1;
                motion.Y = 1;
            }
            else if (down && right)
            {
                motion.X = 1;
                motion.Y = 1;
            }
            else if (up)
                motion.Y = -1;
            else if (left)
                motion.X = -1;
            else if (down)
                motion.Y = 1;
            else if (right)
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
