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
using System.CodeDom.Compiler;

namespace WindowsFormsApp1
{

    public partial class Form4 : Form
    {
        private const string NAME_KEY = "SuperEncrypter";
        private const string SOFTWARE_KEY = "Software";
        private const string PUBLIC_KEY = "Public";
        private const string PRIVATE_KEY = "Private";
        private const string addressip = "127.0.0.1";
        //private const string addressip = "192.168.43.94";
        //private const string addressip = "192.168.43.215";
        private bool recieveFile = false;
        private byte[] _receivedFile = null;
        private TcpClient client;
        private NetworkStream ns;
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        public static void AppendToFile(string fileToWrite, byte[] DT)
        {
            using (FileStream FS = new FileStream(fileToWrite, FileMode.Append, FileAccess.Write))
            {
                FS.Write(DT, 0, DT.Length);
                FS.Close();
            }
        }
        private void saveButton_Click(object sender, EventArgs e)
        {
            Progress_odbierania_pliku progressOdbieraniaPliku = new Progress_odbierania_pliku();
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                client = new TcpClient(addressip, 11000);
                recieveFile = true;
                Invoke(new Action(() =>
                {
                    label4.Text = "Nawiązano połączenie z klientem";
                    label4.ForeColor = Color.Green;
                }));
                progressOdbieraniaPliku.ShowDialog();
            }).Start();


            //stworzenie klucza programu na wypadek gdyby taki jeszcze nie istniał
            Registry.CurrentUser.CreateSubKey(SOFTWARE_KEY).CreateSubKey(NAME_KEY);

            
            //zahashowanie nazwy używkownika
            string encrytpedUserName = Encoding.Default.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(loginTextBox.Text)));

            //sprawdzenie czy występuje już taki użytkownik jakiego nazwę wpisano
            if (Registry.CurrentUser.OpenSubKey(SOFTWARE_KEY).OpenSubKey(NAME_KEY).OpenSubKey(encrytpedUserName) == null)
            {
                MessageBox.Show("Taki użytkownik nie istnieje!\nWprowadź inną nazwę użytkownika!", "Błędna nazwa użytkownika", MessageBoxButtons.OK);
                return;
            }

            byte[] exponent = (byte[])Registry.CurrentUser.OpenSubKey(SOFTWARE_KEY).OpenSubKey(NAME_KEY).OpenSubKey(encrytpedUserName).OpenSubKey("Public").GetValue("Exponent");
            byte[] modulus = (byte[])Registry.CurrentUser.OpenSubKey(SOFTWARE_KEY).OpenSubKey(NAME_KEY).OpenSubKey(encrytpedUserName).OpenSubKey("Public").GetValue("Modulus");
            // wysłanie klucza publicznego do serwera <========================================================================== PUB KEY SENDING =========================

            Console.WriteLine("expodent :{0} \n", Encoding.Default.GetString(exponent));
            Console.WriteLine("modulus : {0} \n", Encoding.Default.GetString(modulus));

            while (!recieveFile) { }
            try
            {
                ns = client.GetStream();
                ns.Write(exponent, 0, exponent.Length);
                ns.Write(modulus, 0, modulus.Length);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            
            //Otrzymywanie zakodowanej wiadomości

            //Tutaj Kacprowa magia <================================================================================================== ODBIÓR PLIKU =======================

            /*
                *    W WIADOMOŚCI POTRZEBUJĘ:
                * 1. Klucz sesyjny
                * 2. Wektor inicjujący
                * 3. Tryb szyfrowania
                * 4. Wiadomość
                * */

            //tu trzeba wrzucić zaszyfrowany klucz odebrany z serwera
            byte[] receivedSessionKey = new byte[128];

            //tu trzeba wrzucić zaszyfrowaną wiadomość
            byte[] receivedMessage = null;

            //       Dwóch poniższych chyba nie trzeba szyfrować

            //tu trzeba wrzucić wektor inicjujący
            byte[] receivedIV = new byte[16];

            //tu trzeba wrzucić tryb szyfrowania
            string cipherMode = "";

            //===================================================================================================================== KONIEC ODBIORU PLIKU ===============

            while (!ns.DataAvailable) { }
            string tempFile = Path.GetTempFileName();

         
            byte[] tmp = new byte[8];
            long fileSize;
            string fileName="";
            byte[] tmpString = new byte[3];

            ns.Read(receivedSessionKey, 0, receivedSessionKey.Length);              // Klucz sesyjny
            Console.WriteLine("Po odebraniu klucza");

            ns.Read(tmp, 0, tmp.Length);                                          // Wielkosc pliku
            fileSize = BitConverter.ToInt64(tmp, 0);
            Console.WriteLine("Po odebraniu wielkosci pliku: {0}", fileSize);

            tmp = new byte[4];
            ns.Read(tmp, 0, 4);                                                   // Wielkosc nazwy
            int fileNameSize = BitConverter.ToInt32(tmp, 0);
            Console.WriteLine("Po odebraniu wielkosci nazwy");

            tmp = new byte[fileNameSize];
            ns.Read(tmp, 0, tmp.Length);                                                    // Nazwa pliku
            fileName = Encoding.ASCII.GetString(tmp);
            Console.WriteLine("Po odebraniu nazwy pliku: {0}", fileName);

            ns.Read(receivedIV, 0, receivedIV.Length);                              // IV
            ns.Read(tmpString, 0, tmpString.Length);                                // Nazwa trybu szyfrowania
            cipherMode = Encoding.ASCII.GetString(tmpString);
            Console.WriteLine("Po odebraniu iv i trybu");

            Thread.Sleep(1000);

            Invoke(new Action(() =>
            {
                progressOdbieraniaPliku.ProgressOdbioruPliku.Maximum = (int)fileSize;
                progressOdbieraniaPliku.ProgressOdbioruPliku.Minimum = 0;
                progressOdbieraniaPliku.ProgressOdbioruPliku.Value = 0;
                progressOdbieraniaPliku.ProgressOdbioruPliku.Step = 1024;
            }));

            FileInfo fileInfo = new FileInfo(tempFile);

            var watch = System.Diagnostics.Stopwatch.StartNew();

            while (fileInfo.Length < fileSize)
            {
                fileInfo = new FileInfo(tempFile);
                tmp = new byte[Math.Min((int)(fileSize - fileInfo.Length), 1024)];

                while (!ns.CanRead) {
                    Console.WriteLine(fileInfo.Length);
                }
                ns.Read(tmp, 0, Math.Min((int)(fileSize - fileInfo.Length), 1024));                                          // Plik
                AppendToFile(tempFile, tmp.ToArray());
                Invoke(new Action(() =>
                {
                    progressOdbieraniaPliku.ProgressOdbioruPliku.PerformStep();
                }));
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Odebranie pliku zajeło: {0}", elapsedMs);

            Invoke(new Action(() =>
            {
                progressOdbieraniaPliku.Close();
            }));
            
            Console.WriteLine("Po odebraniu pliku");

            ns.Close();
            client.Close();

            receivedMessage = FileToByteArray(tempFile);



            //int a = 0;
            //byte k = 0;

            //while (receivedMessage.Length - a > 0)
            //{
            //    if (receivedMessage[a] == 0)
            //        Console.WriteLine("Error {0}", a);
            //    k++;
            //    a++;
            //}



            /*
            *      SCENARIUSZ INTERAKCJI
            *  0. Pobranie całej wiadomości
            *  1. Zahashowanie hasła użytkownika
            *  2. Odszyfrowanie klucza prywatnego
            *  3. Odszyfrowanie klucza sesyjnego
            *  4. Odszyfrowanie wiadomosci
            *  5. Zapis 
            */

            #region Deszyfrowanie_wiadomości

            //odszyfrowanie kluczas sesyjnego algorytmem RSA
            byte[] sessionKey = DecryptRSA(receivedSessionKey);
            
            //odszyfrowanie pliku za pomocą odszyfrowanego klucza sesji, otrzymanego IV i trybu szyfrowania      
            byte[] message =  DecryptMessage(sessionKey, receivedIV, receivedMessage, cipherMode);

            #endregion

            //zapis do pliku
            ByteArrayToFile(message, fileName);
            MessageBox.Show("Udało się odebrać plik!", "Odebranie sie udało", MessageBoxButtons.OK);
            Application.Exit();
        }

        private byte[] DecryptPrivateData(byte[] hashedPassword, byte[] encryptedPrivateData)
        {
            // wektor inicjujący do odszyfrowania hasła
            byte[] IV = new byte[16];
            for (int i = 0; i < 16; i++)
                IV[i] = 0;

            return new Encoder(hashedPassword, IV).DecryptByCBC(encryptedPrivateData);
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

        private byte[] FileToByteArray(string fileName)
        {
            byte[] array;
            using (FileStream fs = File.OpenRead(fileName))
            {
                using (BinaryReader binaryReader = new BinaryReader(fs))
                {
                    array = binaryReader.ReadBytes((int)fs.Length);
                }
            }
            return array;
        }
        private void ByteArrayToFile(byte[] array, string name)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = name;
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
