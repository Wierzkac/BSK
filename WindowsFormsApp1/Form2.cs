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

        private Encoder enc = new Encoder();
        private byte[] encrypted = null;
        private string choosenMode = null;
        private byte[] encryptedSessionKey = null;

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

            Thread.Sleep(4000);

            /* ************************************************************************************************************************************************************************
             *                                                                                                                                 TU PROPONUJĘ ODBIÓR
             * *************************************************************************************************************************************************************************
             */

            // dane pobrane od usera
           byte[] Modulus = mainForm.modulus;
           byte[] Exponent = mainForm.exponent;


           encryptedSessionKey = EncryptRSA(Exponent, Modulus, enc.Key);

            Invoke(new Action(() =>
            {
                trySendFile();
            }));
            /* *************************************************************************************************************************************************************************
             * Pod encrypted jest zaszyfrowana wiadomość
             * Klucz sesyjny pod encryptedSessionKey                                                                                            TU PROPONUJĘ WYSYŁANIE
             * Wektor inicjujący pod encryptedIV
             * *************************************************************************************************************************************************************************
             */

        }
        private void trySendFile()
        {
            try
            {
                mainForm.ns = mainForm.client.GetStream();
                mainForm.ns.Write(encryptedSessionKey, 0, encryptedSessionKey.Length);
                mainForm.ns.Write(enc.IV, 0, enc.IV.Length);
                mainForm.ns.Write(Encoding.ASCII.GetBytes(choosenMode), 0, Encoding.ASCII.GetBytes(choosenMode).Length);
                mainForm.ns.Write(encrypted, 0, encrypted.Length);
                mainForm.ns.Close();
                mainForm.client.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
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
