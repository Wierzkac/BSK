using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace WindowsFormsApp1
{
    class Encoder
    { 

        private byte[] _Key;
        private byte[] _IV;
        private int _KeySize;
        private Form2 _form2;

        public Form2 Form2
        {
            get { return _form2; }
            set { _form2 = value; }
        }

        public int KeySize
        {
            get { return _KeySize; }
            set { _KeySize = value; }
        }

        public byte[] Key
        {
            get { return _Key; }
            set { _Key = value; }
        }

        public byte[] IV
        {
            get { return _IV; }
            set { _IV = value; }
        }

        public Encoder(Form2 form)
        {
            Key = AesManaged.Create().Key;
            IV = AesManaged.Create().IV;
            KeySize = Key.Length * 8;
            Form2 = form;
        }


        public void EncryptByECB(byte[] message)
        {

            CheckData(message);   

            AesManaged aes = new AesManaged
            {
                Key = this.Key,
                IV = this.IV,
                KeySize = this.KeySize,
                BlockSize = 128,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.Zeros
            };

            byte[] block = new byte[16];
            byte[] encryptedMessage = new byte[message.Length + (16 - message.Length % 16)];

            for (int j = 0; j < message.Length; j += 16)
            {
                for (int i = 0; i < 16 && i + j < message.Length; i++)
                {
                    //this.Form2.EncodingProgressBar.PerformStep();
                    block[i] = message[i + j];
                }
               byte[] encryptedBlock = EncryptBlockOfData(block, aes);

                //wysyłanie bloku <--------------------------------------------------------------------------WYSYŁANIE BLOKU DANYCH---------------------------------------------------------------


                /* Dane Do przesłania
                1. Nazwa pliku
                2. Rozmiar orginalnego pliku
                3. Rozmiar zaszyfrowanego pliku

                */

                for (int i = 0; i < 16; i++)
                {
                    


                    encryptedMessage[i + j] = encryptedBlock[i];
                    block[i] = 0;
                }
                
            }


            byte lastByte = encryptedMessage[encryptedMessage.Length - 1];

        }

        public void encryptByCBC()
        {

        }

        public void encryptByCFB()
        {

        }

        public void encryptByOFB()
        {

        }


        private void CheckData(byte[] data)
        {

            // checking if data were created succesfully
            if (data == null || data.Length <= 0)
                throw new ArgumentNullException("Data");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
          
        }

        private byte[] EncryptBlockOfData(byte[] data, AesManaged aes)
        {
            byte[] encrypted;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(data);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }

            return encrypted;
        }

    }
}
