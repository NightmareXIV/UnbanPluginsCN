using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnbanPluginsCN
{
    internal class Service : ServiceBase
    {
        protected override void OnStart(string[] args)
        {
            Core.Log($"Starting service");
            Core.Start(false);
        }

        protected override void OnStop()
        {
            Core.Log($"Stopping service");
            Core.Stop();
        }
    }
}
