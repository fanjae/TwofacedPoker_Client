using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwofacedPoker_Client
{
    public partial class ChattingRoom_Form : Form
    {
        private Socket socket;
        private String roomName;
        private String myID;
        private Thread receiveThread;
        private bool isRunning;
        private bool isGamePlaying;

        public ChattingRoom_Form(Socket socket, String roomName, String myID)
        {
            InitializeComponent();
            this.socket = socket;
            this.Text = roomName;
            this.myID = myID;
            this.isRunning = true;
            this.isGamePlaying = false;
            this.KeyPreview = true;


            socket.ReceiveTimeout = 1000;

            sendTextBox.Select(sendTextBox.Text.Length, 0);
            sendTextBox.ScrollToCaret();

            receiveThread = new Thread(Receive);
            receiveThread.IsBackground = true;
            receiveThread.Start();

            string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image", "front10.jpg");
            string imagePath2 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image", "Back10.jpg");
            myFront_Card.Image = System.Drawing.Image.FromFile(imagePath);
            vsFront_Card.Image = System.Drawing.Image.FromFile(imagePath);
            myBack_Card.Image = System.Drawing.Image.FromFile(imagePath2);

            My_ID_Label.Text = "ID : " + myID;
        }

        private bool IsSocketConnected(Socket socket)
        {
            try
            {
                if (socket == null || socket.Connected == false)
                {
                    throw new InvalidOperationException("서버와 연결할 수 없습니다.");
                }
                return true;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("알 수 없는 오류가 발생했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private void ChattingRoom_Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.isGamePlaying == false)
            {
                if (e.KeyCode == Keys.F5 && My_Ready.Text == "<준비>") // 스페이스바를 눌렀을 때
                {
                    My_Ready.Text = "<완료>";
                }
                else if (e.KeyCode == Keys.F5 && My_Ready.Text == "<완료>")
                {
                    My_Ready.Text = "<준비>";
                }
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (IsSocketConnected(socket))
            {
                string request = sendTextBox.Text;
                byte[] buffer = Encoding.UTF8.GetBytes(request);
                socket.Send(buffer);

                sendTextBox.Text = "";
            }
        }

        private void sendTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.SendButton_Click(sender, e);
            }
        }

        private void Receive()
        {
            try
            {
                while (isRunning)
                {
                    byte[] buffer = new byte[4096];
                    int recv_length = 0;

                    try
                    {
                        recv_length = socket.Receive(buffer);
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode == SocketError.TimedOut)
                        {
                            continue;
                        }
                        else
                        {
                            throw;
                        }
                    }
                    if (recv_length > 0)
                    {
                        string response = Encoding.UTF8.GetString(buffer, 0, recv_length);


                        Invoke(new Action(() =>
                        {
                            chattingRoomTextBox.AppendText(response);
                            chattingRoomTextBox.AppendText("\r\n");

                        }));
                    }
                }
            }
            catch (SocketException ex)
            {
                if (isRunning)
                {
                    MessageBox.Show("서버와의 연결이 끊어졌습니다. " + ex.Message);
                }
            }
            catch (ObjectDisposedException)
            {

            }
            catch (InvalidOperationException)
            {

            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ChattingRoom_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsSocketConnected(socket))
            {
                string request = Constants.EXIT_ROOM;
                byte[] buffer = Encoding.UTF8.GetBytes(request);
                socket.Send(buffer);
            }

            isRunning = false;
            if (receiveThread != null && receiveThread.IsAlive)
            {
                receiveThread.Join();
            }
        }
    }
}
