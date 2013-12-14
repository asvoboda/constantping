﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ping
{
    class ProcessIcon : IDisposable
    {
        public NotifyIcon ni;

        public ProcessIcon()
        {
            ni = new NotifyIcon();
        }

        public void Display()
        {
            // Put the icon in the system tray and allow it react to mouse clicks.
            ni.Text = "Constant Ping";
            ni.Visible = true;
            ni.Icon = Properties.Resources.RedIcon;
        }

        public void Dispose()
        {
            ni.Dispose();
        }

        private void setIcon(Icon icon)
        {
            ni.Icon = icon;
        }

        public void setGoodIcon()
        {
            setIcon(Properties.Resources.GreenIcon);
        }

        public void setMediocreIcon()
        {
            setIcon(Properties.Resources.OrangeIcon);
        }

        public void setPoorIcon()
        {
            setIcon(Properties.Resources.RedIcon);
        }

        public void setMessage(double ping)
        {
            ni.Text = "Constant Ping: " + ping + " ms";
        }
    }
}
