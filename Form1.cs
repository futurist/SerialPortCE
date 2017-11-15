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
using System.Globalization;
using System.Text.RegularExpressions;

namespace meter
{
    public partial class Form1 : Form
    {

        CommunicationManager comm = new CommunicationManager();

        CommunicationManager comm2 = new CommunicationManager();
        string transType = string.Empty;

        INIFile ini;

        public string baseFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        
        public int curDir = -1;
        public int totalBytes = 0;
        public string logFileName;
        public StreamWriter stream;
        public Form2 blackForm = null;
        public bool DEBUG = false;
        public bool FULLSCREEN = false;
        public bool PortOpened = false;
        public string CLOSE_KEY = null;
        public string CLOSE_RUN1 = null;
        public string CLOSE_RUN2 = null;
        public string CLOSE_RUN3 = null;
        public string prevCode = "";


        public double meterValue = 0;
        public bool isYard = false;

        public Form1()
        {
            InitializeComponent();

            

            logFileName = Path.Combine(baseFolder, DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".txt");
            string iniFilePath = Path.Combine(baseFolder, @"meter_config.ini");

            //FileStream f= File.OpenRead(iniFilePath);
            //Console.WriteLine("baseFolder", f.ReadByte());

            ini = new INIFile(iniFilePath);

            DEBUG = ini.ReadValueAsString("App", "Debug", "FALSE").Equals("TRUE", StringComparison.OrdinalIgnoreCase);
            FULLSCREEN = ini.ReadValueAsString("App", "FullScreen", "FALSE").Equals("TRUE", StringComparison.OrdinalIgnoreCase);
            CLOSE_KEY = ini.ReadValueAsString("App", "CloseKey", "90 90 A0 A0");  // 90=Return, A0=Enter
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
            openApp();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //rtbDisplay.Text = (String.Format("{0},  {1},  {2},  {3}", CLOSE_KEY, CLOSE_RUN1, CLOSE_RUN2, CLOSE_RUN3));
            rtbDisplay.Height = this.Height - 80;

            comm.CurrentTransmissionType = CommunicationManager.TransmissionType.Hex;
            comm2.CurrentTransmissionType = CommunicationManager.TransmissionType.Hex;

            comm.onData = forward1;
            comm2.onData = forward2;

            if (!DEBUG)
            {
                
            }

            openApp();
        }

        void openApp()
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
            if (ex != null)
            {
                comm.ClosePort();
                MessageBox.Show(comm2.PortName + " open fail:" + ex.Message);
                return;
            }

            PortOpened = true;
            button1.Enabled = false;
            
            if (!DEBUG)
            {

                showForm2();
                
            }

        }

        void showForm2()
        {
            //this.Hide();

            blackForm = new Form2(this);
            blackForm.Show();
            forward1(" 3F FF FE ");
            //forward1("A5 A5");
        }

        public void switchYard()
        {
            isYard = !isYard;
            if (blackForm != null)
            {
                blackForm.updateText();
            }
        }

        public void clearValue()
        {
            comm.WriteData("43");
        }


        public int forward1(String abc)
        {
            string value = Regex.Replace(abc, @"\s+", "");
            
            // Clear
            if (value == "A5A5")
            {
                clearValue();
            }

            // Meter/Yard switch
            if (value == "F7F7")
            {
                switchYard();
            }

            // Close App
            if (prevCode + abc.Trim() == CLOSE_KEY) // 2 keys combo: 90 90 A0 A0
            {
                //Throw: control.Invoke must be used to interact with controls created on a separate thread
                //blackForm.Close();

                closeApp();
                return 0;
            }

            prevCode = abc.Trim() + " ";

            if (DEBUG)
            {
                if (curDir != 0)
                {
                    stream.Write(Environment.NewLine + Environment.NewLine + "<<-- " + Environment.TickCount.ToString() + Environment.NewLine);
                }
                stream.Write(abc);
            }

            curDir = 0;

            if (abc.Trim().Length >= 6)
            {
                comm2.WriteData(abc);

                try
                {
                    meterValue =((double)(
                        Convert.ToInt32("0x400000", 16) - Convert.ToInt32("0x"+value , 16)
                        ) * 0.1);
                    if (blackForm != null)
                    {
                        blackForm.setMeter();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            totalBytes += abc.Length;

            return 1;
        }

        public int forward2(String abc)
        {
            if (DEBUG)
            {
                if (curDir != 1)
                {
                    stream.Write(Environment.NewLine + Environment.NewLine + "-->> " + Environment.TickCount + Environment.NewLine);
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

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            showForm2();
        }


    }
}