using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab3
{
    public partial class FormBrandCarsTable : Form
    {
        TcpClient clientSocket;
        public event Action<List<ICarBrand>> dataEnter;
        List<ICarBrand> CarList = new List<ICarBrand>();
        List<ICarBrand> ResultList = new List<ICarBrand>();
        string currentModel;
        bool isFirst;
        public FormBrandCarsTable(string recievedModel, List<ICarBrand> recievedCarList, bool isFrst, TcpClient recievedSocket)
        {
            InitializeComponent();
            CarList = recievedCarList;
            currentModel = recievedModel;
            isFirst = isFrst;
            clientSocket = recievedSocket;
            if (isFirst) TablesGenerator();
            else TablesFiller();
        }

        private async void TablesGenerator()
        {
            byte[] buffer = new byte[2048];
            using (var reader = new StreamReader(clientSocket.GetStream(), Encoding.UTF8, true, 8192, true))
            { // true - leave open
                byte[] numBytes = new byte[sizeof(int)];
                await reader.BaseStream.ReadAsync(numBytes, 0, numBytes.Length);
                int num = int.Parse(Encoding.UTF8.GetString(numBytes));
                progressBar1.Value = 0;
                for (int i = 0; i < num; i++)
                {
                    ICarBrand jCarBrand = null;
                    string hp = "none";
                    string ms = "none";
                    string response = await reader.ReadLineAsync(); // Читаем до \r\n
                    string[] strings = response.Split('!');
                    for (int j = 0; j < CarList.Count(); j++)
                    {
                        if (strings[0] == CarList[j].Model)
                        {
                            hp = CarList[j].HorsePower.ToString();
                            ms = CarList[j].MaxSpeed.ToString();
                            jCarBrand = CarList[j];
                            if (strings.Length > 4 && strings[1] == "Легковой")
                            {
                                PassengerCar carBrand = new PassengerCar(jCarBrand.Brand, jCarBrand.Model, jCarBrand.HorsePower, jCarBrand.MaxSpeed);
                                dataGridView1.Rows.Add(currentModel, strings[0], hp, ms, strings[4], strings[2], strings[3]);
                                carBrand.RegistrationNumber = strings[4];
                                carBrand.Multimedia = strings[2];
                                carBrand.Airbags = int.Parse(strings[3]);
                                ResultList.Add(carBrand);
                            }
                            else if (strings.Length > 3)
                            {
                                Truck carBrand = new Truck(jCarBrand.Brand, jCarBrand.Model, jCarBrand.HorsePower, jCarBrand.MaxSpeed);
                                dataGridView2.Rows.Add(currentModel, strings[0], hp, ms, strings[4], strings[3], strings[2]);
                                carBrand.RegistrationNumber = strings[4];
                                carBrand.Wheels = int.Parse(strings[3]);
                                carBrand.Volume = float.Parse(strings[2]);
                                ResultList.Add(carBrand);
                            }
                            if (strings.Length > 4 && int.Parse(strings[5]) <= progressBar1.Maximum) progressBar1.Value = int.Parse(strings[5]);
                            break;
                        }
                    }
                    //if (jCarBrand == null) MessageBox.Show(strings[0]);
                }
                progressBar1.Value = 0;
            }
        }

        private void TablesFiller()
        {
            for (int i = 0; i < CarList.Count; i++)
            {
                ICarBrand car = CarList[i];
                if (car is PassengerCar) dataGridView1.Rows.Add(car.Brand, car.Model, car.HorsePower, car.MaxSpeed,
                    car.RegistrationNumber, (car as PassengerCar).Multimedia, (car as PassengerCar).Airbags);
                else dataGridView2.Rows.Add(car.Brand, car.Model, car.HorsePower, car.MaxSpeed,
                    car.RegistrationNumber, (car as Truck).Wheels, (car as Truck).Volume);
            }
        }

        private void FormBrandCarsTable_FormClosing(object sender, FormClosingEventArgs e)
        {
            dataEnter?.Invoke(ResultList);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //progressBar1.Value = Loader.getProgress();
                        /*progressBar1.Value = 0;
            if (isFirst)
            {
                timer1.Start();
                CarList = (from p in CarList where p.Brand == currentModel select p).ToList();
                //CarList = await Loader.load(CarList);
                progressBar1.Value = 0;
            }
            Random random = new Random();
            int num = random.Next(11, 22);
            for (int i = 0; i < num; i++)
            {
                if (car is PassengerCar) dataGridView1.Rows.Add(car.Brand, car.Model, car.HorsePower, car.MaxSpeed,
                    car.RegistrationNumber, (car as PassengerCar).Multimedia, (car as PassengerCar).Airbags);
                else dataGridView2.Rows.Add(car.Brand, car.Model, car.HorsePower, car.MaxSpeed,
                    car.RegistrationNumber, (car as Truck).Wheels, (car as Truck).Volume);
            }
            if (isFirst)
            {
                timer1.Stop();
                progressBar1.Value = 0;
            }*/
        }

    }
}
