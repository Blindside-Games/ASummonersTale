using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASummonersTale.Components.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        private Texture2D texture;

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public MenuComponent(SpriteFont font, Texture2D texture)
        {
            mouseOver = false;

            this.font = font;
            this.texture = texture;
        }

        public MenuComponent(SpriteFont font, Texture2D texture, string[] menuItems) : this(font, texture)
        {
            selectedIndex = 0;

            this.menuItems.AddRange(menuItems);

            MeasureMenu();
        }

        public void Update(GameTime gameTime, PlayerIndex? playerIndex)
        {
            Vector2 menuPosition = position;
            Point point = InputHandler.MouseState.Position;

            Rectangle buttonRectangle;
            mouseOver = false;

            // For each menu item
            for (int i = 0; i < menuItems.Count; i++)
            {
                // Create a button rectangle 
                buttonRectangle = new Rectangle((int)menuPosition.X, (int)menuPosition.Y, texture.Width, texture.Height);

                // If the mouse is inside the button's rectangle
                if (buttonRectangle.Contains(point))
                {
                    selectedIndex = i;
                    mouseOver = true;
                }

                menuPosition.Y += texture.Height + 50;
            }

            if (!mouseOver && InputHandler.KeyReleased(Keys.Up))
            {
                selectedIndex--;

                // Wrap around
                if (selectedIndex < 0)
                    selectedIndex = menuItems.Count - 1;
            }
            else if (!mouseOver && InputHandler.KeyReleased(Keys.Down))
            {
                selectedIndex++;

                if (selectedIndex > menuItems.Count - 1)
                    selectedIndex = 0;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 menuPosition = position;

            Color itemColour;

            for (int i = 0; i < menuItems.Count; i++)
            {
                itemColour = (i == selectedIndex) ? highlightedColour : normalColour;
                
                spriteBatch.Draw(texture, menuPosition, Color.White);

                Vector2 textSize = font.MeasureString(menuItems[i]);
                Vector2 textPosition = menuPosition +
                                       new Vector2((int) (texture.Width - textSize.X) / 2,
                                           (int) (texture.Height - textSize.Y) / 2);

                spriteBatch.DrawString(font, menuItems[i], textPosition, itemColour);

                menuPosition.Y += texture.Height + 50;
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
            width = texture.Width;
            height = 0;

            foreach (var s in menuItems)
            {
                Vector2 size = font.MeasureString(s);

                if (size.X > width)
                    width = (int)size.X;

                height += texture.Height + 50;
            }

            height -= 50;
        }

    }
}
