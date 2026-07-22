using TwofacedPoker_Client.Common;

namespace TwofacedPoker_Client.Protocol.Game
{
    // 베팅 이벤트 종류
    public enum BettingEventType
    {
        Unknown,
        Impossible,
        MyBetUpdate,
        OpponentBetUpdate,
        Both
    }

    // 파싱된 이벤트 타입과 실제 전달된 값을 담는 데이터 객체
    public class BettingEventMessage
    {
        public BettingEventType Type { get; }
        public string Value { get; }

        public BettingEventMessage(BettingEventType type, string value)
        {
            Type = type;
            Value = value;
        }
    }

    public static class BettingEventParser
    {
        public static BettingEventMessage Parse(string message)
        {
            // 베팅 불가능에 대한 예외 처리
            if (message == Constants.BETTING + Constants.IMPOSSIBLE)
            {
                return new BettingEventMessage(BettingEventType.Impossible, string.Empty);
            }

            string prefix = Constants.MY + Constants.BET_UPDATE;

            // 내 베팅 금액 업데이트
            if (message.StartsWith(prefix))
            {
                string value = message.Substring(prefix.Length);
                return new BettingEventMessage(BettingEventType.MyBetUpdate, value);
            }

            prefix = Constants.OTHER + Constants.BET_UPDATE;
            // 상대방 베팅 금액 업데이트
            if (message.StartsWith(prefix))
            {
                string value = message.Substring(prefix.Length);
                return new BettingEventMessage(BettingEventType.OpponentBetUpdate, value);
            }
            // 양면 베팅 이벤트 처리
            if (message.StartsWith(Constants.BOTH, StringComparison.Ordinal))
            {
                string target = message.Substring(Constants.BOTH.Length).Trim();

                return new BettingEventMessage(BettingEventType.Both,target);
            }
            // 알 수 없는 패킷 처리
            return new BettingEventMessage(BettingEventType.Unknown, message);
        }
    }
}