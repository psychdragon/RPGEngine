using System;


namespace RPGEngine
{
    class Program
    {
        
        static void Main(string[] args)
        {
            World ThisWorld = new World();
            Console.WriteLine("RPGENgine Console UI");
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
