namespace WindowsFormsApp1
{
    partial class Progress_odbierania_pliku
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
            this.ProgressOdbioruPliku = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ProgressOdbioruPliku
            // 
            this.ProgressOdbioruPliku.Location = new System.Drawing.Point(12, 54);
            this.ProgressOdbioruPliku.Name = "ProgressOdbioruPliku";
            this.ProgressOdbioruPliku.Size = new System.Drawing.Size(198, 23);
            this.ProgressOdbioruPliku.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(35, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Stan odbioru pliku:";
            // 
            // Progress_odbierania_pliku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 97);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ProgressOdbioruPliku);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Progress_odbierania_pliku";
            this.Text = "Stan odbioru";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ProgressBar ProgressOdbioruPliku;
        private System.Windows.Forms.Label label1;
    }
}