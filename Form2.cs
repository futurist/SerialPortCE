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
        Form1 parentForm;
        public Form2(Form1 _parent)
        {
            parentForm = _parent;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            this.Left = 0;
            this.Top = 0;
            
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ControlBox = false;

            Cursor.Hide();
            if(parentForm.FULLSCREEN) FullScreen.StartFullScreen(this);

            updateText();
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
            parentForm.closeApp();
        }

        private void Form2_Closing(object sender, CancelEventArgs e)
        {
            Cursor.Show();
            if (parentForm.FULLSCREEN) FullScreen.StopFullScreen(this);
            parentForm.blackForm = null;
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            parentForm.clearValue();
        }

        public void setMeter()
        {
            bool isYard = parentForm.isYard ;
            double val = parentForm.meterValue;
            double factor = (isYard ? 0.9144 : 1);
            textBoxMeter.Text = Math.Round(val/factor, 2).ToString() + " " + (isYard? "Y" : "M");
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        public void updateText()
        {
            buttonText.Text = parentForm.isYard ? "切换米" : "切换码";
            setMeter();
        }

        private void buttonText_Click(object sender, EventArgs e)
        {
            parentForm.switchYard();
        }
    }
}

