using System;


namespace RPGEngine
{
    class Program
    {
        
        static void Main(string[] args)
        {
            World ThisWorld = new World();
            Console.WriteLine("RPGENgine - Created by Shahril Azwin Zainul Abidin (PsychDragon)");
            foreach(string arg in args)
            {
                Console.WriteLine("- {0}", arg);
            }
            Console.WriteLine("-------------------------------------------------------------");
                ConsoleUI consoleUI = new ConsoleUI();
            consoleUI.Run(ThisWorld);
        }
    }
}
