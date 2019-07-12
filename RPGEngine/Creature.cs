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
        public bool Hover { set; get; }

    }

    public class SpeciesModifier
    {
        public string SpeciesName { set; get; } = "Unknown";
        public string ElementType { set; get; } = "Unknown";
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
        public string Name { set; get; } = "Unknown";
        public string Description { set; get; } = "Unknown";
        public int Mastery { set; get; } = 0;
        public int Damage { set; get; } = 0;

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
        public string GivenName { set; get; } = "Unknown";
        

        public int Health { set; get; } = 100;
        public int Level { set; get; } = 0;
        public int Attack { set; get; } = 0;
        public int Defence { set; get; } = 0;

        public int Speed { set; get; } = 1;

        public BasicAbility Ability { set; get; } = new BasicAbility();
        public SpeciesModifier Species { set; get; } = new SpeciesModifier();
        public List<AttackMove> AttackMoves { set; get; } = new List<AttackMove>();
        


        public void InitCreature(string name, string description, string spriteImageFile="NoImage", int health=100, int level=0, int attack=0, int defence=0)
        {
            Name = name;
            GivenName = name;
            Description = description;
            SpriteImageFile = spriteImageFile;

            Health = health;
            Level = level;
            Attack = attack;
            Defence = defence;

            AttackMoves.Add(new AttackMove("BasicAttack", "Forward charge. No Weapons.", 1));
            
        }

        
        

        public void SetFaceDirection(int direction=8)
        {
            if (direction > 7) Ability.FaceDirection = GameUtils.RandomInt(0, 8);
            else Ability.FaceDirection = direction;
        }

        public void Move(int turnDirection=0, bool hover=false)
        {
            string moveDir;
            if (!hover)
            {
                Ability.FaceDirection += turnDirection;
                Ability.FaceDirection %= 8;
                moveDir = Enum.GetName(typeof(MoveDirection), Ability.FaceDirection);
            } else moveDir = Enum.GetName(typeof(MoveDirection),turnDirection);

            if (moveDir.Contains("North")) Position.Y += Speed;
            if (moveDir.Contains("South")) Position.Y -= Speed;
            if (moveDir.Contains("East")) Position.X -= Speed;
            if (moveDir.Contains("West")) Position.X += Speed;
            if (moveDir.Contains("Up")) Position.Z += Speed;
            if (moveDir.Contains("Down")) Position.Z -= Speed;

            ConsoleUtils.LogInfo("{0} has moved {4} in the direction of {5} to location ({1},{2},{3})", Name, Position.X, Position.Y, Position.Z, Speed,Ability.FaceDirection);
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
