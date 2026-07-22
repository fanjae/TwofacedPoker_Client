using TwofacedPoker_Client.Network;

namespace TwofacedPoker_Client.Protocol.Room
{
    // 서버가 전달한 방 목록 문자열을 화면에서 사용할 RoomInfo 목록으로 변환
    internal static class RoomListParser
    {
        public static List<RoomInfo> Parse(string rooms)
        {
            List<RoomInfo> roomList = new List<RoomInfo>();

            // 서버는 각 방을 줄 단위로 전달하여 개행을 기준으로 방 정보 분리
            string[] lines = rooms.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                // 첫 번째 공백만 기준으로 나누어 방 이름 내부의 공백은 그대로 유지
                string[] roomDetails = line.Trim().Split(' ', 2);

                // 방 번호 이름이 모두 존재하지 않는 항목은 무시
                if (roomDetails.Length != 2)
                {
                    continue;
                }

                // 방 번호를 정수로 변환할 수 없는 항목은 목록에 포함하지 않음
                if (!int.TryParse(roomDetails[0], out int roomNumber))
                {
                    continue;
                }

                string roomName = roomDetails[1].Trim();

                // 이름이 비어있는 방은 사용자가 입장할 수 없는 잘못된 데이터로 처리
                if (string.IsNullOrEmpty(roomName))
                {
                    continue;
                }

                roomList.Add(new RoomInfo(roomNumber, roomName));
            }

            return roomList;
        }
    }
}