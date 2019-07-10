using System;
using System.Collections.Generic;
using System.Text;

namespace RPGEngine
{
    public class Character : Creature
    {
        public List<InventoryItem> Items { set; get; } = new List<InventoryItem>();

        public Character()
        {
            
        }
    }
}
