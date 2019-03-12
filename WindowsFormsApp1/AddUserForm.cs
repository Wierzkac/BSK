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
        private const string NAME_KEY = "SuperEncrypter";
        private const string SOFTWARE_KEY = "Software";
        private const string PUBLIC_KEY = "Public";
        private const string PRIVATE_KEY = "Private";

        public AddUserForm()
        {
            InitializeComponent();
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            Registry.CurrentUser.CreateSubKey(SOFTWARE_KEY).CreateSubKey(NAME_KEY);

            //zaszyfrowanie nazwy użytkownika
            string encrytpedUserName = Encoding.Default.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(loginTextBox.Text)));

            //sprawdzenie czy nie występuje już taki użytkownik
            if (Registry.CurrentUser.OpenSubKey(SOFTWARE_KEY).OpenSubKey(NAME_KEY).OpenSubKey(encrytpedUserName) != null)
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
            Registry.CurrentUser.CreateSubKey(SOFTWARE_KEY).CreateSubKey(NAME_KEY).CreateSubKey(encrytpedUserName).CreateSubKey(PUBLIC_KEY).SetValue("Exponent", parameters.Exponent);
            Registry.CurrentUser.CreateSubKey(SOFTWARE_KEY).CreateSubKey(NAME_KEY).CreateSubKey(encrytpedUserName).CreateSubKey(PUBLIC_KEY).SetValue("Modulus", parameters.Modulus);


            //zahashowanie hasła użytkownika
            byte[] hashedPassword = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(passwordTextBox.Text));
           
            //zaszyfrowanie klucza prywatnego w trybie CBC za pomocą hasha hasła użytkownika
            byte[] encryptedD = EncryptPrivateData(parameters.D, hashedPassword);
            byte[] encryptedDP = EncryptPrivateData(parameters.DP, hashedPassword);
            byte[] encryptedDQ = EncryptPrivateData(parameters.DQ, hashedPassword);
            byte[] encryptedP = EncryptPrivateData(parameters.P, hashedPassword);
            byte[] encryptedQ = EncryptPrivateData(parameters.Q, hashedPassword);
            byte[] encryptedInverseQ = EncryptPrivateData(parameters.InverseQ, hashedPassword);

            //wpisanie zaszyfrowanego klucza prywatnego do rejestru
            Registry.CurrentUser.CreateSubKey(SOFTWARE_KEY).CreateSubKey(NAME_KEY).CreateSubKey(encrytpedUserName).CreateSubKey(PRIVATE_KEY).SetValue("D", encryptedD);
            Registry.CurrentUser.CreateSubKey(SOFTWARE_KEY).CreateSubKey(NAME_KEY).CreateSubKey(encrytpedUserName).CreateSubKey(PRIVATE_KEY).SetValue("DP", encryptedDP);
            Registry.CurrentUser.CreateSubKey(SOFTWARE_KEY).CreateSubKey(NAME_KEY).CreateSubKey(encrytpedUserName).CreateSubKey(PRIVATE_KEY).SetValue("DQ", encryptedDQ);
            Registry.CurrentUser.CreateSubKey(SOFTWARE_KEY).CreateSubKey(NAME_KEY).CreateSubKey(encrytpedUserName).CreateSubKey(PRIVATE_KEY).SetValue("P", encryptedP);
            Registry.CurrentUser.CreateSubKey(SOFTWARE_KEY).CreateSubKey(NAME_KEY).CreateSubKey(encrytpedUserName).CreateSubKey(PRIVATE_KEY).SetValue("Q", encryptedQ);
            Registry.CurrentUser.CreateSubKey(SOFTWARE_KEY).CreateSubKey(NAME_KEY).CreateSubKey(encrytpedUserName).CreateSubKey(PRIVATE_KEY).SetValue("InverseQ", encryptedInverseQ);


           TestRSA(parameters.Exponent, parameters.Modulus, Encoding.Default.GetBytes("Mleko"));

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

        private byte[] DecryptRSA(byte[] encryptedSessionKey)
        {
            //zahasowanie nazwy użytkownika do danych z rejestru
            string encrytpedUserName = Encoding.Default.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(loginTextBox.Text)));

            //zahashowanie hasła
            byte[] hashedPassword = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(passwordTextBox.Text));

            //pobranie zaszyfrowanych danych z rejestru
            byte[] InverseQ = (byte[])Registry.CurrentUser.OpenSubKey(SOFTWARE_KEY).OpenSubKey(NAME_KEY).OpenSubKey(encrytpedUserName).OpenSubKey(PRIVATE_KEY).GetValue("InverseQ");
            byte[] DP = (byte[])Registry.CurrentUser.OpenSubKey(SOFTWARE_KEY).OpenSubKey(NAME_KEY).OpenSubKey(encrytpedUserName).OpenSubKey(PRIVATE_KEY).GetValue("DP");
            byte[] P = (byte[])Registry.CurrentUser.OpenSubKey(SOFTWARE_KEY).OpenSubKey(NAME_KEY).OpenSubKey(encrytpedUserName).OpenSubKey(PRIVATE_KEY).GetValue("P");
            byte[] DQ = (byte[])Registry.CurrentUser.OpenSubKey(SOFTWARE_KEY).OpenSubKey(NAME_KEY).OpenSubKey(encrytpedUserName).OpenSubKey(PRIVATE_KEY).GetValue("DQ");
            byte[] Q = (byte[])Registry.CurrentUser.OpenSubKey(SOFTWARE_KEY).OpenSubKey(NAME_KEY).OpenSubKey(encrytpedUserName).OpenSubKey(PRIVATE_KEY).GetValue("Q");
            byte[] D = (byte[])Registry.CurrentUser.OpenSubKey(SOFTWARE_KEY).OpenSubKey(NAME_KEY).OpenSubKey(encrytpedUserName).OpenSubKey(PRIVATE_KEY).GetValue("D");

            //odszyfrowanie danych
            InverseQ = DecryptPrivateData(hashedPassword, InverseQ);
            DP = DecryptPrivateData(hashedPassword, DP);
            P = DecryptPrivateData(hashedPassword, P);
            DQ = DecryptPrivateData(hashedPassword, DQ);
            Q = DecryptPrivateData(hashedPassword, Q);
            D = DecryptPrivateData(hashedPassword, D);

            //uzupełnienie parametrów do deszyfrowania klucza sesyjnego
            RSAParameters pars = new RSAParameters()
            {
                Modulus = (byte[])Registry.CurrentUser.OpenSubKey(SOFTWARE_KEY).OpenSubKey(NAME_KEY).OpenSubKey(encrytpedUserName).OpenSubKey(PUBLIC_KEY).GetValue("Modulus"),
                Exponent = (byte[])Registry.CurrentUser.OpenSubKey(SOFTWARE_KEY).OpenSubKey(NAME_KEY).OpenSubKey(encrytpedUserName).OpenSubKey(PUBLIC_KEY).GetValue("Exponent"),
                InverseQ = InverseQ,
                DP = DP,
                P = P,
                DQ = DQ,
                Q = Q,
                D = D
            };

            //stworzenie decryptora i import parametrów
            RSACryptoServiceProvider decryptor = new RSACryptoServiceProvider();
            decryptor.ImportParameters(pars);

            //zwrócenie wartości odszyfrowanego klucza sesji
            return decryptor.Decrypt(encryptedSessionKey, false);
        }

        private byte[] DecryptPrivateData(byte[] hashedPassword, byte[] encryptedPrivateData)
        {
            // wektor inicjujący do odszyfrowania hasła
            byte[] IV = new byte[16];
            for (int i = 0; i < 16; i++)
                IV[i] = 0;

            return new Encoder(hashedPassword, IV).DecryptByCBC(encryptedPrivateData);
        }

        private void TestRSA(byte[] exponent, byte[] modulus, byte[] data)
        {
           byte[] encdata = EncryptRSA(exponent, modulus, data);

           byte[] decdata = DecryptRSA(encdata);

           Console.WriteLine(Encoding.Default.GetChars(decdata));

            ;
        }

    }
}
