// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace UnbanPluginsCN;
class Program {

    private static string appGuid = "92f42221-51a5-4753-9e91-84aeea157d17";
    static NotifyIcon n = null;
    public static void Main(string[] args)
    {
        var mutex = new Mutex(true, appGuid, out var createdNew);

        if (!createdNew)
        {
            MessageBox.Show("Another copy of UnbanPluginCN is already running");
            return;
        }

        n = new NotifyIcon
        {
            Icon = SystemIcons.Application,
            Visible = true,
            Text = "UnbanPluginsCN",
            ContextMenuStrip = new()
        };
        n.ContextMenuStrip.Items.Add("Exit", null, delegate { n.Dispose(); Environment.Exit(0); });

        new Thread(() =>
        {
            Log("This is the beginning.");
            while (true)
            {
                try
                {
                    Thread.Sleep(500);
                    foreach (var x in Process.GetProcessesByName("Dalamud.Updater"))
                    {
                        if (!x.HasExited && x.MainModule != null)
                        {
                            Log($"Found process: {x.MainModule.FileName}");
                            var directory = Path.GetDirectoryName(x.MainModule.FileName);
                            if (directory != null)
                            {
                                Log($"Purging all bannedplugin.json files");
                                foreach (var f in Directory.GetFiles(directory, "bannedplugin.json", SearchOption.AllDirectories))
                                {
                                    if (Path.GetFileName(f) == "bannedplugin.json")
                                    {
                                        Log($"File: {f}");
                                        try
                                        {
                                            File.WriteAllText(f, "[]");
                                        }
                                        catch (Exception e)
                                        {
                                            Log(e.Message);
                                        }
                                    }
                                }
                                using var watcher = new FileSystemWatcher(directory);

                                watcher.NotifyFilter = NotifyFilters.Attributes
                                                     | NotifyFilters.CreationTime
                                                     | NotifyFilters.DirectoryName
                                                     | NotifyFilters.FileName
                                                     | NotifyFilters.LastWrite
                                                     | NotifyFilters.Security
                                                     | NotifyFilters.Size;

                                watcher.Changed += OnChanged;
                                watcher.Created += OnCreated;

                                watcher.Filter = "bannedplugin.json";
                                watcher.IncludeSubdirectories = true;
                                watcher.EnableRaisingEvents = true;
                                x.WaitForExit();
                                watcher.EnableRaisingEvents = false;
                                Log("Process terminated, awaiting next...");
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    Log(ex.Message);
                }
            }
        }).Start();
        Application.Run();
    }

    static void Log(string s)
    {
        
        Console.WriteLine(s);
        File.AppendAllText("UnbanPluginsCN.log", s + "\n");
    }

    private static void OnChanged(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Changed)
        {
            return;
        }
        Log($"Changed: {e.FullPath}");
        try
        {
            File.WriteAllText(e.FullPath, "[]");
        }
        catch (Exception ex)
        {
            Log(ex.Message);
        }
    }

    private static void OnCreated(object sender, FileSystemEventArgs e)
    {
        string value = $"Created: {e.FullPath}";
        Log(value);
        try
        {
            File.WriteAllText(e.FullPath, "[]");
        }
        catch (Exception ex)
        {
            Log(ex.Message);
        }
    }
}