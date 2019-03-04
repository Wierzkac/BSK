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
        //private byte[] fileData = null;

        public byte[] fileData
        {
            get { return fileData; }
        } 

        public Form1()
        {
            
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            encodingModeComboBox.SelectedItem = null;
            encodingModeComboBox.SelectedItem = "ECB";
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

        private void startButton_Click(object sender, EventArgs e)
        {
            Form2 progressForm = new Form2(this);
            this.Hide();
            progressForm.ShowDialog();

        }


        public object ChoosenEncodingMode()
        {
            return encodingModeComboBox.SelectedItem;
        }



        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        { 

           
        }
    }
}
