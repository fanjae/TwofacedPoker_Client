﻿using System;
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

<<<<<<< HEAD
=======
            Pre_Call();            

>>>>>>> 3e9497634cc7449ac557eb1edcd6479bd222cda6
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
                if (e.KeyCode == Keys.F5 && My_Ready.Text == "<준비>") // 스페이스바를 눌렀을 때
                {
                    string request = Constants.GAME_CLIENT_EVENT + Constants.GAME_READY + Constants.READY_DONE;
<<<<<<< HEAD
                    PacketHandler.SendPacket(socket, request);
=======
                    byte[] buffer = Encoding.UTF8.GetBytes(request);
                    socket.Send(buffer);
>>>>>>> 3e9497634cc7449ac557eb1edcd6479bd222cda6

                    My_Ready.Text = "<완료>";
                }
                else if (e.KeyCode == Keys.F5 && My_Ready.Text == "<완료>")
                {
                    string request = Constants.GAME_CLIENT_EVENT + Constants.GAME_READY + Constants.READY_NOT_DONE;
<<<<<<< HEAD
                    PacketHandler.SendPacket(socket, request);
=======
                    byte[] buffer = Encoding.UTF8.GetBytes(request);
                    socket.Send(buffer);
>>>>>>> 3e9497634cc7449ac557eb1edcd6479bd222cda6

                    My_Ready.Text = "<준비>";
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

<<<<<<< HEAD
        private void RoomHandle(string message)
=======
        private void EventHandle(string message)
        {
            MessageBox.Show("Message: " + message);
            MessageBox.Show("Substring: " + message.Substring(Constants.GAME_CLIENT_EVENT.Length));
            MessageBox.Show("Message_length : " + message.Length.ToString());
            try
            {
                if (message.Substring(Constants.GAME_CLIENT_EVENT.Length) == Constants.READY_DONE)
                {
                    Invoke(new Action(() =>
                    {
                        Vs_Ready.Text = "<완료>";
                    }));
                }
                else if (message.Substring(Constants.GAME_CLIENT_EVENT.Length) == Constants.READY_NOT_DONE)
                {
                    Invoke(new Action(() =>
                    {
                        Vs_Ready.Text = "<준비>";
                    }));
                }
                else if (message.Length >= Constants.GAME_CLIENT_EVENT.Length + Constants.LOAD_PLAYER.Length &&  message.Substring(Constants.GAME_CLIENT_EVENT.Length, Constants.LOAD_PLAYER.Length) == Constants.LOAD_PLAYER)
                {
                    Invoke(new Action(() =>
                    {
                        Vs_ID_Label.Text = "ID : " + message.Substring(Constants.GAME_CLIENT_EVENT.Length + Constants.LOAD_PLAYER.Length);
                    }));
                }
                else if (message.Length >= Constants.GAME_CLIENT_EVENT.Length + Constants.GAME_PRE_LOAD_PLAYER.Length && message.Substring(Constants.GAME_CLIENT_EVENT.Length, Constants.GAME_PRE_LOAD_PLAYER.Length) == Constants.GAME_PRE_LOAD_PLAYER)
                {

                    string request = Constants.GAME_CLIENT_EVENT + Constants.GAME_PRE_LOAD_ID + My_ID_Label.Text.Substring(6);
                    byte[] buffer = Encoding.UTF8.GetBytes(request);
                    socket.Send(buffer);

                    if (My_Ready.Text == "<준비>")
                    {
                        request = Constants.GAME_CLIENT_EVENT + Constants.GAME_PRE_LOAD_READY;
                        buffer = Encoding.UTF8.GetBytes(request);
                        socket.Send(buffer);
                    }
                    else
                    {
                        request = Constants.GAME_CLIENT_EVENT + Constants.GAME_PRE_LOAD_DONE;
                        buffer = Encoding.UTF8.GetBytes(request);
                        socket.Send(buffer);
                    }
                }
                else if (message.Length >= Constants.GAME_CLIENT_EVENT.Length + Constants.GAME_PRE_LOAD_ID_DONE.Length && message.Substring(Constants.GAME_CLIENT_EVENT.Length, Constants.GAME_PRE_LOAD_ID_DONE.Length) == Constants.GAME_PRE_LOAD_ID_DONE)
                {
                    Invoke(new Action(() =>
                    {
                        Vs_ID_Label.Text = "ID : " + message.Substring(Constants.GAME_CLIENT_EVENT.Length + Constants.GAME_PRE_LOAD_ID_DONE.Length);
                    }));
                }
                else if (message.Substring(Constants.GAME_CLIENT_EVENT.Length) == Constants.GAME_PRE_LOAD_READY_DONE)
                {
                    Invoke(new Action(() =>
                    {
                        Vs_Ready.Text = "<준비>";

                    }));
                }
                else if (message.Substring(Constants.GAME_CLIENT_EVENT.Length) == Constants.GAME_PRE_LOAD_DONE_DONE)
                {
                    Invoke(new Action(() =>
                    {
                        Vs_Ready.Text = "<완료>";

                    }));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " " + message);
            }
        }

        private void Pre_Call()
        {
            string request = Constants.GAME_CLIENT_EVENT + Constants.GAME_PRE_CALL;
            byte[] buffer = Encoding.UTF8.GetBytes(request);
            socket.Send(buffer);
        }
        private void Receive()
>>>>>>> 3e9497634cc7449ac557eb1edcd6479bd222cda6
        {
            if ((message.Length >= Constants.ROOM_EVENT.Length + Constants.UPDATE_ID.Length && (message.Substring(Constants.ROOM_EVENT.Length, Constants.UPDATE_ID.Length) == Constants.UPDATE_ID)))
            {
                if (InvokeRequired)
                {
<<<<<<< HEAD
                    Invoke(new Action(() =>
=======
                    byte[] buffer = new byte[1024];
                    int recv_length = 0;

                    try
>>>>>>> 3e9497634cc7449ac557eb1edcd6479bd222cda6
                    {
                        Vs_ID_Label.Text = "ID : " + message.Substring(Constants.ROOM_EVENT.Length + Constants.UPDATE_ID.Length);
                    }));
                }
            }
            else if ((message.Length >= Constants.ROOM_EVENT.Length + Constants.UPDATE_READY_STATE.Length && (message.Substring(Constants.ROOM_EVENT.Length, Constants.UPDATE_READY_STATE.Length) == Constants.UPDATE_READY_STATE)))
            {
                string State = message.Substring(Constants.ROOM_EVENT.Length + Constants.UPDATE_READY_STATE.Length);
                if (State == Constants.READY)
                {
                    if (InvokeRequired)
                    {
<<<<<<< HEAD
=======
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
                        if (response.StartsWith(Constants.GAME_CLIENT_EVENT))
                        {
                            EventHandle(response);
                            continue;
                        }


>>>>>>> 3e9497634cc7449ac557eb1edcd6479bd222cda6
                        Invoke(new Action(() =>
                        {
                            Vs_ID_Label.Text = "<준비>";
                        }));
                    }
                }
                else if (State == Constants.DONE)
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            Vs_ID_Label.Text = "<완료>";
                        }));
                    }
                }
            }
        }
        private void EventHandle(string message)
        {
            try
            {
                if (message.Substring(Constants.GAME_CLIENT_EVENT.Length) == Constants.READY_DONE)
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            Vs_Ready.Text = "<완료>";
                        }));
                    }
                }
                else if (message.Substring(Constants.GAME_CLIENT_EVENT.Length) == Constants.READY_NOT_DONE)
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            Vs_Ready.Text = "<준비>";
                        }));
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
                         RoomHandle(response);
                    }
                    else if (response.StartsWith(Constants.GAME_CLIENT_EVENT))
                    {
                         EventHandle(response);
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
            this.Close();
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
    }
}
