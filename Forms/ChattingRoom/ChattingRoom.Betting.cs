using TwofacedPoker_Client.Common;
using TwofacedPoker_Client.Game.Betting;
using TwofacedPoker_Client.Protocol;
using TwofacedPoker_Client.Protocol.Game;

namespace TwofacedPoker_Client
{
    public partial class ChattingRoom_Form
    {
        // 입력값과 게임 규칙 검증 후 서버에 베팅 요청 
        private void Bet_Chip_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(Bet_Chip_Count.Text, out int betChipCount))
            {
                ShowBettingError("유효하지 않은 값입니다. 숫자만 입력해 주세요.");
                return;
            }

            // 보유 칩, 최소 베팅 등 게임 상태 기반 규칙 처리
            string errorMessage = BettingRules.GetBetErrorMessage(gameState, betChipCount);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ShowBettingError(errorMessage);
                return;
            }

            string betTypeText = BetPacketConverter.ToPacketText(gameState.SelectedBetType);

            if (string.IsNullOrEmpty(betTypeText))
            {
                return;
            }

            string request = Constants.GAME_CLIENT_EVENT + Constants.BETTING + betTypeText + betChipCount;

            TrySendPacket(request);
        }

        private void Front_Bet_Button_Click(object sender, EventArgs e)
        {
            SelectBetType(BetType.Front);
        }

        private void Both_Bet_Button_Click(object sender, EventArgs e)
        {
            SelectBetType(BetType.Both);
        }

        private void Back_Bet_Button_Click(object sender, EventArgs e)
        {
            SelectBetType(BetType.Back);
        }

        // 아직 확정된 베팅 면이 없을때만 현재 선택 취소 가능
        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            if (!gameState.IsGamePlaying || gameState.BetType != BetType.None)
            {
                return;
            }

            gameState.SelectedBetType = BetType.None;

            DisableBetTypeButtons();
            EnableAvailableBetTypeButtons();
        }

        // 포기 요청의 중복 전송을 막기 위해 버튼을 잠금
        private void Die_Bet_Button_Click(object sender, EventArgs e)
        {
            if (!gameState.IsGamePlaying || gameState.HasFolded)
            {
                return;
            }

            gameState.HasFolded = true;
            Die_Bet_Button.Enabled = false;

            string request = ClientRequestFactory.CreateFoldRequest();

            if (TrySendPacket(request))
            {
                return;
            }

            // 전송 실패 시 다시 포기할 수 있도록 변경 상태 복구
            gameState.HasFolded = false;
            Die_Bet_Button.Enabled = true;
        }

        // 서버가 확정한 배팅 결과를 화면에 반영
        private void HandleMyBetUpdate(string betMessage)
        {
            if (!BetPacketConverter.TryParse(betMessage, out BetType betType, out int betCount))
            {
                return;
            }

            gameState.ApplyMyBet(betType, betCount);

            RunOnUiThread(() =>
            {
                switch (betType)
                {
                    case BetType.Front:
                        My_Front_Chip.Text = betCount.ToString();
                        break;

                    case BetType.Back:
                        My_Back_Chip.Text = betCount.ToString();
                        break;

                    case BetType.Both:
                        My_Front_Chip.Text = betCount.ToString();
                        My_Back_Chip.Text = betCount.ToString();
                        Both_Bet_Button.Enabled = false;
                        break;
                }
            });
        }

        // 서버가 전달한 상대 베팅 결과를 화면에 반영
        private void HandleOpponentBetUpdate(string betMessage)
        {
            if (!BetPacketConverter.TryParse(betMessage, out BetType betType, out int betCount))
            {
                return;
            }

            gameState.ApplyOpponentBet(betType, betCount);

            RunOnUiThread(() =>
            {
                switch (betType)
                {
                    case BetType.Front:
                        Vs_Front_Chip.Text = betCount.ToString();
                        break;

                    case BetType.Back:
                        Vs_Back_Chip.Text = betCount.ToString();
                        break;

                    case BetType.Both:
                        Vs_Front_Chip.Text = betCount.ToString();
                        Vs_Back_Chip.Text = betCount.ToString();
                        Both_Bet_Button.Enabled = false;
                        break;
                }
            });
        }

        // 베팅 관련 이벤트를 파싱하여 이벤트 처리를 진행.
        private bool HandleBettingEvent(string message)
        {
            BettingEventMessage bettingEvent = BettingEventParser.Parse(message);

            switch (bettingEvent.Type)
            {
                case BettingEventType.Impossible:
                    HandleBettingImpossible();
                    return true;

                case BettingEventType.MyBetUpdate:
                    HandleMyBetUpdate(bettingEvent.Value);
                    return true;

                case BettingEventType.OpponentBetUpdate:
                    HandleOpponentBetUpdate(bettingEvent.Value);
                    return true;

                case BettingEventType.Both:
                    HandleBothBetting(bettingEvent.Value);
                    return true;

                case BettingEventType.Unknown:
                    return false;

                default:
                    return false;
            }
        }

        private void HandleBettingImpossible()
        {
            RunOnUiThread(() =>
            {
                System_Message.Text = "<System> : 베팅을 다시 진행해 주세요.";
            });
        }

        private void ShowBettingError(string message)
        {
            Bet_Chip_Count.Clear();

            MessageBox.Show(message, "베팅 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // 한 차례 안에서 기존 확정 면과 다른 면을 선택하지 못하게 제한.
        private void SelectBetType(BetType selectedBetType)
        {
            if (!gameState.IsGamePlaying)
            {
                return;
            }

            if (gameState.BetType != BetType.None && gameState.BetType != selectedBetType)
            {
                return;
            }

            gameState.SelectedBetType = selectedBetType;

            if (selectedBetType != BetType.Front)
            {
                Front_Bet_Button.Enabled = false;
            }

            if (selectedBetType != BetType.Both)
            {
                Both_Bet_Button.Enabled = false;
            }

            if (selectedBetType != BetType.Back)
            {
                Back_Bet_Button.Enabled = false;
            }
        }

        private void DisableBetTypeButtons()
        {
            Front_Bet_Button.Enabled = false;
            Both_Bet_Button.Enabled = false;
            Back_Bet_Button.Enabled = false;
        }

        // 현재 라운드의 확정 베팅 면과 양면 베팅 사용 여부에 따라 선택 가능한 면만 활성화
        private void EnableAvailableBetTypeButtons()
        {
            switch (gameState.BetType)
            {
                case BetType.None:
                    Front_Bet_Button.Enabled = true;
                    Back_Bet_Button.Enabled = true;
                    Both_Bet_Button.Enabled = !gameState.IsBothBettingUsed;
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
        }

        private void SetBettingControlsEnabled(bool front, bool both, bool back, bool bet, bool cancel, bool die)
        {
            Front_Bet_Button.Enabled = front;
            Both_Bet_Button.Enabled = both;
            Back_Bet_Button.Enabled = back;
            Bet_Chip.Enabled = bet;
            Cancle_Button.Enabled = cancel;
            Die_Bet_Button.Enabled = die;
        }
    }
}