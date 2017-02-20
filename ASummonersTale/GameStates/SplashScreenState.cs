using ASummonersTale.GameStates.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace ASummonersTale.GameStates
{
	public class SplashScreenState : GameState, ISplashScreenState
	{
		Texture2D background;
		Rectangle backgroundDestination;
		SpriteFont font;
		TimeSpan elapsed;
		Vector2 position;
		string message;

		public SplashScreenState(Game game) : base(game)
		{
			game.Services.AddService(typeof(ISplashScreenState), this);
		}

		public override void Initialize()
		{
			backgroundDestination = ASummonersTaleGame.ScreenRectangle;
			elapsed = TimeSpan.Zero;
			message = "Press Space to Continue";

			base.Initialize();
		}

		protected override void LoadContent()
		{
			background = content.Load<Texture2D>(@"Images\Menu Screens\titlescreen");
			font = content.Load<SpriteFont>(@"Fonts\InterfaceFont");

			Vector2 size = font.MeasureString(message);

			position = new Vector2((ASummonersTaleGame.ScreenRectangle.Width - size.X) / 2,
									ASummonersTaleGame.ScreenRectangle.Bottom - 50 - font.LineSpacing);

			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			PlayerIndex index = PlayerIndex.One;
			elapsed += gameTime.ElapsedGameTime; 
				
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			GameReference.SpriteBatch.Begin();

			GameReference.SpriteBatch.Draw(background, backgroundDestination, Color.White);

			// This creates a pulsing colour effect
			Color textColour = new Color(1f, 1f, 1f) * (float)Math.Abs(Math.Sin(elapsed.TotalSeconds * 2));

			GameReference.SpriteBatch.DrawString(font, message, position, textColour);

			GameReference.SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
