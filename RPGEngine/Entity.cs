using System;
using System.Collections.Generic;
using System.Text;
using Aze.Utilities;

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
        public enum MoveDirection
        {
            North_East, North, North_West,
            East, West,
            South_East, South, South_West,

            Up_North_East, Up_North, Up_North_West,
            Up_East,       Up,       Up_West,
            Up_South_East, Up_South, Up_South_West,

            Down_North_East, Down_North, Down_North_West,
            Down_East,       Down,       Down_West,
            Down_South_East, Down_South, Down_South_West
        }

        public string Name { set; get; } = "Unknown";
        public string Description { set; get; } = "Unknown";

        public string SpriteImageFile { set; get; } = "ImageFile";

        public int Weight { set; get; }
        public EntitySize Size { set; get; } = new EntitySize();
        public EntityPosition Position { set; get; } = new EntityPosition();

        public void SetPosition(int x, int y, int z = 0, int reality = 0)
        {
            Position.X = x;
            Position.Y = y;
            Position.Z = z;
            Position.Reality = reality;
            ConsoleUtils.LogInfo("{0} is now at location ({1},{2},{3})", Name, Position.X, Position.Y, Position.Z);
        }
    }
}
