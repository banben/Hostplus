using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hostplus.Properties;
using System.Drawing;
using Hostplus.Controller;

namespace Hostplus.View
{
    class ProcessIcon:IDisposable
    {
        private NotifyIcon ni;
        private UpdateChecker updateChecker;

        public ProcessIcon()
        {
            ni = new NotifyIcon();
            updateChecker = new UpdateChecker();
        }
        public void Display()
        {
            Bitmap icon = Resources.hp;
            ni.Icon = Icon.FromHandle(icon.GetHicon()); ;
            ni.Text = "Hostplus";
            ni.Visible = true;
            ni.ContextMenuStrip = new ContextMenu().Create();
            updateChecker.NewVersionFound += updateChecker_NewVersionFound;
            updateChecker.CheckUpdate();
        }
        void updateChecker_NewVersionFound(object sender, EventArgs e)
        {
            string title = "Hostplus Update Found Version " + updateChecker.LatestVersionNumber;
            ShowBalloonTip(title, "Click here to download", ToolTipIcon.Info, 5000);
            ni.BalloonTipClicked += notifyIcon1_BalloonTipClicked;
        }
        void ShowBalloonTip(string title, string content, ToolTipIcon icon, int timeout)
        {
            ni.BalloonTipTitle = title;
            ni.BalloonTipText = content;
            ni.BalloonTipIcon = icon;
            ni.ShowBalloonTip(timeout);
        }
        void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(updateChecker.LatestVersionURL);
            ni.BalloonTipClicked -= notifyIcon1_BalloonTipClicked;
        }
        public void Dispose()
        {
            ni.Dispose();
        }
    }
}
