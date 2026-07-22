using TwofacedPoker_Client.Common;

namespace TwofacedPoker_Client.Game.Betting
{
    internal static class BetPacketConverter
    {
        public static bool TryParse(string betMessage, out BetType betType, out int betCount)
        {
            betType = BetType.None;
            betCount = 0;

            string betCountText;

            // 패킷의 접두사를 확인하여 베팅 타입과 칩 개수 문자열 분리
            if (betMessage.StartsWith(Constants.FRONT))
            {
                betType = BetType.Front;
                betCountText = betMessage.Substring(Constants.FRONT.Length);
            }
            else if (betMessage.StartsWith(Constants.BACK))
            {
                betType = BetType.Back;
                betCountText = betMessage.Substring(Constants.BACK.Length);
            }
            else if (betMessage.StartsWith(Constants.BOTH))
            {
                betType = BetType.Both;
                betCountText = betMessage.Substring(Constants.BOTH.Length);
            }
            else
            {
                return false;
            }

            // 분리한 문자열을 숫자로 반환
            return int.TryParse(betCountText, out betCount);
        }

        public static string ToPacketText(BetType betType)
        {
            return betType switch
            {
                BetType.Front => Constants.FRONT,
                BetType.Both => Constants.BOTH,
                BetType.Back => Constants.BACK,
                _ => string.Empty
            };
        }
    }
}