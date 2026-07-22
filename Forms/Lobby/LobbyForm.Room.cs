using System.Net.Sockets;
using TwofacedPoker_Client.Common;
using TwofacedPoker_Client.Network;
using TwofacedPoker_Client.Protocol;
using TwofacedPoker_Client.Protocol.Room;

namespace TwofacedPoker_Client
{
    public partial class LobbyForm
    {
        // 서버에 최신 방 목록을 요청하고 응답을 ListBox에 표시
        private async void GetRoomListButton_Click(object sender, EventArgs e)
        {
            if (isRequesting)
            {
                return;
            }

            if (!IsSocketConnected())
            {
                return;
            }

            SetRequestState(true);

            try
            {
                Socket currentSocket = socket!;

                string rooms = await Task.Run(() =>
                {
                    PacketHandler.SendPacket(currentSocket, Constants.GET_CHATTING_ROOM);
                    return PacketHandler.ReceivePacket(currentSocket);
                });

                ShowRoomList(rooms);
            }
            catch (Exception ex)
            {
                HandleConnectionError(ex);
            }
            finally
            {
                SetRequestState(false);
            }
        }
        
        // 서버의 방 목록 문자열을 RoomInfo 목록으로 파싱하여 화면 갱신

        private void ShowRoomList(string rooms)
        {
            RoomList.Items.Clear();

            if (rooms == Constants.NO_ROOM)
            {
                MessageBox.Show("현재 생성된 방이 없습니다.", "방 리스트", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<RoomInfo> roomList = RoomListParser.Parse(rooms);

            foreach (RoomInfo roomInfo in roomList)
            {
                RoomList.Items.Add(roomInfo);
            }
        }

        // 방 제목 입력 대화상자에서 확인된 경우 생성 요청 후 즉시 해당 방에 입장
        private async void CreateRoomButton_Click(object sender, EventArgs e)
        {
            if (isRequesting)
            {
                return;
            }

            if (!IsSocketConnected())
            {
                return;
            }

            using CreateRoom roomForm = new CreateRoom();

            DialogResult result = roomForm.ShowDialog();

            if (result != DialogResult.OK)
            {
                return;
            }

            SetRequestState(true);

            try
            {
                Socket currentSocket = socket!;

                string request = ClientRequestFactory.CreateRoomRequest(roomForm.getRoomNameTextBox());

                string roomNumber = await Task.Run(() =>
                {
                    PacketHandler.SendPacket(currentSocket, request);
                    return PacketHandler.ReceivePacket(currentSocket);
                });

                MessageBox.Show("방 생성이 완료되었습니다.", "방 생성 성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AppendLog("방을 생성했습니다.");

                string joinRequest = ClientRequestFactory.CreateJoinRoomRequest(roomNumber);

                await JoinChattingRoomAsync(joinRequest);
            }
            catch (Exception ex)
            {
                HandleConnectionError(ex);
            }
            finally
            {
                SetRequestState(false);
            }
        }

        // 선택된 RoomInfo의 방 번호를 이용해 서버에 입장 요청을 보냄
        private async void JoinButton_Click(object sender, EventArgs e)
        {
            if (isRequesting)
            {
                return;
            }

            if (RoomList.SelectedItem == null)
            {
                MessageBox.Show("선택된 방이 없습니다.", "방 입장 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsSocketConnected())
            {
                return;
            }

            RoomInfo selectedRoom = (RoomInfo)RoomList.SelectedItem;
            string request = ClientRequestFactory.CreateJoinRoomRequest(selectedRoom.RoomNumber);

            SetRequestState(true);

            try
            {
                await JoinChattingRoomAsync(request);
            }
            catch (Exception ex)
            {
                HandleConnectionError(ex);
            }
            finally
            {
                SetRequestState(false);
            }
        }

        // 방 입장 응답을 받은 뒤 같은 소켓을 채팅방 폼에 넘김.
        private async Task JoinChattingRoomAsync(string request)
        {
            if (socket == null)
            {
                return;
            }

            if (clientId == null)
            {
                MessageBox.Show("클라이언트 ID가 없습니다.", "방 입장 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Socket currentSocket = socket;

            string message = await Task.Run(() =>
            {
                PacketHandler.SendPacket(currentSocket, request);
                return PacketHandler.ReceivePacket(currentSocket);
            });

            if (message == Constants.NOT_EXIST_ROOM)
            {
                MessageBox.Show("방이 존재하지 않습니다.", "방 입장 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 채팅방이 닫힐 때 까지의 로비의 소켓 사용을 중단하여 동시 수신을 막음
            using ChattingRoom_Form room = new ChattingRoom_Form(currentSocket, message, clientId);

            room.ShowDialog();

            // 채팅방에서 소켓이 강제 종료된 경우 로비 로그인 상태도 함께 초기화
            if (!currentSocket.Connected)
            {
                socket = null;
                clientId = null;

                IDLabel.Text = "ID : 미생성";

                AppendLog("서버 연결이 종료되었습니다. 다시 연결해 주세요.");
            }
        }
    }
}