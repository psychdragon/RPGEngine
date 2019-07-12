using System;
using System.Collections.Specialized;
using System.Text;
using System.Net;
using System.Threading;
using System.Reflection;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Web;
using Aze.Utilities;

namespace RPGEngine
{
    class WebUI
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> _responderMethod;

        const string EndPoint = "http://localhost:5050/";
        readonly World ThisWorld;


        public WebUI(World world)
        {
            ThisWorld = world;
            //Url = "http://" + Host + ":" + Port + EndPoint;

            string Url = EndPoint;
            
            if (!HttpListener.IsSupported)
            {
                throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");
            }


            Console.WriteLine("Listening on {0}", Url);
            _listener.Prefixes.Add(Url);
            _responderMethod = SendResponse;
            _listener.Start();
        }

        public void InvokeBrowser()
        {
            Console.Write("Open Browser?(y/n) -->");
            if (Console.ReadKey().Key == ConsoleKey.Y) OpenBrowser(EndPoint);
        }

        public static void OpenBrowser(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}")); // Works ok on windows
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);  // Works ok on linux
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url); // Not tested
            }
            else
            {
                Console.WriteLine("Platform Not Supported for browser invocation. Please manually open your web browser and navigate to {0}", url);
            }
        }

        public string SendResponse(HttpListenerRequest request)
        {
            ConsoleUtils.LogWarning("request received : {0}", request.RawUrl);
            if (request.RawUrl != "/") return ProcessRawUrl(request);
            if (request.HasEntityBody) return ProcessPostData(request);
            else return ProcessGetData(request);
        }

        public string ProcessRawUrl(HttpListenerRequest request)
        {
            return WebUtils.MergeStrings(WebUtils.ReadPage(request.RawUrl));
        }
        

        public string ProcessPostData(HttpListenerRequest request)
        {
            
            using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
            string text = WebUtility.UrlDecode(reader.ReadToEnd());
            reader.Close();
            
            string[] content = text.Split('&');

            JObject jObj = new JObject();
            foreach(string line in content)
            {
                string[] fields = line.Split('=');
                jObj.Add(fields[0],fields[1]);
            }

            WebUtils.InitPage();

            ExecuteMethod(jObj["btnClick"].ToString(), "");

            return WebUtils.Render("gameUI.html");



        }

        public string ProcessGetData(HttpListenerRequest request)
        {
            
            if (request.QueryString.Count == 0) return DefaultPage();
            return string.Format("QueryString: \n {0}", MergeNameValueCollection(request.QueryString));
        }

        public string MergeNameValueCollection(NameValueCollection input)
        {
            string output = "";
            foreach (string line in input)
            {
                output += "\t" + line + " : " + input[line] + "\n";
            }
            return output;
        }

        

        public string DefaultPage()
        {
            string[] content = WebUtils.ReadPage("gameUI.html");
            return WebUtils.MergeStrings(content);
        }

        

        

        public void ExecuteMethod(string method, string data)
        {
            ConsoleUtils.LogInfo("executing {0}...", method);

            Type t = this.GetType();
            try
            {
                t.InvokeMember(method, BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, this, new object[] { data });
            }
            catch (Exception e)
            {
                ConsoleUtils.LogDanger("Error: {0}", e.Message);
            }
            
            ConsoleUtils.LogSuccess("method {0} executed successfully.",method);
            
        }

        public void ListCreatures(string data)
        {
            CreatureSelectionMenu();
            /*
            foreach (Creature creature in ThisWorld.Creatures)
                WebUtils.GetStats(creature);
                */
            //ConsoleUtils.LogSuccess("List Creatures Here :"+data);
        }

        public void NavURL(string data)
        {
            
        }

        public void CreatureSelectionMenu()
        {
            foreach (Creature creature in ThisWorld.Creatures)
            {
                string line = string.Format("<a href='#' onclick='DisplayCreature({0})'>{1}</a>", ThisWorld.Creatures.IndexOf(creature), creature.Name);
                if (creature.Health <= 0) WebUtils.WebLog(line + "(dead)");
                else WebUtils.WebLog(line);
            }
        }

        public void Run()
        {

            ThreadPool.QueueUserWorkItem(o =>
            {
                Console.WriteLine("Webserver running...");
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem(c =>
                        {
                            try
                            {
                                if (!(c is HttpListenerContext ctx))
                                {
                                    return;
                                }

                                var rstr = _responderMethod(ctx.Request);
                                var buf = Encoding.UTF8.GetBytes(rstr);
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch
                            {
                                // ignored
                            }
                            finally
                            {
                                // always close the stream
                                /*if (ctx != null)
                                {
                                    ctx.Response.OutputStream.Close();
                                }*/
                            }
                        }, _listener.GetContext());
                    }
                }
                catch (Exception ex)
                {
                    // ignored
                    Console.WriteLine("Error : {0}", ex.Message);
                }
            });
        }

        public void StopWebUI()
        {
            _listener.Stop();
            _listener.Close();
        }
    }
}