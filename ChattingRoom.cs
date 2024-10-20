using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

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
        private int bet_type;

        public ChattingRoom_Form(Socket socket, String roomName, String myID)
        {
            InitializeComponent();
            this.socket = socket;
            this.Text = roomName;
            this.myID = myID;
            this.isRunning = true;
            this.isGamePlaying = false;
            this.KeyPreview = true;
            this.bet_type = 0;


            socket.SendTimeout = 0;
            socket.ReceiveTimeout = 0;

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
            if (this.isGamePlaying == false && IsSocketConnected(socket))
            {
                if (e.KeyCode == Keys.F5 && My_Ready.Text == "<준비>") // F5를 눌렀을때
                {
                    string request = Constants.ROOM_EVENT + Constants.USER_READY_STATE + Constants.DONE;
                    PacketHandler.SendPacket(socket, request);

                    My_Ready.Text = "<완료>";
                }
                else if (e.KeyCode == Keys.F5 && My_Ready.Text == "<완료>")
                {
                    string request = Constants.ROOM_EVENT + Constants.USER_READY_STATE + Constants.READY;
                    PacketHandler.SendPacket(socket, request);

                    My_Ready.Text = "<준비>";
                }
                else if (e.KeyCode == Keys.F6 && My_Ready.Text == "<완료>" && Vs_Ready.Text == "<완료>") // F6를 눌렀을때
                {
                    string request = Constants.GAME_CLIENT_EVENT + Constants.GAME_START;
                    PacketHandler.SendPacket(socket, request);
                }
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (sendTextBox.Text != "")
            {
                if (IsSocketConnected(socket))
                {
                    string request = sendTextBox.Text;
                    PacketHandler.SendPacket(socket, request);

                    sendTextBox.Text = "";
                }
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

        private void RoomHandle(string message)
        {
            if ((message.Length >= Constants.UPDATE_ID.Length && (message.Substring(0, Constants.UPDATE_ID.Length) == Constants.UPDATE_ID)))
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        Vs_ID_Label.Text = "ID : " + message.Substring(Constants.UPDATE_ID.Length);
                    }));
                }
            }
            else if ((message.Length >= Constants.UPDATE_READY_STATE.Length && (message.Substring(0, Constants.UPDATE_READY_STATE.Length) == Constants.UPDATE_READY_STATE)))
            {
                string State = message.Substring(Constants.UPDATE_READY_STATE.Length);
                if (State == Constants.READY)
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            Vs_Ready.Text = "<준비>";
                        }));
                    }
                }
                else if (State == Constants.DONE)
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            Vs_Ready.Text = "<완료>";
                        }));
                    }
                }
            }
        }
        private void EventHandle(string message)
        {
            try
            {
                if ((message.Length >= Constants.START.Length && (message.Substring(0, Constants.START.Length) == Constants.START)))
                {
                    string State = message.Substring(Constants.START.Length);
                    if (State == Constants.READY)
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                System_Message.Text = "<System> : 모든 유저가 시작을 하지 않은 상태입니다.";
                            }));
                        }
                    }
                    else if (State == Constants.DONE)
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                System_Message.Text = "<System> : 게임을 시작합니다.";
                                this.isGamePlaying = true;
                            }));
                        }
                    }
                }
                else if ((message.Length >= Constants.GAME_INIT.Length && (message.Substring(0, Constants.GAME_INIT.Length) == Constants.GAME_INIT)))
                {
                    InitTableChipSetting();
                }
                if ((message.Length >= Constants.TURN.Length && (message.Substring(0, Constants.TURN.Length) == Constants.TURN)))
                {
                    string State = message.Substring(Constants.TURN.Length);
                    if (State == Constants.MY)
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                System_Message.Text = "<System> : 당신의 차례입니다.";
                                My_Turn.Visible = true;
                                Vs_Turn.Visible = false;
                                Front_Bet_Button.Enabled = true;
                                Both_Bet_Button.Enabled = true;
                                Back_Bet_Button.Enabled = true;
                            }));
                        }
                    }
                    else if (State == Constants.OTHER)
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                System_Message.Text = "<System> : 상대방의 차례입니다.";
                                My_Turn.Visible = false;
                                Vs_Turn.Visible = true;
                                Front_Bet_Button.Enabled = false;
                                Both_Bet_Button.Enabled = false;
                                Back_Bet_Button.Enabled = false;
                            }));
                        }
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " " + message);
            }
        }

        private void Receive()
        {
            string request = Constants.USER_UPDATE;
            PacketHandler.SendPacket(socket, request);
            while (isRunning)
            {
                 try
                 {
                    string response = PacketHandler.ReceivePakcet(socket);
                    if (string.IsNullOrEmpty(response))
                    {
                        MessageBox.Show("서버 연결이 종료되었습니다.");
                        isRunning = false;
                        socket.Close();
                        break;
                    }
                    else if (response == Constants.EXIT_ROOM_COMPLETE)
                    {
                        isRunning = false;
                        break;
                    }
                    else if (response.StartsWith(Constants.ROOM_EVENT)) 
                    {
                         RoomHandle(response.Substring(Constants.ROOM_EVENT.Length));
                    }
                    else if (response.StartsWith(Constants.GAME_CLIENT_EVENT))
                    {
                         EventHandle(response.Substring(Constants.GAME_CLIENT_EVENT.Length));
                    }
                    else
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                chattingRoomTextBox.AppendText(response + "\r\n");
                            }));
                        }
                    }
                 }
                 catch (SocketException ex)
                 {
                     // 소켓 예외 처리
                     MessageBox.Show("서버 연결이 끊어졌습니다: " + ex.Message);
                     isRunning = false;
                 } 
                 catch (Exception ex)
                 {
                    // 일반 예외 처리
                    MessageBox.Show("오류 발생: " + ex.Message);
                    isRunning = false;
                 }
             }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            if (!isGamePlaying)
            {
                this.Close();
            }
        }

        private void ChattingRoom_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsSocketConnected(socket))
            {
                string request = Constants.EXIT_ROOM;
                PacketHandler.SendPacket(socket, request);
            }

            isRunning = false;
            if (receiveThread != null && receiveThread.IsAlive)
            {
                receiveThread.Join();
            }
        }
        private void InitTableChipSetting()
        {
            My_Front_Chip.Text = "0";
            My_Back_Chip.Text = "0";
            Vs_Front_Chip.Text = "0";
            Vs_Back_Chip.Text = "0";
            Dealer_Chip.Text = "0";
            bet_type = 0;
        }
    }
}
