using System.Net;
using System.Net.Sockets;
using TwofacedPoker_Client.Common;
using TwofacedPoker_Client.Network;
using TwofacedPoker_Client.Protocol.Server;

namespace TwofacedPoker_Client
{
    public partial class LobbyForm
    {
        // 로비 기능 실행 전 서버 연결 여부를 검사
        private bool IsSocketConnected()
        {
            if (socket == null || socket.Connected == false)
            {
                MessageBox.Show("서버와 연결되어 있지 않습니다.", "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        // 설정 파일의 서버 주소로 접속하고 로그인 응답에서 클라이언트 ID를 받음

        private async void ConnectButton_Click(object sender, EventArgs e)
        {
            if (isRequesting)
            {
                return;
            }

            SetRequestState(true);

            Socket? newSocket = null;

            try
            {
                (string serverIp, int serverPort) = ServerConfigLoader.Load(Constants.INI_FILE_PATH);

                IPAddress ipAddress = IPAddress.Parse(serverIp);
                IPEndPoint endPoint = new IPEndPoint(ipAddress, serverPort);

                newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // 소켓 연결과 송수신을 작업 스레드에서 실행해 UI 멈춤을 방지
                await Task.Run(() =>
                {
                    newSocket.Connect(endPoint);
                    PacketHandler.SendPacket(newSocket, Constants.LOGIN);
                });

                string response = await Task.Run(() =>
                {
                    return PacketHandler.ReceivePacket(newSocket);
                });

                // 프로토콜 형식에 맞지 않으면 연결 성공으로 간주하지 않음
                if (!LoginResponseParser.TryParse(response, out string receivedId))
                {
                    throw new Exception("서버에서 잘못된 로그인 응답을 받았습니다.");
                }

                socket?.Close();

                socket = newSocket;
                clientId = receivedId;

                IDLabel.Text = "ID : " + clientId;

                MessageBox.Show("서버에 연결되었습니다.", "연결 성공", MessageBoxButtons.OK, MessageBoxIcon.Information);

                AppendLog("서버 연결에 성공했습니다.");
                AppendLog("할당된 ID: " + clientId);
            }
            catch (Exception ex)
            {
                newSocket?.Close();

                MessageBox.Show("서버 연결 중 오류가 발생했습니다: " + ex.Message, "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetRequestState(false);
            }
        }

        // 통신 오류 발생 시 소켓과 로그인 상태를 모두 초기화해 재연결 상태로 만듦
        private void HandleConnectionError(Exception ex)
        {
            socket?.Close();

            socket = null;
            clientId = null;

            IDLabel.Text = "ID : 미생성";
            RoomList.Items.Clear();

            MessageBox.Show("서버 통신 중 오류가 발생했습니다: " + ex.Message, "통신 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            AppendLog("서버 연결이 종료되었습니다.");
        }
    }
}