using TwofacedPoker_Client.Common;

namespace TwofacedPoker_Client.Protocol.Game
{
    // 게임의 전반 상태 흐름을 나타내는 enum
    public enum BasicGameEventType
    {
        Unknown,
        Start,
        Turn,
        GameResult,
        GameInit,
        Battle,
        Wait,
        BasicBetting,
        Special
    }

    public class BasicGameEventMessage
    {
        public BasicGameEventType Type { get; }
        public string Value { get; }

        public BasicGameEventMessage(BasicGameEventType type, string value)
        {
            Type = type;
            Value = value;
        }
    }

    public static class BasicGameEventParser
    {
        public static BasicGameEventMessage Parse(string message)
        {
            // Type 1. 값이 동반되는 패킷을 먼저 검사하고 value 추출
            if (message.StartsWith(Constants.START))
            {
                string value = message.Substring(Constants.START.Length);
                return new BasicGameEventMessage(BasicGameEventType.Start, value);
            }

            if (message.StartsWith(Constants.TURN))
            {
                string value = message.Substring(Constants.TURN.Length);
                return new BasicGameEventMessage(BasicGameEventType.Turn, value);
            }

            if (message.StartsWith(Constants.GAME_RESULT))
            {
                string value = message.Substring(Constants.GAME_RESULT.Length);
                return new BasicGameEventMessage(BasicGameEventType.GameResult, value);
            }

            // Type2. 단일 상태 명령 패킷 (별도로 Value 추출이 필요없는 패킷)
            if (message == Constants.GAME_INIT)
            {
                return new BasicGameEventMessage(BasicGameEventType.GameInit, string.Empty);
            }

            if (message == Constants.BATTLE)
            {
                return new BasicGameEventMessage(BasicGameEventType.Battle, string.Empty);
            }

            if (message == Constants.WAIT)
            {
                return new BasicGameEventMessage(BasicGameEventType.Wait, string.Empty);
            }

            if (message == Constants.BASIC_BETTING)
            {
                return new BasicGameEventMessage(BasicGameEventType.BasicBetting, string.Empty);
            }

            if (message.StartsWith(Constants.SPECIAL))
            {
                string value = message.Substring(Constants.SPECIAL.Length);
                return new BasicGameEventMessage(BasicGameEventType.Special, value);
            }

            return new BasicGameEventMessage(BasicGameEventType.Unknown, message);
        }
    }
}