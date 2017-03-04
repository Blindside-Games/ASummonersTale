using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ASummonersTale.TileEngine
{
    // TODO: Continue
    public class TileLayer
    {
        [ContentSerializer(CollectionItemName = "Tiles")]
        private int[] tiles;
        private int width, height;

        private Point min, max, cameraPoint, viewPoint;

        private Rectangle destination;

        [ContentSerializerIgnore]
        public bool Enabled { get; set; }

        [ContentSerializerIgnore]
        public bool Visible { get; set; }

        [ContentSerializer]
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        [ContentSerializer]
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

    }
}
