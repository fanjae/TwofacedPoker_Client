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
            Vs_Turn = new Label();
            My_Turn = new Label();
            Vs_Ready = new Label();
            My_Ready = new Label();
            My_Chip = new Label();
            Vs_Chip = new Label();
            label5 = new Label();
            Label6 = new Label();
            button3 = new Button();
            button2 = new Button();
            button1 = new Button();
            vsFront_Card = new PictureBox();
            myBack_Card = new PictureBox();
            My_ID_Label = new Label();
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
            panel1.Controls.Add(Vs_Turn);
            panel1.Controls.Add(My_Turn);
            panel1.Controls.Add(Vs_Ready);
            panel1.Controls.Add(My_Ready);
            panel1.Controls.Add(My_Chip);
            panel1.Controls.Add(Vs_Chip);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(Label6);
            panel1.Controls.Add(button3);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
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
            // button3
            // 
            button3.BackColor = Color.Ivory;
            button3.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            button3.Location = new Point(616, 510);
            button3.Name = "button3";
            button3.Size = new Size(91, 107);
            button3.TabIndex = 17;
            button3.Text = "양면";
            button3.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.BackColor = Color.Ivory;
            button2.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            button2.Location = new Point(756, 510);
            button2.Name = "button2";
            button2.Size = new Size(91, 107);
            button2.TabIndex = 16;
            button2.Text = "뒷면";
            button2.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            button1.BackColor = Color.Ivory;
            button1.Font = new Font("맑은 고딕", 14F, FontStyle.Regular, GraphicsUnit.Point, 129);
            button1.Location = new Point(475, 510);
            button1.Name = "button1";
            button1.Size = new Size(91, 107);
            button1.TabIndex = 15;
            button1.Text = "앞면";
            button1.UseVisualStyleBackColor = false;
            // 
            // vsFront_Card
            // 
            vsFront_Card.BackgroundImageLayout = ImageLayout.Stretch;
            vsFront_Card.Image = (Image)resources.GetObject("vsFront_Card.Image");
            vsFront_Card.InitialImage = null;
            vsFront_Card.Location = new Point(122, 149);
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
            panel5.Location = new Point(720, 316);
            panel5.Name = "panel5";
            panel5.Size = new Size(182, 122);
            panel5.TabIndex = 3;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(192, 255, 255);
            panel4.Location = new Point(425, 316);
            panel4.Name = "panel4";
            panel4.Size = new Size(182, 122);
            panel4.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(192, 255, 255);
            panel3.Location = new Point(720, 149);
            panel3.Name = "panel3";
            panel3.Size = new Size(182, 122);
            panel3.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(192, 255, 255);
            panel2.Location = new Point(422, 149);
            panel2.Name = "panel2";
            panel2.Size = new Size(182, 122);
            panel2.TabIndex = 0;
            // 
            // ChattingRoom_Form
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(2465, 741);
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
        private Button button3;
        private Button button2;
        private Button button1;
        private Label Label6;
        private Label My_ID_Label;
        private Label Vs_Chip;
        private Label label5;
        private Label My_Chip;
        private Label Vs_Ready;
        private Label My_Ready;
        private Label Vs_Turn;
        private Label My_Turn;
    }
}