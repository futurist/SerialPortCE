using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace meter
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            this.Left = 0;
            this.Top = 0;

            FullScreen.StartFullScreen(this); 
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Closing(object sender, CancelEventArgs e)
        {
            FullScreen.StopFullScreen(this); 
        }
    }
}

