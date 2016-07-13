using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Hostplus.Properties;
using System.Runtime.InteropServices;

namespace Hostplus.Controller
{
    class HostsChecker
    {
        public static string HOST_FILE = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers/etc/hosts");
        
        [DllImport("dnsapi.dll", EntryPoint = "DnsFlushResolverCache")]
        private static extern UInt32 DnsFlushResolverCache();

        public void Check() 
        {
            int counter = 0;
            string line;
            bool flag = false;
            if (File.Exists(HOST_FILE))
            {
                System.IO.StreamReader file =
                    new System.IO.StreamReader(HOST_FILE);
                while ((line = file.ReadLine()) != null && counter != 50)
                {
                    if (line.Contains("Delete this line to custom hosts configuration"))
                        flag = true;
                    counter++;
                }
                file.Close();
                if (flag)
                    Restore();
            }
            else
                Restore();
        }

        public void Restore()
        {
            try
            {
                if (File.Exists(HOST_FILE))
                    File.Delete(HOST_FILE);
                FileManager.UncompressFile(HOST_FILE, Resources.hosts);
            }
            catch { }
            try
            {
                DnsFlushResolverCache();
            }
            catch { }
        }
    }
}
