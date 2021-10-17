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

namespace TrueRandomAutoHotspot_1
{

    public partial class Form1 : Form
    {
        public SerialPort myport;


        private ProcessStartInfo ps = null;
        private string message = "";

        private void Init() {
            ps = new ProcessStartInfo("cmd.exe");
            ps.UseShellExecute = false;
            ps.RedirectStandardOutput = true;
            ps.CreateNoWindow = false;
            ps.FileName = "netsh";
        }

        public void Start() {
            ps.Arguments = "wlan start hosted network";
            Execute(ps);
        }

        public void create(string ssid, string key) {
            ps.Arguments = String.Format("wlan set hostednetwork mode=allow ssid={0} key={1}", ssid, key);
            Execute(ps);
        }

        public void Stop()
        {
            ps.Arguments = "wlan stop hosted network";
            Execute(ps);
        }



        private void Execute(ProcessStartInfo ps) {
            bool isExecuted = false;
            try {
                using (Process p = Process.Start(ps)) {
                    message = p.StandardOutput.ReadToEnd() + "\n";
                    p.WaitForExit();
                    isExecuted = true;
                }
            }
            catch (Exception e) {
                message = "";
                message += e.Message;
                isExecuted = false;
            }
        }

        public Form1()
        {
            InitializeComponent();
            label2.Text = "";
            label1.Text =  "";
        }


        private string TrueRandomPassphase(int length) { 
            myport = new SerialPort();
            myport.BaudRate = 9600;
            myport.PortName = "COM17";
            myport.Open();
            int number = length;
            myport.WriteLine(number.ToString());
            string str = "";
            char[] array1 = new char[number +1];


            for (int i = 0; i < number; i++)
            {
                //array1[i] = myport.ReadLine();
                array1[i] = (char) myport.ReadChar();
            }

            for (int i = 0; i < number; i++)
            {
                str += array1[i];
                Console.WriteLine(array1[i]);
            }

            //foreach (var item in array1)
            //{

            //  str += item;
            // Console.WriteLine(item);
            //}
            str = Regex.Replace(str, @"[^0-9a-zA-Z]+", "");

            Console.WriteLine(str);
            myport.WriteLine("F");
            myport.Close();
            return str;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //label1.Text = TrueRandomPassphase(8);
            label1.Text = "12345678";
            XMLWrite();
        }

        private void XMLWrite()
        {
            XmlTextWriter textWriter = new XmlTextWriter("C:\\Users\\Tyler\\myXmFile.xml", null);
            // Opens the document  
            textWriter.WriteStartDocument();
            textWriter.WriteStartElement("WLANProfile", "http://www.microsoft.com/networking/WLAN/profile/v1");
            textWriter.WriteStartElement("name");
            textWriter.WriteString("");
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("SSIDConfig");
            textWriter.WriteStartElement("SSID");
            textWriter.WriteStartElement("hex");
            textWriter.WriteString(ToHex(""));
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("name");
            textWriter.WriteString("");
            textWriter.WriteEndElement();
            textWriter.WriteEndElement();
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("connectionType");
            textWriter.WriteString("ESS");
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("connectionMode");
            textWriter.WriteString("auto");
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("MSM");
            textWriter.WriteStartElement("security");
            textWriter.WriteStartElement("authEncryption");
            textWriter.WriteStartElement("authentication");
            textWriter.WriteString("WPA2PSK");
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("encryption");
            textWriter.WriteString("AES");
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("useOneX");
            textWriter.WriteString("false");
            textWriter.WriteEndElement();
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("sharedKey");
            textWriter.WriteStartElement("keyType");
            textWriter.WriteString("passPhrase");
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("protected");
            textWriter.WriteString("false");
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("keyMaterial");
            textWriter.WriteString("");
            textWriter.WriteEndElement();
            textWriter.WriteEndElement();
            textWriter.WriteEndElement();
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("MacRandomization", "http://www.microsoft.com/networking/WLAN/profile/v3");
            textWriter.WriteStartElement("enableRandomization");
            textWriter.WriteString("false");
            textWriter.WriteEndElement();
            textWriter.WriteEndElement();
            textWriter.WriteEndElement();
            //textWriter.WriteEndElement();
            textWriter.Close();
        }


        private string ToHex(String str) {
            byte[] ba = Encoding.Default.GetBytes(str);
            var hexString = BitConverter.ToString(ba);
            hexString = hexString.Replace("-", "");
            return hexString;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Init();
            create("helloworld", label1.Text);
            Start();
            label2.Text = message;

        }

        

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
