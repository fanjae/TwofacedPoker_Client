using TwofacedPoker_Client.Common;
using TwofacedPoker_Client.Game.Betting;

namespace TwofacedPoker_Client.Protocol
{
    // UI에서 발생한 사용자 행동을 서버 프로토콜 형식의 요청 문자열로 조립
    internal static class ClientRequestFactory
    {
        public static string CreateReadyStateRequest(bool isReady)
        {
            // 현재 상태의 반대 값을 전송하여 준비/완료를 토글
            string state = isReady ? Constants.DONE : Constants.READY;
            return Constants.ROOM_EVENT + Constants.USER_READY_STATE + state;
        }

        public static string CreateGameStartRequest()
        {
            return Constants.GAME_CLIENT_EVENT + Constants.GAME_START;
        }

        public static string CreateBettingRequest(BetType betType, int betCount)
        {
            // 클라이언트 enum 값을 프로토콜 문자열로 변환
            string betTypeText = BetPacketConverter.ToPacketText(betType);

            // 지원하지 않는 베팅 종류는 잘못된 패킷을 만들지 않고 빈 문자열로 반환
            if (string.IsNullOrEmpty(betTypeText))
            {
                return string.Empty;
            }

            return Constants.GAME_CLIENT_EVENT + Constants.BETTING + betTypeText + betCount;
        }

        // 포기는 일반 베팅 헤더 뒤에 DIE 명령을 붙여 전송
        public static string CreateFoldRequest()
        {
            return Constants.GAME_CLIENT_EVENT + Constants.BETTING + Constants.DIE;
        }

        // 사용자가 입력한 방 이름을 방 생성 명령 뒤에 연결
        public static string CreateRoomRequest(string roomName)
        {
            return Constants.CREATE_CHATTING_ROOM + roomName;
        }

        // 정수형 방 번호를 이용한 입장 요청 생성
        public static string CreateJoinRoomRequest(int roomNumber)
        {
            return Constants.JOIN_CHATTING_ROOM + roomNumber;
        }

        // 이미 문자열로 전달된 방 번호를 이용한 입장 요청 생성
        public static string CreateJoinRoomRequest(string roomNumber)
        {
            return Constants.JOIN_CHATTING_ROOM + roomNumber;
        }
    }
}