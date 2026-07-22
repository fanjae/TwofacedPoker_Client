namespace TwofacedPoker_Client.Protocol.Server
{
    // 로그인 응답에서 서버가 발급한 클라이언트 ID를 추출
    internal static class LoginResponseParser
    {
        private const string IdPrefix = "ID :";

        public static bool TryParse(string response, out string clientId)
        {
            // 파싱 실패시 호출자가 이전 ID를 잘못 사용하지 않도록 기본값 초기화
            clientId = string.Empty;

            // 비어 있거나 공백뿐인 응답은 정상 로그인 응답으로 인정하지 않음
            if (string.IsNullOrWhiteSpace(response))
            {
                return false;
            }
            
            // 서버가 약속한 ID 접두사가 없는 응답은 다른 메시지로 판단
            if (!response.StartsWith(IdPrefix))
            {
                return false;
            }

            // 접두사를 제거하고 실제 ID 앞뒤의 불필요한 공백 정리
            clientId = response.Substring(IdPrefix.Length).Trim();

            return !string.IsNullOrEmpty(clientId);
        }
    }
}