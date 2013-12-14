using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ping
{
    class ProcessForm : Form
    {

        delegate void SetTextCallback(double ping);
        private System.Windows.Forms.ContextMenu cm;
        private System.Windows.Forms.MenuItem exitItem;
        private System.ComponentModel.IContainer components;
        private BackgroundWorker backgroundWorker1;
        private Label label1;

        public ProcessIcon pi;

        public ProcessForm()
        {
            this.components = new System.ComponentModel.Container();
            this.cm = new System.Windows.Forms.ContextMenu();
            this.exitItem = new System.Windows.Forms.MenuItem();

            this.cm.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.exitItem });

            this.exitItem.Index = 0;
            this.exitItem.Text = "E&xit";
            this.exitItem.Click += new System.EventHandler(this.exitItem_Click);

            this.pi = new ProcessIcon();
            this.pi.ni.ContextMenu = this.cm;
            this.pi.Display();

            this.InitializeComponent();
            this.WindowState = FormWindowState.Minimized;

            Task.Factory.StartNew(() => this.runConstantPing());

        }

        private void runConstantPing()
        {
            while (true)
            {
                double ping = PingTimeAverage(Properties.Resources.pingUrl, 4);
                if (ping > 400)
                {
                    this.pi.setPoorIcon();
                }
                else if (ping > 200)
                {
                    this.pi.setMediocreIcon();
                }
                else
                {
                    this.pi.setGoodIcon();
                }
                this.pi.setMessage(ping);
                this.SetText(ping);
                Thread.Sleep(50); //we don't have to be very exact here
            }
        }

        private void SetText(double ping)
        {
            String text = "Pinging " + Properties.Resources.pingUrl + ".. " + ping + " ms";
            if (this.label1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                try
                {
                    this.Invoke(d, new object[] { ping });
                }
                catch (ObjectDisposedException)
                {}
            }
            else
            {
                this.label1.Text = text;
            }
        }

        private void exitItem_Click(object Sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void InitializeComponent()
        {
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Constant Ping: 0 ms";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label1.UseWaitCursor = true;
            // 
            // ProcessForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 35);
            this.Controls.Add(this.label1);
            this.Name = "ProcessForm";
            this.ShowIcon = false;
            this.Text = "Ping";
            this.UseWaitCursor = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public static double PingTimeAverage(string host, int echoNum)
        {
            long totalTime = 0;
            int timeout = 120;
            Ping pingSender = new Ping();

            for (int i = 0; i < echoNum; i++)
            {
                try
                {
                    PingReply reply = pingSender.Send(host, timeout);
                    if (reply.Status == IPStatus.Success)
                    {
                        totalTime += reply.RoundtripTime;
                    }
                }
                catch (PingException)
                {
                    totalTime += 1000; //artificially high if something goes wrong
                }
            }
            return totalTime / echoNum;
        }
    }

}
