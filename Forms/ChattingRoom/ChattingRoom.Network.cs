using System.Net.Sockets;
using TwofacedPoker_Client.Common;
using TwofacedPoker_Client.Network;

namespace TwofacedPoker_Client
{
    public partial class ChattingRoom_Form
    {
        // 소켓 연결뿐 아니라 현재 폼이 종료 절차 중인지 확인
        private bool IsSocketConnected()
        {
            return socket != null && socket.Connected && !isClosing;
        }

        // 서버 패킷을 지속적으로 수신하는 백그라운드 스레드 진입점
        private void Receive()
        {
            try
            {
                // 방 입장 직후 상대 사용자 정보 갱신 요청
                if (!TrySendPacket(Constants.USER_UPDATE))
                {
                    return;
                }

                // 연결이 유지되는 동안 하나의 패킷 단위로 계속 수신
                while (isRunning)
                {
                    string response = PacketHandler.ReceivePacket(socket);

                    // 빈 응답은 원격 서버가 연결을 정상 종료한 경우 처리
                    if (string.IsNullOrEmpty(response))
                    {
                        ShowDisconnectedMessage("서버 연결이 종료되었습니다.");
                        break;
                    }

                    // 퇴장 완료처럼 수신 루프를 끝내야 하는 메시지면 종료
                    if (!ProcessReceivedMessage(response))
                    {
                        break;
                    }
                }
            }
            catch (SocketException ex)
            {
                if (!isClosing)
                {
                    ShowReceiveError("서버 연결이 끊어졌습니다: " + ex.Message);
                }
            }
            catch (EndOfStreamException)
            {
                if (!isClosing)
                {
                    ShowDisconnectedMessage("서버가 연결을 종료했습니다.");
                }
            }
            catch (ObjectDisposedException)
            {
                if (!isClosing)
                {
                    ShowDisconnectedMessage("서버 연결이 종료되었습니다.");
                }
            }
            catch (Exception ex)
            {
                if (!isClosing)
                {
                    ShowReceiveError("오류가 발생했습니다: " + ex.Message);
                }
            }
            finally
            {
                // 폼 종료 로직이 수신 스레드 종료를 확인할 수 있도록 설정
                isRunning = false;
                receiveStopped.Set();
            }
        }

        // 연결 상태를 검사한 뒤 패킷 전송, 전송 실패를 호출자에게 bool로 반환
        private bool TrySendPacket(string request)
        {
            if (!IsSocketConnected())
            {
                return false;
            }

            try
            {
                PacketHandler.SendPacket(socket, request);
                return true;
            }
            catch (SocketException ex)
            {
                RunOnUiThread(() =>
                {
                    MessageBox.Show("서버로 데이터를 전송하지 못했습니다: " + ex.Message, "전송 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });

                return false;
            }
            catch (EndOfStreamException)
            {
                return false;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
        }

        // 서버 공통 헤더를 파싱, 방/게임/채팅 처리로 핸들
        private bool ProcessReceivedMessage(string response)
        {
            ServerMessage message = ServerMessageParser.Parse(response);

            switch (message.Type)
            {
                case ServerMessageType.ExitRoomComplete:
                    return false;

                case ServerMessageType.RoomEvent:
                    RoomHandle(message.Content);
                    return true;

                case ServerMessageType.GameEvent:
                    EventHandle(message.Content);
                    return true;

                case ServerMessageType.Chat:
                    AppendChatMessage(message.Content);
                    return true;

                default:
                    return true;
            }
        }

        private void ShowDisconnectedMessage(string message)
        {
            RunOnUiThread(() =>
            {
                MessageBox.Show(message, "연결 종료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        }

        private void ShowReceiveError(string message)
        {
            RunOnUiThread(() =>
            {
                MessageBox.Show(message, "수신 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            });
        }
    }
}