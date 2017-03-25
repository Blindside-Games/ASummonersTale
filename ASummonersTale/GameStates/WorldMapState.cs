using ASummonersTale.Components.Input;
using ASummonersTale.GameStates.Interfaces;
using ASummonersTale.TileEngine;
using Microsoft.Xna.Framework;
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

            bool down = InputHandler.IsKeybindDown(GameReference.Settings.Bindings[Action.MoveMapDown]) || MouseBottom,
             left = InputHandler.IsKeybindDown(GameReference.Settings.Bindings[Action.MoveMapLeft]) || MouseLeft,
             right = InputHandler.IsKeybindDown(GameReference.Settings.Bindings[Action.MoveMapRight]) || MouseRight,
             up = InputHandler.IsKeybindDown(GameReference.Settings.Bindings[Action.MoveMapUp]) || MouseTop;

            if (InputHandler.IsKeybindReleased(GameReference.Settings.Bindings[Action.ToggleMap]))
                CloseMap();

            if (up && left)
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


        private void CloseMap()
        {
            Engine.ScaleFactor = 1;

            manager.ChangeState((PlayState)GameReference.PlayState, PlayerIndex.One);
        }

        private readonly int ScrollUpZone = 80, ScrollLeftZone = 80, ScrollRightZone = ASummonersTaleGame.ScreenRectangle.Width - 80, ScrollDownZone = ASummonersTaleGame.ScreenRectangle.Height - 80;

        private bool MouseTop => InputHandler.MouseState.Y < ScrollUpZone && InputHandler.MouseState.Y >= 0 ? true : false;

        private bool MouseLeft => InputHandler.MouseState.X < ScrollLeftZone && InputHandler.MouseState.X >= 0 ? true : false;

        private bool MouseBottom => InputHandler.MouseState.Y > ScrollDownZone && InputHandler.MouseState.Y <= ASummonersTaleGame.ScreenRectangle.Height ? true : false;

        private bool MouseRight => InputHandler.MouseState.X > ScrollRightZone && InputHandler.MouseState.X <= ASummonersTaleGame.ScreenRectangle.Width ? true : false;
    }
}
