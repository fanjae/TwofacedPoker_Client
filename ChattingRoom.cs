using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;
using static System.Windows.Forms.AxHost;

namespace TwofacedPoker_Client
{
    public partial class ChattingRoom_Form : Form
    {
        private Socket socket;
        private String myID;
        private Thread receiveThread;
        private bool isRunning;
        private bool isGamePlaying;
        private bool bothBetting;
        private int bet_type;
        private int temp_bet_type;
        private int chips = 0;
        private int vs_chips = 0;
        private bool die;
        private bool bet;
        private AudioPlayer player;

        public ChattingRoom_Form(Socket socket, String roomName, String myID)
        {
            InitializeComponent();
            this.socket = socket;
            this.Text = roomName;
            this.myID = myID;
            this.isRunning = true;
            this.isGamePlaying = false;
            this.bothBetting = false;
            this.KeyPreview = true;
            this.bet_type = 0;
            this.chips = 0;
            this.vs_chips = 0;
            this.die = false;
            this.bet = false;


            socket.SendTimeout = 0;
            socket.ReceiveTimeout = 0;

            sendTextBox.Select(sendTextBox.Text.Length, 0);
            sendTextBox.ScrollToCaret();

            receiveThread = new Thread(Receive);
            receiveThread.IsBackground = true;
            receiveThread.Start();

            string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image", "Front10.jpg");
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
                            ExitButton.Enabled = true;
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
                            ExitButton.Enabled = false;
                        }));
                    }
                }
            }
        }
        private async void EventHandle(string message)
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
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            Front_Bet_Button.Enabled = true;
                            Both_Bet_Button.Enabled = true;
                            Back_Bet_Button.Enabled = true;
                            Cancle_Button.Enabled = true;
                            Die_Bet_Button.Enabled = true;
                            sendTextBox.Enabled = true;
                            SendButton.Enabled = true;
                            die = false;
                        }));
                    }
                }
                else if ((message.Length >= Constants.TURN.Length && (message.Substring(0, Constants.TURN.Length) == Constants.TURN)))
                {
                    string State = message.Substring(Constants.TURN.Length);
                    if (State == Constants.MY)
                    {
                        if (player == null)
                        {
                            player = new AudioPlayer();
                        }
                        player.PlaySound("my_turn.mp3");

                        /*
                        string currentDir = Directory.GetCurrentDirectory();
                        string soundFilePath = Path.Combine(currentDir, "sound", "my_turn.mp3");
                        wmp.URL = soundFilePath;
                        wmp.controls.play();*/

                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                System_Message.Text = "<System> : 당신의 차례입니다.";
                                My_Turn.Visible = true;
                                Vs_Turn.Visible = false;
                                Cancle_Button.Enabled = true;
                                Die_Bet_Button.Enabled = true;
                                Bet_Chip.Enabled = true;
                                die = false;
                                if (this.bet_type == 0)
                                {
                                    Front_Bet_Button.Enabled = true;
                                    if (this.bothBetting == false)
                                    {
                                        Both_Bet_Button.Enabled = true;
                                    }
                                    Back_Bet_Button.Enabled = true;
                                }
                                else if (this.bet_type == 1)
                                {
                                    Front_Bet_Button.Enabled = true;
                                }
                                else if (this.bet_type == 2)
                                {
                                    Both_Bet_Button.Enabled = true;
                                }
                                else if (this.bet_type == 3)
                                {
                                    Back_Bet_Button.Enabled = true;
                                }
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
                                Cancle_Button.Enabled = false;
                                Die_Bet_Button.Enabled = false;
                                Bet_Chip.Enabled = false;
                            }));
                        }
                    }
                }
                else if ((message.Length >= Constants.BASIC_BETTING.Length && (message.Substring(0, Constants.BASIC_BETTING.Length) == Constants.BASIC_BETTING)))
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            Dealer_Chip.Text = "2";
                        }));
                    }
                }
                else if ((message.Length >= Constants.MY.Length + Constants.CHIP_UPDATE.Length && (message.Substring(0, Constants.MY.Length + Constants.CHIP_UPDATE.Length) == Constants.MY + Constants.CHIP_UPDATE)))
                {
                    string chip_count = message.Substring(Constants.MY.Length + Constants.CHIP_UPDATE.Length);
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            My_Chip.Text = chip_count;
                            chips = int.Parse(chip_count);
                        }));
                    }
                }
                else if ((message.Length >= Constants.OTHER.Length + Constants.CHIP_UPDATE.Length && (message.Substring(0, Constants.OTHER.Length + Constants.CHIP_UPDATE.Length) == Constants.OTHER + Constants.CHIP_UPDATE)))
                {
                    string chip_count = message.Substring(Constants.OTHER.Length + Constants.CHIP_UPDATE.Length);
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            Vs_Chip.Text = chip_count;
                            vs_chips = int.Parse(chip_count);
                        }));
                    }
                }
                else if ((message.Length >= Constants.MY.Length + Constants.CARD_UPDATE.Length && (message.Substring(0, Constants.MY.Length + Constants.CARD_UPDATE.Length) == Constants.MY + Constants.CARD_UPDATE)))
                {
                    string imagePath;
                    string valuePath;
                    string front_or_back = message.Substring(Constants.MY.Length + Constants.CARD_UPDATE.Length);
                    if (front_or_back.Length >= Constants.FRONT.Length && (front_or_back.Substring(0, Constants.FRONT.Length) == Constants.FRONT))
                    {
                        valuePath = "Front" + front_or_back.Substring(Constants.FRONT.Length) + ".jpg";
                        imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image", valuePath);
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                myFront_Card.Image = System.Drawing.Image.FromFile(imagePath);
                            }));
                        }
                    }
                    else if (front_or_back.Length >= Constants.BACK.Length && (front_or_back.Substring(0, Constants.BACK.Length) == Constants.BACK))
                    {
                        valuePath = "Back" + front_or_back.Substring(Constants.BACK.Length) + ".jpg";
                        imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image", valuePath);
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                myBack_Card.Image = System.Drawing.Image.FromFile(imagePath);
                            }));
                        }
                    }
                }
                else if ((message.Length >= Constants.OTHER.Length + Constants.CARD_UPDATE.Length && (message.Substring(0, Constants.OTHER.Length + Constants.CARD_UPDATE.Length) == Constants.OTHER + Constants.CARD_UPDATE)))
                {
                    string other_Front_Value = "Front" + message.Substring(Constants.OTHER.Length + Constants.CARD_UPDATE.Length) + ".jpg";
                    string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image", other_Front_Value);
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            vsFront_Card.Image = System.Drawing.Image.FromFile(imagePath);
                        }));
                    }
                }
                else if ((message.Length >= Constants.BETTING.Length + Constants.IMPOSSIBLE.Length && (message.Substring(0, Constants.BETTING.Length + Constants.IMPOSSIBLE.Length) == Constants.BETTING + Constants.IMPOSSIBLE)))
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            System_Message.Text = "<System> : 베팅을 다시 진행해주시길 바랍니다.";
                        }));
                    }
                }
                else if ((message.Length >= Constants.MY.Length + Constants.BET_UPDATE.Length && (message.Substring(0, Constants.MY.Length + Constants.BET_UPDATE.Length) == Constants.MY + Constants.BET_UPDATE)))
                {
                    string parse_message = message.Substring(Constants.MY.Length + Constants.BET_UPDATE.Length);
                    if (parse_message.Length >= Constants.FRONT.Length && (parse_message.Substring(0, Constants.FRONT.Length) == Constants.FRONT))
                    {
                        int bet_count = int.Parse(parse_message.Substring(Constants.FRONT.Length));
                        this.bet_type = 1;
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                My_Front_Chip.Text = Convert.ToString(bet_count);
                            }));
                        }
                    }
                    else if (parse_message.Length >= Constants.BACK.Length && (parse_message.Substring(0, Constants.BACK.Length) == Constants.BACK))
                    {
                        int bet_count = int.Parse(parse_message.Substring(Constants.BACK.Length));
                        this.bet_type = 3;
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                My_Back_Chip.Text = Convert.ToString(bet_count);
                            }));
                        }
                    }
                }
                else if ((message.Length >= Constants.OTHER.Length + Constants.BET_UPDATE.Length && (message.Substring(0, Constants.OTHER.Length + Constants.BET_UPDATE.Length) == Constants.OTHER + Constants.BET_UPDATE)))
                {
                    string parse_message = message.Substring(Constants.OTHER.Length + Constants.BET_UPDATE.Length);
                    if (parse_message.Length >= Constants.FRONT.Length && (parse_message.Substring(0, Constants.FRONT.Length) == Constants.FRONT))
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                Vs_Front_Chip.Text = Convert.ToString(parse_message.Substring(Constants.FRONT.Length));
                            }));
                        }
                    }
                    else if (parse_message.Length >= Constants.BACK.Length && (parse_message.Substring(0, Constants.BACK.Length) == Constants.BACK))
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                Vs_Back_Chip.Text = Convert.ToString(parse_message.Substring(Constants.BACK.Length));
                            }));
                        }
                    }
                    else if (parse_message.Length >= Constants.BOTH.Length && (parse_message.Substring(0, Constants.BOTH.Length) == Constants.BOTH))
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                bothBetting = true;
                                Both_Bet_Button.Enabled = false;
                            }));
                        }
                    }
                }
                else if ((message.Length == Constants.BATTLE.Length && (message == Constants.BATTLE)))
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            System_Message.Text = "<System> : 상대와의 승부를 시작합니다.";
                            sendTextBox.Enabled = false;
                            SendButton.Enabled = false;
                        }));
                    }
                    Thread.Sleep(3000);
                }
                else if ((message.Length >= Constants.OTHER.Length + Constants.PRINT.Length && (message.Substring(0, Constants.OTHER.Length + Constants.PRINT.Length) == Constants.OTHER + Constants.PRINT)))
                {
                    string parse_message = message.Substring(Constants.OTHER.Length + Constants.PRINT.Length);
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            System_Message.Text = "<System> : 상대의 뒷면카드는 " + parse_message + "입니다.";
                        }));
                    }
                    Thread.Sleep(3000);
                }
                else if ((message.Length == Constants.WAIT.Length))
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            System_Message.Text = "<System> : 뒷면에 베팅이 진행되어, 뒷면 카드를 상대에게 오픈합니다.";
                            sendTextBox.Enabled = false;
                            SendButton.Enabled = false;
                        }));
                    }
                    Thread.Sleep(3000);
                }
                else if ((message.Length >= Constants.GAME_RESULT.Length) && (message.Substring(0, Constants.GAME_RESULT.Length) == Constants.GAME_RESULT))
                {
                    string parse_message = message.Substring(Constants.GAME_RESULT.Length);
                    string event_message = "";
                    bool game_end = false;
                    if (parse_message == Constants.WIN)
                    {
                        /*
                        string currentDir = Directory.GetCurrentDirectory();
                        string soundFilePath = Path.Combine(currentDir, "sound", "win.mp3");
                        wmp.URL = soundFilePath;
                        wmp.controls.play();*/

                        if (player == null)
                        {
                            player = new AudioPlayer();
                        }
                        player.PlaySound("win.mp3");
                        event_message = "<System> : 이번 베팅은 당신의 승리입니다.";
                    }
                    else if (parse_message == Constants.DRAW)
                    {
                        event_message = "<System> : 무승부 입니다.";
                    }
                    else if (parse_message == Constants.LOSE)
                    {
                        if (player == null)
                        {
                            player = new AudioPlayer();
                        }
                        player.PlaySound("lose.mp3");
                        event_message = "<System> : 이번 베팅은 당신의 패배입니다.";
                    }
                    else if (parse_message == Constants.DIE)
                    {
                        event_message = "<System> : 상대가 베팅을 포기하였습니다.";
                    }
                    else if (parse_message == Constants.BOTHWIN)
                    {
                        if (player == null)
                        {
                            player = new AudioPlayer();
                        }
                        player.PlaySound("win.mp3");
                        event_message = "<System> : 양면 베팅에서 승리하였습니다. 칩 10개를 추가로 받습니다.";
                    }
                    else if (parse_message == Constants.BOTHLOSE)
                    {
                        if (player == null)
                        {
                            player = new AudioPlayer();
                        }
                        player.PlaySound("lose.mp3");
                        event_message = "<System> : 앙면 베팅에서 패배하였습니다. 칩 10개를 패널티로 냅니다.";
                    }
                    else if (parse_message == Constants.FINALWIN)
                    {
                        if (player == null)
                        {
                            player = new AudioPlayer();
                        }
                        player.PlaySound("final_win.mp3");
                        event_message = "<System> : 당신의 최종 승리로, 게임을 종료합니다.";
                        game_end = true;
                    }
                    else if (parse_message == Constants.FINALLOSE)
                    {
                        event_message = "<System> : 당신의 최종 패배로, 게임을 종료합니다.";
                        game_end = true;
                    }

                    if (game_end == false)
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                System_Message.Text = event_message;
                                sendTextBox.Enabled = false;
                                SendButton.Enabled = false;
                            }));
                        }
                        Thread.Sleep(3000);
                    }
                    else
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                System_Message.Text = event_message;
                                InitTableChipSetting();
                                isGamePlaying = false;
                                My_Ready.Text = "<준비>";
                                Vs_Ready.Text = "<준비>";
                                ExitButton.Enabled = true;
                                sendTextBox.Enabled = true;
                                SendButton.Enabled = true;
                            }));
                        }
                    }
                }
                else if ((message.Length >= Constants.DEALER.Length + Constants.CHIP_UPDATE.Length) && (message.Substring(0, Constants.DEALER.Length + Constants.CHIP_UPDATE.Length) == Constants.DEALER + Constants.CHIP_UPDATE))
                {
                    string parse_message = message.Substring(Constants.DEALER.Length + Constants.CHIP_UPDATE.Length);
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            Dealer_Chip.Text = parse_message;
                        }));
                    }
                }
                else if ((message.Length >= Constants.SPECIAL.Length) && (message.Substring(0, Constants.SPECIAL.Length) == Constants.SPECIAL))
                {
                    string parse_message = message.Substring(Constants.SPECIAL.Length);
                    string request;
                    if (parse_message == Constants.MY)
                    {
                        parse_message = "<System> : 베팅이 불가능하여 카드를 오픈합니다.";
                        if (IsSocketConnected(socket))
                        {
                            request = Constants.GAME_CLIENT_EVENT + Constants.BETTING + Constants.SPECIAL + 0;
                            PacketHandler.SendPacket(socket, request);
                        }
                    }
                    else if (parse_message == Constants.OTHER)
                    {
                        parse_message = "<System> : 베팅이 불가능하여 카드를 오픈합니다.";
                    }

                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            System_Message.Text = parse_message;
                        }));
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "Error : " + message);
            }
        }

        private void Receive()
        {
            Thread.Sleep(50);

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
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    My_Front_Chip.Text = "0";
                    My_Back_Chip.Text = "0";
                    Vs_Front_Chip.Text = "0";
                    Vs_Back_Chip.Text = "0";
                    bet_type = 0;
                    temp_bet_type = 0;
                    bothBetting = false;
                }));
            }
        }

        private int Get_my_bet_chip()
        {
            return Math.Max(int.Parse(My_Front_Chip.Text), int.Parse(My_Back_Chip.Text));
        }
        private int Get_vs_bet_chip()
        {
            return Math.Max(int.Parse(Vs_Front_Chip.Text), int.Parse(Vs_Back_Chip.Text));
        }
        private bool Can_bet(int chip_check)
        {
            int my_Chips_Count = this.chips;
            int vs_Chips_Count = this.vs_chips;
            int my_bet_chip_count = Get_my_bet_chip();
            int vs_bet_chip_count = Get_vs_bet_chip();

            if (temp_bet_type == 0)
            {
                Bet_Chip_Count.Text = "";
                MessageBox.Show("앞면 / 양면 / 뒷면 中 1개를 선택하여 주시길 바랍니다.", "베팅 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (temp_bet_type == 2 && ((my_Chips_Count < chip_check * 2) || (vs_Chips_Count + vs_bet_chip_count < chip_check + my_bet_chip_count)))
            {
                Bet_Chip_Count.Text = "";
                MessageBox.Show("자신 또는 상대가 보유한 칩 보다 더 많은 숫자를 입력하였습니다.", "베팅 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (vs_bet_chip_count > my_bet_chip_count + chip_check)
            {
                Bet_Chip_Count.Text = "";
                MessageBox.Show("상대가 베팅한 칩 개수 이상 베팅해야 합니다.", "베팅 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                if (my_Chips_Count < chip_check || vs_Chips_Count + vs_bet_chip_count < chip_check + my_bet_chip_count)
                {
                    Bet_Chip_Count.Text = "";
                    MessageBox.Show("본인 혹은 상대가 보유한 칩보다 더 많은 숫자를 입력하였습니다.", "베팅 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private void Bet_Chip_Click(object sender, EventArgs e)
        {
            this.bet = true;
            try
            {
                int bet_Chip_Count_Value = int.Parse(Bet_Chip_Count.Text);
                string game_type = "";
                string request = "";

                if (temp_bet_type == 1)
                {
                    game_type = Constants.FRONT;

                }
                else if (temp_bet_type == 2)
                {
                    game_type = Constants.BOTH;
                }
                else if (temp_bet_type == 3)
                {
                    game_type = Constants.BACK;
                }

                if (Can_bet(bet_Chip_Count_Value) == true)
                {
                    this.bet_type = temp_bet_type;
                    request = Constants.GAME_CLIENT_EVENT + Constants.BETTING + game_type + bet_Chip_Count_Value;
                    PacketHandler.SendPacket(socket, request);
                }
                else
                {
                    this.bet = false;
                }
            }
            catch (FormatException)
            {
                Bet_Chip_Count.Text = "";
                MessageBox.Show("유효하지 않은 값을 입력하였습니다. 숫자만 입력 해주시길 바랍니다.", "베팅 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.bet = false;
                return;
            }
        }

        private void Front_Bet_Button_Click(object sender, EventArgs e)
        {
            if (isGamePlaying == true)
            {
                if (this.bet_type == 0)
                {
                    Both_Bet_Button.Enabled = false;
                    Back_Bet_Button.Enabled = false;
                    temp_bet_type = 1;
                    System_Message.Text = "<System> : 앞면 베팅을 선택하였습니다. : ";
                }
            }
        }

        private void Both_Bet_Button_Click(object sender, EventArgs e)
        {
            if (isGamePlaying == true)
            {
                if (this.bet_type == 0)
                {
                    Front_Bet_Button.Enabled = false;
                    Back_Bet_Button.Enabled = false;
                    temp_bet_type = 2;
                    System_Message.Text = "<System> : 앙면 베팅을 선택하였습니다. : ";
                }
            }
        }

        private void Back_Bet_Button_Click(object sender, EventArgs e)
        {
            if (isGamePlaying == true)
            {
                if (this.bet_type == 0)
                {
                    Front_Bet_Button.Enabled = false;
                    Both_Bet_Button.Enabled = false;
                    temp_bet_type = 3;
                    System_Message.Text = "<System> : 뒷면 베팅을 선택하였습니다. : ";
                }
            }
        }

        private void Cancle_Button_Click(object sender, EventArgs e)
        {
            if (isGamePlaying == true)
            {
                if (this.bet_type == 0)
                {
                    Front_Bet_Button.Enabled = true;
                    Both_Bet_Button.Enabled = true;
                    Back_Bet_Button.Enabled = true;
                    temp_bet_type = 0;
                }
                if (bothBetting)
                {
                    Both_Bet_Button.Enabled = false;
                }
            }
        }

        private void Die_Bet_Button_Click(object sender, EventArgs e)
        {
            if (isGamePlaying == true && die == false)
            {
                die = true;
                if (IsSocketConnected(socket))
                {
                    string request = Constants.GAME_CLIENT_EVENT + Constants.BETTING + Constants.DIE;
                    PacketHandler.SendPacket(socket, request);

                    sendTextBox.Text = "";
                }
            }
        }
    }
}
