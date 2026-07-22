using TwofacedPoker_Client.Common;

namespace TwofacedPoker_Client
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // 고해상도 화면, 기본 글꼴 등 전역 설정 초기화. 
            ApplicationConfiguration.Initialize();
            Application.Run(new LobbyForm());
        }
    }
}