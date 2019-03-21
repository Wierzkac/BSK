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

        public Encoder()
        {
            Key = Aes.Create().Key;
            IV = Aes.Create().IV;
            KeySize = Key.Length * 8;
           
        }

        public Encoder(byte[] Key, byte[] IV)
        {
            this.Key = Key;
            this.IV = IV;
            KeySize = Key.Length * 8;
        }


        public byte[] EncryptByECB(byte[] message)
        {

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider
            {
                BlockSize = 128,
                KeySize = this.KeySize,
                Key = this.Key,
                IV = this.IV,
                Padding = PaddingMode.Zeros,
                Mode = CipherMode.ECB
            };


            var watch = System.Diagnostics.Stopwatch.StartNew();
            ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] encrypted = encryptor.TransformFinalBlock(message, 0, message.Length);
            encryptor.Dispose();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Zaszyfrowanie pliku zajeło: {0}", elapsedMs);

            return encrypted;
        }

        public byte[] EncryptByCBC(byte[] message)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider
            {
                BlockSize = 128,
                KeySize = this.KeySize,
                Key = this.Key,
                IV = this.IV,
                Padding = PaddingMode.Zeros,
                Mode = CipherMode.CBC
            };

            var watch = System.Diagnostics.Stopwatch.StartNew();
            ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] encrypted = encryptor.TransformFinalBlock(message, 0, message.Length);
            encryptor.Dispose();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Zaszyfrowanie pliku zajeło: {0}", elapsedMs);

            return encrypted;
        }
       
        public byte[] EncryptByCFB(byte[] message)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider
            {
                BlockSize = 128,
                KeySize = this.KeySize,
                Key = this.Key,
                IV = this.IV,
                Padding = PaddingMode.Zeros, 
                Mode = CipherMode.CFB
            };

            var watch = System.Diagnostics.Stopwatch.StartNew();
            ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] encrypted = encryptor.TransformFinalBlock(message, 0, message.Length);
            encryptor.Dispose();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Zaszyfrowanie pliku zajeło: {0}", elapsedMs);

            return encrypted;
        }

        public byte[] EncryptByOFB(byte[] message)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider
            {
                BlockSize = 128,
                KeySize = this.KeySize,
                Key = this.Key,
                IV = this.IV,
                Padding = PaddingMode.Zeros,
                Mode = CipherMode.OFB
            };

            var watch = System.Diagnostics.Stopwatch.StartNew();
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] encrypted = encryptor.TransformFinalBlock(message, 0, message.Length);
            encryptor.Dispose();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Zaszyfrowanie pliku zajeło: {0}", elapsedMs);

            return encrypted;
        }

        public byte[] DecryptByECB(byte[] message)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider
            {
                BlockSize = 128,
                KeySize = this.KeySize,
                Key = this.Key,
                IV = this.IV,
                Padding = PaddingMode.Zeros,
                Mode = CipherMode.ECB
            };

            ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] decrypted = decryptor.TransformFinalBlock(message, 0, message.Length);
            decryptor.Dispose();

            return decrypted;
        }

        public byte[] DecryptByCBC(byte[] message)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider
            {
                BlockSize = 128,
                KeySize = this.KeySize,
                Key = this.Key,
                IV = this.IV,
                Padding = PaddingMode.Zeros,
                Mode = CipherMode.CBC
            };

            ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] decrypted = decryptor.TransformFinalBlock(message, 0, message.Length);
            decryptor.Dispose();

            return decrypted;
        }

        public byte[] DecryptByCFB(byte[] message)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider
            {
                BlockSize = 128,
                KeySize = this.KeySize,
                Key = this.Key,
                IV = this.IV,
                Padding = PaddingMode.Zeros,
                Mode = CipherMode.CFB
            };

            ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] decrypted = decryptor.TransformFinalBlock(message, 0, message.Length);
            decryptor.Dispose();

            return decrypted;
        }

        public byte[] DecryptByOFB(byte[] message)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider
            {
                BlockSize = 128,
                KeySize = this.KeySize,
                Key = this.Key,
                IV = this.IV,
                Padding = PaddingMode.Zeros,
                Mode = CipherMode.OFB
            };

            ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] decrypted = decryptor.TransformFinalBlock(message, 0, message.Length);
            decryptor.Dispose();

            return decrypted;
        }
    }
}
