using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace meter
{
    public partial class Form1 : Form
    {

        CommunicationManager comm = new CommunicationManager();

        CommunicationManager comm2 = new CommunicationManager();
        string transType = string.Empty;

        INIFile ini;

        string baseFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        
        public int curDir = -1;
        public int totalBytes = 0;
        public string logFileName;
        public StreamWriter stream;
        public Form2 blackForm = null;
        public bool DEBUG = false;
        public bool PortOpened = false;
        string CLOSE_KEY = null;
        string CLOSE_RUN1 = null;
        string CLOSE_RUN2 = null;
        string CLOSE_RUN3 = null;

        public Form1()
        {
            InitializeComponent();

            

            logFileName = Path.Combine(baseFolder, DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".txt");
            string iniFilePath = Path.Combine(baseFolder, @"meter_config.ini");

            //FileStream f= File.OpenRead(iniFilePath);
            //Console.WriteLine("baseFolder", f.ReadByte());

            ini = new INIFile(iniFilePath);

            DEBUG = ini.ReadValueAsString("App", "Debug", "FALSE").Equals("TRUE", StringComparison.OrdinalIgnoreCase);
            CLOSE_KEY = ini.ReadValueAsString("App", "CloseKey", "90 90");
            CLOSE_RUN1 = Path.Combine(baseFolder, ini.ReadValueAsString("App", "CloseRun1", ""));
            CLOSE_RUN2 = Path.Combine(baseFolder, ini.ReadValueAsString("App", "CloseRun2", ""));
            CLOSE_RUN3 = Path.Combine(baseFolder, ini.ReadValueAsString("App", "CloseRun3", ""));

            if (DEBUG)
            {
                stream = new StreamWriter(logFileName, true);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (PortOpened) return;
            comm.PortName = ini.ReadValueAsString("Port1", "PortName", "COM5");
            comm.BaudRate = ini.ReadValueAsString("Port1", "BaudRate", "9600"); 
            comm.Parity = ini.ReadValueAsString("Port1", "Parity", "None");
            comm.StopBits = ini.ReadValueAsString("Port1", "StopBits", "One");
            comm.DataBits = ini.ReadValueAsString("Port1", "DataBits", "8");

            if (DEBUG) comm.DisplayWindow = rtbDisplay;
            Exception ex = comm.OpenPort();
            if (ex != null)
            {
                MessageBox.Show(comm.PortName + " open fail:" + ex.Message);
                return;
            }

            comm2.PortName = ini.ReadValueAsString("Port2", "PortName", "COM2");
            comm2.BaudRate = ini.ReadValueAsString("Port2", "BaudRate", "9600");
            comm2.Parity = ini.ReadValueAsString("Port2", "Parity", "None");
            comm2.StopBits = ini.ReadValueAsString("Port2", "StopBits", "One");
            comm2.DataBits = ini.ReadValueAsString("Port2", "DataBits", "8");
            // comm2.DisplayWindow = rtbDisplay;
            ex = comm2.OpenPort();
            if (ex!=null)
            {
                comm.ClosePort();
                MessageBox.Show(comm2.PortName + " open fail:" + ex.Message);
                return;
            }

            PortOpened = true;
            button1.Enabled = false;

            if (! DEBUG)
            {
                blackForm = new Form2(this);
                blackForm.Show();
            }

        }


        private void Form1_Load(object sender, EventArgs e)
        {

            rtbDisplay.Height = this.Height - 80;

            comm.CurrentTransmissionType = CommunicationManager.TransmissionType.Hex;
            comm2.CurrentTransmissionType = CommunicationManager.TransmissionType.Hex;

            comm.onData = forward1;
            comm2.onData = forward2;
        }


        int forward1(String abc)
        {
            if (abc.Trim() == CLOSE_KEY)
            {
                //Throw: control.Invoke must be used to interact with controls created on a separate thread
                //blackForm.Close();

                closeApp();
                return 0;
            }

            if (DEBUG)
            {
                if (curDir != 0)
                {
                    stream.Write(Environment.NewLine + Environment.NewLine + "<<-- " + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") + Environment.NewLine);
                }
                stream.Write(abc);
            }

            curDir = 0;
            
            comm2.WriteData(abc);
            totalBytes += abc.Length;

            return 1;
        }

        int forward2(String abc)
        {
            if (DEBUG)
            {
                if (curDir != 1)
                {
                    stream.Write(Environment.NewLine + Environment.NewLine + "-->> " + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ" + Environment.NewLine));
                }
                stream.Write(abc);
            }

            curDir = 1;

            comm.WriteData(abc);
            totalBytes += abc.Length;
            return 1;
        }

        void runApp(string exePath, string args)
        {
            if (!File.Exists(exePath)) return;
            ProcessStartInfo startInfo = new ProcessStartInfo(exePath, args);
            startInfo.UseShellExecute = true;//This should not block your program
            startInfo.WorkingDirectory = Path.GetDirectoryName(exePath);
            Process.Start(startInfo);
        }

        public void closeApp()
        {
            cleanUp();

            runApp(CLOSE_RUN1, "");
            runApp(CLOSE_RUN2, "");
            runApp(CLOSE_RUN3, "");

            Application.Exit();
        }

        public void cleanUp()
        {
            if (stream != null)
            {
                stream.Flush();
                stream.Close();
                stream = null;

                if (totalBytes == 0)
                {
                    File.Delete(logFileName);
                }
            }

            comm.ClosePort();
            comm2.ClosePort();
        }

        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            cleanUp();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            closeApp();
        }


    }
}