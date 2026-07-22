namespace TwofacedPoker_Client.Common
{
    internal static class ServerConfigLoader
    {
        public static (string ServerIp, int ServerPort) Load(string path)
        {
            string serverIp = string.Empty;
            int serverPort = 0;

            string[] lines = File.ReadAllLines(path);

            // ini 파일을 줄 단위로 읽으면서 Key-Value 파싱
            foreach (string line in lines)
            {
                if (line.StartsWith("server="))
                {
                    serverIp = line.Substring("server=".Length).Trim();
                }
                else if (line.StartsWith("port="))
                {
                    string portText = line.Substring("port=".Length).Trim();

                    if (int.TryParse(portText, out int parsedPort))
                    {
                        serverPort = parsedPort;
                    }
                }
            }

            // 필수 설정값 누락 및 범위 유효성 검사
            if (string.IsNullOrEmpty(serverIp))
            {
                throw new Exception("server.ini 파일에 server 값이 없습니다.");
            }

            if (serverPort <= 0 || serverPort > 65535)
            {
                throw new Exception("server.ini 파일의 port 값이 올바르지 않습니다.");
            }

            return (serverIp, serverPort);
        }
    }
}