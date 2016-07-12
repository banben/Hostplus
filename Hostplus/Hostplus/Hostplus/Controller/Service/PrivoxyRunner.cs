using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Hostplus.Properties;
using System.IO;
using System.Net.NetworkInformation;
using System.Net;
using Microsoft.Win32;

namespace Hostplus.Controller
{
    class PrivoxyRunner
    {
        private Process _process;
        private static string temppath;
        private int _runningPort;

        static PrivoxyRunner()
        {
            temppath = Path.GetTempPath();
            try
            {
                FileManager.UncompressFile(temppath + "/hp_privoxy.exe", Resources.privoxy_exe);
                FileManager.UncompressFile(temppath + "/mgwz.dll", Resources.mgwz_dll);
                FileManager.UncompressFile(temppath + "/user-rule.txt", Resources.user_rule_txt);
            }
            catch { }
        }

        public int RunningPort
        {
            get
            {
                return _runningPort;
            }
        }

        private int GetFreePort()
        {
            int defaultPort = 13787;
            try
            {
                IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
                IPEndPoint[] tcpEndPoints = properties.GetActiveTcpListeners();

                List<int> usedPorts = new List<int>();
                foreach (IPEndPoint endPoint in IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners())
                {
                    usedPorts.Add(endPoint.Port);
                }
                for (int port = defaultPort; port <= 65535; port++)
                {
                    if (!usedPorts.Contains(port))
                    {
                        return port;
                    }
                }
            }
            catch
            {
                // in case access denied
                return defaultPort;
            }
            throw new Exception("No free port found.");
        }

        public void Start()
        {
            if (_process == null)
            {
                Process[] existingPrivoxy = Process.GetProcessesByName("hp_privoxy");
                foreach (Process p in existingPrivoxy)
                {
                    try
                    {
                        p.Kill();
                        p.WaitForExit();
                    }
                    catch { }
                }
                string privoxyConfig = Resources.privoxy_conf;
                _runningPort = this.GetFreePort();
                privoxyConfig = privoxyConfig.Replace("__PRIVOXY_BIND_PORT__", _runningPort.ToString());
                privoxyConfig = privoxyConfig.Replace("__TEMP_PATH__", temppath);
                FileManager.ByteArrayToFile(temppath + "/privoxy_conf.txt", System.Text.Encoding.UTF8.GetBytes(privoxyConfig));

                if (!(temppath.EndsWith("\\") || temppath.EndsWith("/")))
                {
                    temppath = temppath + "\\";
                }
                _process = new Process();
                // Configure the process using the StartInfo properties.
                _process.StartInfo.FileName = temppath + "hp_privoxy.exe";
                _process.StartInfo.Arguments = " \"" + temppath + "privoxy_conf.txt\"";
                _process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                _process.StartInfo.UseShellExecute = true;
                _process.StartInfo.CreateNoWindow = true;
                //_process.StartInfo.RedirectStandardOutput = true;
                //_process.StartInfo.RedirectStandardError = true;
                _process.Start();
            }
            //RefreshTrayArea();
        }

        public void AutoStart()
        {
            FileManager.UncompressFile(temppath + "/hp_auto_startup.exe", Resources.hp_auto_startup_exe);
            try
            {
                RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                rkApp.SetValue("Hostplus", temppath + "hp_auto_startup.exe");
            }
            catch { }
        }

        public void DisableAutoStart()
        {
            try
            {
                RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                rkApp.DeleteValue("Hostplus", false);
            }
            catch { }
        }

        public void Stop()
        {
            if (_process != null)
            {
                try
                {
                    _process.Kill();
                    _process.WaitForExit();
                }
                catch { }
                _process = null;
            }
        }
    }
}
