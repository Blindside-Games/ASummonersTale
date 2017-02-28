using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ASummonersTale.Components.Clouds
{
    public class Cloud
    {
        public static Texture2D Texture;
        public Vector2 Position;

        public static readonly int Speed = 50;

        public bool ShouldBeDestroyed;
    }
}
