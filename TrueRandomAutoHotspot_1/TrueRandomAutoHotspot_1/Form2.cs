using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrueRandomAutoHotspot_1
{
    public partial class Form2 : Form
    {
        public static bool arduinoSet = false;
        public static String shared = "";
        public static int port = 0;
        public static int timer_count = 0;
        public static String ssid_name = "";

        public Form2()
        {
            InitializeComponent();
            button1.Text = "Start";
            checkBox1.Text = "Arduino present?";
            label2.Text = "Port Number:";
            label3.Text = "Timer (seconds):";
            label4.Text = "SSID Name: ";
            label1.Text = "Please type in shared drive/folder location. For example: \\\\LAPTOP\\files\\";
            label5.Text = "Welcome to the Random Hotspot Generator! Please select your options below to begin";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // set variables being sent to next screen
            shared = textBox1.Text;
            port = (int) (numericUpDown1.Value);
            timer_count = (int)(numericUpDown2.Value); ;
            ssid_name = textBox2.Text;

            var frm = new Form1();
            frm.Location = this.Location;
            frm.StartPosition = FormStartPosition.Manual;
            frm.FormClosing += delegate { this.Show(); };
            frm.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                arduinoSet = true;
            }
            else {
                arduinoSet = false;
            }
        }


        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
