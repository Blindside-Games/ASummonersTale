using ASummonersTale.StateManager.Interfaces;
using ASummonersTale.GameStates;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ASummonersTale.StateManager
{
	public class GameStateManager : GameComponent, IGameStateManager
	{
		private readonly Stack<AbstractGameState> states = new Stack<AbstractGameState>();

		private const int startDrawOrder = 5000, drawOrderIncrement = 50;
		private int drawOrder;

		public AbstractGameState CurrentState
		{
			get
			{
				return states.Peek();
			}
		}

		public event EventHandler StateChanged;

		public GameStateManager(Game game) : base(game)
		{
			Game.Services.AddService(typeof(IGameStateManager), this);
		}

		public void PushState(AbstractGameState state, PlayerIndex? index)
		{
			drawOrder += drawOrderIncrement;

			AddState(state, index);

			OnStateChanged();
		}

		public void ChangeState(AbstractGameState state, PlayerIndex? index)
		{
			while (states.Count > 0)
				RemoveState();

			drawOrder = startDrawOrder;
			state.DrawOrder = drawOrder;

			drawOrder += drawOrderIncrement;

            AddState(state, index);
            OnStateChanged();
		}

		public void PopState()
		{
			if (states.Count != 0)
			{
				RemoveState();

				drawOrder -= drawOrderIncrement;
				OnStateChanged();
			}
		}

		public bool ContainsState(AbstractGameState state) => states.Contains(state);

		void AddState(AbstractGameState state, PlayerIndex? index)
		{
			states.Push(state);
			state.PlayerInControl = index;

			Game.Components.Add(state);

			StateChanged += state.StateChanged;
		}

		void OnStateChanged()
		{
			if (StateChanged != null)
				StateChanged(this, null);
		}

		void RemoveState()
		{
			AbstractGameState state = states.Peek();

			StateChanged -= StateChanged;

			Game.Components.Remove(state);

			states.Pop();
		}
	}
}
