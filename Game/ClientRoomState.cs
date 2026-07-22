namespace TwofacedPoker_Client.Game
{
    internal sealed class ClientRoomState
    {
        public string OpponentId { get; private set; } = string.Empty;
        public bool IsMyReady { get; private set; }
        public bool IsOpponentReady { get; private set; }

        // 양쪽 플레이어가 모두 레디 상태여야 게임 시작 가능
        public bool CanStartGame => IsMyReady && IsOpponentReady;

        public void SetOpponentId(string opponentId)
        {
            OpponentId = opponentId;
        }

        public void SetMyReady(bool isReady)
        {
            IsMyReady = isReady;
        }

        public void SetOpponentReady(bool isReady)
        {
            IsOpponentReady = isReady;
        }

        public void Reset()
        {
            OpponentId = string.Empty;
            IsMyReady = false;
            IsOpponentReady = false;
        }
    }
}