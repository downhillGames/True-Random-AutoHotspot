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

namespace TrueRandomAutoHotspotClient
{
	public partial class Form1 : Form
	{
        private ProcessStartInfo ps = null;
        private string message = "";
        public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
		
		}
        private void button1_Click(object sender, EventArgs e)
        {
            //Status();
            //List();
            Init();
            create();
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

        public void create()
        {
            ps.Arguments = String.Format("wlan add profile filename={0}", "C:\\Users\\Tyler\\myXmFile.xml");
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
    }

}
