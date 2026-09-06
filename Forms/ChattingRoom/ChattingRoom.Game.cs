using TwofacedPoker_Client.Common;
using TwofacedPoker_Client.Game;
using TwofacedPoker_Client.Protocol.Game;

namespace TwofacedPoker_Client
{
    // 
    public partial class ChattingRoom_Form
    {
        // 게임 패킷을 기본 이벤트, 칩/카드 이벤트, 베팅 이벤트 순서대로 처리
        private void EventHandle(string message)
        {
            EnqueuePresentation(message);
        }

        // 큐에서 꺼낸 게임 이벤트를 기존 게임 이벤트 처리 순서에 맞춰 실행
        private async Task ProcessGameEventAsync(string message,CancellationToken token)
        {
            try
            {
                ChipAndCardEventMessage chipEvent =
                    ChipAndCardEventParser.Parse(message);

                // 상대 뒷면 카드 공개는 중간 문구와 실제 카드 표시를 분리
                if (chipEvent.Type == ChipAndCardEventType.OpponentCardPrint)
                {
                    await HandleOpponentCardPrintAsync(chipEvent.Value, token);
                    return;
                }

                if (HandleBasicGameEvent(message))
                {
                    await DelayForPresentationAsync(message, token);
                    return;
                }

                if (HandleChipAndCardEvent(message))
                {
                    return;
                }

                if (HandleBettingEvent(message))
                {
                    return;
                }

                AppendUnknownGameMessage(message);
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                // 폼 종료 시 정상적으로 중단
            }
            catch (Exception ex)
            {
                RunOnUiThread(() =>
                {
                    MessageBox.Show(ex.Message + Environment.NewLine + "수신 메시지: " + message,"패킷 처리 오류",MessageBoxButtons.OK,MessageBoxIcon.Error);
                });
            }
        }

        // 모든 패킷을 지연시키지 않고, 사용자가 확인해야 하는 주요 게임 단계에만 연출 시간을 적용
        private static async Task DelayForPresentationAsync(string message,CancellationToken token)
        {
            BasicGameEventMessage gameEvent =
                BasicGameEventParser.Parse(message);

            int delayMilliseconds = gameEvent.Type switch
            {
                BasicGameEventType.Battle => 800,
                BasicGameEventType.GameResult => 1500,
                BasicGameEventType.Wait => 1000,
                _ => 0
            };

            if (delayMilliseconds > 0)
            {
                await Task.Delay(delayMilliseconds, token);
            }
        }

        // 새 라운드 시작 또는 게임 종료 시 베팅 상태와 테이블 UI 초기화
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

                SetBettingControlsEnabled(false, false, false, false, false, false);
            });
        }

        // 서버의 결과 코드를 사용자 메시지와 효과음으로 변환, 최종 종료 여부를 처리
        private void HandleGameResult(string result)
        {
            string eventMessage;
            bool gameEnd = false;

            switch (result)
            {
                case Constants.WIN:
                    PlaySound("win.mp3");
                    eventMessage = "<System> : 이번 베팅은 당신의 승리입니다.";
                    break;

                case Constants.DRAW:
                    eventMessage = "<System> : 무승부입니다.";
                    break;

                case Constants.LOSE:
                    PlaySound("lose.mp3");
                    eventMessage = "<System> : 이번 베팅은 당신의 패배입니다.";
                    break;

                case Constants.DIE:
                    eventMessage = "<System> : 상대가 베팅을 포기하였습니다.";
                    break;

                case Constants.BOTHWIN:
                    PlaySound("win.mp3");
                    eventMessage = "<System> : 양면 베팅에서 승리하였습니다. 칩 10개를 추가로 받습니다.";
                    break;

                case Constants.BOTHLOSE:
                    PlaySound("lose.mp3");
                    eventMessage = "<System> : 양면 베팅에서 패배하였습니다. 칩 10개를 패널티로 냅니다.";
                    break;

                case Constants.FINALWIN:
                    PlaySound("final_win.mp3");
                    eventMessage = "<System> : 당신의 최종 승리로, 게임을 종료합니다.";
                    gameEnd = true;
                    break;

                case Constants.FINALLOSE:
                    eventMessage = "<System> : 당신의 최종 패배로, 게임을 종료합니다.";
                    gameEnd = true;
                    break;

                default:
                    return;
            }

            // 일반 라운드 결과는 게임 상태를 유지하고 다음 서버 이벤트를 기다림
            if (!gameEnd)
            {
                RunOnUiThread(() =>
                {
                    System_Message.Text = eventMessage;
                    SetChatEnabled(false);
                });

                return;
            }

            // 최종 승패가 결정되면 전체 게임 상태를 초기화해 다시 준비
            gameState.ResetGame();
            InitTableChipSetting();

            RunOnUiThread(() =>
            {
                System_Message.Text = eventMessage;

                My_Ready.Text = "<준비>";
                Vs_Ready.Text = "<준비>";

                ExitButton.Enabled = true;
                SetChatEnabled(true);
            });
        }

        // 서버가 지정한 현재 턴에 맞춰 상태와 베팅 컨트롤을 전환 
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

                    SetBettingControlsEnabled(false, false, false, true, true, true);
                    EnableAvailableBetTypeButtons();
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

                SetBettingControlsEnabled(false, false, false, false, false, false);
            });
        }

        // 준비 단계와 실제 게임 시작 상태를 구분하여 화면에 반영
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

        // 동일한 칩 갱신 패킷 형식을 내 칩과 상대 칩에 공통으로 적용
        private void HandleChipUpdate(string message, bool isMine)
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

        // 카드 면과 값을 이미지 파일명으로 변환해 해당 PictureBox에 표시
        private void HandleMyCardUpdate(string cardMessage)
        {
            CardMessage card = CardProtocol.Parse(cardMessage);
            string fileName = CardProtocol.GetImageFileName(card.Side, card.Value);

            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            switch (card.Side)
            {
                case CardSide.Front:
                    SetCardImage(myFront_Card, fileName);
                    break;

                case CardSide.Back:
                    SetCardImage(myBack_Card, fileName);
                    break;
            }
        }

        private void HandleOpponentCardUpdate(string cardValue)
        {
            string fileName = CardProtocol.GetImageFileName(CardSide.Front, cardValue);

            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            SetCardImage(vsFront_Card, fileName);
        }

        private void UpdateDealerChip(string chipText)
        {
            RunOnUiThread(() =>
            {
                Dealer_Chip.Text = chipText;
            });
        }

        // 서버 라운드 초기화 통지에 맞춰 베팅 상태와 컨트롤 준비
        private void HandleGameInit()
        {
            gameState.HasFolded = false;
            InitTableChipSetting();

            RunOnUiThread(() =>
            {
                SetBettingControlsEnabled(true, true, true, false, true, true);
                SetChatEnabled(true);
            });
        }

        // 승부 판정 중에는 채팅 입력을 잠가서 연출과 ㅇ비력이 겹치지 않게 처리
        private void HandleBattle()
        {
            RunOnUiThread(() =>
            {
                System_Message.Text = "<System> : 상대와의 승부를 시작합니다.";
                SetChatEnabled(false);
            });
            
        }

        // 공개 안내 문구와 실제 카드 결과를 분리하여 사용자가 카드 공개 과정을 인지하도록 처리
        private async Task HandleOpponentCardPrintAsync(string cardValue,CancellationToken token)
        {
            RunOnUiThread(() =>
            {
                System_Message.Text = "<System> : 상대의 뒷면 카드를 공개합니다.";
            });

            await Task.Delay(1000, token);

            RunOnUiThread(() =>
            {
                System_Message.Text ="<System> : 상대의 뒷면카드는 " + cardValue + "입니다.";
            });

            // 실제 카드 값도 잠시 유지
            await Task.Delay(1000, token);
        }

        private void HandleWait()
        {
            RunOnUiThread(() =>
            {
                System_Message.Text = "<System> : 뒷면에 베팅이 진행되어, 뒷면 카드를 상대에게 오픈합니다.";
                SetChatEnabled(false);
            });
        }

        // 정상 베팅이 불가능한 특수 상황에서 대상 플레이어가 서버에 카드 공개 처리를 요청
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
        private void HandleBothBetting(string target)
        {
            if (target == Constants.MY)
            {
                gameState.SetBothBetOwner(BothBetOwner.Me);
                return;
            }

            if (target == Constants.OTHER)
            {
                gameState.SetBothBetOwner(BothBetOwner.Opponent);
            }
        }

        // 게임 흐름 자체를 변경하는 기본 이벤트를 처리
        private bool HandleBasicGameEvent(string message)
        {
            BasicGameEventMessage gameEvent = BasicGameEventParser.Parse(message);

            switch (gameEvent.Type)
            {
                case BasicGameEventType.Start:
                    HandleStart(gameEvent.Value);
                    return true;

                case BasicGameEventType.Turn:
                    HandleTurn(gameEvent.Value);
                    return true;

                case BasicGameEventType.GameResult:
                    HandleGameResult(gameEvent.Value);
                    return true;

                case BasicGameEventType.GameInit:
                    HandleGameInit();
                    return true;

                case BasicGameEventType.GameAborted:
                    HandleGameAborted();
                    return true;

                case BasicGameEventType.Battle:
                    HandleBattle();
                    return true;

                case BasicGameEventType.Wait:
                    HandleWait();
                    return true;

                case BasicGameEventType.BasicBetting:
                    UpdateDealerChip("2");
                    return true;

                case BasicGameEventType.Special:
                    HandleSpecial(gameEvent.Value);
                    return true;

                default:
                    return false;
            }
        }

        // 칩 수치와 카드 표시처럼 데이터 갱신 중심의 이벤트를 처리
        private bool HandleChipAndCardEvent(string message)
        {
            ChipAndCardEventMessage gameEvent = ChipAndCardEventParser.Parse(message);

            switch (gameEvent.Type)
            {
                case ChipAndCardEventType.MyChipUpdate:
                    HandleChipUpdate(gameEvent.Value, true);
                    return true;

                case ChipAndCardEventType.OpponentChipUpdate:
                    HandleChipUpdate(gameEvent.Value, false);
                    return true;

                case ChipAndCardEventType.MyCardUpdate:
                    HandleMyCardUpdate(gameEvent.Value);
                    return true;

                case ChipAndCardEventType.OpponentCardUpdate:
                    HandleOpponentCardUpdate(gameEvent.Value);
                    return true;

                case ChipAndCardEventType.DealerChipUpdate:
                    UpdateDealerChip(gameEvent.Value);
                    return true;

                default:
                    return false;
            }
        }

        private void HandleGameAborted()
        {
            // 진행 중이던 게임과 방의 준비 상태를 모두 폐기
            gameState.ResetGame();
            roomState.Reset();

            // 화면에 남아 있는 칩 정보도 초기화
            gameState.MyChips = 0;
            gameState.OpponentChips = 0;

            // 현재 판의 베팅 상태와 베팅 버튼 초기화
            InitTableChipSetting();

            // 생성자에서 사용한 기본 카드 이미지로 복구
            SetCardImage(myFront_Card, "Front10.jpg");
            SetCardImage(myBack_Card, "Back10.jpg");
            SetCardImage(vsFront_Card, "Front10.jpg");

            RunOnUiThread(() =>
            {
                System_Message.Text =
                    "<System> : 상대방이 나가 게임이 중단되었습니다.";

                // 준비 상태 초기화
                My_Ready.Text = "<준비>";
                Vs_Ready.Text = "<준비>";

                // 상대방 정보 초기화
                Vs_ID_Label.Text = "ID : ???";

                // 보유 칩 및 공동 판돈 화면 초기화
                My_Chip.Text = "0";
                Vs_Chip.Text = "0";
                Dealer_Chip.Text = "0";

                // 턴 표시 제거
                My_Turn.Visible = false;
                Vs_Turn.Visible = false;

                // 게임이 중단됐으므로 퇴장과 채팅을 다시 허용
                ExitButton.Enabled = true;
                SetChatEnabled(true);

                chattingRoomTextBox.AppendText(
                    "<System> : 상대방이 나가 게임이 중단되었습니다."
                    + Environment.NewLine
                );
            });
        }

        private void AppendUnknownGameMessage(string message)
        {
            RunOnUiThread(() =>
            {
                chattingRoomTextBox.AppendText("[알 수 없는 게임 패킷] " + message + Environment.NewLine);
            });
        }
    }
}