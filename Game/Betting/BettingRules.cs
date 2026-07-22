using TwofacedPoker_Client.Game;

namespace TwofacedPoker_Client.Game.Betting
{
    internal static class BettingRules
    {
        // 베팅이 유효한지 검사하고 실패 시 에러 메시지 반환.
        public static string GetBetErrorMessage(ClientGameState gameState, int chipCount)
        {
            int myChips = gameState.MyChips;
            int opponentChips = gameState.OpponentChips;
            int myBet = gameState.MyHighestBet;
            int opponentBet = gameState.OpponentHighestBet;

            if (chipCount <= 0)
            {
                return "베팅 칩은 1개 이상 입력해야 합니다.";
            }

            if (gameState.SelectedBetType == BetType.None)
            {
                return "앞면 / 양면 / 뒷면 중 하나를 선택해 주세요.";
            }

            // 양면 베팅인 경우 입력한 칩의 2배 계산.
            int requiredChips = gameState.SelectedBetType == BetType.Both ? chipCount * 2 : chipCount;

            // 내 보유 칩 한도 초과 검사
            if (requiredChips > myChips)
            {
                return "보유한 칩보다 많은 수를 입력했습니다.";
            }

            // 상대방의 최대 수용 가능 칩 초과 검사
            if (opponentChips + opponentBet < chipCount + myBet)
            {
                return "상대가 보유한 칩보다 많은 수를 입력했습니다.";
            }

            // 최소 콜 금액 검사 
            if (opponentBet > myBet + chipCount)
            {
                return "상대가 베팅한 칩 개수 이상 베팅해야 합니다.";
            }

            return string.Empty;
        }
    }
}