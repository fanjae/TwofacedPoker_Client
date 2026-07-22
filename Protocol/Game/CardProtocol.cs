using TwofacedPoker_Client.Common;

namespace TwofacedPoker_Client.Protocol.Game
{
    public enum CardSide
    {
        Unknown,
        Front,
        Back
    }

    public class CardMessage
    {
        public CardSide Side { get; }
        public string Value { get; } // 카드의 실제 값

        public CardMessage(CardSide side, string value)
        {
            Side = side;
            Value = value;
        }
    }

    public static class CardProtocol
    {
        public static CardMessage Parse(string message)
        {
            // 앞/뒷면 여부를 판별하고 카드 숫자 추출
            if (message.StartsWith(Constants.FRONT))
            {
                string value = message.Substring(Constants.FRONT.Length);
                return new CardMessage(CardSide.Front, value);
            }

            if (message.StartsWith(Constants.BACK))
            {
                string value = message.Substring(Constants.BACK.Length);
                return new CardMessage(CardSide.Back, value);
            }

            return new CardMessage(CardSide.Unknown, message);
        }

        // 판별된 카드 면과 값을 조합해서 불러올 파일명을 생성함.
        public static string GetImageFileName(CardSide side, string cardValue)
        {
            return side switch
            {
                CardSide.Front => "Front" + cardValue + ".jpg",
                CardSide.Back => "Back" + cardValue + ".jpg",
                _ => string.Empty
            };
        }
    }
}