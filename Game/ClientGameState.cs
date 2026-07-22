using TwofacedPoker_Client.Game.Betting;

namespace TwofacedPoker_Client.Game
{
    internal enum BothBetOwner
    {
        None,
        Me,
        Opponent
    }

    internal sealed class ClientGameState
    {
        // 양면 베팅 상태 및 베팅 유저 확인
        public bool IsGamePlaying { get; set; }

        public bool IsBothBettingUsed { get; set; }

        public BothBetOwner BothBetOwner { get; set; }

        public bool HasFolded { get; set; }

        public BetType BetType { get; set; } = BetType.None;

        public BetType SelectedBetType { get; set; } = BetType.None;

        public int MyChips { get; set; }

        public int OpponentChips { get; set; }

        public int MyFrontBet { get; set; }

        public int MyBackBet { get; set; }

        public int OpponentFrontBet { get; set; }

        public int OpponentBackBet { get; set; }

        // 현재 판의 가장 높은 베팅액 계산
        public int MyHighestBet => Math.Max(MyFrontBet, MyBackBet);

        public int OpponentHighestBet => Math.Max(OpponentFrontBet, OpponentBackBet);

        public void ApplyMyBet(BetType betType, int betCount)
        {
            BetType = betType;
            SelectedBetType = betType;

            // 베팅 타입에 따라 칩 분배 로직 적용
            switch (betType)
            {
                case BetType.Front:
                    MyFrontBet = betCount;
                    break;

                case BetType.Back:
                    MyBackBet = betCount;
                    break;

                case BetType.Both:
                    IsBothBettingUsed = true;
                    BothBetOwner = BothBetOwner.Me;

                    MyFrontBet = betCount;
                    MyBackBet = betCount;
                    break;
            }
        }

        public void ApplyOpponentBet(BetType betType, int betCount)
        {
            switch (betType)
            {
                case BetType.Front:
                    OpponentFrontBet = betCount;
                    break;

                case BetType.Back:
                    OpponentBackBet = betCount;
                    break;

                case BetType.Both:
                    IsBothBettingUsed = true;
                    BothBetOwner = BothBetOwner.Opponent;

                    OpponentFrontBet = betCount;
                    OpponentBackBet = betCount;
                    break;
            }
        }

        public void SetBothBetOwner(BothBetOwner owner)
        {
            IsBothBettingUsed = owner != BothBetOwner.None;
            BothBetOwner = owner;
        }

        public void ResetRound()
        {
            BetType = BetType.None;
            SelectedBetType = BetType.None;

            MyFrontBet = 0;
            MyBackBet = 0;
            OpponentFrontBet = 0;
            OpponentBackBet = 0;

            IsBothBettingUsed = false;
            BothBetOwner = BothBetOwner.None;
            HasFolded = false;
        }

        public void ResetGame()
        {
            ResetRound();
            IsGamePlaying = false;
        }


    }
}