namespace FishML {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing ) {
            if ( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.openFD = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblReadFile = new System.Windows.Forms.Label();
            this.lblWriteFile = new System.Windows.Forms.Label();
            this.btn1 = new System.Windows.Forms.Button();
            this.btn2 = new System.Windows.Forms.Button();
            this.mynotifyicon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(664, 229);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(639, 203);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 1;
            // 
            // lblReadFile
            // 
            this.lblReadFile.AutoSize = true;
            this.lblReadFile.Location = new System.Drawing.Point(50, 30);
            this.lblReadFile.Name = "lblReadFile";
            this.lblReadFile.Size = new System.Drawing.Size(74, 13);
            this.lblReadFile.TabIndex = 2;
            this.lblReadFile.Text = "Απομένουν ...";
            // 
            // lblWriteFile
            // 
            this.lblWriteFile.AutoSize = true;
            this.lblWriteFile.Location = new System.Drawing.Point(50, 97);
            this.lblWriteFile.Name = "lblWriteFile";
            this.lblWriteFile.Size = new System.Drawing.Size(74, 13);
            this.lblWriteFile.TabIndex = 3;
            this.lblWriteFile.Text = "Απομένουν ...";
            // 
            // btn1
            // 
            this.btn1.Location = new System.Drawing.Point(49, 46);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(133, 23);
            this.btn1.TabIndex = 4;
            this.btn1.Text = "Εκτέλεση τώρα";
            this.btn1.UseVisualStyleBackColor = true;
            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // btn2
            // 
            this.btn2.Location = new System.Drawing.Point(53, 113);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(133, 23);
            this.btn2.TabIndex = 5;
            this.btn2.Text = "Εκτέλεση τώρα";
            this.btn2.UseVisualStyleBackColor = true;
            this.btn2.Click += new System.EventHandler(this.btn2_Click);
            // 
            // mynotifyicon
            // 
            this.mynotifyicon.BalloonTipText = "Διαχείριση αρχείων";
            this.mynotifyicon.Icon = ((System.Drawing.Icon)(resources.GetObject("mynotifyicon.Icon")));
            this.mynotifyicon.Text = "Διαχείριση αρχείων";
            this.mynotifyicon.Visible = true;
            this.mynotifyicon.DoubleClick += new System.EventHandler(this.mynotifyicon_DoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(543, 200);
            this.Controls.Add(this.btn2);
            this.Controls.Add(this.btn1);
            this.Controls.Add(this.lblWriteFile);
            this.Controls.Add(this.lblReadFile);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Διαχείριση αρχείων";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFD;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblReadFile;
        private System.Windows.Forms.Label lblWriteFile;
        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.Button btn2;
        private System.Windows.Forms.NotifyIcon mynotifyicon;
    }
}

