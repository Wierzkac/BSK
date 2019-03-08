using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static AsynchronousSocketListener;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private OpenFileDialog openFileDialog1;
        public byte[] fileData = null;

        public OpenFileDialog openfileDialog1
        {
            get { return openFileDialog1; }
        }
        
        public Form1()
        {           
            InitializeComponent();     
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            encodingModeComboBox.SelectedItem = null;
            encodingModeComboBox.SelectedItem = "ECB";
            
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                StartListening(this);
            }).Start();
        }

        private void WybierzPlik_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(openFileDialog1.FileName);
                long fileSize = fi.Length;
                if (fileSize > 104857600 || fileSize < 1024) {
                    MessageBox.Show("Twój plik jest albo poniżej 1kB albo powyżej 100MB!\n Wybierz ponownie plik.",
                    "ERROR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                }
                else {
                    var filePath = openFileDialog1.FileName;
                    fileData = FileToByteArray(openFileDialog1.FileName);
                    textBox1.Text = openFileDialog1.SafeFileName;
                }
            }
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

        private void ByteArrayToFile(byte[] array)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|mp3 files (*.mp3)|*.mp3|png files (*.png)|*.png|avi files (*.avi)|*.avi|jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
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

            if (textBox1.Text != "") {
                Form2 progressForm = new Form2(this);
                //this.Hide();
                progressForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Nie wybrano pliku!", "Błędny plik", MessageBoxButtons.OK);
            }

        }


        public object ChoosenEncodingMode()
        {
            return encodingModeComboBox.SelectedItem;
        }
    }
}
