using TwofacedPoker_Client.Common;
using TwofacedPoker_Client.Protocol;
using TwofacedPoker_Client.Protocol.Room;

namespace TwofacedPoker_Client
{
    public partial class ChattingRoom_Form
    {
        // 현재 준비 상태의 반대 값을 서버에 전송 후 로컬 표시 갱신
        private void ToggleReadyState()
        {
            bool nextReadyState = !roomState.IsMyReady;
            string request = ClientRequestFactory.CreateReadyStateRequest(nextReadyState);

            if (!TrySendPacket(request))
            {
                return;
            }

            roomState.SetMyReady(nextReadyState);
            UpdateMyReadyDisplay();
        }

        // 방 관련 패킷을 세부 이벤트 타입으로 분기
        private void RoomHandle(string message)
        {
            RoomEventMessage roomEvent = RoomEventParser.Parse(message);

            switch (roomEvent.Type)
            {
                case RoomEventType.UpdateId:
                    UpdateOpponentId(roomEvent.Value);
                    break;

                case RoomEventType.UpdateReadyState:
                    UpdateOpponentReadyState(roomEvent.Value);
                    break;

                case RoomEventType.Unknown:
                    AppendUnknownRoomMessage(roomEvent.Value);
                    break;
            }
        }

        private void UpdateMyReadyDisplay()
        {
            My_Ready.Text = roomState.IsMyReady ? "<완료>" : "<준비>";
        }

        private void UpdateOpponentId(string opponentId)
        {
            roomState.SetOpponentId(opponentId);

            RunOnUiThread(() =>
            {
                Vs_ID_Label.Text = "ID : " + roomState.OpponentId;
            });
        }

        private void UpdateOpponentReadyState(string state)
        {
            // 정의되지 않은 준비 상태 값은 반영하지 않도록 처리
            if (state != Constants.READY && state != Constants.DONE)
            {
                return;
            }

            roomState.SetOpponentReady(state == Constants.DONE);

            RunOnUiThread(() =>
            {
                Vs_Ready.Text = roomState.IsOpponentReady ? "<완료>" : "<준비>";
                // 상대가 준비를 완료한 뒤 내가 퇴장하여 게임 시작 조건이 깨지는 것을 방지
                ExitButton.Enabled = !roomState.IsOpponentReady;
            });
        }

        private void AppendUnknownRoomMessage(string message)
        {
            RunOnUiThread(() =>
            {
                chattingRoomTextBox.AppendText("[알 수 없는 방 패킷] " + message + Environment.NewLine);
            });
        }
    }
}