﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace meter
{
    public partial class Form1 : Form
    {

        CommunicationManager comm = new CommunicationManager();

        CommunicationManager comm2 = new CommunicationManager();

        INIFile ini;

        string baseFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        string transType = string.Empty;
        int curDir = -1;
        int totalBytes = 0;
        string fileName;
        StreamWriter stream;


        public Form1()
        {
            InitializeComponent();

            

            fileName = DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".txt";
            stream = new StreamWriter(fileName, true);

            string iniFilePath = Path.Combine(baseFolder, @"meter_config.ini");

            //FileStream f= File.OpenRead(iniFilePath);
            //Console.WriteLine("baseFolder", f.ReadByte());

            ini = new INIFile(iniFilePath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comm.PortName = ini.ReadValueAsString("Port1", "PortName", "COM5");
            comm.BaudRate = ini.ReadValueAsString("Port1", "BaudRate", "9600"); 
            comm.Parity = ini.ReadValueAsString("Port1", "Parity", "None");
            comm.StopBits = ini.ReadValueAsString("Port1", "StopBits", "One");
            comm.DataBits = ini.ReadValueAsString("Port1", "DataBits", "8");
            comm.DisplayWindow = rtbDisplay;
            comm.OpenPort();

            comm2.PortName = ini.ReadValueAsString("Port2", "PortName", "COM2");
            comm2.BaudRate = ini.ReadValueAsString("Port2", "BaudRate", "9600");
            comm2.Parity = ini.ReadValueAsString("Port2", "Parity", "None");
            comm2.StopBits = ini.ReadValueAsString("Port2", "StopBits", "One");
            comm2.DataBits = ini.ReadValueAsString("Port2", "DataBits", "8");
            // comm2.DisplayWindow = rtbDisplay;
            comm2.OpenPort();


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comm.CurrentTransmissionType = CommunicationManager.TransmissionType.Hex;
            comm2.CurrentTransmissionType = CommunicationManager.TransmissionType.Hex;

            comm.onData = forward1;
            comm2.onData = forward2;
        }


        int forward1(String abc)
        {
            if (curDir != 0)
            {
                stream.Write(Environment.NewLine+Environment.NewLine+"<<-- " + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ")+Environment.NewLine);
            }
            curDir = 0;
            stream.Write(abc);
            comm2.WriteData(abc);
            totalBytes += abc.Length;
            return 1;
        }

        int forward2(String abc)
        {
            if (curDir != 1)
            {
                stream.Write(Environment.NewLine + Environment.NewLine + "-->> " + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ" + Environment.NewLine));
            }
            curDir = 1;
            stream.Write(abc);
            comm.WriteData(abc);
            totalBytes += abc.Length;
            return 1;
        }


        void cleanUp()
        {
            if (stream != null)
            {
                stream.Flush();
                stream.Close();
                stream = null;

                if (totalBytes == 0)
                {
                    File.Delete(fileName);
                }
            }

            comm.ClosePort();
            comm2.ClosePort();
        }

        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            cleanUp();
        }


    }
}