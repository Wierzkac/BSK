using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;


namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        private Form1 mainForm = null;
        public Form2()
        {
            InitializeComponent();
        }

        public Form2(Form1 mF)
        {
            mainForm = mF;
            InitializeComponent();
           
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            fileNameLabel.Text = mainForm.openfileDialog1.SafeFileName;
            encryptionModeLabel.Text = mainForm.ChoosenEncodingMode().ToString();

            progressBarsDefaultSettings();



            Thread encoding = new Thread(startEncoding);
            encoding.Start();



        }

        private void startEncoding()
        {
            Encoder enc = new Encoder();


            byte[] encrypted = enc.EncryptByECB(mainForm.fileData);
            byte[] decrypted = enc.DecryptByECB(encrypted);


            //encrypted = enc.EncryptByCBC(decrypted);
            //decrypted = enc.DecryptByCBC(encrypted);

            //encrypted = enc.EncryptByCFB(decrypted);
            //decrypted = enc.DecryptByCFB(encrypted);

            encrypted = enc.EncryptByOFB(decrypted);
            decrypted = enc.DecryptByOFB(encrypted);

            for (int i = 0; i < mainForm.fileData.Length; i++)
            {
                if(decrypted[i] != mainForm.fileData[i])
                {
                    ;
                }
            }

            ;
        }

        private void progressBarsDefaultSettings()
        {
            encodingProgressBar.Maximum = mainForm.fileData.Length;
            encodingProgressBar.Minimum = 1;
            encodingProgressBar.Value = 1;
            encodingProgressBar.Step = 1;

            SendingProgressBar.Maximum = mainForm.fileData.Length;
            SendingProgressBar.Minimum = 1;
            SendingProgressBar.Value = 1;
            SendingProgressBar.Step = 1;
        }

    }
}
