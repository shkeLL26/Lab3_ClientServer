using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab3_server
{
    public partial class FormPortChooser : Form
    {
        public event Action<IPAddress, int, int> dataEnter;
        IPAddress IP = IPAddress.Any;
        int commandPort = 7000;
        int dataPort = 7001;
        CancellationTokenSource cts;
        public FormPortChooser()
        {
            InitializeComponent();
            textBox1.Enabled = false;
            textBox1.Text = IPAddress.Any.ToString();
            textBox2.Text = commandPort.ToString();
            textBox3.Text = dataPort.ToString();
            cts = new CancellationTokenSource();
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
