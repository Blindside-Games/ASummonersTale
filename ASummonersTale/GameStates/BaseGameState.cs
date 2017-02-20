using System;
using Microsoft.Xna.Framework;

namespace ASummonersTale.GameStates
{
	/// <summary>
	/// Base class for game states.
	/// </summary>
	public class GameState : AbstractGameState
	{
		protected static Random random = new Random();

		protected ASummonersTaleGame GameReference;

		public GameState(Game game) : base(game)
		{
			GameReference = game as ASummonersTaleGame;
		}

		protected override void LoadContent()
		{
			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
		}
	}
}
