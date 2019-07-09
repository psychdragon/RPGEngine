using System;
using System.Collections.Generic;
using System.Text;
using Aze.Utilities;

namespace RPGEngine
{
    static class GameUtils
    {
        readonly static Random Rnd = new Random();
        public static bool Fortune()
        {
            int result = Rnd.Next(0, 10);
            ConsoleUtils.LogInfo("Random fortune result : {0}", result);
            if (result > 5) return true;
            else return false;
        }
    }
}
