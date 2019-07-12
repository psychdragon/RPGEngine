using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection;
using Aze.Utilities;


namespace RPGEngine
{
    

    

    class ConsoleUI
    {
        
        World ThisWorld;
        WebUI webUI;

        public ConsoleUI()
        {
            IOUtils.LoadSettings();
            InitMenu();
            
        }

        public void Run(World world)
        {
            ThisWorld = world;

            LoadCreatures();
            InitWorld();
            webUI = new WebUI(ThisWorld);
            webUI.Run();
            /*
            ThisWorld.AddCreature("Wolf", "Simple wolf. nothing fancy");
            ThisWorld.AddCreature("Dog", "Simple dog. nothing fancy");
            */
            
            ConsoleUtils.DisplayMenu();
            CommandWait();
        }

        private void CommandWait()
        {
            string Command;
            while (true)
            {
                Command = ConsoleUtils.GetFromConsole("Command -->");
                ExecuteMethod(ConsoleUtils.SearchMethod(Command));
            }
        }

        public void DisplayMenu()
        {
            ConsoleUtils.DisplayMenu();
        }

        

        public void ExitProgram()
        {
            ConsoleUtils.LogSuccess("Goodbye!");
            System.Environment.Exit(1);
        }

        public void ExecuteMethod(string method)
        {
            ConsoleUtils.LogInfo("executing {0}...", method);
            Type t = this.GetType();
            try
            {
                t.InvokeMember(method, BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, this, null);
            }
            catch (Exception e)
            {
                ConsoleUtils.LogDanger("{1} Error:{0}\nThis feature is still under development", e.Message, method);
            }

        }


        

        

        public void AddCreature()
        {
            string name = ConsoleUtils.GetFromConsole("Creature Name : ");
            string description = ConsoleUtils.GetFromConsole("Description : ");
            string spriteImage = ConsoleUtils.GetFromConsole("Sprite Image Path : ");
            ThisWorld.AddCreature(name,description,spriteImage);
        }

        public void BattleTest()
        {
            ThisWorld.CreatureSelectionMenu();
            string attacker = ConsoleUtils.GetFromConsole("Choose Attacker(0-{0}) : ",ThisWorld.Creatures.Count-1);
            string defender = ConsoleUtils.GetFromConsole("Choose Target(0-{0}) : ", ThisWorld.Creatures.Count - 1);
            AttackTest(attacker, defender);
        }

        public void AttackTest(string attacker, string defender)
        {
            Creature Attacker, Defender;
            if (int.Parse(attacker) < ThisWorld.Creatures.Count && int.Parse(defender) < ThisWorld.Creatures.Count)
            {
                Attacker = ThisWorld.Creatures[int.Parse(attacker)];
                Defender = ThisWorld.Creatures[int.Parse(defender)];
                if (Attacker.IsAlive()) ThisWorld.Strike(Attacker, Defender);
                else ConsoleUtils.LogDanger("{0} is already dead.",Attacker.Name);
                if (!Defender.IsAlive()) ConsoleUtils.LogDanger("{0} is already dead.", Defender.Name);
            }
            else
            {
                Console.WriteLine("Attacker/Defender not found");
                return;
            }
            string nextAction = ConsoleUtils.GetFromConsole("Again(A)/Reverse(R)/Til Death(D)/Stop(S)? :").ToLower();
            if (nextAction == "a") AttackTest(attacker, defender);
            else if (nextAction == "r") AttackTest(defender, attacker);
            else if (nextAction == "d") ThisWorld.DeathDuel(Attacker, Defender);
            
            
        }

        

        public void ListCreatures()
        {
            ThisWorld.ListCreatures();
        }

        public void SaveCreatures()
        {
            List<string> creatureList = new List<string>();
            foreach (Creature creature in ThisWorld.Creatures)
            {
                string creaturestr = JsonConvert.SerializeObject(creature);
                creatureList.Add(creaturestr);
            }
            
            try
            {
                IOUtils.SaveFile(IOUtils.AppSettings["CreatureList"].ToString(), creatureList.ToArray());
            } catch (Exception Ex)
            {
                ConsoleUtils.LogDanger("Error: {0}", Ex.Message);
            } finally
            {
                ConsoleUtils.LogSuccess("Creature list has been successfully saved");
            }
            
            
        }

        public void LoadCreatures()
        {
            //TODO
            string[] creatureList= IOUtils.ReadFile(IOUtils.AppSettings["CreatureList"].ToString());
            ThisWorld.LoadCreatures(creatureList);


        }

        public void AddSetting()
        {
            IOUtils.AddSetting();
        }

        public void ListSettings()
        {
            IOUtils.DisplaySettings();
        }

        public void SaveSettings()
        {
            IOUtils.SaveSettings();
        }

        public void LoadSettings()
        {
            IOUtils.LoadSettings();
        }

        public void EditSettings()
        {
            IOUtils.EditSettings();
        }

        public void EditCreature()
        {
            ThisWorld.EditCreature();
        }

        public void MoveTest()
        {
            foreach(Creature creature in ThisWorld.Creatures)
            {
                creature.Move(GameUtils.RandomInt(0, 8));
            }
        }

        public void InitWorld()
        {
            foreach(Creature creature in ThisWorld.Creatures)
            {
                creature.SetPosition(GameUtils.RandomInt(0, ThisWorld.Borders.X), GameUtils.RandomInt(0, ThisWorld.Borders.Y));
                creature.SetFaceDirection();
            }
        }

        public void OpenBrowser()
        {
            webUI.InvokeBrowser();
        }

        private void InitMenu()
        {
            ConsoleUtils.AddMenuItem("1", "Display This Menu", "DisplayMenu");
            ConsoleUtils.AddMenuItem("2", "Add Creature", "AddCreature");
            ConsoleUtils.AddMenuItem("3", "List Creatures", "ListCreatures");
            ConsoleUtils.AddMenuItem("4", "Save Creatures", "SaveCreatures");
            ConsoleUtils.AddMenuItem("5", "Edit Creature", "EditCreature");
            ConsoleUtils.AddMenuItem("6", "Battle Test", "BattleTest");
            ConsoleUtils.AddMenuItem("7", "Move Test", "MoveTest");
            ConsoleUtils.AddMenuItem("8", "Open Browser", "OpenBrowser");
            /*
            ConsoleUtils.AddMenuItem("6", "Add Setting", "AddSetting");
            ConsoleUtils.AddMenuItem("7", "Edit Settings", "EditSettings");
            ConsoleUtils.AddMenuItem("8", "List Settings", "ListSettings");
            ConsoleUtils.AddMenuItem("9", "Save Settings", "SaveSettings");
            */
            ConsoleUtils.AddMenuItem("x", "Exit Program", "ExitProgram");
        }

        

    }
}

