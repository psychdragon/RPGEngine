using System;
using System.Collections.Generic;
using System.Text;
using Aze.Utilities;
using Newtonsoft.Json;


namespace RPGEngine
{

     
    class World
    {
        public List<Creature> Creatures = new List<Creature>();

        

        public void AddCreature(string name, string description, string spriteImageFile="NoImage")
        {
            Creatures.Add(new Creature(name, description, spriteImageFile));
        }

        public void ListCreatures()
        {
            CreatureSelectionMenu();

            string selection;
            do
            {
                selection = ConsoleUtils.GetFromConsole("Select creature to view stats(X to go back to main menu) : ");
                if (selection.Trim().ToLower() == "x") break;

                if (int.Parse(selection) < Creatures.Count) Creatures[int.Parse(selection)].GetStats();
                else ConsoleUtils.LogInfo("Invalid creature selection");
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

                if (int.Parse(selection) < Creatures.Count) Creatures[int.Parse(selection)].UpdateStats();
                else ConsoleUtils.LogInfo("Invalid creature selection");
            } while (selection.ToLower() != "x");

        }

        public void UpdateCreature(Creature creature)
        {
            
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
                ConsoleUtils.LogOptions("{0} - {1}", Creatures.IndexOf(creature), creature.Name);
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
