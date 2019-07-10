using System;
using System.Collections.Generic;
using System.Text;

namespace RPGEngine
{
    public class EntityPosition
    {
        public int X { set; get; }
        public int Y { set; get; }
        public int Z { set; get; }
        public int Reality { set; get; }

        
    }

    public class EntitySize
    {
        public float Height { set; get; }
        public float Width { set; get; }
        public float Depth { set; get; }
    }

    public class Entity
    {
        public string Name { set; get; } = "Unknown";
        public string Description { set; get; } = "Unknown";

        public string SpriteImageFile { set; get; } = "ImageFile";

        public int Weight { set; get; }
        public EntitySize Size { set; get; } = new EntitySize();
        public EntityPosition Position { set; get; } = new EntityPosition();


    }
}
