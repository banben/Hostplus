using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hostplus.Properties;
using Hostplus.Controller;
using System.Security.Principal;
using System.Diagnostics;
using Hostplus.View;
using System.Runtime.InteropServices;
using System.Threading;

namespace Hostplus
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, "Hostplus", out createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                    bool administrativeMode = principal.IsInRole(WindowsBuiltInRole.Administrator);

                    if (!administrativeMode)
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.Verb = "runas";
                        startInfo.FileName = Application.ExecutablePath;
                        try
                        {
                            Process.Start(startInfo);
                        }
                        catch
                        {
                            return;
                        }
                        return;
                    }
                    ProcessIcon processIcon = new ProcessIcon();
                    processIcon.Display();

                    Application.Run();
                }
                else
                {
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id)
                        {
                            SetForegroundWindow(process.MainWindowHandle);
                            break;
                        }
                    }
                }
            }
            
        }
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
