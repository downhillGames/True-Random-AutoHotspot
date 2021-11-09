using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Security.Principal;
using System.Xml;
using System.IO;

namespace TrueRandomAutoHotspotClient
{
	public partial class Form1 : Form
	{
        private ProcessStartInfo ps = null;
        private string message = "";
        public static String shared_folder = "";
        private static int tmer_interval = Form2.timer_count * 1000;
        private int time_left = tmer_interval / 1000;

        Timer timer1 = new Timer
        {
            Interval = tmer_interval
        };

        Timer timer2 = new Timer
        {
            Interval = 1000
        };

        public Form1()
		{
			InitializeComponent();
            button1.Text = "Start Client pairing";
            button2.Text = "Stop Client pairing";
            shared_folder = Form2.shared;
            label1.Text = "Timer: ";
            label2.Text = time_left.ToString();
        }

		private void Form1_Load(object sender, EventArgs e)
		{
		
		}
        private void button1_Click(object sender, EventArgs e)
        {
            //Status();
            //List();
            Init();
            create(shared_folder);
            timer1.Enabled = true;
            timer1.Tick += new System.EventHandler(OnTimerEvent);
            timer2.Enabled = true;
            timer2.Tick += new System.EventHandler(OnSecondEvent);
        }


        private void OnTimerEvent(object sender, EventArgs e)
        {
            Stop();
            Init();
            create(shared_folder);
            time_left = tmer_interval / 1000;
        }


        private void Init()
        {
            ps = new ProcessStartInfo("cmd.exe");
            ps.UseShellExecute = false;
            ps.RedirectStandardOutput = true;
            ps.CreateNoWindow = false;
            ps.FileName = "netsh";
        }

        public void Start()
        {
            ps.Arguments = "wlan start hosted network";
            Execute(ps);
        }

        public void create(String network_location)
        {
            ps.Arguments = String.Format("wlan add profile filename={0}", network_location + "myXmFile.xml");
            Execute(ps);
        }

        public void Stop()
        {
            ps.Arguments = "wlan stop hosted network";
            Execute(ps);
        }



        private void Execute(ProcessStartInfo ps)
        {
            bool isExecuted = false;
            try
            {
                using (Process p = Process.Start(ps))
                {
                    message = p.StandardOutput.ReadToEnd() + "\n";
                    p.WaitForExit();
                    isExecuted = true;
                }
            }
            catch (Exception e)
            {
                message = "";
                message += e.Message;
                isExecuted = false;
            }
        }
       
        /*Trigger for timer 1, starts a new Windows hotspot with a random passprase*/
        private void OnSecondEvent(object sender, EventArgs e)
        {
            time_left -= 1;
            label2.Text = time_left.ToString();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Stop();
        }
    }

}
