using System;
using ASummonersTale.GameStates;
using Microsoft.Xna.Framework;

namespace ASummonersTale.StateManager.Interfaces
{
	public interface IGameStateManager
	{
		AbstractGameState CurrentState { get; }

		event EventHandler StateChanged;

		void PushState(AbstractGameState state, PlayerIndex? index);
		void ChangeState(AbstractGameState state, PlayerIndex? index);
		void PopState();

		bool ContainsState(AbstractGameState state);
	}
}
