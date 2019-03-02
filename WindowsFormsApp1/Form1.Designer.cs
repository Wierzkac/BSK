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
            this.SuspendLayout();
            // 
            // WybierzPlik
            // 
            this.WybierzPlik.Location = new System.Drawing.Point(74, 173);
            this.WybierzPlik.Name = "WybierzPlik";
            this.WybierzPlik.Size = new System.Drawing.Size(130, 48);
            this.WybierzPlik.TabIndex = 0;
            this.WybierzPlik.Text = "Wybierz plik do zaszyfrowania";
            this.WybierzPlik.UseVisualStyleBackColor = true;
            this.WybierzPlik.Click += new System.EventHandler(this.WybierzPlik_Click);
            // 
            // ZapiszJako
            // 
            this.ZapiszJako.Location = new System.Drawing.Point(524, 185);
            this.ZapiszJako.Name = "ZapiszJako";
            this.ZapiszJako.Size = new System.Drawing.Size(143, 36);
            this.ZapiszJako.TabIndex = 1;
            this.ZapiszJako.Text = "Zapisz plik jako...";
            this.ZapiszJako.UseVisualStyleBackColor = true;
            this.ZapiszJako.Click += new System.EventHandler(this.ZapiszJako_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ZapiszJako);
            this.Controls.Add(this.WybierzPlik);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button WybierzPlik;
        private System.Windows.Forms.Button ZapiszJako;
    }
}

