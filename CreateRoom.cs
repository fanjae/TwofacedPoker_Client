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

        public string getRoomNameTextBox()
        {
            return roomNameTextBox.Text;
        }

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
