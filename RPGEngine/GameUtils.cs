﻿using System;
using System.Collections.Generic;
using System.Text;
using Aze.Utilities;

namespace RPGEngine
{
    static class GameUtils
    {
        readonly static Random Rnd = new Random();

        //Fortune function simply returns a true or false randomly similar to a coin toss.
        public static bool Fortune()
        {
            int result = Rnd.Next(0, 10);
            ConsoleUtils.LogInfo("Random fortune result : {0}", result);
            if (result > 5) return true;
            else return false;
        }

        //Returns a random integer for use in random positioning in X,Y,Z coordinates
        public static int RandomInt(int min, int max)
        {
            return Rnd.Next(min, max);
        }

        public static void GetListOfStats(string ListName,object ListItems, int Indent)
        {
            ConsoleUtils.LogOptions(ListName + " --> {0}",(ListItems as List<AttackMove>)[0].GetType());
            foreach(AttackMove line in (ListItems as List<AttackMove>))
            {
                GetStats(line, Indent + 1);
            }
        }

        //Displays all the properties of the particular object sent to it. Object can be a creature, player or any item in the simulated world
        public static void GetStats(object Element,int Indent=0)
        {
            foreach (var propertyInfo in Element.GetType().GetProperties())
            {
                var CurrentVal = propertyInfo.GetValue(Element, null);
                if (CurrentVal != null)
                {
                    string ValueType = CurrentVal.GetType().ToString();
                    ConsoleUtils.Indent(Indent, "  ");
                    if (ValueType == "System.String" || ValueType == "System.Int32" || ValueType == "System.Boolean" || ValueType == "System.Single")
                        ConsoleUtils.LogOptions(propertyInfo.Name + " : {0}", propertyInfo.GetValue(Element, null));
                    else if (ValueType.Contains("[RPGEngine.AttackMove]"))
                    {
                        ConsoleUtils.LogWarning(propertyInfo.Name + " -- ");
                        foreach (AttackMove lineItem in (CurrentVal as List<AttackMove>))
                            GetStats(lineItem, Indent + 1);
                    }
                    else if (ValueType.Contains("[RPGEngine.InventoryItem]"))
                    {
                        ConsoleUtils.LogWarning(propertyInfo.Name + " -- ");
                        foreach(InventoryItem lineItem in (CurrentVal as List<InventoryItem>))
                            GetStats(lineItem, Indent + 1);
                    }
                    else
                    {
                        ConsoleUtils.LogWarning(propertyInfo.Name + " -- ");
                        GetStats(CurrentVal, Indent + 1);
                    }
                }
            }
        }

        //Runs through all the stats of any object to be edited. Default value is taken when empty string is provided
        public static void UpdateStats(object Element, int Indent=0)
        {
            foreach (var propertyInfo in Element.GetType().GetProperties())
            {
                var CurrentVal = propertyInfo.GetValue(Element, null);
                ConsoleUtils.Indent(Indent,"  ");
                //ConsoleUtils.LogWarning(CurrentVal.GetType().ToString());
                string newValue = ConsoleUtils.GetFromConsole("{0} ({1}) : ", propertyInfo.Name, CurrentVal);
                if (newValue.Trim() != "")
                {
                    
                    if (CurrentVal.GetType().ToString() == "System.String") propertyInfo.SetValue(Element, newValue, null);
                    else if (CurrentVal.GetType().ToString() == "System.Int32") propertyInfo.SetValue(Element, int.Parse(newValue), null);
                    
                }

            }
        }
    }
}
