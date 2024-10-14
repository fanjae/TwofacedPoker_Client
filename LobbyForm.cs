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
                    throw new InvalidOperationException("������ ������ �� �����ϴ�.");
                }
                return true;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "���� ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("�� �� ���� ������ �߻��߽��ϴ�: " + ex.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                MessageBox.Show("������ ����Ǿ����ϴ�.", "���� ����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogTextBox.Text += "[Log] ���� ������ �����Ͽ����ϴ�. \r\n";
                LogTextBox.Text += client_ID + "\r\n";


            }
            catch (SocketException ex)
            {
                MessageBox.Show("������ ������ �� �����ϴ�: " + ex.Message, "���� ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show($"server.ini ������ �д� �� ������ �߻��߽��ϴ�: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1); // ������ ���� ���ϸ� ���α׷� ����
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
                    MessageBox.Show("���� ������ ���� �����ϴ�.", "�� ����Ʈ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RoomList.Items.Clear();  // ListBox �ʱ�ȭ
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
                        MessageBox.Show("�̹� �����ϴ� ���Դϴ�.", "�� ���� ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("�� ������ �Ϸ�Ǿ����ϴ�.", "�� ���� ����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LogTextBox.Text += "[Log] ���� �����Ͽ����ϴ�. \r\n";

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
                MessageBox.Show("���õ� ���� �����ϴ�.", "�� ���� ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("���� �������� �ʽ��ϴ�.", "�� ���� ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                ChattingRoom_Form room = new ChattingRoom_Form(socket, message, client_ID);

                room.ShowDialog();
            }
        }



    }
}
