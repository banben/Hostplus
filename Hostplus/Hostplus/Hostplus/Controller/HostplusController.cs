using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hostplus.Controller
{
    class HostplusController
    {
        public void Enable()
        {
            PrivoxyRunner privoxyRunner = new PrivoxyRunner();
            privoxyRunner.Start();
            privoxyRunner.AutoStart();
            HostsChecker hostsChecker = new HostsChecker();
            hostsChecker.Check();
            SystemProxy.Update(true);
        }
        public void RestoreHosts()
        {
            HostsChecker hostsChecker = new HostsChecker();
            hostsChecker.Restore();
        }
        public void Disable()
        {
            PrivoxyRunner privoxyRunner = new PrivoxyRunner();
            privoxyRunner.Stop();
            privoxyRunner.DisableAutoStart();
            SystemProxy.Update(false);
        }
    }
}
