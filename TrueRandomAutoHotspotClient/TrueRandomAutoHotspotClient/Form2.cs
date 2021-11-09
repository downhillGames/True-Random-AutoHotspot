using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TrueRandomAutoHotspotClient
{
    public partial class Form2 : Form
    {
        public static String shared = "";
        public static int timer_count = 0;
        public Form2()
        {
            InitializeComponent();
            label1.Text = "True Random Number Generator Client";
            label2.Text = "Please type in shared drive/folder location. For example: \\\\LAPTOP\\files\\";
            button1.Text = "Next";
            label3.Text = "Timer:";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            //set variables to go to next screen
            shared = textBox1.Text;
            timer_count = (int)(numericUpDown1.Value);
            var frm = new Form1();
            frm.Location = this.Location;
            frm.StartPosition = FormStartPosition.Manual;
            frm.FormClosing += delegate { this.Show(); };
            frm.Show();
            this.Hide();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
