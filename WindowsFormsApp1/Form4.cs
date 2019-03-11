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
using Microsoft.Win32;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace WindowsFormsApp1
{

    public partial class Form4 : Form
    {
        private byte[] _receivedFile = null;
        private TcpClient client;
        private NetworkStream ns;
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                client = new TcpClient("127.0.0.1", 11000);
                ns = client.GetStream();
                Invoke(new Action(() =>
                {
                    label4.Text = "Nawiązano połączenie z klientem";
                    label4.ForeColor = Color.Green;
                }));

            }).Start();

        }
        public static void AppendToFile(string fileToWrite, byte[] DT)
        {
            using (FileStream FS = new FileStream(fileToWrite, File.Exists(fileToWrite) ? FileMode.Append : FileMode.OpenOrCreate, FileAccess.Write))
            {
                FS.Write(DT, 0, DT.Length);
                FS.Close();
            }
        }
        private void saveButton_Click(object sender, EventArgs e)
        {

            if (label4.Text == "Brak wykrytego serwera")
            {
                MessageBox.Show("Nie wykryto połączenia z serwerem!", "Błąd połączenia!", MessageBoxButtons.OK);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|mp3 files (*.mp3)|*.mp3|png files (*.png)|*.png|avi files (*.avi)|*.avi|jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            saveFileDialog.ShowDialog();
            FileStream fs = (FileStream)saveFileDialog.OpenFile();
            fs.Close();
            
            byte[] tmp = new byte[1024];
            while (ns.DataAvailable)
            {
                ns.Read(tmp, 0, tmp.Length);
                AppendToFile(fs.Name, tmp.ToArray());
            }
            //Tutaj Kacprowa magia <================================================================================================== ODBIÓR PLIKU =======================

            /*
             *    W WIADOMOŚCI POTRZEBUJĘ:
             * 1. Klucz sesyjny
             * 2. Wektor inicjujący
             * 3. Tryb szyfrowania
             * 4. Wiadomość
             * */

            //tu trzeba wrzucić zaszyfrowany klucz odebrany z serwera
            byte[] receivedSessionKey = new byte[32];

            //tu trzeba wrzucić zaszyfrowaną wiadomość
            byte[] receivedMessage = new byte[0];

            //       Dwóch poniższych chyba nie trzeba szyfrować

            //tu trzeba wrzucić wektor inicjujący
            byte[] receivedIV = new byte[16];

            //tu trzeba wrzucić tryb szyfrowania
            string cipherMode = "";


            /*
        *      SCENARIUSZ INTERAKCJI
        *  0. Pobranie całej wiadomości
        *  1. Zahashowanie hasła użytkownika
        *  2. Odszyfrowanie klucza prywatnego
        *  3. Odszyfrowanie klucza sesyjnego
        *  4. Odszyfrowanie wiadomosci
        *  5. Zapis 
        */

            byte[] hashedPassword = HashUserPassword(passwordTextBox.Text);
            byte[] privateKey = DecryptPrivateKey(hashedPassword);
            byte[] sessionKey = DecryptSessionKey(receivedSessionKey, privateKey);
            byte[] message =  DecryptMessage(sessionKey, receivedIV, receivedMessage, cipherMode);
            ByteArrayToFile(message);

        }
        private void CreateFirstRegistration()
        {
            EncryptPrivateKey(Aes.Create().Key, HashUserPassword("Plagiacik12"));
        }

        private void EncryptPrivateKey(byte[] privateKey, byte[] hashedPassword)
        {
            // wektor inicjujący do zaszyfrowania hasła
            byte[] IV = new byte[16];
            for (int i = 0; i < 16; i++)
                IV[i] = 0;

            // zaszyfrowanie klucza prywatnego za pomocą skrótu hasła
            byte[] encryptedPrivateKey = new Encoder(hashedPassword, IV).EncryptByCBC(privateKey);

            //zapis zaszyfrowanego klucza prywatnego do rejetru
            Registry.CurrentUser.CreateSubKey("SuperEncrypter").SetValue("P_KEY", encryptedPrivateKey);
        }

        private byte[] DecryptPrivateKey(byte[] hashedPassword)
        {
            // wektor inicjujący do odszyfrowania hasła
            byte[] IV = new byte[16];
            for (int i = 0; i < 16; i++)
                IV[i] = 0;

            //odczyt zaszyfrowanego klucza prywatnego
            byte[] encryptedPrivateKey = (byte[])Registry.CurrentUser.OpenSubKey("SuperEncrypter").GetValue("P_KEY");

            return new Encoder(hashedPassword, IV).DecryptByCBC(encryptedPrivateKey);
        }

        private byte[] HashUserPassword(string plainPassword)
        {
            return MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(plainPassword));
        }


        private byte[] DecryptSessionKey(byte[] encryptedSessionKey, byte[] privateKey)
        {
            /*
             *  JAK ODSZYFROWAĆ KLUCZ SESYJNY?
             *  
             *  Mam klucz prywatny
             *  
             *  I chyba muszę użyć algorytmu RSA do odszyfrowania
             *  
             *          ...tylko jeszcze nie wiem jak
             */

            return new byte[1];
        }

        private byte[] DecryptMessage(byte[] Key, byte[] IV, byte[] message, string cipherMode)
        {
            Encoder enc = new Encoder(Key, IV);
            byte[] decrypted = null;

            switch (cipherMode)
            {
                case "ECB":
                    decrypted = enc.DecryptByECB(message);
                    break;

                case "CBC":
                    decrypted = enc.DecryptByCBC(message);
                    break;

                case "CFB":
                    decrypted = enc.DecryptByCFB(message);
                    break;

                case "OFB":
                    decrypted = enc.DecryptByOFB(message);
                    break;

                default:
                    MessageBox.Show("Niepoprawny tryb szyfrowania!\nPobierz plik jeszcze raz!", "Błąd trybu szyfrowania!", MessageBoxButtons.OK);
                    break;
            }

            return decrypted;
        }

        private void ByteArrayToFile(byte[] array)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|mp3 files (*.mp3)|*.mp3|png files (*.png)|*.png|avi files (*.avi)|*.avi|jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            saveFileDialog.ShowDialog();

            FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile();
            fs.Close();
            File.WriteAllBytes(fs.Name, array);
        }



    }
}
