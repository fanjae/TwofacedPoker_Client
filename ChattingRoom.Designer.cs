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
            Vs_ID_Label = new Label();
            panel1 = new Panel();
            Cancle_Button = new Button();
            Die_Bet_Button = new Button();
            Bet_Chip = new Button();
            Bet_Chip_Count = new TextBox();
            Dealer_Label = new Label();
            panel6 = new Panel();
            Dealer_Chip = new Label();
            System_Message = new Label();
            label8 = new Label();
            label7 = new Label();
            Vs_Turn = new Label();
            My_Turn = new Label();
            Vs_Ready = new Label();
            My_Ready = new Label();
            My_Chip = new Label();
            Vs_Chip = new Label();
            label5 = new Label();
            Label6 = new Label();
            Both_Bet_Button = new Button();
            Back_Bet_Button = new Button();
            Front_Bet_Button = new Button();
            vsFront_Card = new PictureBox();
            myBack_Card = new PictureBox();
            My_ID_Label = new Label();
            myFront_Card = new PictureBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            panel5 = new Panel();
            My_Back_Chip = new Label();
            panel4 = new Panel();
            My_Front_Chip = new Label();
            panel3 = new Panel();
            Vs_Front_Chip = new Label();
            panel2 = new Panel();
            Vs_Back_Chip = new Label();
            label10 = new Label();
            panel1.SuspendLayout();
            panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)vsFront_Card).BeginInit();
            ((System.ComponentModel.ISupportInitialize)myBack_Card).BeginInit();
            ((System.ComponentModel.ISupportInitialize)myFront_Card).BeginInit();
            panel5.SuspendLayout();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // sendTextBox
            // 
            sendTextBox.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            sendTextBox.Location = new Point(34, 667);
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
            chattingRoomTextBox.Size = new Size(883, 620);
            chattingRoomTextBox.TabIndex = 1;
            // 
            // SendButton
            // 
            SendButton.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            SendButton.Location = new Point(617, 667);
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
            ExitButton.Location = new Point(774, 667);
            ExitButton.Name = "ExitButton";
            ExitButton.Size = new Size(143, 45);
            ExitButton.TabIndex = 4;
            ExitButton.Text = "나가기";
            ExitButton.UseVisualStyleBackColor = true;
            ExitButton.Click += ExitButton_Click;
            // 
            // Vs_ID_Label
            // 
            Vs_ID_Label.AutoSize = true;
            Vs_ID_Label.BackColor = Color.PaleGreen;
            Vs_ID_Label.Font = new Font("맑은 고딕", 14F, FontStyle.Bold);
            Vs_ID_Label.Location = new Point(1143, 27);
            Vs_ID_Label.Name = "Vs_ID_Label";
            Vs_ID_Label.Size = new Size(113, 38);
            Vs_ID_Label.TabIndex = 5;
            Vs_ID_Label.Text = "ID : ???";
            // 
            // panel1
            // 
            panel1.BackColor = Color.PaleGreen;
            panel1.Controls.Add(Cancle_Button);
            panel1.Controls.Add(Die_Bet_Button);
            panel1.Controls.Add(Bet_Chip);
            panel1.Controls.Add(Bet_Chip_Count);
            panel1.Controls.Add(Dealer_Label);
            panel1.Controls.Add(panel6);
            panel1.Controls.Add(System_Message);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(Vs_Turn);
            panel1.Controls.Add(My_Turn);
            panel1.Controls.Add(Vs_Ready);
            panel1.Controls.Add(My_Ready);
            panel1.Controls.Add(My_Chip);
            panel1.Controls.Add(Vs_Chip);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(Label6);
            panel1.Controls.Add(Both_Bet_Button);
            panel1.Controls.Add(Back_Bet_Button);
            panel1.Controls.Add(Front_Bet_Button);
            panel1.Controls.Add(vsFront_Card);
            panel1.Controls.Add(myBack_Card);
            panel1.Controls.Add(My_ID_Label);
            panel1.Controls.Add(myFront_Card);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(Vs_ID_Label);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(panel5);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Location = new Point(946, 24);
            panel1.Name = "panel1";
            panel1.Size = new Size(1493, 688);
            panel1.TabIndex = 7;
            // 
            // Cancle_Button
            // 
            Cancle_Button.BackColor = Color.Ivory;
            Cancle_Button.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            Cancle_Button.Location = new Point(890, 510);
            Cancle_Button.Name = "Cancle_Button";
            Cancle_Button.Size = new Size(91, 107);
            Cancle_Button.TabIndex = 34;
            Cancle_Button.Text = "취소";
            Cancle_Button.UseVisualStyleBackColor = false;
            Cancle_Button.Click += Cancle_Button_Click;
            // 
            // Die_Bet_Button
            // 
            Die_Bet_Button.BackColor = Color.Ivory;
            Die_Bet_Button.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            Die_Bet_Button.Location = new Point(1017, 387);
            Die_Bet_Button.Name = "Die_Bet_Button";
            Die_Bet_Button.Size = new Size(91, 107);
            Die_Bet_Button.TabIndex = 33;
            Die_Bet_Button.Text = "포기";
            Die_Bet_Button.UseVisualStyleBackColor = false;
            // 
            // Bet_Chip
            // 
            Bet_Chip.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            Bet_Chip.Location = new Point(752, 635);
            Bet_Chip.Name = "Bet_Chip";
            Bet_Chip.Size = new Size(143, 45);
            Bet_Chip.TabIndex = 8;
            Bet_Chip.Text = "베팅";
            Bet_Chip.UseVisualStyleBackColor = true;
            Bet_Chip.Click += Bet_Chip_Click;
            // 
            // Bet_Chip_Count
            // 
            Bet_Chip_Count.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            Bet_Chip_Count.Location = new Point(590, 636);
            Bet_Chip_Count.Name = "Bet_Chip_Count";
            Bet_Chip_Count.Size = new Size(141, 45);
            Bet_Chip_Count.TabIndex = 8;
            // 
            // Dealer_Label
            // 
            Dealer_Label.AutoSize = true;
            Dealer_Label.BackColor = Color.PaleGreen;
            Dealer_Label.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            Dealer_Label.Location = new Point(1015, 207);
            Dealer_Label.Name = "Dealer_Label";
            Dealer_Label.Size = new Size(99, 38);
            Dealer_Label.TabIndex = 32;
            Dealer_Label.Text = "Dealer";
            // 
            // panel6
            // 
            panel6.BackColor = Color.FromArgb(192, 255, 255);
            panel6.Controls.Add(Dealer_Chip);
            panel6.Location = new Point(976, 248);
            panel6.Name = "panel6";
            panel6.Size = new Size(182, 122);
            panel6.TabIndex = 31;
            // 
            // Dealer_Chip
            // 
            Dealer_Chip.AutoSize = true;
            Dealer_Chip.BackColor = Color.FromArgb(192, 255, 255);
            Dealer_Chip.Font = new Font("맑은 고딕", 20F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Dealer_Chip.Location = new Point(68, 32);
            Dealer_Chip.Name = "Dealer_Chip";
            Dealer_Chip.Size = new Size(46, 54);
            Dealer_Chip.TabIndex = 30;
            Dealer_Chip.Text = "0";
            // 
            // System_Message
            // 
            System_Message.AutoSize = true;
            System_Message.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            System_Message.Location = new Point(425, 27);
            System_Message.Name = "System_Message";
            System_Message.Size = new Size(595, 38);
            System_Message.TabIndex = 28;
            System_Message.Text = "<Message : 게임 진행은 여기서 출력됩니다.>";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            label8.Location = new Point(107, 27);
            label8.Name = "label8";
            label8.Size = new Size(179, 38);
            label8.TabIndex = 27;
            label8.Text = "<상대 카드>";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            label7.Location = new Point(137, 643);
            label7.Name = "label7";
            label7.Size = new Size(151, 38);
            label7.TabIndex = 26;
            label7.Text = "<내 카드>";
            // 
            // Vs_Turn
            // 
            Vs_Turn.AutoSize = true;
            Vs_Turn.BackColor = Color.PaleGreen;
            Vs_Turn.Font = new Font("맑은 고딕", 14F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Vs_Turn.ForeColor = Color.Red;
            Vs_Turn.Location = new Point(1092, 27);
            Vs_Turn.Name = "Vs_Turn";
            Vs_Turn.Size = new Size(45, 38);
            Vs_Turn.TabIndex = 25;
            Vs_Turn.Text = "▶";
            // 
            // My_Turn
            // 
            My_Turn.AutoSize = true;
            My_Turn.BackColor = Color.PaleGreen;
            My_Turn.Font = new Font("맑은 고딕", 14F, FontStyle.Bold, GraphicsUnit.Point, 129);
            My_Turn.ForeColor = Color.Red;
            My_Turn.Location = new Point(1083, 520);
            My_Turn.Name = "My_Turn";
            My_Turn.Size = new Size(45, 38);
            My_Turn.TabIndex = 24;
            My_Turn.Text = "▶";
            // 
            // Vs_Ready
            // 
            Vs_Ready.AutoSize = true;
            Vs_Ready.BackColor = Color.PaleGreen;
            Vs_Ready.Font = new Font("맑은 고딕", 14F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Vs_Ready.ForeColor = Color.Red;
            Vs_Ready.Location = new Point(1239, 85);
            Vs_Ready.Name = "Vs_Ready";
            Vs_Ready.Size = new Size(113, 38);
            Vs_Ready.TabIndex = 23;
            Vs_Ready.Text = "<준비>";
            // 
            // My_Ready
            // 
            My_Ready.AutoSize = true;
            My_Ready.BackColor = Color.PaleGreen;
            My_Ready.Font = new Font("맑은 고딕", 14F, FontStyle.Bold, GraphicsUnit.Point, 129);
            My_Ready.ForeColor = Color.Red;
            My_Ready.Location = new Point(1239, 589);
            My_Ready.Name = "My_Ready";
            My_Ready.Size = new Size(113, 38);
            My_Ready.TabIndex = 22;
            My_Ready.Text = "<준비>";
            // 
            // My_Chip
            // 
            My_Chip.AutoSize = true;
            My_Chip.BackColor = Color.PaleGreen;
            My_Chip.Font = new Font("맑은 고딕", 14F, FontStyle.Bold);
            My_Chip.Location = new Point(1425, 520);
            My_Chip.Name = "My_Chip";
            My_Chip.Size = new Size(33, 38);
            My_Chip.TabIndex = 21;
            My_Chip.Text = "0";
            // 
            // Vs_Chip
            // 
            Vs_Chip.AutoSize = true;
            Vs_Chip.BackColor = Color.PaleGreen;
            Vs_Chip.Font = new Font("맑은 고딕", 14F, FontStyle.Bold);
            Vs_Chip.Location = new Point(1425, 27);
            Vs_Chip.Name = "Vs_Chip";
            Vs_Chip.Size = new Size(33, 38);
            Vs_Chip.TabIndex = 20;
            Vs_Chip.Text = "0";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.PaleGreen;
            label5.Font = new Font("맑은 고딕", 14F, FontStyle.Bold);
            label5.Location = new Point(1336, 520);
            label5.Name = "label5";
            label5.Size = new Size(95, 38);
            label5.TabIndex = 19;
            label5.Text = "Chip :";
            // 
            // Label6
            // 
            Label6.AutoSize = true;
            Label6.BackColor = Color.PaleGreen;
            Label6.Font = new Font("맑은 고딕", 14F, FontStyle.Bold);
            Label6.Location = new Point(1336, 27);
            Label6.Name = "Label6";
            Label6.Size = new Size(95, 38);
            Label6.TabIndex = 18;
            Label6.Text = "Chip :";
            // 
            // Both_Bet_Button
            // 
            Both_Bet_Button.BackColor = Color.Ivory;
            Both_Bet_Button.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            Both_Bet_Button.Location = new Point(616, 510);
            Both_Bet_Button.Name = "Both_Bet_Button";
            Both_Bet_Button.Size = new Size(91, 107);
            Both_Bet_Button.TabIndex = 17;
            Both_Bet_Button.Text = "양면";
            Both_Bet_Button.UseVisualStyleBackColor = false;
            Both_Bet_Button.Click += Both_Bet_Button_Click;
            // 
            // Back_Bet_Button
            // 
            Back_Bet_Button.BackColor = Color.Ivory;
            Back_Bet_Button.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            Back_Bet_Button.Location = new Point(756, 510);
            Back_Bet_Button.Name = "Back_Bet_Button";
            Back_Bet_Button.Size = new Size(91, 107);
            Back_Bet_Button.TabIndex = 16;
            Back_Bet_Button.Text = "뒷면";
            Back_Bet_Button.UseVisualStyleBackColor = false;
            Back_Bet_Button.Click += Back_Bet_Button_Click;
            // 
            // Front_Bet_Button
            // 
            Front_Bet_Button.BackColor = Color.Ivory;
            Front_Bet_Button.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            Front_Bet_Button.Location = new Point(475, 510);
            Front_Bet_Button.Name = "Front_Bet_Button";
            Front_Bet_Button.Size = new Size(91, 107);
            Front_Bet_Button.TabIndex = 15;
            Front_Bet_Button.Text = "앞면";
            Front_Bet_Button.UseVisualStyleBackColor = false;
            Front_Bet_Button.Click += Front_Bet_Button_Click;
            // 
            // vsFront_Card
            // 
            vsFront_Card.BackgroundImageLayout = ImageLayout.Stretch;
            vsFront_Card.Image = (Image)resources.GetObject("vsFront_Card.Image");
            vsFront_Card.InitialImage = null;
            vsFront_Card.Location = new Point(122, 85);
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
            myBack_Card.Location = new Point(225, 437);
            myBack_Card.Name = "myBack_Card";
            myBack_Card.Size = new Size(150, 180);
            myBack_Card.TabIndex = 13;
            myBack_Card.TabStop = false;
            // 
            // My_ID_Label
            // 
            My_ID_Label.AutoSize = true;
            My_ID_Label.BackColor = Color.PaleGreen;
            My_ID_Label.Font = new Font("맑은 고딕", 14F, FontStyle.Bold);
            My_ID_Label.Location = new Point(1134, 520);
            My_ID_Label.Name = "My_ID_Label";
            My_ID_Label.Size = new Size(113, 38);
            My_ID_Label.TabIndex = 6;
            My_ID_Label.Text = "ID : ???";
            // 
            // myFront_Card
            // 
            myFront_Card.BackgroundImageLayout = ImageLayout.Zoom;
            myFront_Card.Image = (Image)resources.GetObject("myFront_Card.Image");
            myFront_Card.InitialImage = null;
            myFront_Card.Location = new Point(24, 437);
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
            label4.Location = new Point(772, 441);
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
            label3.Location = new Point(475, 441);
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
            label2.Location = new Point(772, 108);
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
            label1.Location = new Point(484, 108);
            label1.Name = "label1";
            label1.Size = new Size(75, 38);
            label1.TabIndex = 8;
            label1.Text = "Back";
            // 
            // panel5
            // 
            panel5.BackColor = Color.FromArgb(192, 255, 255);
            panel5.Controls.Add(My_Back_Chip);
            panel5.Location = new Point(720, 316);
            panel5.Name = "panel5";
            panel5.Size = new Size(182, 122);
            panel5.TabIndex = 3;
            // 
            // My_Back_Chip
            // 
            My_Back_Chip.AutoSize = true;
            My_Back_Chip.BackColor = Color.FromArgb(192, 255, 255);
            My_Back_Chip.Font = new Font("맑은 고딕", 20F, FontStyle.Bold, GraphicsUnit.Point, 129);
            My_Back_Chip.Location = new Point(68, 37);
            My_Back_Chip.Name = "My_Back_Chip";
            My_Back_Chip.Size = new Size(46, 54);
            My_Back_Chip.TabIndex = 29;
            My_Back_Chip.Text = "0";
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(192, 255, 255);
            panel4.Controls.Add(My_Front_Chip);
            panel4.Location = new Point(425, 316);
            panel4.Name = "panel4";
            panel4.Size = new Size(182, 122);
            panel4.TabIndex = 2;
            // 
            // My_Front_Chip
            // 
            My_Front_Chip.AutoSize = true;
            My_Front_Chip.BackColor = Color.FromArgb(192, 255, 255);
            My_Front_Chip.Font = new Font("맑은 고딕", 20F, FontStyle.Bold, GraphicsUnit.Point, 129);
            My_Front_Chip.Location = new Point(67, 33);
            My_Front_Chip.Name = "My_Front_Chip";
            My_Front_Chip.Size = new Size(46, 54);
            My_Front_Chip.TabIndex = 30;
            My_Front_Chip.Text = "0";
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(192, 255, 255);
            panel3.Controls.Add(Vs_Front_Chip);
            panel3.Location = new Point(720, 149);
            panel3.Name = "panel3";
            panel3.Size = new Size(182, 122);
            panel3.TabIndex = 1;
            // 
            // Vs_Front_Chip
            // 
            Vs_Front_Chip.AutoSize = true;
            Vs_Front_Chip.BackColor = Color.FromArgb(192, 255, 255);
            Vs_Front_Chip.Font = new Font("맑은 고딕", 20F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Vs_Front_Chip.Location = new Point(68, 32);
            Vs_Front_Chip.Name = "Vs_Front_Chip";
            Vs_Front_Chip.Size = new Size(46, 54);
            Vs_Front_Chip.TabIndex = 30;
            Vs_Front_Chip.Text = "0";
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(192, 255, 255);
            panel2.Controls.Add(Vs_Back_Chip);
            panel2.Controls.Add(label10);
            panel2.Location = new Point(422, 149);
            panel2.Name = "panel2";
            panel2.Size = new Size(182, 122);
            panel2.TabIndex = 0;
            // 
            // Vs_Back_Chip
            // 
            Vs_Back_Chip.AutoSize = true;
            Vs_Back_Chip.BackColor = Color.FromArgb(192, 255, 255);
            Vs_Back_Chip.Font = new Font("맑은 고딕", 20F, FontStyle.Bold, GraphicsUnit.Point, 129);
            Vs_Back_Chip.Location = new Point(71, 32);
            Vs_Back_Chip.Name = "Vs_Back_Chip";
            Vs_Back_Chip.Size = new Size(46, 54);
            Vs_Back_Chip.TabIndex = 31;
            Vs_Back_Chip.Text = "0";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = Color.FromArgb(192, 255, 255);
            label10.Font = new Font("맑은 고딕", 20F, FontStyle.Bold, GraphicsUnit.Point, 129);
            label10.Location = new Point(70, 28);
            label10.Name = "label10";
            label10.Size = new Size(46, 54);
            label10.TabIndex = 32;
            label10.Text = "0";
            // 
            // ChattingRoom_Form
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(2466, 736);
            Controls.Add(ExitButton);
            Controls.Add(SendButton);
            Controls.Add(chattingRoomTextBox);
            Controls.Add(sendTextBox);
            Controls.Add(panel1);
            Name = "ChattingRoom_Form";
            Text = "ChattingRoom";
            FormClosing += ChattingRoom_Form_FormClosing;
            KeyDown += ChattingRoom_Form_KeyDown;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)vsFront_Card).EndInit();
            ((System.ComponentModel.ISupportInitialize)myBack_Card).EndInit();
            ((System.ComponentModel.ISupportInitialize)myFront_Card).EndInit();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox sendTextBox;
        private TextBox chattingRoomTextBox;
        private Button SendButton;
        private Button ExitButton;
        private Label Vs_ID_Label;
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
        private Button Both_Bet_Button;
        private Button Back_Bet_Button;
        private Button Front_Bet_Button;
        private Label Label6;
        private Label My_ID_Label;
        private Label Vs_Chip;
        private Label label5;
        private Label My_Chip;
        private Label Vs_Ready;
        private Label My_Ready;
        private Label Vs_Turn;
        private Label My_Turn;
        private Label label7;
        private Label label8;
        private Label System_Message;
        private Label My_Back_Chip;
        private Label Dealer_Label;
        private Panel panel6;
        private Label Dealer_Chip;
        private Label My_Front_Chip;
        private Label Vs_Front_Chip;
        private Label Vs_Back_Chip;
        private Label label10;
        private TextBox Bet_Chip_Count;
        private Button Bet_Chip;
        private Button Die_Bet_Button;
        private Button Cancle_Button;
    }
}