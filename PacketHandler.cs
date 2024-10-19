using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TwofacedPoker_Client
{
    public static class PacketHandler
    {
        public static string ReceivePakcet(Socket clientSocket)
        {
            string message = string.Empty;
            byte[] length_buffer = new byte[4];

            int byte_length = clientSocket.Receive(length_buffer);

            if (byte_length == 0)
            {
                
            }
            if (byte_length < 0)
            {
                 throw new Exception("Length_ Error");
            }
            int packet_length = BitConverter.ToInt32(length_buffer, 0);
            packet_length = IPAddress.NetworkToHostOrder(packet_length);
            if (packet_length <= 0 || packet_length > 1024)
            {
                 throw new Exception("Packet_length Error");
            }

            byte[] buffer = new byte[packet_length];
            byte_length = clientSocket.Receive(buffer, packet_length, SocketFlags.None);

            if (byte_length < 0)
            {
                throw new Exception("Packet_Load_Error");
            }
            message = Encoding.UTF8.GetString(buffer, 0, byte_length);

            LogPacketToFile("Receive : " + byte_length.ToString() + " " + message);

            return message;
        }
        public static void SendPacket(Socket clientSocket, string message)
        {

            byte[] message_bytes = System.Text.Encoding.UTF8.GetBytes(message);
            int packet_length = message_bytes.Length;

            LogPacketToFile("Send : " + message + " " + packet_length.ToString());

            byte[] length_buffer = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(packet_length));



            int byte_send_length = clientSocket.Send(length_buffer);
            byte_send_length = clientSocket.Send(message_bytes);
        }

        private static void LogPacketToFile(string message)
        {
            string logFilePath = "packet_log.txt";  // 로그 파일 경로
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
    }
}
