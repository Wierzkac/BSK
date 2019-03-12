using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class AddUserForm : Form
    {
        private const string NAME = "SuperEncrypter";
        public AddUserForm()
        {
            InitializeComponent();
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {

            //zaszyfrowanie nazwy użytkownika
            string encrytpedUserName = Encoding.Default.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(loginTextBox.Text)));

            //sprawdzenie czy nie występuje już taki użytkownik
            if (Registry.CurrentUser.OpenSubKey(NAME).OpenSubKey(encrytpedUserName) != null)
            {
                MessageBox.Show("Taki użytkownik już istnieje!\nWprowadź inną nazwę użytkownika!", "Błędna nazwa użytkownika", MessageBoxButtons.OK);
                return;
            }

            //filtracja zerowego hasła
            if(passwordTextBox.Text.Length == 0)
            {
                MessageBox.Show("Należy podać hasło!", "Błędne hasło", MessageBoxButtons.OK);
                return;
            }

            //sprawdzenie czy hasła są zgodne
            if(passwordTextBox.Text != repeatPasswordTextBox.Text)
            {
                MessageBox.Show("Hasła nie są identyczne!\n Wprowadź hasło ponownie", "Błędne hasło", MessageBoxButtons.OK);
                return;
            }

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            // generowanie klucza prywatnego i publicznego
            RSAParameters parameters = rsa.ExportParameters(true);
            

            // wpisanie klucza publicznego
            Registry.CurrentUser.CreateSubKey(NAME).CreateSubKey(encrytpedUserName).SetValue("Exponent", parameters.Exponent);
            Registry.CurrentUser.CreateSubKey(NAME).CreateSubKey(encrytpedUserName).SetValue("Modulus", parameters.Modulus);

            //zaszyfrowanie klucza prywatnego w trybie CBC za pomocą hasha hasła użytkownika
            byte[] encryptedD = EncryptPrivateData(parameters.D, SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(passwordTextBox.Text)));

            //wpisanie zaszyfrowanego klucza prywatnego do rejestru
            Registry.CurrentUser.CreateSubKey(NAME).CreateSubKey(encrytpedUserName).SetValue("P_Key", encryptedD);

            Registry.CurrentUser.CreateSubKey(NAME).CreateSubKey(encrytpedUserName).SetValue("DP", parameters.DP);
            Registry.CurrentUser.CreateSubKey(NAME).CreateSubKey(encrytpedUserName).SetValue("DQ", parameters.DQ);
            Registry.CurrentUser.CreateSubKey(NAME).CreateSubKey(encrytpedUserName).SetValue("P", parameters.P);
            Registry.CurrentUser.CreateSubKey(NAME).CreateSubKey(encrytpedUserName).SetValue("Q", parameters.Q);
            Registry.CurrentUser.CreateSubKey(NAME).CreateSubKey(encrytpedUserName).SetValue("InverseQ", parameters.InverseQ);




            TestRSA(parameters.Exponent, parameters.Modulus, Encoding.Default.GetBytes("Mleko"), parameters.D);


            //zamknięcie formy
            this.Close();
        }

        private byte[] EncryptPrivateData(byte[] privateData, byte[] hashedPassword)
        {
            // wektor inicjujący do zaszyfrowania hasła
            byte[] IV = new byte[16];
            for (int i = 0; i < 16; i++)
                IV[i] = 0;

            // zaszyfrowanie klucza prywatnego za pomocą skrótu hasła trybem CBC
            return new Encoder(hashedPassword, IV).EncryptByCBC(privateData);      
        }


        private byte[] EncryptRSA(byte[] exponent, byte[] modulus, byte[] plainData)
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
            return rsaCrypto.Encrypt(plainData, false);
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

        private void TestRSA(byte[] exponent, byte[] modulus, byte[] data, byte[] decryptedPrivateKey)
        {
           byte[] encdata = EncryptRSA(exponent, modulus, data);

           byte[] decdata = DecryptRSA(encdata, decryptedPrivateKey);

           Console.WriteLine(Encoding.Default.GetChars(decdata));

            ;
        }

    }
}
