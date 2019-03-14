namespace WindowsFormsApp1
{
    partial class Form2
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
            this.label1 = new System.Windows.Forms.Label();
            this.fileNameLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.encryptionModeLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.encodingProgressBar = new System.Windows.Forms.ProgressBar();
            this.SendingProgressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Wybrany plik:";
            // 
            // fileNameLabel
            // 
            this.fileNameLabel.AutoSize = true;
            this.fileNameLabel.Location = new System.Drawing.Point(133, 33);
            this.fileNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Size = new System.Drawing.Size(46, 17);
            this.fileNameLabel.TabIndex = 1;
            this.fileNameLabel.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 71);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(174, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Wybrany tryb szyfrowania:";
            // 
            // encryptionModeLabel
            // 
            this.encryptionModeLabel.AutoSize = true;
            this.encryptionModeLabel.Location = new System.Drawing.Point(214, 71);
            this.encryptionModeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.encryptionModeLabel.Name = "encryptionModeLabel";
            this.encryptionModeLabel.Size = new System.Drawing.Size(46, 17);
            this.encryptionModeLabel.TabIndex = 3;
            this.encryptionModeLabel.Text = "label4";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 128);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Postęp szyfrowania";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 204);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Postęp wysyłania";
            // 
            // encodingProgressBar
            // 
            this.encodingProgressBar.Location = new System.Drawing.Point(36, 148);
            this.encodingProgressBar.Margin = new System.Windows.Forms.Padding(4);
            this.encodingProgressBar.Name = "encodingProgressBar";
            this.encodingProgressBar.Size = new System.Drawing.Size(327, 28);
            this.encodingProgressBar.TabIndex = 6;
            // 
            // SendingProgressBar
            // 
            this.SendingProgressBar.Location = new System.Drawing.Point(36, 224);
            this.SendingProgressBar.Margin = new System.Windows.Forms.Padding(4);
            this.SendingProgressBar.Name = "SendingProgressBar";
            this.SendingProgressBar.Size = new System.Drawing.Size(327, 28);
            this.SendingProgressBar.TabIndex = 7;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 321);
            this.Controls.Add(this.SendingProgressBar);
            this.Controls.Add(this.encodingProgressBar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.encryptionModeLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.fileNameLabel);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form2";
            this.Text = "SuperEncrypter";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label fileNameLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label encryptionModeLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar encodingProgressBar;
        private System.Windows.Forms.ProgressBar SendingProgressBar;


        public System.Windows.Forms.ProgressBar EncodingProgressBar
        {
            get { return encodingProgressBar; }
        }


    }
}