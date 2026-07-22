using System.Net;
using System.Net.Sockets;
using TwofacedPoker_Client.Common;
using TwofacedPoker_Client.Network;
using TwofacedPoker_Client.Protocol;
using TwofacedPoker_Client.Protocol.Room;
using TwofacedPoker_Client.Protocol.Server;

namespace TwofacedPoker_Client
{
    public partial class LobbyForm : Form
    {
        private Socket? socket;
        private string? clientId;
        private bool isRequesting;

        public LobbyForm()
        {
            InitializeComponent();
        }

        // 하나의 요청이 처리되는 동안 다른 버튼을 잠가 동일 소켓의 동시 송수신 방지
        private void SetRequestState(bool requesting)
        {
            isRequesting = requesting;

            ConnectButton.Enabled = !requesting;
            GetRoomListButton.Enabled = !requesting;
            CreateRoomButton.Enabled = !requesting;
            JoinButton.Enabled = !requesting;
        }

        // 로비에서 발생한 연결 및 방 관련 상태를 사용자에게 누적 표시
        private void AppendLog(string message)
        {
            LogTextBox.AppendText("[Log] " + message + Environment.NewLine);
        }
    }
}