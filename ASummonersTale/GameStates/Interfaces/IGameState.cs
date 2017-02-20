using Microsoft.Xna.Framework;

namespace ASummonersTale.GameStates.Interfaces
{
	/// <summary>
	/// Interface for game states
	/// </summary>
	public interface IGameState
	{

		/// <summary>
		/// Gets the tag.
		/// </summary>
		/// <value>The tag.</value>
		AbstractGameState Tag { get; }

		/// <summary>
		/// Allows the use of controllers
		/// </summary>
		/// <value>The index of the player in control.</value>
		PlayerIndex? PlayerInControl { get; set; }
	}
}
