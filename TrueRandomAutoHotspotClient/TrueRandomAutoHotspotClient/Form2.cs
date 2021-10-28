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
        public Form2()
        {
            InitializeComponent();
            label1.Text = "True Random Number Generator Client";
            label2.Text = "Please type in shared drive/folder location. For example: \\\\LAPTOP\\files\\";
            button1.Text = "Next";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            shared = textBox1.Text;
            var frm = new Form1();
            frm.Location = this.Location;
            frm.StartPosition = FormStartPosition.Manual;
            frm.FormClosing += delegate { this.Show(); };
            frm.Show();
            this.Hide();
        }
    }
}
