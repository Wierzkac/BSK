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
using static AsynchronousClient;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.IO;

namespace WindowsFormsApp1
{

    public partial class Form4 : Form
    {
        private byte[] _receivedFile = null;

        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                StartClient(this);
            }).Start();

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if(label4.Text == "Brak wykrytego serwera")
            {
                MessageBox.Show("Nie wykryto połączenia z serwerem!", "Błąd połączenia!", MessageBoxButtons.OK);
                return;
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
