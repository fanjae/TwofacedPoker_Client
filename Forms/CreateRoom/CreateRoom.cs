using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwofacedPoker_Client
{
    public partial class CreateRoom : Form
    {
        public CreateRoom()
        {
            InitializeComponent();
        }

        // 대화 상자가 닫힌 뒤 로비 폼이 입력한 방 제목을 가져갈 수 있도록 반환
        public string getRoomNameTextBox()
        {
            return roomNameTextBox.Text;
        }

        // 방 제목이 입력된 경우에만 DialogResult.OK로 종료하여 실제 생성 요청
        private void CreateRoomButton_Click(object sender, EventArgs e)
        {
            if (roomNameTextBox.Text == "")
            {
                MessageBox.Show("방 주소는 필수 입력 값입니다.", "방 생성 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
