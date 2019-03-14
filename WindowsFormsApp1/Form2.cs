using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;
using System.Security.Cryptography;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        
        private NetworkStream ns;
        private TcpClient client;
        private TcpListener listener;
        private const string addressip = "192.168.43.94";
        //private const string addressip = "127.0.0.1";
        private Encoder enc = new Encoder();
        private byte[] encrypted = null;
        private string choosenMode = null;
        private byte[] encryptedSessionKey = null;
        private byte[] exponent = new byte[3];
        private byte[] modulus = new byte[128];
        private bool przesylDone = false;

        private Form1 mainForm = null;
        public Form2()
        {
            InitializeComponent();
        }

        public Form2(Form1 mF)
        {
            mainForm = mF;
            InitializeComponent();

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                listener = new TcpListener(IPAddress.Parse(addressip), 11000);
                listener.Start();

                while (true)
                {
                    Console.Write("Waiting for connection...");
                    client = listener.AcceptTcpClient();

                    Console.WriteLine("Connection accepted.");
                    Invoke(new Action(() =>
                    {
                        mainForm.label4.Text = "Nawiązano połączenie z klientem";
                        mainForm.label4.ForeColor = Color.Green;
                    }));
                    ns = client.GetStream();
                    while (!ns.DataAvailable) { }
                    ns.Read(exponent, 0, 3);
                    ns.Read(modulus, 0, 128);
                    przesylDone = true;
                    Console.WriteLine("expodent :{0}", Encoding.Default.GetString(exponent));
                    Console.WriteLine("modulus : {0}", Encoding.Default.GetString(modulus));
                }

            }).Start();

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
                    //encrypted = enc.EncryptByOFB(mainForm.fileData);
                    MessageBox.Show("Ten tryb szyfrowania nie jest wspierany!\n\nWybierz inny tryb!", "Błąd trybu szyfrowania!", MessageBoxButtons.OK);
                    break;

                default:
                    MessageBox.Show("Błąd wyboru trybu szyfrowania!\n Wybierz tryb jeszcze raz", "Błąd trybu szyfrowania!", MessageBoxButtons.OK);
                    return;
            }
            while (!przesylDone) { } // OCZEKIWANIE NA KLUCZ OD KLIENTA

            //zaznaczenie progresu szyfrowania
            this.Invoke(new Action(() =>
            {
                this.encodingProgressBar.Value = this.encodingProgressBar.Maximum;
            }
            ));

            /* ************************************************************************************************************************************************************************
            *                                                                                                                                 TU PROPONUJĘ ODBIÓR
            * *************************************************************************************************************************************************************************
            */

            // dane pobrane od usera
            FileInfo f = new FileInfo(mainForm.openfileDialog1.FileName);
            long s1 = f.Length;
            byte[] fileSize = BitConverter.GetBytes(s1);
            encryptedSessionKey = EncryptRSA(exponent, modulus, enc.Key);
            byte[] fileName = Encoding.ASCII.GetBytes(fileNameLabel.Text);
            try
            {
                ns = client.GetStream();
                ns.Write(encryptedSessionKey, 0, encryptedSessionKey.Length);                                       // Klucz sesyjny
                ns.Write(fileSize, 0, fileSize.Length);                                                             // Wielkosc pliku
                ns.Write(BitConverter.GetBytes(fileName.Length), 0, 4);                                             // Wielkosc nazwy
                ns.Write(fileName, 0, fileName.Length);                                                             // Nazwa pliku
                ns.Write(enc.IV, 0, enc.IV.Length);                                                                 // IV
                ns.Write(Encoding.ASCII.GetBytes(choosenMode), 0, Encoding.ASCII.GetBytes(choosenMode).Length);     // Nazwa trybu szyfrowania
                ns.Write(encrypted, 0, encrypted.Length);                                                           // Plik

                while (!ns.DataAvailable) { }
                byte[] tmpString = new byte[10];
                string sygnal;
                ns.Read(tmpString, 0, tmpString.Length);
                sygnal = Encoding.ASCII.GetString(tmpString);
                if (sygnal == "Udalo sie!")
                    ns.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
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
            return rsaCrypto.Encrypt(SessionKey, false); 
        }

    }
}
