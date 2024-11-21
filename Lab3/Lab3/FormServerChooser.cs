using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    public partial class FormServerChooser : Form
    {
        public event Action<string, int, int> dataEnter;
        string IP = "127.0.0.1";
        //string IP = "192.168.8.102";
        int commandPort = 7000;
        int dataPort = 7001;
        public FormServerChooser()
        {
            InitializeComponent();
            textBox1.Enabled = false;
            textBox1.Text = "127.0.0.1";
            //textBox1.Text = "192.168.8.102";
            textBox2.Text = commandPort.ToString();
            textBox3.Text = dataPort.ToString();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            dataEnter?.Invoke(IP, commandPort, dataPort);
            this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            foreach (char c in textBox2.Text)
            {
                if (!char.IsDigit(c)) textBox2.Text = textBox2.Text.Replace(c, '\0');
            }
            commandPort = int.Parse(textBox2.Text);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            foreach (char c in textBox3.Text)
            {
                if (!char.IsDigit(c)) textBox3.Text = textBox3.Text.Replace(c, '\0');
            }
            dataPort = int.Parse(textBox3.Text);
        }
    }
}
