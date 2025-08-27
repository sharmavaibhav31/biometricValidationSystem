namespace TMF20DemoApp
{
    partial class TMF20Demo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TMF20Demo));
            this.btn_deviceinfo = new System.Windows.Forms.Button();
            this.btn_capture = new System.Windows.Forms.Button();
            this.rBtn_fingerprint1 = new System.Windows.Forms.RadioButton();
            this.rBtn_fingerprint2 = new System.Windows.Forms.RadioButton();
            this.btn_match = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.statusBox = new System.Windows.Forms.RichTextBox();
            this.btn_isDeviceConnected = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_deviceinfo
            // 
            this.btn_deviceinfo.Location = new System.Drawing.Point(512, 271);
            this.btn_deviceinfo.Name = "btn_deviceinfo";
            this.btn_deviceinfo.Size = new System.Drawing.Size(91, 23);
            this.btn_deviceinfo.TabIndex = 2;
            this.btn_deviceinfo.Text = "Device Info";
            this.btn_deviceinfo.UseVisualStyleBackColor = true;
            this.btn_deviceinfo.Click += new System.EventHandler(this.btn_deviceinfo_Click);
            // 
            // btn_capture
            // 
            this.btn_capture.Location = new System.Drawing.Point(345, 306);
            this.btn_capture.Name = "btn_capture";
            this.btn_capture.Size = new System.Drawing.Size(100, 23);
            this.btn_capture.TabIndex = 3;
            this.btn_capture.Text = "Capture";
            this.btn_capture.UseVisualStyleBackColor = true;
            this.btn_capture.Click += new System.EventHandler(this.btn_capture_Click);
            // 
            // rBtn_fingerprint1
            // 
            this.rBtn_fingerprint1.AutoSize = true;
            this.rBtn_fingerprint1.Checked = true;
            this.rBtn_fingerprint1.Location = new System.Drawing.Point(334, 235);
            this.rBtn_fingerprint1.Name = "rBtn_fingerprint1";
            this.rBtn_fingerprint1.Size = new System.Drawing.Size(80, 17);
            this.rBtn_fingerprint1.TabIndex = 4;
            this.rBtn_fingerprint1.TabStop = true;
            this.rBtn_fingerprint1.Text = "Fingerprint1";
            this.rBtn_fingerprint1.UseVisualStyleBackColor = true;
            // 
            // rBtn_fingerprint2
            // 
            this.rBtn_fingerprint2.AutoSize = true;
            this.rBtn_fingerprint2.Location = new System.Drawing.Point(504, 235);
            this.rBtn_fingerprint2.Name = "rBtn_fingerprint2";
            this.rBtn_fingerprint2.Size = new System.Drawing.Size(80, 17);
            this.rBtn_fingerprint2.TabIndex = 5;
            this.rBtn_fingerprint2.Text = "Fingerprint2";
            this.rBtn_fingerprint2.UseVisualStyleBackColor = true;
            // 
            // btn_match
            // 
            this.btn_match.Location = new System.Drawing.Point(451, 306);
            this.btn_match.Name = "btn_match";
            this.btn_match.Size = new System.Drawing.Size(118, 23);
            this.btn_match.TabIndex = 6;
            this.btn_match.Text = "Match";
            this.btn_match.UseVisualStyleBackColor = true;
            this.btn_match.Click += new System.EventHandler(this.btn_match_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = global::TMF20DemoApp.Properties.Resources.blank;
            this.pictureBox1.Location = new System.Drawing.Point(307, 52);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(120, 160);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Image = global::TMF20DemoApp.Properties.Resources.blank;
            this.pictureBox2.Location = new System.Drawing.Point(483, 52);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(120, 160);
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Image = global::TMF20DemoApp.Properties.Resources.blank;
            this.pictureBox.Location = new System.Drawing.Point(70, 52);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(150, 200);
            this.pictureBox.TabIndex = 9;
            this.pictureBox.TabStop = false;
            // 
            // statusBox
            // 
            this.statusBox.BackColor = System.Drawing.Color.White;
            this.statusBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBox.Location = new System.Drawing.Point(24, 271);
            this.statusBox.Name = "statusBox";
            this.statusBox.ReadOnly = true;
            this.statusBox.Size = new System.Drawing.Size(232, 87);
            this.statusBox.TabIndex = 10;
            this.statusBox.Text = "";
            // 
            // btn_isDeviceConnected
            // 
            this.btn_isDeviceConnected.Location = new System.Drawing.Point(388, 271);
            this.btn_isDeviceConnected.Name = "btn_isDeviceConnected";
            this.btn_isDeviceConnected.Size = new System.Drawing.Size(118, 23);
            this.btn_isDeviceConnected.TabIndex = 11;
            this.btn_isDeviceConnected.Text = "IsDeviceConnected";
            this.btn_isDeviceConnected.UseVisualStyleBackColor = true;
            this.btn_isDeviceConnected.Click += new System.EventHandler(this.btn_isDeviceConnected_Click);
            // 
            // btn_refresh
            // 
            this.btn_refresh.Location = new System.Drawing.Point(307, 271);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(75, 23);
            this.btn_refresh.TabIndex = 12;
            this.btn_refresh.Text = "Reset";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // btn_save
            // 
            this.btn_save.Enabled = false;
            this.btn_save.Location = new System.Drawing.Point(412, 335);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 23);
            this.btn_save.TabIndex = 13;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // TMF20Demo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 389);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btn_refresh);
            this.Controls.Add(this.btn_isDeviceConnected);
            this.Controls.Add(this.statusBox);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btn_match);
            this.Controls.Add(this.rBtn_fingerprint2);
            this.Controls.Add(this.rBtn_fingerprint1);
            this.Controls.Add(this.btn_capture);
            this.Controls.Add(this.btn_deviceinfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TMF20Demo";
            this.Text = "TMF20Demo";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_deviceinfo;
        private System.Windows.Forms.Button btn_capture;
        private System.Windows.Forms.RadioButton rBtn_fingerprint1;
        private System.Windows.Forms.RadioButton rBtn_fingerprint2;
        private System.Windows.Forms.Button btn_match;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.RichTextBox statusBox;
        private System.Windows.Forms.Button btn_isDeviceConnected;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.Button btn_save;
    }
}

