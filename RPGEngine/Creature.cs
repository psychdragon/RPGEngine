using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Aze.Utilities;

namespace RPGEngine
{
    public class BasicAbility
    {
        public int FaceDirection { set; get; }
        public int VisionDistance { set; get; }
        public int VisionAngleRange { set; get; }
        public bool Flying { set; get; }

    }

    public class SpeciesModifier
    {
        public string SpeciesName { set; get; }
        public string ElementType { set; get; }
    }

    public class InventoryItem : Entity
    {
        public int OccupyingSpace { set; get; } = 1;
        public int MaxStorage { set; get; } = 0;

        public JObject UseModifyers { set; get; } = new JObject(); //What stats are modified when item is used.
        public JObject CarryModifers { set; get; } = new JObject(); //What stats are modified when item is carried.
        public bool SingleUse { set; get; } = true; //If set to true, item will cease to exist after use.
    }

    public class AttackMove
    {
        public string Name { set; get; }
        public string Description { set; get; }
        public int Mastery { set; get; }
        public int Damage { set; get; }

        public AttackMove(string name,string description,int damage)
        {
            Name = name;
            Description = description;
            Damage = damage;
            Mastery = 0;
        }
        
    }

    public class Creature : Entity
    {
        public string GivenName { set; get; }
        public string SpriteImageFile { set; get; }

        public int Health { set; get; }
        public int Level { set; get; }
        public int Attack { set; get; }
        public int Defence { set; get; }

        public BasicAbility Ability { set; get; } = new BasicAbility();
        public SpeciesModifier Species { set; get; } = new SpeciesModifier();
        public List<AttackMove> AttackMoves { set; get; } = new List<AttackMove>();
        public List<InventoryItem> Items { set; get; } = new List<InventoryItem>();


        public Creature(string name, string description, string spriteImageFile="NoImage", int health=100, int level=0, int attack=0, int defence=0)
        {
            Name = name;
            Description = description;
            SpriteImageFile = spriteImageFile;

            Health = health;
            Level = level;
            Attack = attack;
            Defence = defence;

            AttackMoves.Add(new AttackMove("BasicAttack", "Forward charge. No Weapons.", 1));
            
            
            
        }

        
        public void SetPosition(int x, int y, int z=0, int reality=0,int direction=8)
        {
            Position.X = x;//GameUtils.RandomInt(0, 500);
            Position.Y = y;// GameUtils.RandomInt(0, 500);
            Position.Z = z;
            Position.Reality = reality;
            if (direction > 7) Ability.FaceDirection = GameUtils.RandomInt(0, 8);
            else Ability.FaceDirection = direction;
            ConsoleUtils.LogInfo("{0} is now at location ({1},{2})", Name, Position.X, Position.Y);
        }

        public void Move(int speed,int direction=0)
        {
            Ability.FaceDirection += direction;
            Ability.FaceDirection %= 8;
            int[] DirForward = { 0,1,2 };
            int[] DirBackward = { 5, 6, 7 };
            int[] DirLeft = { 0, 3, 6 };
            int[] DirRight = { 2, 4, 7 };
            if (DirForward.Contains(Ability.FaceDirection)) Position.Y += speed;
            if (DirBackward.Contains(Ability.FaceDirection)) Position.Y -= speed;
            if (DirLeft.Contains(Ability.FaceDirection)) Position.X -= speed;
            if (DirRight.Contains(Ability.FaceDirection)) Position.X += speed;
            ConsoleUtils.LogInfo("{0} has moved {3} in the direction of {4} to location ({1},{2})", Name, Position.X, Position.Y,speed,Ability.FaceDirection);
        }



        public int GetDamage(string attackMove)
        {
            foreach(AttackMove move in AttackMoves)
            {
                if (move.Name == attackMove) return move.Damage;
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
