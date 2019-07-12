using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace Aze.Utilities
{
    static class WebUtils
    {
        private static string Outpage;
        private static XmlDocument gameBoard;

        public static string Render(string pageFile)
        {
            string[] content = ReplaceStrings(ReadPage(pageFile),"{InfoArea}", Outpage);
            
            content = ReplaceStrings(content, "{gameBoard}", Outpage);
            return MergeStrings(content);
            
        }

        public static string XmlNodeString(string tag, Dictionary<string, string> attributes = null,string content="")
        {
            string attributeList = "";
            foreach(KeyValuePair<string,string> line in attributes)
            {
                attributeList += string.Format("{0}='{1}' ", line.Key, line.Value);
            }

            return string.Format("<{0} {1} >{2}</{0}>", tag,attributeList,content);
        }

        public static void InitBoard(int width, int height)
        {
            InitPage();
            Dictionary<string, string> attributes = new Dictionary<string, string>()
            {
                {"height",height.ToString() },
                {"width", width.ToString() }
            };

            string rectXml = XmlNodeString("rect", attributes);

            attributes.Add("style", "fill:rgb(0,0,255);stroke-width:10;stroke:rgb(0,0,0)");

            string svgXml = XmlNodeString("svg", attributes, rectXml);
            Outpage += svgXml;
            
            /*
             <svg width="400" height="400">
            <rect width="400" height="400" style="fill:rgb(0,0,255);stroke-width:10;stroke:rgb(0,0,0)" />
        </svg>
             */

        }

        

        public static XmlAttribute NewAttribute(string key,string value)
        {
            XmlAttribute attribute = gameBoard.CreateAttribute(key);
            attribute.Value = value;
            return attribute;
        }

        public static string[] ReplaceStrings(string[] content,string oldString, string newString)
        {
           for (int i = 0; i < content.Length; i++)
            {
                content[i] = content[i].Replace(oldString, newString);
            }
            return content;
        }

        public static string MergeStrings(string[] content)
        {
            string output = "";
            foreach (string line in content) output += line;
            return output;
        }

        public static string[] ReadPage(string filename)
        {
            if (filename[0] != '\\') filename = "\\"+filename;
            return IOUtils.ReadFile("WebAssets" + filename);
        }

        public static void InitPage()
        {
            Outpage = "";
        }

        public static void WebIndent(int spaces,string filler=" ")
        {
            for (int i = 0; i < spaces; i++) Outpage += filler;
        }

        public static void WebLog(string text,object args=null)
        {
            Outpage += string.Format(text, args);
            Outpage += "<br>";
        }



        public static void GetStats(object Element, int Indent = 0)
        {
           
            foreach (var propertyInfo in Element.GetType().GetProperties())
            {
                var CurrentVal = propertyInfo.GetValue(Element, null);
                if (CurrentVal != null)
                {
                    ConsoleUtils.LogInfo("{1} : {2}", CurrentVal, propertyInfo.Name);
                    string ValueType = CurrentVal.GetType().ToString();
                    WebIndent(Indent, "  ");
                    if (ValueType == "System.String" || ValueType == "System.Int32" || ValueType == "System.Boolean" || ValueType == "System.Single")
                        WebLog(propertyInfo.Name + " : {0}", propertyInfo.GetValue(Element, null));
                    
                    else if (ValueType.Contains("[RPGEngine.AttackMove]"))
                    {
                        WebLog(propertyInfo.Name + " -- ");

                        //foreach (AttackMove lineItem in (CurrentVal as List<AttackMove>))
                         //   GetStats(lineItem, Indent + 1);
                    }
                    else if (ValueType.Contains("[RPGEngine.InventoryItem]"))
                    {
                        WebLog(propertyInfo.Name + " -- ");
                        //foreach (InventoryItem lineItem in (CurrentVal as List<InventoryItem>))
                         //   GetStats(lineItem, Indent + 1);
                    }
                    else
                    {
                        WebLog(propertyInfo.Name + " -- ");
                        GetStats(CurrentVal, Indent + 1);
                    }
                }
            }
            
        }
    }
}
