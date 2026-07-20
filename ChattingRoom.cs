using System.Net.Sockets;
using System.IO;

namespace TwofacedPoker_Client
{
    public partial class ChattingRoom_Form : Form
    {
        private Socket socket;
        private string myID;
        private Thread receiveThread;
        private volatile bool isRunning;
        private volatile bool isClosing;
        private AudioPlayer player;
        private readonly ClientGameState gameState = new ClientGameState();

        public ChattingRoom_Form(Socket socket,string roomName,string myID)
        {
            InitializeComponent();

            this.socket = socket;
            this.myID = myID;

            Text = roomName;
            isRunning = true;
            KeyPreview = true;

            socket.SendTimeout = 0;
            socket.ReceiveTimeout = 0;

            myFront_Card.SizeMode = PictureBoxSizeMode.Zoom;
            myBack_Card.SizeMode = PictureBoxSizeMode.Zoom;
            vsFront_Card.SizeMode = PictureBoxSizeMode.Zoom;

            SetCardImage(myFront_Card, "Front10.jpg");
            SetCardImage(vsFront_Card, "Front10.jpg");
            SetCardImage(myBack_Card, "Back10.jpg");

            My_ID_Label.Text = "ID : " + myID;

            sendTextBox.Select(
                sendTextBox.Text.Length,
                0);

            sendTextBox.ScrollToCaret();

            receiveThread = new Thread(Receive)
            {
                IsBackground = true
            };

            receiveThread.Start();
        }

        private bool IsSocketConnected()
        {
            return socket != null && socket.Connected && !isClosing;
        }
        private void ChattingRoom_Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.gameState.IsGamePlaying == false && IsSocketConnected())
            {
                if (e.KeyCode == Keys.F5 && My_Ready.Text == "<준비>") // F5를 눌렀을때
                {
                    string request = Constants.ROOM_EVENT + Constants.USER_READY_STATE + Constants.DONE;
                    if (TrySendPacket(request))
                    {
                        My_Ready.Text = "<완료>";
                    }
                }
                else if (e.KeyCode == Keys.F5 && My_Ready.Text == "<완료>")
                {
                    string request = Constants.ROOM_EVENT + Constants.USER_READY_STATE + Constants.READY;
                    if (TrySendPacket(request))
                    {
                        My_Ready.Text = "<준비>";
                    }
                }
                else if (e.KeyCode == Keys.F6 && My_Ready.Text == "<완료>" && Vs_Ready.Text == "<완료>") // F6를 눌렀을때
                {
                    string request = Constants.GAME_CLIENT_EVENT + Constants.GAME_START;
                    TrySendPacket(request);
                }
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            string message = sendTextBox.Text.Trim();

            if (message.Length == 0)
            {
                return;
            }

            if (TrySendPacket(message))
            {
                sendTextBox.Clear();
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
            if (message.StartsWith(Constants.UPDATE_ID))
            {
                string opponentId = message.Substring(Constants.UPDATE_ID.Length);

                RunOnUiThread(() =>
                {
                    Vs_ID_Label.Text = "ID : " + opponentId;
                });

                return;
            }

            if (!message.StartsWith(Constants.UPDATE_READY_STATE))
            {
                return;
            }

            string state = message.Substring(Constants.UPDATE_READY_STATE.Length);

            if (state == Constants.READY)
            {
                RunOnUiThread(() =>
                {
                    Vs_Ready.Text = "<준비>";
                    ExitButton.Enabled = true;
                });
            }
            else if (state == Constants.DONE)
            {
                RunOnUiThread(() =>
                {
                    Vs_Ready.Text = "<완료>";
                    ExitButton.Enabled = false;
                });
            }
        }
        private void EventHandle(string message)
        {
            try
            {
                if (message.StartsWith(Constants.START))
                {
                    HandleStart(message.Substring(Constants.START.Length));
                }
                else if (message.StartsWith(Constants.TURN))
                {
                    string state = message.Substring(Constants.TURN.Length);
                    HandleTurn(state);
                }
                else if (message.StartsWith(Constants.GAME_RESULT))
                {
                    string result =
                        message.Substring(Constants.GAME_RESULT.Length);

                    HandleGameResult(result);
                }
                else if (message == Constants.GAME_INIT)
                {
                    HandleGameInit();
                }
                else if (message.StartsWith(Constants.MY + Constants.CHIP_UPDATE))
                {
                    string chipText = message.Substring(Constants.MY.Length + Constants.CHIP_UPDATE.Length);
                    HandleChipUpdate(chipText, true);
                }
                else if (message.StartsWith(Constants.OTHER + Constants.CHIP_UPDATE))
                {
                    string chipText = message.Substring(Constants.OTHER.Length + Constants.CHIP_UPDATE.Length);
                    HandleChipUpdate(chipText, false);
                }
                else if (message.StartsWith(Constants.MY + Constants.CARD_UPDATE))
                {
                    string cardMessage = message.Substring(Constants.MY.Length + Constants.CARD_UPDATE.Length);
                    HandleMyCardUpdate(cardMessage);
                }
                else if (message.StartsWith(Constants.OTHER + Constants.CARD_UPDATE))
                {
                    string cardValue = message.Substring(Constants.OTHER.Length + Constants.CARD_UPDATE.Length);
                    HandleOpponentCardUpdate(cardValue);
                }
                else if (message == Constants.BETTING + Constants.IMPOSSIBLE)
                {
                    HandleBettingImpossible();
                }
                else if (message.StartsWith(Constants.MY + Constants.BET_UPDATE))
                {
                    string betMessage = message.Substring(Constants.MY.Length + Constants.BET_UPDATE.Length);
                    HandleMyBetUpdate(betMessage);
                }
                else if (message.StartsWith(Constants.OTHER + Constants.BET_UPDATE))
                {
                    string betMessage = message.Substring(Constants.OTHER.Length + Constants.BET_UPDATE.Length);
                    HandleOpponentBetUpdate(betMessage);
                }

                else if (message == Constants.BATTLE)
                {
                    HandleBattle();
                }
                else if (message.StartsWith(Constants.OTHER + Constants.PRINT))
                {
                    string cardValue = message.Substring(Constants.OTHER.Length + Constants.PRINT.Length);
                    HandleOpponentCardPrint(cardValue);
                }
                else if (message == Constants.WAIT)
                {
                    HandleWait();
                }
                else if (message == Constants.BASIC_BETTING)
                {
                    UpdateDealerChip("2");
                }
                else if (message.StartsWith(Constants.DEALER + Constants.CHIP_UPDATE))
                {
                    string dealerChip = message.Substring(Constants.DEALER.Length + Constants.CHIP_UPDATE.Length);
                    UpdateDealerChip(dealerChip);
                }
                else if (message.StartsWith(Constants.SPECIAL))
                {
                    string target = message.Substring(Constants.SPECIAL.Length);
                    HandleSpecial(target);
                }
            }
            catch (Exception ex)
            {
                RunOnUiThread(() =>
                {
                    MessageBox.Show(ex.Message + Environment.NewLine + "수신 메시지: " + message,"패킷 처리 오류",MessageBoxButtons.OK,MessageBoxIcon.Error);
                });
            }
        }

        private void Receive()
        {

            if (!TrySendPacket(Constants.USER_UPDATE))
            {
                isRunning = false;
                return;
            }
            while (isRunning)
            {
                try
                {
                    string response = PacketHandler.ReceivePacket(socket);
                    if (string.IsNullOrEmpty(response))
                    {
                        RunOnUiThread(() =>
                        {
                            MessageBox.Show("서버 연결이 종료되었습니다.","연결 종료",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        });
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
                        RunOnUiThread(() =>
                        {
                            chattingRoomTextBox.AppendText(response + Environment.NewLine);
                        });
                    }
                }
                catch (SocketException ex)
                {
                    isRunning = false;

                    if (isClosing)
                    {
                        return;
                    }

                    RunOnUiThread(() =>
                    {
                        MessageBox.Show("서버 연결이 끊어졌습니다: " + ex.Message,"연결 오류",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    });
                }
                catch (ObjectDisposedException)
                {
                    isRunning = false;

                    if (!isClosing)
                    {
                        RunOnUiThread(() =>
                        {
                            MessageBox.Show("서버 연결이 종료되었습니다.","연결 종료", MessageBoxButtons.OK,MessageBoxIcon.Information);
                        });
                    }
                }
                catch (Exception ex)
                {
                    isRunning = false;

                    if (isClosing)
                    {
                        return;
                    }

                    RunOnUiThread(() =>
                    {
                        MessageBox.Show("오류 발생: " + ex.Message,"수신 오류",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    });
                }
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            if (!gameState.IsGamePlaying)
            {
                this.Close();
            }
        }

        private void ChattingRoom_Form_FormClosing(object sender,FormClosingEventArgs e)
        {
            isClosing = true;

            try
            {
                if (socket != null && socket.Connected)
                {
                    PacketHandler.SendPacket(socket,Constants.EXIT_ROOM);
                }
            }
            catch (SocketException)
            {
            }
            catch (EndOfStreamException)
            {
            }
            catch (ObjectDisposedException)
            {
            }

            if (receiveThread != null && receiveThread.IsAlive && Thread.CurrentThread != receiveThread)
            {
                receiveThread.Join(500);
            }
        }
        private void InitTableChipSetting()
        {
            gameState.ResetRound();

            RunOnUiThread(() =>
            {
                My_Front_Chip.Text = "0";
                My_Back_Chip.Text = "0";
                Vs_Front_Chip.Text = "0";
                Vs_Back_Chip.Text = "0";

                Bet_Chip_Count.Text = string.Empty;

                Front_Bet_Button.Enabled = false;
                Both_Bet_Button.Enabled = false;
                Back_Bet_Button.Enabled = false;
                Bet_Chip.Enabled = false;
                Cancle_Button.Enabled = false;
                Die_Bet_Button.Enabled = false;
            });
        }

        private bool CanBet(int chipCount)
        {
            int myChips = gameState.MyChips;
            int opponentChips = gameState.OpponentChips;
            int myBet = gameState.MyHighestBet;
            int opponentBet = gameState.OpponentHighestBet;

            if (chipCount <= 0)
            {
                Bet_Chip_Count.Text = string.Empty;
                MessageBox.Show("베팅 칩은 1개 이상 입력해야 합니다.","베팅 오류",MessageBoxButtons.OK,MessageBoxIcon.Error);

                return false;
            }

            if (gameState.SelectedBetType == BetType.None)
            {
                Bet_Chip_Count.Text = string.Empty;
                MessageBox.Show("앞면 / 양면 / 뒷면 중 하나를 선택해 주세요.","베팅 오류",MessageBoxButtons.OK,MessageBoxIcon.Error);

                return false;
            }

            if (gameState.SelectedBetType == BetType.Both)
            {
                bool insufficientMyChips = myChips < chipCount * 2;
                bool insufficientOpponentChips = opponentChips + opponentBet < chipCount + myBet;

                if (insufficientMyChips || insufficientOpponentChips)
                {
                    Bet_Chip_Count.Text = string.Empty;
                    MessageBox.Show("자신 또는 상대가 보유한 칩보다 많은 수를 입력했습니다.","베팅 오류",MessageBoxButtons.OK,MessageBoxIcon.Error);

                    return false;
                }
            }
            else
            {
                bool insufficientMyChips = myChips < chipCount;
                bool insufficientOpponentChips = opponentChips + opponentBet < chipCount + myBet;

                if (insufficientMyChips || insufficientOpponentChips)
                {
                    Bet_Chip_Count.Text = string.Empty;
                    MessageBox.Show("자신 또는 상대가 보유한 칩보다 많은 수를 입력했습니다.","베팅 오류",MessageBoxButtons.OK,MessageBoxIcon.Error);

                    return false;
                }
            }

            if (opponentBet > myBet + chipCount)
            {
                Bet_Chip_Count.Text = string.Empty;
                MessageBox.Show("상대가 베팅한 칩 개수 이상 베팅해야 합니다.","베팅 오류",MessageBoxButtons.OK,MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        private void Bet_Chip_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(Bet_Chip_Count.Text,out int betChipCount))
            {
                Bet_Chip_Count.Clear();
                MessageBox.Show("유효하지 않은 값입니다. 숫자만 입력해 주세요.","베팅 오류", MessageBoxButtons.OK,MessageBoxIcon.Error);
               
                return;
            }

            if (!CanBet(betChipCount))
            {
                return;
            }

            string gameType = gameState.SelectedBetType switch
            {
                BetType.Front => Constants.FRONT,
                BetType.Both => Constants.BOTH,
                BetType.Back => Constants.BACK,
                _ => string.Empty
            };

            if (gameType.Length == 0)
            {
                return;
            }

            string request = Constants.GAME_CLIENT_EVENT + Constants.BETTING + gameType + betChipCount;
            TrySendPacket(request);
        }

        private void Front_Bet_Button_Click(object sender, EventArgs e)
        {
            if (!gameState.IsGamePlaying)
            {
                return;
            }

            if (gameState.BetType != BetType.None && gameState.BetType != BetType.Front)
            {
                return;
            }

            gameState.SelectedBetType = BetType.Front;

            Both_Bet_Button.Enabled = false;
            Back_Bet_Button.Enabled = false;
        }

        private void Both_Bet_Button_Click(object sender, EventArgs e)
        {
            if (!gameState.IsGamePlaying)
            {
                return;
            }

            if (gameState.BetType != BetType.None &&
                gameState.BetType != BetType.Both)
            {
                return;
            }

            gameState.SelectedBetType = BetType.Both;

            Front_Bet_Button.Enabled = false;
            Back_Bet_Button.Enabled = false;
        }

        private void Back_Bet_Button_Click(object sender, EventArgs e)
        {
            if (!gameState.IsGamePlaying)
            {
                return;
            }

            if (gameState.BetType != BetType.None && gameState.BetType != BetType.Back)
            {
                return;
            }

            gameState.SelectedBetType = BetType.Back;

            Front_Bet_Button.Enabled = false;
            Both_Bet_Button.Enabled = false;
        }

        private void Cancel_Button_Click(object sender,EventArgs e)
        {
            if (!gameState.IsGamePlaying)
            {
                return;
            }

            if (gameState.BetType != BetType.None)
            {
                return;
            }

            gameState.SelectedBetType = BetType.None;

            Front_Bet_Button.Enabled = true;
            Back_Bet_Button.Enabled = true;
            Both_Bet_Button.Enabled = !gameState.IsBothBettingUsed;
        }

        private void Die_Bet_Button_Click(object sender, EventArgs e)
        {
            if (gameState.IsGamePlaying == true && gameState.HasFolded == false)
            {
                gameState.HasFolded = true;
                string request = Constants.GAME_CLIENT_EVENT + Constants.BETTING + Constants.DIE;

                if (TrySendPacket(request))
                {
                    sendTextBox.Clear();
                }
                else
                {
                    gameState.HasFolded = false;
                }
            }
        }
        private void RunOnUiThread(Action action)
        {
            if (IsDisposed || Disposing || !IsHandleCreated)
            {
                return;
            }

            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(action);
                    return;
                }

                action();
            }
            catch (InvalidOperationException)
            {
                // 폼 종료 과정에서 핸들이 제거된 경우
            }
        }

        private void HandleGameResult(string result)
        {
            string eventMessage;
            bool gameEnd = false;

            switch (result)
            {
                case var value when value == Constants.WIN:
                    PlaySound("win.mp3");
                    eventMessage = "<System> : 이번 베팅은 당신의 승리입니다.";
                    break;

                case var value when value == Constants.DRAW:
                    eventMessage = "<System> : 무승부입니다.";
                    break;

                case var value when value == Constants.LOSE:
                    PlaySound("lose.mp3");
                    eventMessage = "<System> : 이번 베팅은 당신의 패배입니다.";
                    break;

                case var value when value == Constants.DIE:
                    eventMessage = "<System> : 상대가 베팅을 포기하였습니다.";
                    break;

                case var value when value == Constants.BOTHWIN:
                    PlaySound("win.mp3");
                    eventMessage = "<System> : 양면 베팅에서 승리하였습니다. " +
                        "칩 10개를 추가로 받습니다.";
                    break;

                case var value when value == Constants.BOTHLOSE:
                    PlaySound("lose.mp3");
                    eventMessage = "<System> : 양면 베팅에서 패배하였습니다. " +
                        "칩 10개를 패널티로 냅니다.";
                    break;

                case var value when value == Constants.FINALWIN:
                    PlaySound("final_win.mp3");
                    eventMessage = "<System> : 당신의 최종 승리로, 게임을 종료합니다.";
                    gameEnd = true;
                    break;

                case var value when value == Constants.FINALLOSE:
                    eventMessage = "<System> : 당신의 최종 패배로, 게임을 종료합니다.";
                    gameEnd = true;
                    break;

                default:
                    return;
            }

            if (!gameEnd)
            {
                RunOnUiThread(() =>
                {
                    System_Message.Text = eventMessage;
                    sendTextBox.Enabled = false;
                    SendButton.Enabled = false;
                });

                return;
            }

            gameState.ResetGame();
            InitTableChipSetting();

            RunOnUiThread(() =>
            {
                System_Message.Text = eventMessage;

                My_Ready.Text = "<준비>";
                Vs_Ready.Text = "<준비>";

                ExitButton.Enabled = true;
                sendTextBox.Enabled = true;
                SendButton.Enabled = true;
            });
        }
        private void HandleTurn(string state)
        {
            if (state == Constants.MY)
            {
                gameState.HasFolded = false;
                PlaySound("my_turn.mp3");

                RunOnUiThread(() =>
                {
                    System_Message.Text = "<System> : 당신의 차례입니다.";

                    My_Turn.Visible = true;
                    Vs_Turn.Visible = false;

                    Cancle_Button.Enabled = true;
                    Die_Bet_Button.Enabled = true;
                    Bet_Chip.Enabled = true;

                    Front_Bet_Button.Enabled = false;
                    Both_Bet_Button.Enabled = false;
                    Back_Bet_Button.Enabled = false;

                    switch (gameState.BetType)
                    {
                        case BetType.None:
                            Front_Bet_Button.Enabled = true;
                            Back_Bet_Button.Enabled = true;

                            if (!gameState.IsBothBettingUsed)
                            {
                                Both_Bet_Button.Enabled = true;
                            }

                            break;

                        case BetType.Front:
                            Front_Bet_Button.Enabled = true;
                            break;

                        case BetType.Both:
                            Both_Bet_Button.Enabled = true;
                            break;

                        case BetType.Back:
                            Back_Bet_Button.Enabled = true;
                            break;
                    }
                });

                return;
            }

            if (state != Constants.OTHER)
            {
                return;
            }

            RunOnUiThread(() =>
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
            });
        }
        private void HandleStart(string state)
        {
            if (state == Constants.READY)
            {
                RunOnUiThread(() =>
                {
                    System_Message.Text = "<System> : 모든 유저가 시작하지 않은 상태입니다.";
                });

                return;
            }

            if (state == Constants.DONE)
            {
                gameState.IsGamePlaying = true;

                RunOnUiThread(() =>
                {
                    System_Message.Text = "<System> : 게임을 시작합니다.";
                });
            }
        }
        private void HandleChipUpdate(string message,bool isMine)
        {
            if (!int.TryParse(message, out int chipCount))
            {
                return;
            }

            if (isMine)
            {
                gameState.MyChips = chipCount;

                RunOnUiThread(() =>
                {
                    My_Chip.Text = chipCount.ToString();
                });

                return;
            }

            gameState.OpponentChips = chipCount;

            RunOnUiThread(() =>
            {
                Vs_Chip.Text = chipCount.ToString();
            });
        }
        private void HandleMyCardUpdate(string cardMessage)
        {
            if (cardMessage.StartsWith(Constants.FRONT))
            {
                string cardValue = cardMessage.Substring(Constants.FRONT.Length);
                SetCardImage(myFront_Card,"Front" + cardValue + ".jpg");
            }
            else if (cardMessage.StartsWith(Constants.BACK))
            {
                string cardValue = cardMessage.Substring(Constants.BACK.Length);
                SetCardImage(myBack_Card, "Back" + cardValue + ".jpg");
            }
        }
        private void HandleOpponentCardUpdate(string cardValue)
        {
            SetCardImage(vsFront_Card,"Front" + cardValue + ".jpg");
        }
        private void HandleMyBetUpdate(string betMessage)
        {
            if (betMessage.StartsWith(Constants.FRONT))
            {
                string betText = betMessage.Substring(Constants.FRONT.Length);

                if (!int.TryParse(betText, out int betCount))
                {
                    return;
                }

                gameState.BetType = BetType.Front;
                gameState.SelectedBetType = BetType.Front;
                gameState.MyFrontBet = betCount;

                RunOnUiThread(() =>
                {
                    My_Front_Chip.Text = betCount.ToString();
                });

                return;
            }

            if (betMessage.StartsWith(Constants.BACK))
            {
                string betText = betMessage.Substring(Constants.BACK.Length);

                if (!int.TryParse(betText, out int betCount))
                {
                    return;
                }

                gameState.BetType = BetType.Back;
                gameState.SelectedBetType = BetType.Back;
                gameState.MyBackBet = betCount;

                RunOnUiThread(() =>
                {
                    My_Back_Chip.Text = betCount.ToString();
                });

                return;
            }

            if (!betMessage.StartsWith(Constants.BOTH))
            {
                return;
            }

            string bothBetText = betMessage.Substring(Constants.BOTH.Length);

            if (!int.TryParse(bothBetText, out int bothBetCount))
            {
                return;
            }

            gameState.BetType = BetType.Both;
            gameState.SelectedBetType = BetType.Both;
            gameState.IsBothBettingUsed = true;
            gameState.MyFrontBet = bothBetCount;
            gameState.MyBackBet = bothBetCount;

            RunOnUiThread(() =>
            {
                My_Front_Chip.Text = bothBetCount.ToString();
                My_Back_Chip.Text = bothBetCount.ToString();
                Both_Bet_Button.Enabled = false;
            });
        }
        private void HandleOpponentBetUpdate(string betMessage)
        {
            if (betMessage.StartsWith(Constants.FRONT))
            {
                string betText = betMessage.Substring(Constants.FRONT.Length);
                if (!int.TryParse(betText, out int betCount))
                {
                    return;
                }

                gameState.OpponentFrontBet = betCount;

                RunOnUiThread(() =>
                {
                    Vs_Front_Chip.Text = betCount.ToString();
                });

                return;
            }

            if (betMessage.StartsWith(Constants.BACK))
            {
                string betText = betMessage.Substring(Constants.BACK.Length);
                if (!int.TryParse(betText, out int betCount))
                {
                    return;
                }

                gameState.OpponentBackBet = betCount;

                RunOnUiThread(() =>
                {
                    Vs_Back_Chip.Text = betCount.ToString();
                });

                return;
            }

            if (betMessage.StartsWith(Constants.BOTH))
            {
                string betText =
                    betMessage.Substring(Constants.BOTH.Length);

                if (!int.TryParse(betText, out int betCount))
                {
                    return;
                }

                gameState.IsBothBettingUsed = true;
                gameState.OpponentFrontBet = betCount;
                gameState.OpponentBackBet = betCount;

                RunOnUiThread(() =>
                {
                    Vs_Front_Chip.Text = betCount.ToString();
                    Vs_Back_Chip.Text = betCount.ToString();
                    Both_Bet_Button.Enabled = false;
                });
            }
        }
        private void UpdateDealerChip(string chipText)
        {
            RunOnUiThread(() =>
            {
                Dealer_Chip.Text = chipText;
            });
        }
        private void HandleGameInit()
        {
            gameState.HasFolded = false;
            InitTableChipSetting();

            RunOnUiThread(() =>
            {
                Front_Bet_Button.Enabled = true;
                Both_Bet_Button.Enabled = true;
                Back_Bet_Button.Enabled = true;

                Cancle_Button.Enabled = true;
                Die_Bet_Button.Enabled = true;

                sendTextBox.Enabled = true;
                SendButton.Enabled = true;
            });
        }
        private void HandleBettingImpossible()
        {
            RunOnUiThread(() =>
            {
                System_Message.Text =
                    "<System> : 베팅을 다시 진행해 주세요.";
            });
        }
        private void HandleBattle()
        {
            RunOnUiThread(() =>
            {
                System_Message.Text =
                    "<System> : 상대와의 승부를 시작합니다.";

                sendTextBox.Enabled = false;
                SendButton.Enabled = false;
            });
        }
        private void HandleOpponentCardPrint(string cardValue)
        {
            RunOnUiThread(() =>
            {
                System_Message.Text = "<System> : 상대의 뒷면카드는 " + cardValue + "입니다.";
            });
        }
        private void HandleWait()
        {
            RunOnUiThread(() =>
            {
                System_Message.Text = "<System> : 뒷면에 베팅이 진행되어, " + "뒷면 카드를 상대에게 오픈합니다.";

                sendTextBox.Enabled = false;
                SendButton.Enabled = false;
            });
        }
        private void HandleSpecial(string target)
        {
            const string systemMessage = "<System> : 베팅이 불가능하여 카드를 오픈합니다.";

            if (target == Constants.MY)
            {
                string request = Constants.GAME_CLIENT_EVENT + Constants.BETTING + Constants.SPECIAL + "0";
                TrySendPacket(request);
            }

            RunOnUiThread(() =>
            {
                System_Message.Text = systemMessage;
            });
        }

        private void PlaySound(string fileName)
        {
            player ??= new AudioPlayer();
            player.PlaySound(fileName);
        }
        private void SetCardImage( PictureBox pictureBox, string fileName)
        {
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"image",fileName);

            RunOnUiThread(() =>
            {
                Image oldImage = pictureBox.Image;
                using Image loadedImage = Image.FromFile(imagePath);

                pictureBox.Image = new Bitmap(loadedImage);
                oldImage?.Dispose();
            });
        }
        private bool TrySendPacket(string request)
        {
            if (!IsSocketConnected())
            {
                return false;
            }

            try
            {
                PacketHandler.SendPacket(socket, request);
                return true;
            }
            catch (SocketException ex)
            {
                RunOnUiThread(() =>
                {
                    MessageBox.Show("서버로 데이터를 전송하지 못했습니다: " + ex.Message,"전송 오류",MessageBoxButtons.OK,MessageBoxIcon.Error);
                });

                return false;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
        }
    }
}
