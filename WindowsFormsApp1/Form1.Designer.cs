using System;

namespace WindowsFormsApp1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.WybierzPlik = new System.Windows.Forms.Button();
            this.encodingModeComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.colorDialog2 = new System.Windows.Forms.ColorDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // WybierzPlik
            // 
            this.WybierzPlik.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.WybierzPlik.Location = new System.Drawing.Point(10, 28);
            this.WybierzPlik.Margin = new System.Windows.Forms.Padding(2);
            this.WybierzPlik.Name = "WybierzPlik";
            this.WybierzPlik.Size = new System.Drawing.Size(71, 24);
            this.WybierzPlik.TabIndex = 0;
            this.WybierzPlik.Text = "Otwórz";
            this.WybierzPlik.UseVisualStyleBackColor = true;
            this.WybierzPlik.Click += new System.EventHandler(this.WybierzPlik_Click);
            // 
            // encodingModeComboBox
            // 
            this.encodingModeComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.encodingModeComboBox.FormattingEnabled = true;
            this.encodingModeComboBox.Items.AddRange(new object[] {
            "ECB",
            "CBC",
            "CFB",
            "OFB"});
            this.encodingModeComboBox.Location = new System.Drawing.Point(199, 28);
            this.encodingModeComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.encodingModeComboBox.Name = "encodingModeComboBox";
            this.encodingModeComboBox.Size = new System.Drawing.Size(120, 24);
            this.encodingModeComboBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(196, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Wybierz tryb szyfrowania";
            // 
            // startButton
            // 
            this.startButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.startButton.Location = new System.Drawing.Point(238, 142);
            this.startButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(81, 26);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "Wyślij";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 69);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Wczytałeś plik:";
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(10, 85);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(307, 20);
            this.textBox1.TabIndex = 3;
            // 
            // colorDialog2
            // 
            this.colorDialog2.Color = System.Drawing.Color.Maroon;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 108);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Stan połączenia:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(9, 121);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Brak wykrytego clienta";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 7);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(151, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Wybierz plik do zaszyfrowania:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(80, 248);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 19);
            this.button1.TabIndex = 7;
            this.button1.Text = "wyslij";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 179);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.encodingModeComboBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.WybierzPlik);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "SuperEncrypter";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button WybierzPlik;
        private System.Windows.Forms.ComboBox encodingModeComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button startButton;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ColorDialog colorDialog2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
    }
}

