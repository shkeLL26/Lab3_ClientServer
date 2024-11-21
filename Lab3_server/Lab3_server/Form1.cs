using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

namespace Lab3_server
{
    public partial class Form1 : Form
    {
        private IPAddress serverIP;
        private int commandPort;
        private int dataPort;

        TcpListener serverCommandSocket;
        TcpListener serverDataSocket;
        List<Client> clientsList = new List<Client>();
        
        List<string[]> previosDataPacks = new List<string[]>();
        List<string[]> previosResultPacks = new List<string[]>();

        CancellationTokenSource cts;

        struct Client
        {
            public TcpClient commandSocket;
            public TcpClient dataSocket;
            public string IP;
            public string commandPort;
            public string dataPort;
        }

        bool isRunning = false;

        public Form1()
        {
            InitializeComponent();
            InitializeEndPoint();
        }

        #region Dialog Tasks
        private async Task AsyncConnector(CancellationToken cancellationToken)
        {
            while (isRunning)
            {
                try
                {

                    Client clientSocket = new Client();
                    clientSocket.commandSocket = await serverCommandSocket.AcceptTcpClientAsync();
                    clientSocket.dataSocket = await serverDataSocket.AcceptTcpClientAsync();
                    clientSocket.IP = clientSocket.commandSocket.Client.RemoteEndPoint.ToString().Split(':')[0];
                    clientSocket.commandPort = clientSocket.commandSocket.Client.RemoteEndPoint.ToString().Split(':')[1];
                    clientSocket.dataPort = clientSocket.dataSocket.Client.RemoteEndPoint.ToString().Split(':')[1];
                    lock (clientsList) clientsList.Add(clientSocket);
                    listBoxFiller();
                    //Task dialogTask = HandleClientAsync(clientsList.Count-1, cancellationToken);
                    Task commandDialogTask = CommandDialogAsync(clientsList.Count - 1, cancellationToken);
                    Task dataDialogTask = DataDialogAsync(clientsList.Count - 1, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    MessageBox.Show("Сервер остановлен");
                }
                catch (ObjectDisposedException)
                {
                    MessageBox.Show("Disposed");
                    Finisher();
                }
            }
        }

        private async Task CommandDialogAsync(int index, CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                try
                {
                    Task<int> readCommandTask = clientsList[index].commandSocket.GetStream().ReadAsync(buffer, 0, buffer.Length);
                    if (await Task.WhenAny(readCommandTask, Task.Delay(5000, cancellationToken)) == readCommandTask)
                    {
                        int bytesRead = await readCommandTask; // Ожидаем завершения readCommandTask
                        if (bytesRead > 0)
                        {
                            string message = Encoding.UTF8.GetString(buffer, 0, readCommandTask.Result);
                            if (message == "KEEP-ALIVE")
                            {
                                await clientsList[index].commandSocket.GetStream().WriteAsync(Encoding.UTF8.GetBytes("KEEP-ALIVE_RESPONSE"), 0, "KEEP-ALIVE_RESPONSE".Length);
                                clientsList[index].commandSocket.GetStream().Flush();
                            }
                            else if (message == "FIN")
                            {
                                clientsList[index].commandSocket.Close();
                                clientsList[index].dataSocket.Close();
                                clientsList.Remove(clientsList[index]);
                                listBoxFiller();
                                break;
                            }
                        }
                        else
                        {
                            clientsList[index].commandSocket.Close();
                            clientsList[index].dataSocket.Close();
                            clientsList.Remove(clientsList[index]);
                            listBoxFiller();
                            break;
                        }
                    }
                    else
                    {
                        clientsList[index].commandSocket.Close();
                        clientsList[index].dataSocket.Close();
                        clientsList.Remove(clientsList[index]);
                        listBoxFiller();
                        break;
                    }
                }
                catch (OperationCanceledException)
                {
                    clientsList[index].commandSocket.Close();
                    clientsList[index].dataSocket.Close();
                    clientsList.Remove(clientsList[index]);
                    listBoxFiller();
                    break;
                }
            }
        }

        private async Task DataDialogAsync(int index, CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                try
                {
                    Task<int> readDataTask = clientsList[index].dataSocket.GetStream().ReadAsync(buffer, 0, buffer.Length);
                    int bytesRead = await readDataTask; // Ожидаем завершения readCommandTask
                    if (bytesRead > 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, readDataTask.Result);
                        BinaryFormatter formatter = new BinaryFormatter();
                        using (MemoryStream stream = new MemoryStream(buffer, 0, readDataTask.Result))
                        {
                            string[] strings = (string[])formatter.Deserialize(stream);
                            Array.Sort(strings);
                            if (PreviosPacksChecker(strings) == -1)
                            {
                                await ResultGenerator(strings, index);
                                previosDataPacks.Add(strings);
                            }
                            else
                            {
                                await ResultSender(strings, index);
                            }
                        }
                    }
                    else
                    {
                        clientsList[index].commandSocket.Close();
                        clientsList[index].dataSocket.Close();
                        clientsList.Remove(clientsList[index]);
                        listBoxFiller();
                        break;
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                /*catch 
                {
                    clientsList[index].commandSocket.Close();
                    clientsList[index].dataSocket.Close();
                    clientsList.Remove(clientsList[index]);
                    listBoxFiller();
                    break;
                }*/

            }
        }

        private async Task ResultGenerator(string[] recievedModels, int index)
        {
            Random random = new Random();
            int num = random.Next(11, 22);
            string[] result = new string[num + 1];
            result[0] = num.ToString();
            // Отправляем число num в бинарном формате
            byte[] numBytes = Encoding.UTF8.GetBytes(num.ToString());
            clientsList[index].dataSocket.GetStream().Flush();
            await clientsList[index].dataSocket.GetStream().WriteAsync(numBytes, 0, numBytes.Length);
            await clientsList[index].dataSocket.GetStream().WriteAsync(Encoding.UTF8.GetBytes("\r\n"), 0, 2);
            for (int i = 0; i < num; i++)
            {
                string car = await Loader.load(recievedModels, i, num - 1);
                result[i + 1] = car;
                await clientsList[index].dataSocket.GetStream().WriteAsync(Encoding.UTF8.GetBytes(car), 0, Encoding.UTF8.GetBytes(car).Length);
                await clientsList[index].dataSocket.GetStream().WriteAsync(Encoding.UTF8.GetBytes("\r\n"), 0, 2);
            }
            previosResultPacks.Add(result);
        }

        private async Task ResultSender(string[] recievedModels, int index)
        {
            int prevResultIndex = PreviosPacksChecker(recievedModels);
            int num = int.Parse(previosResultPacks[prevResultIndex][0]);
            byte[] numBytes = Encoding.UTF8.GetBytes(previosResultPacks[prevResultIndex][0]);
            clientsList[index].dataSocket.GetStream().Flush();
            await clientsList[index].dataSocket.GetStream().WriteAsync(numBytes, 0, numBytes.Length);
            await clientsList[index].dataSocket.GetStream().WriteAsync(Encoding.UTF8.GetBytes("\r\n"), 0, 2);
            //Random random = new Random();
            for (int i = 1; i < num + 1; i++)
            {
                //await Task.Delay(random.Next(0, 501));
                await clientsList[index].dataSocket.GetStream().WriteAsync(Encoding.UTF8.GetBytes(previosResultPacks[prevResultIndex][i]), 0, Encoding.UTF8.GetBytes(previosResultPacks[prevResultIndex][i]).Length);
                await clientsList[index].dataSocket.GetStream().WriteAsync(Encoding.UTF8.GetBytes("\r\n"), 0, 2);
            }
        }
        #endregion

        #region Spec Functions
        private void listBoxFiller()
        {
            int counter = 0;
            listBoxClients.Items.Clear();
            foreach (Client client in clientsList)
            {
                counter++;
                listBoxClients.Items.Add((counter).ToString() + ". IP: " + client.IP + "    Порты: " + client.commandPort + " " + client.dataPort);
            }
        }

        private void InitializeEndPoint()
        {
            panelIndicator.BackColor = Color.White;
            FormPortChooser formPortChooser = new FormPortChooser();
            formPortChooser.dataEnter += DataReciever;
            formPortChooser.ShowDialog();
            this.Text = "Сервер " + serverIP.ToString() + ':' + commandPort.ToString() + '/' + dataPort.ToString();
            cts = new CancellationTokenSource();
        }

        private void DataReciever(IPAddress IP, int cmdPort, int dtPort)
        {
            //serverIP = IPAddress.Parse(IP);
            serverIP = IP;
            commandPort = cmdPort;
            dataPort = dtPort;
        }

        private int PreviosPacksChecker(string[] recievedArray)
        {
            for (int i = 0; i < previosDataPacks.Count; i++) if (recievedArray.SequenceEqual(previosDataPacks[i])) return i;
            return -1;
        }

        private void Finisher()
        {
            cts.Cancel();
            isRunning = false;
            if (timer1.Enabled) timer1.Stop();
            if (serverCommandSocket != null) serverCommandSocket.Stop();
            if (serverDataSocket != null) serverDataSocket.Stop();
            serverCommandSocket = serverDataSocket = null;
            clientsList.Clear();
            listBoxClients.Items.Clear();
            panelIndicator.BackColor = Color.White;
        }
        #endregion

        private async void buttonStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isRunning)
                {
                    cts = new CancellationTokenSource();
                    serverCommandSocket = new TcpListener(serverIP, commandPort);
                    serverDataSocket = new TcpListener(serverIP, dataPort);
                    timer1.Start();
                    serverCommandSocket.Start();
                    serverDataSocket.Start();
                    isRunning = true;
                    await AsyncConnector(cts.Token);
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Ошибка подключения! " + ex.Message);
                timer1.Stop();
                InitializeEndPoint();
            }
        }

        private void buttonFinish_Click(object sender, EventArgs e)
        {
            //IAsyncResult ar = null;
            //clientSocket.EndConnect()ж
            Finisher();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (clientsList.Count > 0) panelIndicator.BackColor = Color.Green;
            else panelIndicator.BackColor = Color.Red;
            panelIndicator.Invalidate();
        }
    }
}
