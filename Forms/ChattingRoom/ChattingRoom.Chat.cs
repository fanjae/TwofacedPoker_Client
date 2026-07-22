namespace TwofacedPoker_Client
{
    public partial class ChattingRoom_Form
    {
        // 보내기 버튼
        // 공백 메시지는 전송하지 않고, 전송 성공시 입력창 비움.
        private void SendButton_Click(object sender, EventArgs e)
        {
            string message = sendTextBox.Text.Trim();

            if (message.Length == 0)
            {
                return;
            }

            if (TrySendPacket(message))
            {
                sendTextBox.Clear();
            }
        }

        // Enter 키를 보내기 버튼과 동일하게 처리, 기본 개행을 막음

        private void sendTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.SuppressKeyPress = true;
            SendButton_Click(sender, e);
        }

        private void AppendChatMessage(string message)
        {
            RunOnUiThread(() =>
            {
                chattingRoomTextBox.AppendText(message + Environment.NewLine);
            });
        }

        private void SetChatEnabled(bool enabled)
        {
            sendTextBox.Enabled = enabled;
            SendButton.Enabled = enabled;
        }
    }
}