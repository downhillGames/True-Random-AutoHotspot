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
        private bool arduinoSet = false;
        private int index = 0;
        private ProcessStartInfo ps = null;
        private string message = "";
        private string sharedFolder = Form2.shared;
        private int given_port = Form2.port;
        private static int tmer_interval = Form2.timer_count * 1000;
        private int time_left = tmer_interval / 1000;
        private string ssid_name_given = Form2.ssid_name;

        Timer timer1 = new Timer
        {
            Interval = tmer_interval
        };

        Timer timer2 = new Timer
        {
            Interval = 1000
        };

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
            index += 1;
            ps.Arguments = "wlan stop hosted network";
            Execute(ps);
        }


        /*Executes given process in command prompt*/
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
        
        /*Initializes the main screen*/
        public Form1()
        {
            InitializeComponent();
            label2.Text = "";
            label3.Text = "Passprase";
            label1.Text =  "";
            label5.Text = "";
            label4.Text = "SSID:";
            label6.Text = "Timer:";
            label7.Text = time_left.ToString();
            if (Form2.arduinoSet) {
                arduinoSet = true;
            }
            button1.Text = "Generate passphrase";
            button2.Text = "Start Hotspot";
            button3.Text = "Stop Hotspot";
            sharedFolder = Form2.shared;
        }

        private string PseudoRandomPassphrase(int length) {
            var rand = new Random();
            String passphrase = "";
            for (int i = 0; i<length; i++) {
                int random = rand.Next(0, 62);
                //int rand = 10;
                String randStr = "";
                if (random >= 36)
                {
                    randStr = ((char)(random + 61)).ToString();
                }
                else if (random > 9 && random < 36)
                {
                    randStr = ((char)(random + 55)).ToString();
                }
                else
                {
                    randStr =  random.ToString();
                }
                passphrase += randStr;
            }
            return passphrase;
        }
        private string TrueRandomPassphase(int length, int port) { 
            myport = new SerialPort();
            myport.BaudRate = 9600;
            string port_string = "COM" + port.ToString();
            myport.PortName = port_string;
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

    
            str = Regex.Replace(str, @"[^0-9a-zA-Z]+", "");

            Console.WriteLine(str);
            myport.WriteLine("F");
            myport.Close();
            return str;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //label1.Text = TrueRandomPassphase(8);
            String randPassword = "";
            if (arduinoSet)
            {
                randPassword = TrueRandomPassphase(8, given_port);
            }
            else {
               randPassword = PseudoRandomPassphrase(8);
            }
            label1.Text = randPassword;
        }

        /*This ugly function outputs the hotspot info into an XML that command prompt can read*/
        private void XMLWrite(String SSID, String Passprase, String sharedFldr)
        {
            String fileName = sharedFldr + "myXmFile.xml";
            XmlTextWriter textWriter = new XmlTextWriter(fileName, null);
            // Opens the document  
            textWriter.WriteStartDocument();
            textWriter.WriteStartElement("WLANProfile", "http://www.microsoft.com/networking/WLAN/profile/v1");
            textWriter.WriteStartElement("name");
            textWriter.WriteString(SSID);
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("SSIDConfig");
            textWriter.WriteStartElement("SSID");
            textWriter.WriteStartElement("hex");
            textWriter.WriteString(ToHex(SSID));
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("name");
            textWriter.WriteString(SSID);
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
            textWriter.WriteString(Passprase);
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
            create(ssid_name_given + "_" + index.ToString(), label1.Text);
            Start();
            label2.Text = message;
            Console.WriteLine(sharedFolder);
            label5.Text = ssid_name_given + "_" + index.ToString();
            XMLWrite(ssid_name_given + "_" + index.ToString(), label1.Text, sharedFolder);

            //enable timer to switch SSID / passprase
            timer1.Enabled = true;
            timer1.Tick += new System.EventHandler(OnTimerEvent);
            
            //enable "second" timer
            timer2.Enabled = true;
            timer2.Tick += new System.EventHandler(OnSecondEvent);

        }



        /*Trigger for timer 1, starts a new Windows hotspot with a random passprase*/
        private void OnSecondEvent(object sender, EventArgs e)
        {
            time_left -= 1;
            label7.Text = time_left.ToString();
        }




        /*Trigger for timer 1, starts a new Windows hotspot with a random passprase*/
        private void OnTimerEvent(object sender, EventArgs e)
        {
            Stop();
            String randPassword = "";
            if (arduinoSet)
            {
                randPassword = TrueRandomPassphase(8, given_port);
            }
            else
            {
                randPassword = PseudoRandomPassphrase(8);
            }
            label1.Text = randPassword;
            Task.Delay(1000);
            Init();
            create(ssid_name_given + "_" + index.ToString(), label1.Text);
            Start();
            label2.Text = message;
            label5.Text = ssid_name_given + "_" + index.ToString();
            Console.WriteLine(sharedFolder);
            XMLWrite(ssid_name_given  + "_" + index.ToString(), label1.Text, sharedFolder);
            time_left = (tmer_interval / 1000) - 1;
    }

   

 
        private void button3_Click(object sender, EventArgs e)
        {
            Stop();
            timer1.Enabled = false;
            timer2.Enabled = false;
            label2.Text = message;
        }

    }
}
