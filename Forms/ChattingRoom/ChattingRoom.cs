using System.IO;
using System.Net.Sockets;
using System.Threading.Channels;
using TwofacedPoker_Client.Common;
using TwofacedPoker_Client.Game;
using TwofacedPoker_Client.Protocol;
using TwofacedPoker_Client.Protocol.Room;

namespace TwofacedPoker_Client
{
    public partial class ChattingRoom_Form : Form
    {
        private Socket socket;
        private string myID;
        private Thread receiveThread;
        private bool isInitialized;

        private volatile bool isRunning;
        private volatile bool isClosing;

        private readonly ManualResetEventSlim receiveStopped = new(false);
        private bool allowFormClose;

        private AudioPlayer? player;
        private readonly ClientGameState gameState = new ClientGameState();
        private readonly ClientRoomState roomState = new ClientRoomState();

        // 서버 게임 이벤트를 수신 순서대로 보관하여 연출이 서로 덮어쓰이지 않게 처리
        private readonly Channel<string> presentationQueue = Channel.CreateUnbounded<string>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false
        });

        // 폼 종료 시 대기 중인 연출 작업과 Task.Delay를 중단하기 위한 취소 토큰
        private readonly CancellationTokenSource presentationCancellation = new();
        private Task? presentationWorker;

        // 로비에서 사용하던 소켓을 넘겨받아 동일한 서버 연결을 계속 사용
        public ChattingRoom_Form(Socket socket,string roomName,string myID)
        {
            InitializeComponent();

            this.socket = socket;
            this.myID = myID;

            Text = roomName;
            isRunning = true;
            KeyPreview = true;

            // 채팅 방에서 별도의 타임아웃 없이 수신 스레드가 패킷 대기
            socket.SendTimeout = 0;
            socket.ReceiveTimeout = 0;

            myFront_Card.SizeMode = PictureBoxSizeMode.Zoom;
            myBack_Card.SizeMode = PictureBoxSizeMode.Zoom;
            vsFront_Card.SizeMode = PictureBoxSizeMode.Zoom;

            My_ID_Label.Text = "ID : " + myID;

            // Form이 화면에 표시된 뒤 핸들이 생성되면 초기화와 수신 스레드를 시작
            Shown += ChattingRoom_Form_Shown;
        }

        private void ChattingRoom_Form_Shown(object? sender, EventArgs e)
        {
            if (isInitialized || isClosing || IsDisposed)
            {
                return;
            }

            isInitialized = true;

            // Form 핸들이 생성된 뒤 기본 카드 이미지를 설정
            SetCardImage(myFront_Card, "Front10.jpg");
            SetCardImage(vsFront_Card, "Front10.jpg");
            SetCardImage(myBack_Card, "Back10.jpg");

            // UI 스레드가 멈추지 않도록 백그라운드 스레드에서 계속 처리
            receiveThread = new Thread(Receive)
            {
                IsBackground = true
            };

            // UI 스레드를 차단하지 않도록 게임 이벤트 연출을 별도 작업에서 순차 처리
            presentationWorker = Task.Run(() => RunPresentationQueueAsync(presentationCancellation.Token));

            receiveThread = new Thread(Receive)
            {
                IsBackground = true
            };

            receiveThread.Start();
        }

        private void ChattingRoom_Form_KeyDown(object sender, KeyEventArgs e)
        {
            // 게임 진행 중이거나 서버 연결이 끊긴 상태에서 준비/시작 단축키 처리하지 않음
            if (gameState.IsGamePlaying || !IsSocketConnected())
            {
                return;
            }

            // F5 : 내 준비 상태 전환
            if (e.KeyCode == Keys.F5)
            {
                ToggleReadyState();
                return;
            }

            // F6 : 두 플레이어가 모두 준비된 경우 게임 시작 요청.
            if (e.KeyCode == Keys.F6 && roomState.CanStartGame)
            {
                string request = ClientRequestFactory.CreateGameStartRequest();
                TrySendPacket(request);
            }
        }
        // 수신 스레드에서 발생한 UI 변경을 안전하게 UI 스레드로 전달
        private void RunOnUiThread(Action action)
        {
            if (IsDisposed || Disposing || !IsHandleCreated)
            {
                return;
            }

            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(action);
                    return;
                }

                action();
            }
            catch (ObjectDisposedException)
            {
                // 폼이 이미 Dispose된 경우
            }
            catch (InvalidOperationException)
            {
                // 폼 종료 과정에서 핸들이 제거된 경우
            }
        }

        private void PlaySound(string fileName)
        {
            player ??= new AudioPlayer();
            player.PlaySound(fileName);
        }

        // 새 카드 이미지를 적용, 기존 Bitmap을 해제하여 GDI 리소스 누수 방지.
        private void SetCardImage(PictureBox pictureBox, string fileName)
        {
            RunOnUiThread(() =>
            {
                Bitmap newImage = CardImageLoader.Load(fileName);
                Image oldImage = pictureBox.Image;

                pictureBox.Image = newImage;

                oldImage?.Dispose();
            });
        }

        // 수신 스레드에서 직접 UI를 변경하지 않고 연출 큐에 이벤트를 추가
        private void EnqueuePresentation(string message)
        {
            if (isClosing)
            {
                return;
            }

            presentationQueue.Writer.TryWrite(message);
        }

        // 단일 소비자가 이벤트를 하나씩 처리하여 서버 이벤트 순서와 화면 연출 순서를 일치시킴
        private async Task RunPresentationQueueAsync(CancellationToken token)
        {
            try
            {
                await foreach (string message in
                    presentationQueue.Reader.ReadAllAsync(token))
                {
                    await ProcessGameEventAsync(message, token);
                }
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                // 폼 종료 시 정상적으로 연출 큐를 중단
            }
        }
    }
}
