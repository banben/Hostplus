using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace hp_auto_startup
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string temppath;
            Process _process;
            temppath = Path.GetTempPath();
            try
            {
                _process = new Process();
                _process.StartInfo.FileName = temppath + "hp_privoxy.exe";
                _process.StartInfo.Arguments = " \"" + temppath + "privoxy_conf.txt\"";
                _process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                _process.StartInfo.UseShellExecute = true;
                _process.StartInfo.CreateNoWindow = true;
                _process.Start();
            }
            catch { }
        }
    }
}
