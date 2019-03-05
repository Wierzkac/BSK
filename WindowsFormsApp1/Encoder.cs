using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace WindowsFormsApp1
{
    class Encoder
    {

        private byte[] _Key;
        private byte[] _IV;

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

        public void encryptByECB(byte[] message)
        {
         
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

    }
}
