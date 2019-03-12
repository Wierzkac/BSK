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
        private const string NAME = "SuperEncrypter";

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
            
            //zahashowanie nazwy używkownika
            string encrytpedUserName = Encoding.Default.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(loginTextBox.Text)));

            //sprawdzenie czy występuje już taki użytkownik jakiego nazwę wpisano
            if (Registry.CurrentUser.OpenSubKey(NAME).OpenSubKey(encrytpedUserName) == null)
            {
                MessageBox.Show("Taki użytkownik nie istnieje!\nWprowadź inną nazwę użytkownika!", "Błędna nazwa użytkownika", MessageBoxButtons.OK);
                return;
            }


            // wysłanie klucza publicznego do serwera <========================================================================== PUB KEY SENDING =========================



            
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
            byte[] decprivateKey = DecryptPrivateKey(hashedPassword);
            byte[] sessionKey = DecryptRSA(receivedSessionKey, decprivateKey);
            byte[] IV = DecryptRSA(receivedIV, decprivateKey);
            byte[] message =  DecryptMessage(sessionKey, IV, receivedMessage, cipherMode);
            ByteArrayToFile(message);

        }

        private byte[] DecryptPrivateKey(byte[] hashedPassword)
        {
            // wektor inicjujący do odszyfrowania hasła
            byte[] IV = new byte[16];
            for (int i = 0; i < 16; i++)
                IV[i] = 0;

            //zahasowanie nazwy użytkownika do odczytu zaszyfrowanego klucza prywatnego
            string encrytpedUserName = Encoding.Default.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(loginTextBox.Text)));

            //odczyt zaszyfrowanego klucza prywatnego
            byte[] encryptedPrivateKey = (byte[])Registry.CurrentUser.OpenSubKey(NAME).OpenSubKey(encrytpedUserName).GetValue("P_Key");

            return new Encoder(hashedPassword, IV).DecryptByCBC(encryptedPrivateKey);
        }

        public byte[] HashUserPassword(string plainPassword)
        {
            return SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(plainPassword));
        }

        private byte[] DecryptRSA(byte[] encryptedSessionKey, byte[] decryptedPrivateKey)
        {
            //zahasowanie nazwy użytkownika do danych z rejestru
            string encrytpedUserName = Encoding.Default.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(loginTextBox.Text)));

            //uzupełnienie parametrów do deszyfrowania klucza sesyjnego
            RSAParameters pars = new RSAParameters()
            {
                Modulus = (byte[])Registry.CurrentUser.OpenSubKey(NAME).OpenSubKey(encrytpedUserName).GetValue("Modulus"),
                Exponent = (byte[])Registry.CurrentUser.OpenSubKey(NAME).OpenSubKey(encrytpedUserName).GetValue("Exponent"),
                InverseQ = (byte[])Registry.CurrentUser.OpenSubKey(NAME).OpenSubKey(encrytpedUserName).GetValue("InverseQ"),
                DP = (byte[])Registry.CurrentUser.OpenSubKey(NAME).OpenSubKey(encrytpedUserName).GetValue("DP"),
                P = (byte[])Registry.CurrentUser.OpenSubKey(NAME).OpenSubKey(encrytpedUserName).GetValue("P"),
                DQ = (byte[])Registry.CurrentUser.OpenSubKey(NAME).OpenSubKey(encrytpedUserName).GetValue("DQ"),
                Q = (byte[])Registry.CurrentUser.OpenSubKey(NAME).OpenSubKey(encrytpedUserName).GetValue("Q"),
                D = decryptedPrivateKey
            };

            //stworzenie decryptora i import parametrów
            RSACryptoServiceProvider decryptor = new RSACryptoServiceProvider();
            decryptor.ImportParameters(pars);

            //zwrócenie wartości odszyfrowanego klucza sesji
            return decryptor.Decrypt(encryptedSessionKey, false);
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

        private void addUserButton_Click(object sender, EventArgs e)
        {
           new AddUserForm().ShowDialog();
        }
    }
}
