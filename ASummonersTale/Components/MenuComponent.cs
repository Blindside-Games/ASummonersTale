using ASummonersTale.Components.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ASummonersTale.Components
{
    public class MenuComponent
    {
        private SpriteFont font;

        private readonly List<string> menuItems = new List<string>();

        private int selectedIndex = -1;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = (int)MathHelper.Clamp(value, 0, menuItems.Count - 1); }
        }

        private bool mouseOver;
        public bool MouseOver => mouseOver;

        private int width, height;
        public int Width => width;
        public int Height => height;

        private Color normalColour = Color.White, highlightedColour = Color.Red;

        public Color NormalColor
        {
            get { return normalColour; }
            set { normalColour = value; }
        }

        public Color HighlightedColor
        {
            get { return highlightedColour; }
            set { highlightedColour = value; }
        }

        private Texture2D normalTexture, activeTexture, inactiveTexture;

        private SoundEffect buttonIndexChanged;
        private SoundEffectInstance buttonIndexChangedEffectInstance;

        private Rectangle previousButton;

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }



        public MenuComponent(SpriteFont font, ASummonersTaleGame gameReference)
        {
            mouseOver = false;

            this.font = font;
            activeTexture = gameReference.Content.Load<Texture2D>(@"Images\Miscellaneous\menu_button_normal");
            normalTexture = gameReference.Content.Load<Texture2D>(@"Images\Miscellaneous\menu_button_activated");
            inactiveTexture = gameReference.Content.Load<Texture2D>(@"Images\Miscellaneous\menu_button_inactive");

            buttonIndexChanged = gameReference.Content.Load<SoundEffect>(@"Sounds\button_sound");
            buttonIndexChangedEffectInstance = buttonIndexChanged.CreateInstance();
        }

        public MenuComponent(SpriteFont font, string[] menuItems, ASummonersTaleGame gameReference) : this(font, gameReference)
        {
            selectedIndex = ASummonersTaleGame.SavedGamePresent ? 0 : 1;

            this.menuItems.AddRange(menuItems);

            MeasureMenu();
        }

        public void Update(GameTime gameTime, PlayerIndex? playerIndex)
        {
            Vector2 menuPosition = position;
            Point point = InputHandler.MouseState.Position;

            Rectangle currentButtonRectangle;

            mouseOver = false;

            // For each menu item
            for (int i = 0; i < menuItems.Count; i++)
            {
                // Create a button rectangle 
                currentButtonRectangle = new Rectangle((int)menuPosition.X, (int)menuPosition.Y, normalTexture.Width, normalTexture.Height);

                
                // If the mouse is inside the button's rectangle
                if (currentButtonRectangle.Contains(point))
                {
                    if (buttonIndexChangedEffectInstance.State == SoundState.Stopped)
                        buttonIndexChangedEffectInstance.Play();

                    selectedIndex = i;
                    mouseOver = true;
                }

                menuPosition.Y += normalTexture.Height + 80;
            }

            if (!mouseOver && InputHandler.KeyReleased(Keys.Up))
            {
                selectedIndex--;

                buttonIndexChangedEffectInstance.Play();

                // Wrap around
                if (selectedIndex < 0 || (selectedIndex == 0 && !ASummonersTaleGame.SavedGamePresent))
                    selectedIndex = menuItems.Count - 1;
            }
            else if (!mouseOver && InputHandler.KeyReleased(Keys.Down))
            {
                selectedIndex++;

                buttonIndexChangedEffectInstance.Play();

                if (selectedIndex > menuItems.Count - 1)
                    selectedIndex = ASummonersTaleGame.SavedGamePresent ? 0 : 1;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 menuPosition = position;

            Texture2D drawTexture;

            for (int i = 0; i < menuItems.Count; i++)
            {
                drawTexture = (i == selectedIndex) ? activeTexture : normalTexture;

                if (!ASummonersTaleGame.SavedGamePresent && i == 0)
                    drawTexture = inactiveTexture;

                spriteBatch.Draw(drawTexture, menuPosition, Color.White);

                Vector2 textSize = font.MeasureString(menuItems[i]) / 2;
                Vector2 textPosition = menuPosition +
                                       new Vector2((normalTexture.Width - textSize.X) / 2,
                                            (normalTexture.Height - textSize.Y) / 2 + 7);

                spriteBatch.DrawString(font, menuItems[i], textPosition, Color.White, 0f, Vector2.Zero, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0);

                menuPosition.Y += normalTexture.Height + 75;
            }
        }

        public void SetMenuItems(string[] items)
        {
            menuItems.Clear();
            menuItems.AddRange(items);

            MeasureMenu();

            selectedIndex = 0;
        }

        private void MeasureMenu()
        {
            width = normalTexture.Width;
            height = 0;

            foreach (var s in menuItems)
            {
                Vector2 size = font.MeasureString(s);

                if (size.X > width)
                    width = (int)size.X;

                height += normalTexture.Height + 50;
            }

            height -= 50;
        }

    }
}
