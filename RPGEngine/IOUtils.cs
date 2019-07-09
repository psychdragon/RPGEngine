using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Aze.Utilities
{
    static class IOUtils
    {
        public static JObject AppSettings = new JObject { };
        private static readonly string SettingsFile = "AppSettings.json";

        public static void LoadSettings()
        {
            string jsonstr="";
            string[] settings = ReadFile(SettingsFile);
            foreach (string line in settings) jsonstr += line;
            AppSettings = StringToJson(jsonstr);
        }

        public static void SaveSettings()
        {
            List<string> newSettings = new List<string>
            {
                AppSettings.ToString()
            };
            SaveFile(SettingsFile,newSettings.ToArray());
        }

        public static void EditSettings()
        {
            foreach(JProperty property in AppSettings.Properties())
            {
                string input = ConsoleUtils.GetFromConsole("{0} ({1}) : ", property.Name, property.Value);
                if (input.Trim() != "") AppSettings[property.Name] = input;
            }
        }

        public static void AddSetting()
        {
            string key = ConsoleUtils.GetFromConsole("Key :");
            string value = ConsoleUtils.GetFromConsole("Value :");

            if (key.Trim() != "") AppSettings[key] = value;
        }

        public static void DisplaySettings()
        {
            foreach (JProperty property in AppSettings.Properties())
            {
                ConsoleUtils.LogInfo("{0} : {1}", property.Name, property.Value);
                
            }
        }

        public static string[] ReadFile(string fileName)
        {
            try
            {
                return File.ReadAllLines(fileName);
            } catch (Exception ex)
            {
                ConsoleUtils.LogDanger("Error : {0}", ex.Message);
                return null;
            }
            
            
        }

        public static void SaveFile(string filename, string[] content)
        {
            try
            {
                File.WriteAllLines(filename, content);
            }catch(Exception ex)
            {
                ConsoleUtils.LogDanger("Error : {0}", ex.Message);
            }

        }

        public static JObject StringToJson(string jsonstr)
        {
            try
            {
                return JsonConvert.DeserializeObject<JObject>(jsonstr);

            }catch (Exception ex)
            {
                ConsoleUtils.LogDanger("Error : {0}", ex.Message);
                return null;
            }
        }

        
    }
}
