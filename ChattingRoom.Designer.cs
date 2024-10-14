namespace TwofacedPoker_Client
{
    partial class ChattingRoom_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChattingRoom_Form));
            sendTextBox = new TextBox();
            chattingRoomTextBox = new TextBox();
            SendButton = new Button();
            ExitButton = new Button();
            Vs_ID = new Label();
            My_ID = new Label();
            panel1 = new Panel();
            vsFront_Card = new PictureBox();
            myBack_Card = new PictureBox();
            myFront_Card = new PictureBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            panel5 = new Panel();
            panel4 = new Panel();
            panel3 = new Panel();
            panel2 = new Panel();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)vsFront_Card).BeginInit();
            ((System.ComponentModel.ISupportInitialize)myBack_Card).BeginInit();
            ((System.ComponentModel.ISupportInitialize)myFront_Card).BeginInit();
            SuspendLayout();
            // 
            // sendTextBox
            // 
            sendTextBox.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            sendTextBox.Location = new Point(34, 583);
            sendTextBox.Name = "sendTextBox";
            sendTextBox.Size = new Size(577, 45);
            sendTextBox.TabIndex = 0;
            sendTextBox.KeyDown += sendTextBox_KeyDown;
            // 
            // chattingRoomTextBox
            // 
            chattingRoomTextBox.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            chattingRoomTextBox.Location = new Point(34, 21);
            chattingRoomTextBox.Multiline = true;
            chattingRoomTextBox.Name = "chattingRoomTextBox";
            chattingRoomTextBox.ReadOnly = true;
            chattingRoomTextBox.ScrollBars = ScrollBars.Vertical;
            chattingRoomTextBox.Size = new Size(883, 542);
            chattingRoomTextBox.TabIndex = 1;
            // 
            // SendButton
            // 
            SendButton.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            SendButton.Location = new Point(621, 583);
            SendButton.Name = "SendButton";
            SendButton.Size = new Size(143, 45);
            SendButton.TabIndex = 3;
            SendButton.Text = "보내기";
            SendButton.UseVisualStyleBackColor = true;
            SendButton.Click += SendButton_Click;
            // 
            // ExitButton
            // 
            ExitButton.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            ExitButton.Location = new Point(774, 583);
            ExitButton.Name = "ExitButton";
            ExitButton.Size = new Size(143, 45);
            ExitButton.TabIndex = 4;
            ExitButton.Text = "나가기";
            ExitButton.UseVisualStyleBackColor = true;
            ExitButton.Click += ExitButton_Click;
            // 
            // Vs_ID
            // 
            Vs_ID.AutoSize = true;
            Vs_ID.BackColor = Color.PaleGreen;
            Vs_ID.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            Vs_ID.Location = new Point(24, 18);
            Vs_ID.Name = "Vs_ID";
            Vs_ID.Size = new Size(110, 38);
            Vs_ID.TabIndex = 5;
            Vs_ID.Text = "ID : ???";
            // 
            // My_ID
            // 
            My_ID.AutoSize = true;
            My_ID.BackColor = Color.PaleGreen;
            My_ID.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            My_ID.Location = new Point(962, 544);
            My_ID.Name = "My_ID";
            My_ID.Size = new Size(110, 38);
            My_ID.TabIndex = 6;
            My_ID.Text = "ID : ???";
            // 
            // panel1
            // 
            panel1.BackColor = Color.PaleGreen;
            panel1.Controls.Add(vsFront_Card);
            panel1.Controls.Add(myBack_Card);
            panel1.Controls.Add(My_ID);
            panel1.Controls.Add(myFront_Card);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(Vs_ID);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(panel5);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Location = new Point(946, 24);
            panel1.Name = "panel1";
            panel1.Size = new Size(1237, 604);
            panel1.TabIndex = 7;
            // 
            // vsFront_Card
            // 
            vsFront_Card.BackgroundImageLayout = ImageLayout.Stretch;
            vsFront_Card.Image = (Image)resources.GetObject("vsFront_Card.Image");
            vsFront_Card.InitialImage = null;
            vsFront_Card.Location = new Point(109, 92);
            vsFront_Card.Name = "vsFront_Card";
            vsFront_Card.Size = new Size(149, 179);
            vsFront_Card.TabIndex = 14;
            vsFront_Card.TabStop = false;
            // 
            // myBack_Card
            // 
            myBack_Card.BackgroundImageLayout = ImageLayout.Stretch;
            myBack_Card.Image = (Image)resources.GetObject("myBack_Card.Image");
            myBack_Card.InitialImage = null;
            myBack_Card.Location = new Point(201, 387);
            myBack_Card.Name = "myBack_Card";
            myBack_Card.Size = new Size(150, 180);
            myBack_Card.TabIndex = 13;
            myBack_Card.TabStop = false;
            // 
            // myFront_Card
            // 
            myFront_Card.BackgroundImageLayout = ImageLayout.Zoom;
            myFront_Card.Image = (Image)resources.GetObject("myFront_Card.Image");
            myFront_Card.InitialImage = null;
            myFront_Card.Location = new Point(24, 387);
            myFront_Card.Name = "myFront_Card";
            myFront_Card.Size = new Size(150, 180);
            myFront_Card.TabIndex = 12;
            myFront_Card.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.PaleGreen;
            label4.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            label4.Location = new Point(731, 441);
            label4.Name = "label4";
            label4.Size = new Size(75, 38);
            label4.TabIndex = 11;
            label4.Text = "Back";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.PaleGreen;
            label3.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            label3.Location = new Point(471, 441);
            label3.Name = "label3";
            label3.Size = new Size(84, 38);
            label3.TabIndex = 10;
            label3.Text = "Front";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.PaleGreen;
            label2.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            label2.Location = new Point(731, 108);
            label2.Name = "label2";
            label2.Size = new Size(84, 38);
            label2.TabIndex = 9;
            label2.Text = "Front";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.PaleGreen;
            label1.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            label1.Location = new Point(471, 108);
            label1.Name = "label1";
            label1.Size = new Size(75, 38);
            label1.TabIndex = 8;
            label1.Text = "Back";
            // 
            // panel5
            // 
            panel5.BackColor = Color.FromArgb(192, 255, 255);
            panel5.Location = new Point(685, 316);
            panel5.Name = "panel5";
            panel5.Size = new Size(182, 122);
            panel5.TabIndex = 3;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(192, 255, 255);
            panel4.Location = new Point(411, 316);
            panel4.Name = "panel4";
            panel4.Size = new Size(182, 122);
            panel4.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(192, 255, 255);
            panel3.Location = new Point(685, 149);
            panel3.Name = "panel3";
            panel3.Size = new Size(182, 122);
            panel3.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(192, 255, 255);
            panel2.Location = new Point(411, 149);
            panel2.Name = "panel2";
            panel2.Size = new Size(182, 122);
            panel2.TabIndex = 0;
            // 
            // ChattingRoom_Form
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2213, 809);
            Controls.Add(ExitButton);
            Controls.Add(SendButton);
            Controls.Add(chattingRoomTextBox);
            Controls.Add(sendTextBox);
            Controls.Add(panel1);
            Name = "ChattingRoom_Form";
            Text = "ChattingRoom";
            FormClosing += ChattingRoom_Form_FormClosing;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)vsFront_Card).EndInit();
            ((System.ComponentModel.ISupportInitialize)myBack_Card).EndInit();
            ((System.ComponentModel.ISupportInitialize)myFront_Card).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox sendTextBox;
        private TextBox chattingRoomTextBox;
        private Button SendButton;
        private Button ExitButton;
        private Label Vs_ID;
        private Label My_ID;
        private Panel panel1;
        private Panel panel2;
        private PictureBox myFront_Card;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Panel panel5;
        private Panel panel4;
        private Panel panel3;
        private PictureBox vsFront_Card;
        private PictureBox myBack_Card;
    }
}