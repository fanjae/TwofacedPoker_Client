using TwofacedPoker_Client.Common;

namespace TwofacedPoker_Client
{
    // 채팅방에서 수신할 수 있는 최상위 서버 메시지 종류를 표현
    public enum ServerMessageType
    {
        Chat,
        RoomEvent,
        GameEvent,
        ExitRoomComplete
    }

    // 메시지 분류 결과와 헤더를 제거한 실제 내용을 함께 보관
    public class ServerMessage
    {
        public ServerMessageType Type { get; }
        public string Content { get; }

        public ServerMessage(ServerMessageType type, string content)
        {
            Type = type;
            Content = content;
        }
    }

    // 서버 응답의 공통 헤더를 확인하여 각 전용 처리기로 전달할 메시지 반환
    public static class ServerMessageParser
    {
        public static ServerMessage Parse(string response)
        {
            // 정상 퇴장 완료 메시지는 내용 없이 별도 타입으로 분류
            if (response == Constants.EXIT_ROOM_COMPLETE)
            {
                return new ServerMessage(ServerMessageType.ExitRoomComplete, string.Empty);
            }

            // 방 이벤트 헤더를 제거하고, RoomEventParser가 처리할 내용만 반환
            if (response.StartsWith(Constants.ROOM_EVENT))
            {
                string content = response.Substring(Constants.ROOM_EVENT.Length);

                return new ServerMessage(ServerMessageType.RoomEvent, content);
            }

            // 게임 이벤트 헤더를 제거한 뒤 게임 이벤트 처리기로 전달
            if (response.StartsWith(Constants.GAME_CLIENT_EVENT))
            {
                string content = response.Substring(Constants.GAME_CLIENT_EVENT.Length);

                return new ServerMessage(ServerMessageType.GameEvent, content);
            }

            return new ServerMessage(ServerMessageType.Chat, response);
        }
    }
}