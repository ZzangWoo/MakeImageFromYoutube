namespace MakeImageFromYoutube
{
    partial class mainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.savedVideoFileNameTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.downloadButton = new System.Windows.Forms.Button();
            this.findVideoLocationButton = new System.Windows.Forms.Button();
            this.youtubeURLTextBox = new System.Windows.Forms.TextBox();
            this.storageLocationTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.advancedPannel = new System.Windows.Forms.Panel();
            this.saveImageNameTextBox = new System.Windows.Forms.TextBox();
            this.saveImagePathCheckBox = new System.Windows.Forms.CheckBox();
            this.videoPathCheckBox = new System.Windows.Forms.CheckBox();
            this.saveImagePathButton = new System.Windows.Forms.Button();
            this.videoPathButton = new System.Windows.Forms.Button();
            this.saveImagePathTextBox = new System.Windows.Forms.TextBox();
            this.videoPathTextBox = new System.Windows.Forms.TextBox();
            this.frameRateUpDown = new System.Windows.Forms.NumericUpDown();
            this.changeImageButton = new System.Windows.Forms.Button();
            this.secondTextBox = new System.Windows.Forms.TextBox();
            this.minuteTextBox = new System.Windows.Forms.TextBox();
            this.hourTextBox = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.yesRadioButton = new System.Windows.Forms.RadioButton();
            this.noRadioButton = new System.Windows.Forms.RadioButton();
            this.label20 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.advancedPannel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frameRateUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "영상 저장할 경로";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Youtube 주소";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 12F);
            this.label3.Location = new System.Drawing.Point(11, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "고급기능";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "시작시간";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.savedVideoFileNameTextBox);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.downloadButton);
            this.panel1.Controls.Add(this.findVideoLocationButton);
            this.panel1.Controls.Add(this.youtubeURLTextBox);
            this.panel1.Controls.Add(this.storageLocationTextBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label1);
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(774, 221);
            this.panel1.TabIndex = 2;
            // 
            // savedVideoFileNameTextBox
            // 
            this.savedVideoFileNameTextBox.Location = new System.Drawing.Point(16, 126);
            this.savedVideoFileNameTextBox.Name = "savedVideoFileNameTextBox";
            this.savedVideoFileNameTextBox.Size = new System.Drawing.Size(570, 21);
            this.savedVideoFileNameTextBox.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 110);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 12);
            this.label7.TabIndex = 5;
            this.label7.Text = "저장할 영상 이름";
            // 
            // downloadButton
            // 
            this.downloadButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.downloadButton.Location = new System.Drawing.Point(628, 70);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(119, 125);
            this.downloadButton.TabIndex = 5;
            this.downloadButton.Text = "다운로드";
            this.downloadButton.UseVisualStyleBackColor = false;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // findVideoLocationButton
            // 
            this.findVideoLocationButton.Location = new System.Drawing.Point(524, 70);
            this.findVideoLocationButton.Name = "findVideoLocationButton";
            this.findVideoLocationButton.Size = new System.Drawing.Size(62, 23);
            this.findVideoLocationButton.TabIndex = 2;
            this.findVideoLocationButton.Text = "찾기";
            this.findVideoLocationButton.UseVisualStyleBackColor = true;
            this.findVideoLocationButton.Click += new System.EventHandler(this.findVideoLocationButton_Click);
            // 
            // youtubeURLTextBox
            // 
            this.youtubeURLTextBox.Location = new System.Drawing.Point(15, 174);
            this.youtubeURLTextBox.Name = "youtubeURLTextBox";
            this.youtubeURLTextBox.Size = new System.Drawing.Size(571, 21);
            this.youtubeURLTextBox.TabIndex = 4;
            // 
            // storageLocationTextBox
            // 
            this.storageLocationTextBox.Location = new System.Drawing.Point(15, 70);
            this.storageLocationTextBox.Name = "storageLocationTextBox";
            this.storageLocationTextBox.Size = new System.Drawing.Size(503, 21);
            this.storageLocationTextBox.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 12F);
            this.label5.Location = new System.Drawing.Point(13, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 16);
            this.label5.TabIndex = 1;
            this.label5.Text = "기본기능";
            // 
            // advancedPannel
            // 
            this.advancedPannel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.advancedPannel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.advancedPannel.Controls.Add(this.label20);
            this.advancedPannel.Controls.Add(this.saveImageNameTextBox);
            this.advancedPannel.Controls.Add(this.saveImagePathCheckBox);
            this.advancedPannel.Controls.Add(this.videoPathCheckBox);
            this.advancedPannel.Controls.Add(this.saveImagePathButton);
            this.advancedPannel.Controls.Add(this.videoPathButton);
            this.advancedPannel.Controls.Add(this.saveImagePathTextBox);
            this.advancedPannel.Controls.Add(this.videoPathTextBox);
            this.advancedPannel.Controls.Add(this.frameRateUpDown);
            this.advancedPannel.Controls.Add(this.changeImageButton);
            this.advancedPannel.Controls.Add(this.secondTextBox);
            this.advancedPannel.Controls.Add(this.minuteTextBox);
            this.advancedPannel.Controls.Add(this.hourTextBox);
            this.advancedPannel.Controls.Add(this.label19);
            this.advancedPannel.Controls.Add(this.label17);
            this.advancedPannel.Controls.Add(this.label15);
            this.advancedPannel.Controls.Add(this.label13);
            this.advancedPannel.Controls.Add(this.label18);
            this.advancedPannel.Controls.Add(this.label16);
            this.advancedPannel.Controls.Add(this.label14);
            this.advancedPannel.Controls.Add(this.label12);
            this.advancedPannel.Controls.Add(this.label11);
            this.advancedPannel.Controls.Add(this.label10);
            this.advancedPannel.Controls.Add(this.label9);
            this.advancedPannel.Controls.Add(this.label8);
            this.advancedPannel.Controls.Add(this.label4);
            this.advancedPannel.Controls.Add(this.label3);
            this.advancedPannel.Location = new System.Drawing.Point(2, 257);
            this.advancedPannel.Name = "advancedPannel";
            this.advancedPannel.Size = new System.Drawing.Size(774, 257);
            this.advancedPannel.TabIndex = 3;
            // 
            // saveImageNameTextBox
            // 
            this.saveImageNameTextBox.Location = new System.Drawing.Point(17, 219);
            this.saveImageNameTextBox.Name = "saveImageNameTextBox";
            this.saveImageNameTextBox.Size = new System.Drawing.Size(569, 21);
            this.saveImageNameTextBox.TabIndex = 10;
            // 
            // saveImagePathCheckBox
            // 
            this.saveImagePathCheckBox.AutoSize = true;
            this.saveImagePathCheckBox.Location = new System.Drawing.Point(17, 172);
            this.saveImagePathCheckBox.Name = "saveImagePathCheckBox";
            this.saveImagePathCheckBox.Size = new System.Drawing.Size(15, 14);
            this.saveImagePathCheckBox.TabIndex = 9;
            this.saveImagePathCheckBox.UseVisualStyleBackColor = true;
            this.saveImagePathCheckBox.CheckedChanged += new System.EventHandler(this.saveImagePathCheckBox_CheckedChanged);
            // 
            // videoPathCheckBox
            // 
            this.videoPathCheckBox.AutoSize = true;
            this.videoPathCheckBox.Location = new System.Drawing.Point(17, 110);
            this.videoPathCheckBox.Name = "videoPathCheckBox";
            this.videoPathCheckBox.Size = new System.Drawing.Size(15, 14);
            this.videoPathCheckBox.TabIndex = 9;
            this.videoPathCheckBox.UseVisualStyleBackColor = true;
            this.videoPathCheckBox.CheckedChanged += new System.EventHandler(this.videoPathCheckBox_CheckedChanged);
            // 
            // saveImagePathButton
            // 
            this.saveImagePathButton.Location = new System.Drawing.Point(525, 169);
            this.saveImagePathButton.Name = "saveImagePathButton";
            this.saveImagePathButton.Size = new System.Drawing.Size(61, 23);
            this.saveImagePathButton.TabIndex = 8;
            this.saveImagePathButton.Text = "찾기";
            this.saveImagePathButton.UseVisualStyleBackColor = true;
            // 
            // videoPathButton
            // 
            this.videoPathButton.Location = new System.Drawing.Point(525, 107);
            this.videoPathButton.Name = "videoPathButton";
            this.videoPathButton.Size = new System.Drawing.Size(61, 23);
            this.videoPathButton.TabIndex = 8;
            this.videoPathButton.Text = "찾기";
            this.videoPathButton.UseVisualStyleBackColor = true;
            // 
            // saveImagePathTextBox
            // 
            this.saveImagePathTextBox.Location = new System.Drawing.Point(38, 169);
            this.saveImagePathTextBox.Name = "saveImagePathTextBox";
            this.saveImagePathTextBox.Size = new System.Drawing.Size(480, 21);
            this.saveImagePathTextBox.TabIndex = 7;
            // 
            // videoPathTextBox
            // 
            this.videoPathTextBox.Location = new System.Drawing.Point(38, 107);
            this.videoPathTextBox.Name = "videoPathTextBox";
            this.videoPathTextBox.Size = new System.Drawing.Size(480, 21);
            this.videoPathTextBox.TabIndex = 7;
            // 
            // frameRateUpDown
            // 
            this.frameRateUpDown.Location = new System.Drawing.Point(477, 57);
            this.frameRateUpDown.Name = "frameRateUpDown";
            this.frameRateUpDown.Size = new System.Drawing.Size(109, 21);
            this.frameRateUpDown.TabIndex = 6;
            this.frameRateUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.frameRateUpDown.ValueChanged += new System.EventHandler(this.frameRateUpDown_ValueChanged);
            // 
            // changeImageButton
            // 
            this.changeImageButton.Location = new System.Drawing.Point(628, 56);
            this.changeImageButton.Name = "changeImageButton";
            this.changeImageButton.Size = new System.Drawing.Size(119, 184);
            this.changeImageButton.TabIndex = 5;
            this.changeImageButton.Text = "이미지 변환";
            this.changeImageButton.UseVisualStyleBackColor = true;
            // 
            // secondTextBox
            // 
            this.secondTextBox.Location = new System.Drawing.Point(140, 56);
            this.secondTextBox.Name = "secondTextBox";
            this.secondTextBox.Size = new System.Drawing.Size(36, 21);
            this.secondTextBox.TabIndex = 4;
            this.secondTextBox.TextChanged += new System.EventHandler(this.secondTextBox_TextChanged);
            // 
            // minuteTextBox
            // 
            this.minuteTextBox.Location = new System.Drawing.Point(76, 56);
            this.minuteTextBox.Name = "minuteTextBox";
            this.minuteTextBox.Size = new System.Drawing.Size(36, 21);
            this.minuteTextBox.TabIndex = 3;
            this.minuteTextBox.TextChanged += new System.EventHandler(this.minuteTextBox_TextChanged);
            // 
            // hourTextBox
            // 
            this.hourTextBox.Location = new System.Drawing.Point(17, 56);
            this.hourTextBox.Name = "hourTextBox";
            this.hourTextBox.Size = new System.Drawing.Size(35, 21);
            this.hourTextBox.TabIndex = 2;
            this.hourTextBox.TextChanged += new System.EventHandler(this.hourTextBox_TextChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.ForeColor = System.Drawing.Color.Red;
            this.label19.Location = new System.Drawing.Point(15, 154);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(536, 12);
            this.label19.TabIndex = 1;
            this.label19.Text = "(기본기능에서 다운로드 클릭시 자동으로 exe 실행 경로에서 저장할 영상 이름으로 폴더 만들어짐)";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ForeColor = System.Drawing.Color.Red;
            this.label17.Location = new System.Drawing.Point(130, 91);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(271, 12);
            this.label17.TabIndex = 1;
            this.label17.Text = "(기본기능에서 다운로드 클릭시 자동으로 설정됨)";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(475, 40);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(81, 12);
            this.label15.TabIndex = 1;
            this.label15.Text = "프레임 레이트";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(305, 40);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(57, 12);
            this.label13.TabIndex = 1;
            this.label13.Text = "영상 길이";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(15, 203);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(109, 12);
            this.label18.TabIndex = 1;
            this.label18.Text = "저장할 이미지 이름";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(15, 140);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(109, 12);
            this.label16.TabIndex = 1;
            this.label16.Text = "저장할 이미지 경로";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(15, 91);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(109, 12);
            this.label14.TabIndex = 1;
            this.label14.Text = "변환할 동영상 경로";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.Maroon;
            this.label12.Location = new System.Drawing.Point(305, 59);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(81, 12);
            this.label12.TabIndex = 1;
            this.label12.Text = "필요하면 개발";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(118, 65);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(16, 12);
            this.label11.TabIndex = 1;
            this.label11.Text = "m";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(182, 65);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(12, 12);
            this.label10.TabIndex = 1;
            this.label10.Text = "s";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(58, 65);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(12, 12);
            this.label9.TabIndex = 1;
            this.label9.Text = "h";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(74, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 12);
            this.label8.TabIndex = 1;
            this.label8.Text = "(ex : 00h 00m 00s)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 237);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "고급기능 사용 여부";
            // 
            // yesRadioButton
            // 
            this.yesRadioButton.AutoSize = true;
            this.yesRadioButton.Location = new System.Drawing.Point(140, 235);
            this.yesRadioButton.Name = "yesRadioButton";
            this.yesRadioButton.Size = new System.Drawing.Size(31, 16);
            this.yesRadioButton.TabIndex = 6;
            this.yesRadioButton.TabStop = true;
            this.yesRadioButton.Text = "Y";
            this.yesRadioButton.UseVisualStyleBackColor = true;
            this.yesRadioButton.CheckedChanged += new System.EventHandler(this.yesRadioButton_CheckedChanged);
            // 
            // noRadioButton
            // 
            this.noRadioButton.AutoSize = true;
            this.noRadioButton.Location = new System.Drawing.Point(177, 235);
            this.noRadioButton.Name = "noRadioButton";
            this.noRadioButton.Size = new System.Drawing.Size(32, 16);
            this.noRadioButton.TabIndex = 7;
            this.noRadioButton.TabStop = true;
            this.noRadioButton.Text = "N";
            this.noRadioButton.UseVisualStyleBackColor = true;
            this.noRadioButton.CheckedChanged += new System.EventHandler(this.noRadioButton_CheckedChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.ForeColor = System.Drawing.Color.Red;
            this.label20.Location = new System.Drawing.Point(560, 40);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(26, 12);
            this.label20.TabIndex = 11;
            this.label20.Text = "1~5";
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(780, 519);
            this.Controls.Add(this.noRadioButton);
            this.Controls.Add(this.yesRadioButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.advancedPannel);
            this.MaximizeBox = false;
            this.Name = "mainForm";
            this.Text = "Youtube -> Image";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.mainForm_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.advancedPannel.ResumeLayout(false);
            this.advancedPannel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frameRateUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel advancedPannel;
        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.Button findVideoLocationButton;
        private System.Windows.Forms.TextBox youtubeURLTextBox;
        private System.Windows.Forms.TextBox storageLocationTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton yesRadioButton;
        private System.Windows.Forms.RadioButton noRadioButton;
        private System.Windows.Forms.TextBox savedVideoFileNameTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox secondTextBox;
        private System.Windows.Forms.TextBox minuteTextBox;
        private System.Windows.Forms.TextBox hourTextBox;
        private System.Windows.Forms.Button changeImageButton;
        private System.Windows.Forms.Button videoPathButton;
        private System.Windows.Forms.TextBox videoPathTextBox;
        private System.Windows.Forms.NumericUpDown frameRateUpDown;
        private System.Windows.Forms.CheckBox saveImagePathCheckBox;
        private System.Windows.Forms.CheckBox videoPathCheckBox;
        private System.Windows.Forms.Button saveImagePathButton;
        private System.Windows.Forms.TextBox saveImagePathTextBox;
        private System.Windows.Forms.TextBox saveImageNameTextBox;
        private System.Windows.Forms.Label label20;
    }
}

