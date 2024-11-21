using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab3
{
    public partial class Form1 : Form
    {
        Dictionary<string, List<ICarBrand>> ModelList = new Dictionary<string, List<ICarBrand>>(); 
        ICarBrand[] BrandList = new ICarBrand[0];
        int lastRow = 0;
        string lastCarType;

        TcpClient clientCommandSocket;
        TcpClient clientDataSocket;
        //NetworkStream stream;
        CancellationTokenSource cts;

        public Form1()
        {
            InitializeComponent();
            FillCarList();
            panel1.BackColor = Color.White;
            panel2.BackColor = Color.White;
            textBox1.Visible = false;
            textBox1.Enabled = false;
            cts = new CancellationTokenSource();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateDataGridView();
        }


        #region базовые функции
        private void FillCarList()
        {
            Array.Resize(ref BrandList, 14);
            BrandList[0] = new PassengerCar("ГАЗ", "М20 Победа", 52, 105);
            BrandList[1] = new Truck("ГАЗ", "33023 ГАЗель", 129, 105);
            BrandList[2] = new PassengerCar("ГАЗ", "24-12 Волга", 95, 140);
            BrandList[3] = new Truck("ГАЗ", "2705 ГАЗель", 117, 115);
            BrandList[4] = new Truck("ГАЗ", "3309 ГАЗон", 129, 105);
            BrandList[5] = new PassengerCar("ГАЗ", "31029 Волга", 100, 145);
            BrandList[6] = new Truck("ГАЗ", "ГАЗель NEXT", 150, 130);
            BrandList[7] = new PassengerCar("ГАЗ", "3110 Волга", 100, 150);
            BrandList[8] = new PassengerCar("LADA", "ВАЗ-2121 4x4", 77, 130);
            BrandList[9] = new PassengerCar("LADA", "ВАЗ-21099", 72, 150);
            BrandList[10] = new PassengerCar("LADA", "ВАЗ-2115", 82, 175);
            BrandList[11] = new Truck("МАЗ", "МАЗ-5440", 400, 95);
            BrandList[12] = new Truck("МАЗ", "МАЗ-6517", 400, 95);
            BrandList[13] = new Truck("МАЗ", "МАЗ-6430", 400, 95);
        }

        private void UpdateDataGridView()
        {
            dataGridView.CurrentCell = null;
            bindingSourceCars.DataSource = BrandList;
            RecolorRows();
        }

        private void RecolorRows()
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                if (dataGridView.Rows[i].Cells[4].Value.ToString() == "Легковой") dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                else if (dataGridView.Rows[i].Cells[4].Value.ToString() == "Грузовик") dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.OrangeRed;
                else dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Aqua;
            }
        }
        #endregion

        #region работа с таблицами
        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex != -1)
            {
                ICarBrand car = BrandList[rowIndex];

                if (car.Type != lastCarType && lastRow == rowIndex)
                {
                    if (lastCarType == "Легковой")
                    {
                        BrandList[rowIndex] = new Truck(car.Brand, car.Model, car.HorsePower, car.MaxSpeed);
                    }
                    else if (lastCarType == "Грузовик")
                    {
                        BrandList[rowIndex] = new PassengerCar(car.Brand, car.Model, car.HorsePower, car.MaxSpeed);
                    }
                }
                if (car.Type == lastCarType && lastRow == rowIndex)
                {
                    if (lastCarType == "Легковой")
                    {
                        BrandList[rowIndex] = new PassengerCar(dataGridView.Rows[rowIndex].Cells[0].Value.ToString(),
                            dataGridView.Rows[rowIndex].Cells[1].Value.ToString(), int.Parse(dataGridView.Rows[rowIndex].Cells[2].Value.ToString()),
                            int.Parse(dataGridView.Rows[rowIndex].Cells[3].Value.ToString()));
                    }
                    else if (lastCarType == "Грузовик")
                    {
                        BrandList[rowIndex] = new Truck(dataGridView.Rows[rowIndex].Cells[0].Value.ToString(),
                            dataGridView.Rows[rowIndex].Cells[1].Value.ToString(), int.Parse(dataGridView.Rows[rowIndex].Cells[2].Value.ToString()), 
                            int.Parse(dataGridView.Rows[rowIndex].Cells[3].Value.ToString()));
                    }
                }
                UpdateDataGridView();
            }
        }

        private void dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            lastRow = e.RowIndex;
            lastCarType = dataGridView[e.ColumnIndex, e.RowIndex].Value.ToString();
        }

        private async void dataGridView_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
            if (ModelList.ContainsKey(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString()))
            {
                FormBrandCarsTable brandCarsTable = new FormBrandCarsTable(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString(), ModelList[dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString()], false, clientDataSocket);
                brandCarsTable.ShowDialog();
            }
            else
            {
                if (clientDataSocket != null)
                {
                    string[] Models = (from p in BrandList where p.Brand == dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString() select (p.Model + '!' + p.Type)).ToArray();
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        formatter.Serialize(stream, Models);
                        byte[] data = stream.ToArray();
                        await clientDataSocket.GetStream().WriteAsync(data, 0, data.Length);
                    }
                    FormBrandCarsTable brandCarsTable = new FormBrandCarsTable(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString(), BrandList.ToList(), true, clientDataSocket);
                    brandCarsTable.dataEnter += DictionaryFiller;
                    brandCarsTable.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Данные не загружены, для загрузки подклюдчитесь к серверу.");
                }
            }
        }

        private void DictionaryFiller(List<ICarBrand> recievedList)
        {
            ModelList.Add(recievedList[0].Brand, recievedList);
        }

        private void buttonAddBrand_Click(object sender, EventArgs e)
        {
            Array.Resize(ref BrandList, BrandList.Length + 1);
            BrandList[BrandList.Length - 1] = new PassengerCar("", "", 0, 0);
            UpdateDataGridView();
        }
        #endregion

        #region XML
        private void SaveXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //vehicle.PassCarList = new IEnumerable<PassengerCar>();
            VehicleXML vehicle = new VehicleXML();
            IEnumerable<PassengerCar> pc = from p in BrandList where p is PassengerCar select p as PassengerCar;
            vehicle.PassCarList = pc.ToList();
            IEnumerable<Truck> tr = from p in BrandList where p is Truck select p as Truck;
            //textBox1.Text = vehicle.PassCarList.First().Brand.ToString();
            vehicle.TruckList = tr.ToList();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(VehicleXML));

            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream("Brands.xml", FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, vehicle);
            }
        }

        private void LoadXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int counter = 0;
            VehicleXML vehicle = new VehicleXML();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(VehicleXML));
            using (FileStream fs = new FileStream("Brands.xml", FileMode.OpenOrCreate))
            {
                vehicle = xmlSerializer.Deserialize(fs) as VehicleXML;
            }
            BrandList = new ICarBrand[vehicle.PassCarList.Count() + vehicle.TruckList.Count()];
            foreach (PassengerCar pc in vehicle.PassCarList)
            {
                BrandList[counter] = pc;
                counter++;
            }
            foreach (Truck tr in vehicle.TruckList)
            {
                BrandList[counter] = tr;
                counter++;
            }
            UpdateDataGridView();
            //dataGridView.Rows.Clear();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Серверные функции
        private void ConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormServerChooser serverChooser = new FormServerChooser();
            serverChooser.dataEnter += AsyncConnector;
            serverChooser.ShowDialog();
        }

        private async void AsyncConnector(string IP, int cmdPort, int dtPort)
        {
            try
            {
                cts = new CancellationTokenSource();
                clientCommandSocket = new TcpClient();
                clientDataSocket = new TcpClient();
                await clientCommandSocket.ConnectAsync(IP, cmdPort);
                await clientDataSocket.ConnectAsync(IP, dtPort);
                if (clientCommandSocket.Connected)
                {
                    panel1.BackColor = Color.Green;
                    textBox1.Visible = true;
                    textBox1.Text = IP + ':' + cmdPort.ToString() + '/' + dtPort.ToString();
                    await SendKeepAliveAsync(cts.Token);
                }
            }
            catch //(Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
                panel1.BackColor = Color.Red;
                panel2.BackColor = Color.Red;
                await Task.Delay(1000);
                Disconnector();
            }
        }

        private void DisconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Disconnector();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Disconnector();
        }

        private async Task SendKeepAliveAsync(CancellationToken cancellationToken)
        {
            NetworkStream commandStream = clientCommandSocket.GetStream();
            while (!cancellationToken.IsCancellationRequested && clientCommandSocket.Connected)
            {
                byte[] buffer = new byte[1024];
                await Task.Delay(2000, cancellationToken);
                await commandStream.WriteAsync(Encoding.UTF8.GetBytes("KEEP-ALIVE"), 0, "KEEP-ALIVE".Length);
                Task<int> readTask = commandStream.ReadAsync(buffer, 0, 1024);
                if (await Task.WhenAny(readTask, Task.Delay(2000)) == readTask)
                {
                    int bytesRead = readTask.Result;
                    if (bytesRead > 0)
                    {
                        string response = Encoding.UTF8.GetString(buffer);
                        string oldText = textBox1.Text;
                        textBox1.Text = response;
                        panel2.BackColor = Color.Green;
                        await Task.Delay(1000, cancellationToken);
                        textBox1.Text = oldText;
                        panel2.BackColor = Color.White;
                    }
                    else
                    {
                        panel1.BackColor = Color.Red;
                        panel2.BackColor = Color.Red;
                        await Task.Delay(1000, cancellationToken);
                        Disconnector();
                    }
                }
                else
                {
                    panel1.BackColor = Color.Red;
                    panel2.BackColor = Color.Red;
                    await Task.Delay(1000, cancellationToken);
                    Disconnector();
                }
            }
        }

        private void Disconnector()
        {
            cts.Cancel();
            if (clientCommandSocket != null)
            {
                if (clientCommandSocket.Connected) clientCommandSocket.GetStream().WriteAsync(Encoding.UTF8.GetBytes("FIN"), 0, "FIN".Length);
                clientCommandSocket.Close();
            }
            if (clientDataSocket != null) clientDataSocket.Close();
            clientCommandSocket = clientDataSocket = null;
            textBox1.Visible = false;
            panel1.BackColor = Color.White;
            panel2.BackColor = Color.White;
        }
        #endregion
    }
}
