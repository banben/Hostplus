using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hostplus.Controller;
using System.Diagnostics;

namespace Hostplus.View
{
    class ContextMenu
    {
        private HostplusController _hostplusController;
        private ToolStripMenuItem _item;
        private EventHandler _event;

        public ContextMenuStrip Create()
        {
            // Add the default menu options.
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;
            ToolStripSeparator sep;

            // Disable/Enable Hostplus.
            item = new ToolStripMenuItem();
            _item = item;
            item.Text = "关闭";
            _event = new EventHandler(Disable);
            item.Click += _event;
            
            menu.Items.Add(item);

            // Restore Hosts File.
            item = new ToolStripMenuItem();
            item.Text = "修复Hosts";
            item.Click += new EventHandler(Restore);
            menu.Items.Add(item);

            // About.
            item = new ToolStripMenuItem();
            item.Text = "关于";
            item.Click += new EventHandler(About);
            menu.Items.Add(item);

            // Separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            // Exit.
            item = new ToolStripMenuItem();
            item.Text = "退出";
            item.Click += new System.EventHandler(Exit);
            menu.Items.Add(item);

            _hostplusController = new HostplusController();
            _hostplusController.Enable();

            return menu;
        }
        void Enable(object sender, EventArgs e)
        {
            _hostplusController.Enable();
            _item.Text = "关闭";
            _item.Click -= _event;
            _event = new EventHandler(Disable);
            _item.Click += _event;
        }
        void Disable(object sender, EventArgs e)
        {
            _hostplusController.Disable();
            _item.Text = "开启";
            _item.Click -= _event;
            _event = new EventHandler(Enable);
            _item.Click += _event;
        }
        void Restore(object sender, EventArgs e)
        {
            _hostplusController.RestoreHosts();
        }
        void About(object sender, EventArgs e)
        {
            Process.Start("https://github.com/banben/Hostplus");
        }
        void Exit(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
