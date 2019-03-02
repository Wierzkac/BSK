using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private OpenFileDialog openFileDialog1;
        private byte[] fileData = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void WybierzPlik_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var filePath = openFileDialog1.FileName;
                byte[] plik = FileToByteArray(openFileDialog1.FileName);    
            }
        }
        private byte[] FileToByteArray(string fileName)
        {
            using (FileStream fs = File.OpenRead(fileName))
            {
                using (BinaryReader binaryReader = new BinaryReader(fs))
                {
                    fileData = binaryReader.ReadBytes((int)fs.Length);
                }
            }
            return fileData;
        }
        private void ByteArrayToFile(byte[] array)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.ShowDialog();

            FileStream fs =(System.IO.FileStream)saveFileDialog.OpenFile();
            fs.Close();

            File.WriteAllBytes(fs.Name, array);


        }

        private void ZapiszJako_Click(object sender, EventArgs e)
        {
            ByteArrayToFile(fileData);
        }
    }
}
