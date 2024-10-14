using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TwofacedPoker_Client
{
    public partial class LobbyForm : Form
    {
        private Socket socket;
        private String client_ID;
        public LobbyForm()
        {
            InitializeComponent();
        }

        private bool IsSocketConnected(Socket socket)
        {
            try
            {
                if (socket == null || socket.Connected == false)
                {
                    throw new InvalidOperationException("서버와 연결할 수 없습니다.");
                }
                return true;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("알 수 없는 오류가 발생했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            string path = Constants.INI_FILE_PATH;
            string serverIp;
            int serverPort;

            (serverIp, serverPort) = Read_Server_Config(path);

            IPAddress Ip = IPAddress.Parse(serverIp);
            IPEndPoint endPoint = new IPEndPoint(Ip, serverPort);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(endPoint);

                string request = Constants.LOGIN;
                byte[] buffer = Encoding.UTF8.GetBytes(request);
                socket.Send(buffer);

                byte[] recvBuffer = new byte[1024];
                int recvLength = socket.Receive(recvBuffer);

                string ID = Encoding.UTF8.GetString(recvBuffer, 0, recvLength);
                IDLabel.Text = ID;
                client_ID = ID.Substring("ID :".Length);

                MessageBox.Show("서버에 연결되었습니다.", "연결 성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogTextBox.Text += "[Log] 서버 연결을 성공하였습니다. \r\n";
                LogTextBox.Text += client_ID + "\r\n";


            }
            catch (SocketException ex)
            {
                MessageBox.Show("서버에 연결할 수 없습니다: " + ex.Message, "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static (string, int) Read_Server_Config(string path)
        {
            string serverIp = "";
            int serverPort = 0;

            try
            {
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    if (line.StartsWith("server="))
                    {
                        serverIp = line.Substring("server=".Length);
                    }
                    else if (line.StartsWith("port="))
                    {
                        serverPort = int.Parse(line.Substring("port=".Length));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"server.ini 파일을 읽는 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1); // 파일을 읽지 못하면 프로그램 종료
            }

            return (serverIp, serverPort);
        }

        private void GetRoomListButton_Click(object sender, EventArgs e)
        {
            if (IsSocketConnected(socket))
            {
                string request = Constants.GET_CHATTING_ROOM;
                byte[] buffer = Encoding.UTF8.GetBytes(request);
                socket.Send(buffer);

                byte[] recvBuffer = new byte[1024];
                int recvLength = socket.Receive(recvBuffer);
                string rooms = Encoding.UTF8.GetString(recvBuffer, 0, recvLength);

                if (rooms == Constants.NO_ROOM)
                {
                    MessageBox.Show("현재 생성된 방이 없습니다.", "방 리스트", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RoomList.Items.Clear();  // ListBox 초기화
                }
                else
                {
                    string[] roomList = rooms.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    RoomList.Items.Clear();
                    RoomList.Items.AddRange(roomList);
                }
            }
        }

        private void CreateRoomButton_Click(object sender, EventArgs e)
        {
            if (IsSocketConnected(socket))
            {
                CreateRoom Room = new CreateRoom();

                var result = Room.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string request = Constants.CREATE_CHATTING_ROOM + Room.getRoomNameTextBox();
                    byte[] buffer = Encoding.UTF8.GetBytes(request);
                    socket.Send(buffer);

                    byte[] recvBuffer = new byte[1024];
                    int recvLength = socket.Receive(recvBuffer);
                    string rooms = Encoding.UTF8.GetString(recvBuffer, 0, recvLength);

                    if (rooms == Constants.EXIST_ROOM)
                    {
                        MessageBox.Show("이미 존재하는 방입니다.", "방 생성 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("방 생성이 완료되었습니다.", "방 생성 성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LogTextBox.Text += "[Log] 방을 생성하였습니다. \r\n";

                        request = Constants.JOIN_CHATTING_ROOM + rooms;
                        JoinChattingRoom(request);
                    }
                }
            }
        }

        private void JoinButton_Click(object sender, EventArgs e)
        {
            if (RoomList.SelectedItem == null)
            {
                MessageBox.Show("선택된 방이 없습니다.", "방 입장 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (IsSocketConnected(socket))
            {
                string request = Constants.JOIN_CHATTING_ROOM + RoomList.SelectedItem.ToString();
                JoinChattingRoom(request);
            }
        }
        private void JoinChattingRoom(string request)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(request);
            socket.Send(buffer);

            byte[] recvBuffer = new byte[1024];
            int recvLength = socket.Receive(recvBuffer);
            string message = Encoding.UTF8.GetString(recvBuffer, 0, recvLength);

            if (message == Constants.NOT_EXIST_ROOM)
            {
                MessageBox.Show("방이 존재하지 않습니다.", "방 입장 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                ChattingRoom_Form room = new ChattingRoom_Form(socket, message, client_ID);

                room.ShowDialog();
            }
        }



    }
}
