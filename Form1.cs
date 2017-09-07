using System;
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

        string transType = string.Empty;
        int curDir = -1;
        int totalBytes = 0;
        static string fileName = DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".txt";
        StreamWriter stream = new StreamWriter(Form1.fileName, true);


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comm.PortName = "COM5";
            comm.Parity = "None";
            comm.StopBits = "One";
            comm.DataBits = "8";
            comm.BaudRate = "9600";
            comm.DisplayWindow = rtbDisplay;
            comm.OpenPort();

            comm2.PortName = "COM2";
            comm2.Parity = "None";
            comm2.StopBits = "One";
            comm2.DataBits = "8";
            comm2.BaudRate = "9600";
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
                stream.Write("\r\n\r\n<<-- " + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ\r\n"));
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
                stream.Write("\r\n\r\n-->> " + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ\r\n"));
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
                    File.Delete(Form1.fileName);
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