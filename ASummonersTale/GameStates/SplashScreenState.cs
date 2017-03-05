using ASummonersTale.GameStates.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using ASummonersTale.Components;
using ASummonersTale.Components.Clouds;
using ASummonersTale.Components.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ASummonersTale.GameStates
{
	public class SplashScreenState : GameState, ISplashScreenState
	{
		Texture2D background, foreground, cloud;
		Rectangle backgroundDestination;
		SpriteFont font;
		TimeSpan elapsed;
		Vector2 position;
		string message;

		private SoundEffect windSound;
		 
		private Song menuMusic;

		private SoundEffectInstance windEffectInstance;

		private CloudComponent clouds;

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
			background = content.Load<Texture2D>(@"Images\Menu Screens\splash-screen-bg");
			foreground = content.Load<Texture2D>(@"Images\Menu Screens\splash-screen-fg");
			font = content.Load<SpriteFont>(@"Fonts\Trajan");

			windSound = content.Load<SoundEffect>(@"Sounds\wind");
			menuMusic = content.Load<Song>(@"Sounds\music");

			windEffectInstance = windSound.CreateInstance();
			windEffectInstance.IsLooped = true;
            windEffectInstance.Volume = ASummonersTaleGame.Settings.SoundEffectsVolume;
			windEffectInstance.Play();

			MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = ASummonersTaleGame.Settings.MusicVolume;
			MediaPlayer.Play(menuMusic);

			cloud = content.Load<Texture2D>(@"Images\Miscellaneous\cloud");

			clouds = new CloudComponent(2, cloud, GameReference, 35, 50);

			Vector2 size = font.MeasureString(message);

			position = new Vector2((ASummonersTaleGame.ScreenRectangle.Width - size.X),
									ASummonersTaleGame.ScreenRectangle.Bottom - 50 - font.LineSpacing) ;

			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			PlayerIndex? index = null;
			elapsed += gameTime.ElapsedGameTime; 

			clouds.Update(gameTime);

			if (InputHandler.KeyReleased(Keys.Space))
				manager.ChangeState((MenuState)GameReference.StartMenuState, index);
				
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			GameReference.SpriteBatch.Begin(rasterizerState: GameReference.RasterizerState);

			GameReference.SpriteBatch.Draw(background, backgroundDestination, Color.White);

			clouds.Draw(1, gameTime, GameReference.SpriteBatch);

			GameReference.SpriteBatch.Draw(foreground, backgroundDestination, Color.White);

			clouds.Draw(2, gameTime, GameReference.SpriteBatch);

			// This creates a pulsing colour effect
			Color textColour = new Color(1f, 1f, 1f) * (float)Math.Abs(Math.Sin(elapsed.TotalSeconds * 2));

			GameReference.SpriteBatch.DrawString(font, message, position, textColour, 0f, Vector2.Zero, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0);

			GameReference.SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
