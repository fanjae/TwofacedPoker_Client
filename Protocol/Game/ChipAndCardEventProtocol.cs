using TwofacedPoker_Client.Common;

namespace TwofacedPoker_Client.Protocol.Game
{
    public enum ChipAndCardEventType
    {
        Unknown,
        MyChipUpdate,
        OpponentChipUpdate,
        MyCardUpdate,
        OpponentCardUpdate,
        DealerChipUpdate,
        OpponentCardPrint
    }

    public class ChipAndCardEventMessage
    {
        public ChipAndCardEventType Type { get; }
        public string Value { get; }

        public ChipAndCardEventMessage(ChipAndCardEventType type, string value)
        {
            Type = type;
            Value = value;
        }
    }

    public static class ChipAndCardEventParser
    {
        public static ChipAndCardEventMessage Parse(string message)
        {
            // 각 주체의 칩 변동 및 카드 업데이트 상태를 문자열 Prefix로 비교
            string prefix = Constants.MY + Constants.CHIP_UPDATE;

            if (message.StartsWith(prefix))
            {
                string value = message.Substring(prefix.Length);
                return new ChipAndCardEventMessage(ChipAndCardEventType.MyChipUpdate, value);
            }

            prefix = Constants.OTHER + Constants.CHIP_UPDATE;

            if (message.StartsWith(prefix))
            {
                string value = message.Substring(prefix.Length);
                return new ChipAndCardEventMessage(ChipAndCardEventType.OpponentChipUpdate, value);
            }

            prefix = Constants.MY + Constants.CARD_UPDATE;

            if (message.StartsWith(prefix))
            {
                string value = message.Substring(prefix.Length);
                return new ChipAndCardEventMessage(ChipAndCardEventType.MyCardUpdate, value);
            }

            prefix = Constants.OTHER + Constants.CARD_UPDATE;

            if (message.StartsWith(prefix))
            {
                string value = message.Substring(prefix.Length);
                return new ChipAndCardEventMessage(ChipAndCardEventType.OpponentCardUpdate, value);
            }

            prefix = Constants.DEALER + Constants.CHIP_UPDATE;

            if (message.StartsWith(prefix))
            {
                string value = message.Substring(prefix.Length);
                return new ChipAndCardEventMessage(ChipAndCardEventType.DealerChipUpdate, value);
            }

            prefix = Constants.OTHER + Constants.PRINT;

            if (message.StartsWith(prefix))
            {
                string value = message.Substring(prefix.Length);
                return new ChipAndCardEventMessage(ChipAndCardEventType.OpponentCardPrint, value);
            }

            return new ChipAndCardEventMessage(ChipAndCardEventType.Unknown, message);
        }
    }
}