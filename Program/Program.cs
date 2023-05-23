// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace UnbanPluginsCN;
class Program
{
    public static void Main(string[] args)
    {
        if (Environment.UserInteractive)
        {
            Core.Log($"Program is running in user interactive mode");
            Core.Start(true);
        }
        else
        {
            Core.Log($"Program is running in service mode");
            ServiceBase.Run(new Service());
        }
    }
}