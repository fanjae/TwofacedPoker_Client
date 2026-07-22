using TwofacedPoker_Client.Common;

namespace TwofacedPoker_Client.Protocol.Room
{
    // 방 안에서 발생하는 서버 이벤트 종류 표현
    public enum RoomEventType
    {
        Unknown,
        UpdateId,
        UpdateReadyState
    }

    // 파싱한 방 이벤트의 종류와 실제 데이터 값을 함께 전달
    public class RoomEventMessage
    {
        public RoomEventType Type { get; }
        public string Value { get; }

        public RoomEventMessage(RoomEventType type, string value)
        {
            Type = type;
            Value = value;
        }
    }

    // 서버의 방 이벤트 문자열을 클라이언트가 처리하기 쉽게 변환
    public static class RoomEventParser
    {
        public static RoomEventMessage Parse(string message)
        {
            // 상대 사용자 ID 갱신 이벤트의 헤더 제거 후 실제 ID만 반환
            if (message.StartsWith(Constants.UPDATE_ID))
            {
                string opponentId = message.Substring(Constants.UPDATE_ID.Length);
                return new RoomEventMessage(RoomEventType.UpdateId, opponentId);
            }

            // 상대 준비 상태 갱신 이벤트의 헤더를 제거하고 상태 값만 반환
            if (message.StartsWith(Constants.UPDATE_READY_STATE))
            {
                string state = message.Substring(Constants.UPDATE_READY_STATE.Length);
                return new RoomEventMessage(RoomEventType.UpdateReadyState, state);
            }

            // 정의되지 않은 이벤트 처리
            return new RoomEventMessage(RoomEventType.Unknown, message);
        }
    }
}