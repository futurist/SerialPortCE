namespace meter
{
    partial class Form2
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxMeter = new System.Windows.Forms.TextBox();
            this.buttonReset = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonText = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkGreen;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(100, 100);
            this.panel1.DoubleClick += new System.EventHandler(this.panel1_DoubleClick);
            // 
            // textBoxMeter
            // 
            this.textBoxMeter.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Regular);
            this.textBoxMeter.Location = new System.Drawing.Point(133, 64);
            this.textBoxMeter.Name = "textBoxMeter";
            this.textBoxMeter.ReadOnly = true;
            this.textBoxMeter.Size = new System.Drawing.Size(339, 64);
            this.textBoxMeter.TabIndex = 1;
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(133, 147);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(110, 42);
            this.buttonReset.TabIndex = 2;
            this.buttonReset.Text = "清零";
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DarkGreen;
            this.panel2.Location = new System.Drawing.Point(133, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(100, 32);
            this.panel2.Click += new System.EventHandler(this.panel2_Click);
            // 
            // buttonText
            // 
            this.buttonText.Location = new System.Drawing.Point(362, 147);
            this.buttonText.Name = "buttonText";
            this.buttonText.Size = new System.Drawing.Size(110, 42);
            this.buttonText.TabIndex = 4;
            this.buttonText.Text = "码制";
            this.buttonText.Click += new System.EventHandler(this.buttonText_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.DarkGreen;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(133, 208);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(314, 77);
            this.textBox1.TabIndex = 7;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.DarkGreen;
            this.ClientSize = new System.Drawing.Size(638, 455);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonText);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.textBoxMeter);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.DarkGreen;
            this.Name = "Form2";
            this.Text = "Meter";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form2_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxMeter;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonText;
        private System.Windows.Forms.TextBox textBox1;
    }
}