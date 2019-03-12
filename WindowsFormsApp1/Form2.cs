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
using System.Security.Cryptography;

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

            Thread encoding = new Thread(EncryptMessage);
            encoding.Start();

        }

        private void EncryptMessage()
        {
            Encoder enc = new Encoder();
            byte[] encrypted = null;

            string choosenMode = null;
            mainForm.Invoke(new Action(() =>
            {
                choosenMode = mainForm.ChoosenEncodingMode().ToString();
            }));


            switch(choosenMode)
            {
                case "ECB":
                    encrypted = enc.EncryptByECB(mainForm.fileData);
                    Console.WriteLine("ECB");
                    break;

                case "CBC":
                    encrypted = enc.EncryptByCBC(mainForm.fileData);
                    Console.WriteLine("CBC");
                    break;

                case "CFB":
                    encrypted = enc.EncryptByCFB(mainForm.fileData);
                    Console.WriteLine("CFB");
                    break;

                case "OFB":
                    encrypted = enc.EncryptByOFB(mainForm.fileData);
                    //MessageBox.Show("Ten tryb szyfrowania nie został jeszcze zaimplementowany!\n\nWybierz inny tryb!", "Błąd trybu szyfrowania!", MessageBoxButtons.OK);
                    break;

                default:
                    MessageBox.Show("Błąd wyboru trybu szyfrowania!\n Wybierz tryb jeszcze raz", "Błąd trybu szyfrowania!", MessageBoxButtons.OK);
                    return;
            }

            //zaznaczenie progresu szyfrowania
            this.Invoke(new Action(() =>
            {
                this.encodingProgressBar.Value = this.encodingProgressBar.Maximum;
            }
            ));



            /* *************************************************************************************************************************************************************************
             *                                                                                                                                 TU PROPONUJĘ ODBIÓR
             * *************************************************************************************************************************************************************************
             */

            // dane pobrane od usera
           byte[] Modulus = null;
           byte[] Exponent = null;


           byte[] encryptedSessionKey = EncryptRSA(Exponent, Modulus, enc.Key);
           byte[] encryptedIV = EncryptRSA(Exponent, Modulus, enc.IV);


           /* *************************************************************************************************************************************************************************
            * Pod encrypted jest zaszyfrowana wiadomość
            * Klucz sesyjny pod encryptedSessionKey                                                                                            TU PROPONUJĘ WYSYŁANIE
            * Wektor inicjujący pod encryptedIV
            * *************************************************************************************************************************************************************************
            */

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

        private byte[] EncryptRSA(byte[] exponent, byte[] modulus, byte[] SessionKey)
        {
            //przygotowanie szyfratora
            RSACryptoServiceProvider rsaCrypto = new RSACryptoServiceProvider();

            //wprowadzenie parametrów pobranych od klienta
            RSAParameters pars = new RSAParameters()
            {
                Modulus = modulus,
                Exponent = exponent
            };

            //import parametrów i zaszyfrowanie klucza sesji
            rsaCrypto.ImportParameters(pars);
            return rsaCrypto.Encrypt(SessionKey, true); 
        }

    }
}
