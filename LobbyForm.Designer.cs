namespace TwofacedPoker_Client
{
    partial class LobbyForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ConnectButton = new Button();
            label3 = new Label();
            RoomList = new ListBox();
            GetRoomListButton = new Button();
            LogTextBox = new TextBox();
            label4 = new Label();
            CreateRoomButton = new Button();
            JoinButton = new Button();
            IDLabel = new Label();
            SuspendLayout();
            // 
            // ConnectButton
            // 
            ConnectButton.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            ConnectButton.Location = new Point(112, 26);
            ConnectButton.Name = "ConnectButton";
            ConnectButton.Size = new Size(192, 52);
            ConnectButton.TabIndex = 4;
            ConnectButton.Text = "서버 연결";
            ConnectButton.UseVisualStyleBackColor = true;
            ConnectButton.Click += ConnectButton_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            label3.Location = new Point(37, 137);
            label3.Name = "label3";
            label3.Size = new Size(145, 38);
            label3.TabIndex = 5;
            label3.Text = "Room List";
            // 
            // RoomList
            // 
            RoomList.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            RoomList.FormattingEnabled = true;
            RoomList.ItemHeight = 38;
            RoomList.Location = new Point(37, 200);
            RoomList.Name = "RoomList";
            RoomList.Size = new Size(379, 270);
            RoomList.TabIndex = 8;
            // 
            // GetRoomListButton
            // 
            GetRoomListButton.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            GetRoomListButton.Location = new Point(192, 132);
            GetRoomListButton.Name = "GetRoomListButton";
            GetRoomListButton.Size = new Size(224, 49);
            GetRoomListButton.TabIndex = 13;
            GetRoomListButton.Text = "새로고침";
            GetRoomListButton.UseVisualStyleBackColor = true;
            GetRoomListButton.Click += GetRoomListButton_Click;
            // 
            // LogTextBox
            // 
            LogTextBox.Font = new Font("맑은 고딕", 12F, FontStyle.Regular, GraphicsUnit.Point, 129);
            LogTextBox.Location = new Point(430, 200);
            LogTextBox.Multiline = true;
            LogTextBox.Name = "LogTextBox";
            LogTextBox.ReadOnly = true;
            LogTextBox.Size = new Size(408, 270);
            LogTextBox.TabIndex = 15;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            label4.Location = new Point(548, 137);
            label4.Name = "label4";
            label4.Size = new Size(165, 38);
            label4.TabIndex = 14;
            label4.Text = "System Log";
            // 
            // CreateRoomButton
            // 
            CreateRoomButton.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            CreateRoomButton.Location = new Point(430, 504);
            CreateRoomButton.Name = "CreateRoomButton";
            CreateRoomButton.Size = new Size(408, 49);
            CreateRoomButton.TabIndex = 17;
            CreateRoomButton.Text = "방만들기";
            CreateRoomButton.UseVisualStyleBackColor = true;
            CreateRoomButton.Click += CreateRoomButton_Click;
            // 
            // JoinButton
            // 
            JoinButton.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            JoinButton.Location = new Point(37, 504);
            JoinButton.Name = "JoinButton";
            JoinButton.Size = new Size(379, 49);
            JoinButton.TabIndex = 16;
            JoinButton.Text = "입장";
            JoinButton.UseVisualStyleBackColor = true;
            JoinButton.Click += JoinButton_Click;
            // 
            // IDLabel
            // 
            IDLabel.AutoSize = true;
            IDLabel.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point, 129);
            IDLabel.Location = new Point(536, 30);
            IDLabel.Name = "IDLabel";
            IDLabel.Size = new Size(198, 48);
            IDLabel.TabIndex = 18;
            IDLabel.Text = "ID : 미생성";
            // 
            // LobbyForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(865, 575);
            Controls.Add(IDLabel);
            Controls.Add(CreateRoomButton);
            Controls.Add(JoinButton);
            Controls.Add(LogTextBox);
            Controls.Add(label4);
            Controls.Add(GetRoomListButton);
            Controls.Add(RoomList);
            Controls.Add(label3);
            Controls.Add(ConnectButton);
            Name = "LobbyForm";
            Text = "TwoFacedPoker";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button ConnectButton;
        private Label label3;
        private ListBox RoomList;
        private Button GetRoomListButton;
        private TextBox LogTextBox;
        private Label label4;
        private Button CreateRoomButton;
        private Button JoinButton;
        private Label IDLabel;
    }
}
