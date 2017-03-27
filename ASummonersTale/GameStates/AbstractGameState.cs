using ASummonersTale.GameStates.Interfaces;
using ASummonersTale.StateManager.Interfaces;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;


namespace ASummonersTale.GameStates
{
    public abstract partial class AbstractGameState : DrawableGameComponent, IGameState
    {
        protected AbstractGameState tag;
        protected readonly IGameStateManager manager;

        protected ILog log;

        /// <summary>
        /// The content for this state.
        /// </summary>
        protected ContentManager content;
        protected readonly List<GameComponent> childComponents;

        protected PlayerIndex? playerInControl;

        public PlayerIndex? PlayerInControl
        {
            get
            {
                return playerInControl;
            }

            set
            {
                playerInControl = value;
            }
        }

        public AbstractGameState Tag => tag;

        public List<GameComponent> Components => childComponents;

        protected AbstractGameState(Game game) : base(game)
        {
            tag = this;

            childComponents = new List<GameComponent>();
            content = game.Content;

            manager = (IGameStateManager)Game.Services.GetService(typeof(IGameStateManager));
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in childComponents)
                if (component.Enabled)
                    component.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach (var component in childComponents)
                if ((component as DrawableGameComponent).Visible)
                    (component as DrawableGameComponent).Draw(gameTime);

        }

        protected internal virtual void StateChanged(object sender, EventArgs e)
        {
            if (manager.CurrentState == tag)
                Show();
            else
                Hide();
        }


        /// <summary>
        /// Show this instance  and its child components.
        /// </summary>
        void Show()
        {
            Enabled = true;
            Visible = true;

            foreach (var component in childComponents)
            {
                component.Enabled = true;

                if (component is DrawableGameComponent)
                    (component as DrawableGameComponent).Visible = true;
            }
        }

        /// <summary>
        /// Hide this instance and its child components.
        /// </summary>
        void Hide()
        {
            Enabled = false;
            Visible = false;

            foreach (var component in childComponents)
            {
                component.Enabled = false;

                if (component is DrawableGameComponent)
                    (component as DrawableGameComponent).Visible = false;
            }
        }
    }
}
