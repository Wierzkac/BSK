namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
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
            this.ZapiszJako = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // WybierzPlik
            // 
            this.WybierzPlik.Location = new System.Drawing.Point(95, 12);
            this.WybierzPlik.Name = "WybierzPlik";
            this.WybierzPlik.Size = new System.Drawing.Size(130, 48);
            this.WybierzPlik.TabIndex = 0;
            this.WybierzPlik.Text = "Wybierz plik do zaszyfrowania";
            this.WybierzPlik.UseVisualStyleBackColor = true;
            this.WybierzPlik.Click += new System.EventHandler(this.WybierzPlik_Click);
            // 
            // ZapiszJako
            // 
            this.ZapiszJako.Location = new System.Drawing.Point(95, 402);
            this.ZapiszJako.Name = "ZapiszJako";
            this.ZapiszJako.Size = new System.Drawing.Size(130, 36);
            this.ZapiszJako.TabIndex = 1;
            this.ZapiszJako.Text = "Zapisz plik jako...";
            this.ZapiszJako.UseVisualStyleBackColor = true;
            this.ZapiszJako.Click += new System.EventHandler(this.ZapiszJako_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Wczytałeś plik:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(12, 110);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(307, 22);
            this.textBox1.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 450);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ZapiszJako);
            this.Controls.Add(this.WybierzPlik);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button WybierzPlik;
        private System.Windows.Forms.Button ZapiszJako;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
    }
}

