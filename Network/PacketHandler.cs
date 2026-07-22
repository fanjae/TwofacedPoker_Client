using System.Buffers.Binary;
using System.Net.Sockets;
using System.Text;

namespace TwofacedPoker_Client.Network
{
    public static class PacketHandler
    {
        private const int MaxPacketSize = 1024;

        // 클라이언트에서 하나의 Socket을 공유하기 때문에 전송 보호 
        private static readonly object SendLock = new();

        private static readonly object LogLock = new();
        private static readonly string LogFilePath = Path.Combine(AppContext.BaseDirectory,$"packet_log_{Environment.ProcessId}.txt");
        public static string ReceivePacket(Socket socket)
        {
            byte[] lengthBuffer = new byte[sizeof(uint)];

            ReceiveAll(socket, lengthBuffer);

            uint bodyLength = BinaryPrimitives.ReadUInt32BigEndian(lengthBuffer);

            if (bodyLength == 0 || bodyLength > MaxPacketSize)
            {
                throw new InvalidDataException($"잘못된 패킷 크기입니다: {bodyLength}");
            }

            int bodySize = checked((int)bodyLength);
            byte[] bodyBuffer = new byte[bodySize];
            ReceiveAll(socket, bodyBuffer);

            string message = Encoding.UTF8.GetString(bodyBuffer);

            LogPacketToFile($"receive : {bodyLength} {message}");

            return message;

        }
        public static void SendPacket(Socket socket, string message)
        {
            byte[] bodyBuffer = Encoding.UTF8.GetBytes(message);

            if (bodyBuffer.Length == 0 || bodyBuffer.Length > MaxPacketSize)
            {
                throw new InvalidDataException($"잘못된 송신 패킷 크기입니다: {bodyBuffer.Length}");
            }

            byte[] lengthBuffer = new byte[sizeof(uint)];

            BinaryPrimitives.WriteUInt32BigEndian(lengthBuffer,(uint)bodyBuffer.Length);

            // 헤더 전송과 본문 전송을 하나의 임계 구역으로 묶어야 한다.
            lock (SendLock)
            {
                SendAll(socket, lengthBuffer);
                SendAll(socket, bodyBuffer);
            }

            LogPacketToFile($"Send: {bodyBuffer.Length} {message}");
        }

        private static void ReceiveAll(
            Socket socket,
            byte[] buffer)
        {
            int received = 0;

            while (received < buffer.Length)
            {
                int result = socket.Receive(buffer,received,buffer.Length - received,SocketFlags.None);

                if (result == 0)
                {
                    throw new EndOfStreamException("서버가 연결을 정상적으로 종료했습니다.");
                }

                received += result;
            }
        }

        private static void SendAll(Socket socket,byte[] buffer)
        {
            int sent = 0;

            while (sent < buffer.Length)
            {
                int result = socket.Send(buffer,sent,buffer.Length - sent,SocketFlags.None);

                if (result == 0)
                {
                    throw new EndOfStreamException("패킷 전송 중 연결이 종료되었습니다.");
                }

                sent += result;
            }
        }

        private static void LogPacketToFile(string message)
        {
            try
            {
                lock (LogLock)
                {
                    File.AppendAllText(LogFilePath,$"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}: {message}{Environment.NewLine}",Encoding.UTF8);
                }
            }
            catch (IOException)
            {
                // 로그 기록 실패가 네트워크 및 게임 실행을 종료시키면 안 됨
            }
            catch (UnauthorizedAccessException)
            {
                // 로그 파일 접근 권한 문제도 게임 로직과 분리
            }
        }
    }
}
