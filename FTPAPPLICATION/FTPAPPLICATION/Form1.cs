using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace FTPAPPLICATION
{
    public partial class Form1 : Form
    {
        private TcpClient _ftpClient;
        private NetworkStream _controlStream;
        private StreamReader _reader;
        private StreamWriter _writer;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string serverIp = txtserverIp.Text;  // ������ ��� ��� IP �� �������
            if (string.IsNullOrWhiteSpace(serverIp))
            {
                MessageBox.Show("Please enter the server IP.");
                return;
            }

            try
            {
                _ftpClient = new TcpClient(serverIp, 21);  // ������� ���� FTP Server ��� ������ 21
                _controlStream = _ftpClient.GetStream();
                _reader = new StreamReader(_controlStream, Encoding.ASCII);
                _writer = new StreamWriter(_controlStream, Encoding.ASCII) { AutoFlush = true };

                // ����� ����� ������� �� �������
                string welcomeMessage = _reader.ReadLine();
                LSTOutput.Items.Add(welcomeMessage);

                // ��� ����� ������� (Login)
                string username = txtUsername.Text;
                _writer.WriteLine($"USER {username}");
                LSTOutput.Items.Add(_reader.ReadLine());

                string password = txtPassword.Text;
                _writer.WriteLine($"PASS {password}");
                LSTOutput.Items.Add(_reader.ReadLine());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to server: {ex.Message}");
            }
        }

        private void btnSendCommand_Click(object sender, EventArgs e)
        {
            string command = txtCommand.Text;
            if (string.IsNullOrWhiteSpace(command)) return;

            _writer.WriteLine(command);  // ����� ����� ��� �������
            string response = _reader.ReadLine();  // ����� ���������
            LSTOutput.Items.Add(response);
        }

        // Method to close the connection properly
        private void CloseConnection()
        {
            _reader?.Close();
            _writer?.Close();
            _controlStream?.Close();
            _ftpClient?.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseConnection();
        }
    }
}
