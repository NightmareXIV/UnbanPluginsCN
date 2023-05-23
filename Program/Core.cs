using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnbanPluginsCN
{
    internal static class Core
    {
        private static string appGuid = "92f42221-51a5-4753-9e91-84aeea157d17";
        static NotifyIcon n = null;
        static HashSet<string> ProcessedPathes = new();
        static volatile bool Run = false;
        static FileSystemWatcher watcher;

        internal static void Start(bool applicationRun)
        {
            var mutex = new Mutex(true, appGuid, out var createdNew);

            if (!createdNew)
            {
                MessageBox.Show("Another copy of UnbanPluginCN is already running");
                return;
            }

            Run = true;

            if (applicationRun)
            {
                n = new NotifyIcon
                {
                    Icon = SystemIcons.Application,
                    Visible = true,
                    Text = "UnbanPluginsCN",
                    ContextMenuStrip = new()
                };
                n.ContextMenuStrip.Items.Add("Exit", null, delegate { n.Dispose(); Environment.Exit(0); });
            }

            new Thread(() =>
            {
                Log("This is the beginning.");
                while (Run)
                {
                    try
                    {
                        Thread.Sleep(500);
                        foreach (var x in Process.GetProcessesByName("Dalamud.Updater"))
                        {
                            if (!x.HasExited && x.MainModule != null)
                            {
                                var path = Path.GetDirectoryName(x.MainModule.FileName);
                                if (!ProcessedPathes.Contains(path))
                                {
                                    ProcessedPathes.Add(path);
                                    BeginWatcher(path);
                                }
                            }
                        }
                        foreach (var x in Process.GetProcessesByName("XIVLauncherCN"))
                        {
                            if (!x.HasExited && x.MainModule != null)
                            {
                                var path = Path.GetDirectoryName(x.MainModule.FileName);
                                var parent1 = Directory.GetParent(path);
                                var parent2 = parent1 != null ? Directory.GetParent(parent1.FullName) : null;
                                if (parent2 != null && File.Exists(Path.Combine(parent2.FullName, "XIVLauncherCN.exe")))
                                {
                                    path = parent2.FullName;
                                }
                                else if (parent1 != null && File.Exists(Path.Combine(parent1.FullName, "XIVLauncherCN.exe")))
                                {
                                    path = parent1.FullName;
                                }
                                if (!ProcessedPathes.Contains(path))
                                {
                                    ProcessedPathes.Add(path);
                                    BeginWatcher(path);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log(ex.Message);
                    }
                }
                Log($"Stopped thread");
                if (watcher != null)
                {
                    watcher.EnableRaisingEvents = false;
                }
            }).Start();
            if(applicationRun) System.Windows.Forms.Application.Run();
        }

        internal static void Stop()
        {
            Run = false;
        }

        static void BeginWatcher(string directory)
        {
            if (directory != null)
            {
                Log($"Starting watcher on {directory}");
                Log($"processing all bannedplugin.json and cheatplugin.json files");
                foreach (var f in Directory.GetFiles(directory, "*.json", SearchOption.AllDirectories))
                {
                    if (Path.GetFileName(f) == "bannedplugin.json")
                    {
                        Log($"File: {f}");
                        Scramble(f, true);
                    }
                    if (Path.GetFileName(f) == "cheatplugin.json")
                    {
                        Log($"File: {f}");
                        Scramble(f, false);
                    }
                }
                watcher = new FileSystemWatcher(directory);

                watcher.NotifyFilter = NotifyFilters.Attributes
                                        | NotifyFilters.CreationTime
                                        | NotifyFilters.DirectoryName
                                        | NotifyFilters.FileName
                                        | NotifyFilters.LastWrite
                                        | NotifyFilters.Security
                                        | NotifyFilters.Size;

                watcher.Changed += OnChanged;
                watcher.Created += OnCreated;

                watcher.Filter = "*.json";
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;
            }
        }

        internal static void Log(string s)
        {
            try
            {
                Console.WriteLine(s);
            }
            catch(Exception) { }
            File.AppendAllText(Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), $"UnbanPluginsCN.log"), s + "\n");
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Log($"Changed: {e.FullPath}");
            if (e.Name.EndsWith("bannedplugin.json", StringComparison.OrdinalIgnoreCase))
            {
                Log($"Is bannedplugin file, processing...");
                Scramble(e.FullPath, true);

            }
            if (e.Name.EndsWith("cheatplugin.json", StringComparison.OrdinalIgnoreCase))
            {
                Log($"Is cheatplugin file, processing...");
                Scramble(e.FullPath, false);

            }
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            Log(value);
            if (e.Name.EndsWith("bannedplugin.json", StringComparison.OrdinalIgnoreCase))
            {
                Log($"Is bannedplugin file, processing...");
                Scramble(e.FullPath, true);
            }
            if (e.Name.EndsWith("cheatplugin.json", StringComparison.OrdinalIgnoreCase))
            {
                Log($"Is cheatplugin file, processing...");
                Scramble(e.FullPath, false);
            }
        }

        static string ScrambleFallbackJson(int num)
        {
            List<string> str = new();
            for (int i = 0; i < num; i++)
            {
                str.Add($"{{\"Name\": \"{Utils.RandomString(64)}\", \"AssemblyVersion\": \"{Utils.RandomNumber}.{Utils.RandomNumber}.{Utils.RandomNumber}.{Utils.RandomNumber}\"}}");
            }
            return $"[{string.Join(",", str)}]";
        }

        static void Scramble(string fullPath, bool isConditionalPurge)
        {
            int numPlugins = 1;
            try
            {
                var data = File.ReadAllText(fullPath);
                numPlugins = data.Split(new string[] { "\"Name\"" }, StringSplitOptions.None).Length - 1;
                if (numPlugins <= 0) numPlugins = 1;
                var doReplace = !isConditionalPurge || data.ContainsAny(StringComparison.OrdinalIgnoreCase, "6f3f2d2be3f289693a94a582558a88753f809a7dbc9f6061f488174aea549e9b", "Splatoon");
                if (doReplace)
                {
                    var json = JsonConvert.DeserializeObject<List<BannedPlugin>>(data);
                    for (int i = 0; i < json.Count; i++)
                    {
                        json[i].Name = Utils.RandomString(64);
                    }
                    File.WriteAllText(fullPath, JsonConvert.SerializeObject(json, Formatting.Indented, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore}));
                    Log($"Writing fancy json back");
                }
            }
            catch (Exception ex)
            {
                Log($"An error has occurred, trying fallback method ({numPlugins}): {ex.Message}\n{ex.StackTrace}");
                try
                {
                    File.WriteAllText(fullPath, ScrambleFallbackJson(numPlugins));
                    Log($"Writing fallback json...");
                }
                catch (Exception ex2)
                {
                    Log($"During fallback operation, additional errors has occurred: {ex2.Message}\n{ex2.StackTrace}");
                }
            }
        }
    }
}
