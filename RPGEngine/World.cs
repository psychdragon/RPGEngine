using System;
using System.Collections.Generic;
using System.Text;
using Aze.Utilities;
using Newtonsoft.Json;


namespace RPGEngine
{
    class WorldBorders
    {
        public int X { set; get; } = 50;
        public int Y { set; get; } = 50;
        public int Z { set; get; } = 50;
        public int Reality { set; get; } = 0;
    }
     
    class World
    {
        public List<Creature> Creatures = new List<Creature>();
        public WorldBorders Borders = new WorldBorders();
        

        public void AddCreature(string name, string description, string spriteImageFile="NoImage")
        {
            Creature newCreature = new Creature();
            newCreature.InitCreature(name, description, spriteImageFile);
            Creatures.Add(newCreature);
            Creatures[Creatures.Count - 1].SetPosition(GameUtils.RandomInt(0, Borders.X), GameUtils.RandomInt(0, Borders.Y));
        }

        public void ListCreatures()
        {
            CreatureSelectionMenu();
            string selection;
            do
            {
                selection = ConsoleUtils.GetFromConsole("Select creature to view stats(X to go back to main menu) : ");
                if (selection.Trim().ToLower() == "x") break;
                try
                {
                    if (int.Parse(selection) < Creatures.Count) GameUtils.GetStats(Creatures[int.Parse(selection)]);
                    else ConsoleUtils.LogInfo("Invalid creature selection");
                }
                catch
                {
                    continue;
                }
                
            } while (selection.ToLower() != "x");

        }

        public void EditCreature()
        {
            CreatureSelectionMenu();
            string selection;
            do
            {
                selection = ConsoleUtils.GetFromConsole("Select creature to edit(X to go back to main menu) : ");
                if (selection.Trim().ToLower() == "x") break;
                try
                {
                    if (int.Parse(selection) < Creatures.Count) GameUtils.UpdateStats(Creatures[int.Parse(selection)]);
                    else ConsoleUtils.LogInfo("Invalid creature selection");
                }
                catch
                {
                    continue;
                }
                
            } while (selection.ToLower() != "x");

        }

        

       

        public void LoadCreatures(string[] creatureList)
        {
            Creatures = new List<Creature>();
            foreach(string creature in creatureList)
            {
                Creatures.Add(JsonConvert.DeserializeObject<Creature>(creature));
            }
        }

        public void CreatureSelectionMenu()
        {
            foreach (Creature creature in Creatures)
            {
                string line = string.Format("{0} - {1}", Creatures.IndexOf(creature), creature.Name);
                if (creature.Health <= 0) ConsoleUtils.LogDanger(line + "(dead)");
                else ConsoleUtils.LogInfo(line);
            }
        }

        public void Strike(Creature Attacker, Creature Defender)
        {

            Attacker.AttackCreature(Defender);
        }

        public void DeathDuel(Creature Attacker, Creature Defender)
        {
            {
                do
                {
                    Strike(Attacker, Defender);
                    if (Defender.IsAlive()) Strike(Defender, Attacker);
                } while (Attacker.IsAlive() && Defender.IsAlive());
            }
        }
    }
}
