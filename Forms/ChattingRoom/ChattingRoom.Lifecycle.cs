using System.Net.Sockets;
using TwofacedPoker_Client.Common;
using TwofacedPoker_Client.Network;

namespace TwofacedPoker_Client
{
    public partial class ChattingRoom_Form
    {
        private void ExitButton_Click(object sender, EventArgs e)
        {
            // 게임 도중에 임의의 퇴장을 막고, 게임이 끝난 상태에서만 폼을 닫음
            if (!gameState.IsGamePlaying)
            {
                Close();
            }
        }


        private async void ChattingRoom_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 서버 퇴장 절차 끝난 뒤 FormClosing 종료 허용.
            if (allowFormClose)
            {
                receiveStopped.Dispose();
                player?.Dispose();
                return;
            }

            e.Cancel = true;

            // 중복 종료 요청으로 동일한 정리 절차가 여러번 실행되는것을 방지
            if (isClosing)
            {
                return;
            }

            isClosing = true;

            presentationCancellation.Cancel();
            presentationQueue.Writer.TryComplete();
            ExitButton.Enabled = false;

            try
            {
                if (socket != null && socket.Connected)
                {
                    // 서버가 방 인원과 게임 사앹를 정리할 수 있도록 정상 퇴장 요청
                    PacketHandler.SendPacket(socket, Constants.EXIT_ROOM);
                }
                else
                {
                    isRunning = false;
                }
            }
            catch (SocketException)
            {
                isRunning = false;
            }
            catch (EndOfStreamException)
            {
                isRunning = false;
            }
            catch (ObjectDisposedException)
            {
                isRunning = false;
            }

            // 수신 스레드가 ExitRoomComplete 받고 종료할 때까지 3초 대기
            bool stopped = await Task.Run(() => receiveStopped.Wait(TimeSpan.FromSeconds(3)));

            if (!stopped)
            {
                try
                {
                    socket?.Shutdown(SocketShutdown.Both);
                }
                catch (SocketException)
                {
                }
                catch (ObjectDisposedException)
                {
                }

                socket?.Close();

                MessageBox.Show("로비에서 서버에 다시 연결해 주세요.","오류",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }

            // 정리 절차 끝났으므로, FormClosing에 대한 종료 수행
            allowFormClose = true;
            Close();
        }
    }
}