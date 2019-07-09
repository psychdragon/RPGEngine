using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Aze.Utilities;

namespace RPGEngine
{
    class Creature
    {
        public string Name { set; get; }
        public string Description { set; get; }
        public string SpriteImageFile { set; get; }

        public int Health { set; get; }
        public int Level { set; get; }
        public int Attack { set; get; }
        public int Defence { set; get; }

        public List<JObject> Items = new List<JObject>();
        public List<JObject> AttackMoves = new List<JObject>();

        
        
        public Creature(string name, string description, string spriteImageFile="NoImage", int health=100, int level=0, int attack=0, int defence=0)
        {
            Name = name;
            Description = description;
            SpriteImageFile = spriteImageFile;

            Health = health;
            Level = level;
            Attack = attack;
            Defence = defence;

            AttackMoves.Add(new JObject
            {
                { "Name","BasicAttack" },
                { "Description", "Forward charge. No weapons." },
                { "Damage" , "1" }
            });
        }

        

        public int GetDamage(string attackMove)
        {
            foreach(JObject move in AttackMoves)
            {
                if (move["Name"].ToString() == attackMove) return int.Parse(move["Damage"].ToString());
            }
            return 0;
        }

        public void AttackCreature(Creature opponent,string attackMove = "BasicAttack")
        {
            ConsoleUtils.LogInfo("{0} is attempting {1} on {2}", Name, attackMove, opponent.Name);
            //int result = Rnd.Next(0, 10);
            if (GameUtils.Fortune())
            {
                ConsoleUtils.LogInfo("{0} attacks...", Name);
                
                if (GameUtils.Fortune())
                {
                    ConsoleUtils.LogSuccess("{0} managed to dodge the attack", opponent.Name);
                }
                else
                {
                    int damage = Attack+GetDamage(attackMove) - opponent.Defence;
                    if (damage > 0) opponent.TakeHit(damage);
                    else ConsoleUtils.LogWarning("{0} is immune to that attack and does not take any damage", opponent.Name);
                }
                
            }
            else Console.WriteLine("{0} missed.", Name);
            
        }

        public void GetStats()
        {
            foreach (var propertyInfo in GetType().GetProperties())
            {
                ConsoleUtils.LogOptions(propertyInfo.Name + " : {0}", propertyInfo.GetValue(this, null));
            }
        }

        public void UpdateStats()
        {
            foreach (var propertyInfo in GetType().GetProperties())
            {
                ConsoleUtils.LogInfo("Type : {0}", propertyInfo.GetType().ToString());
                string newValue = ConsoleUtils.GetFromConsole("{0} ({1}):", propertyInfo.Name, propertyInfo.GetValue(this, null));
                if (newValue.Trim() != "")
                {

                    propertyInfo.SetValue(this, newValue, null);
                }
                //ConsoleUtils.LogOptions(propertyInfo.Name + " : {0}", propertyInfo.GetValue(creature, null));
            }
        }

        public void TakeHit(int damage)
        {
            ConsoleUtils.LogDanger("{0} takes {1} damage", Name, damage);
            Health -= damage;
            if (Health <= 0) CreatureDies();
            else ConsoleUtils.LogWarning("{0} now has {1} health points left", Name, Health);
        }

        public void CreatureDies()
        {
            ConsoleUtils.LogDanger("{0} is now dead.", Name);
        }

        public bool IsAlive()
        {
            if (Health > 0) return true;
            else return false;
        }
        
    }
}
