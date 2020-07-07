using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MakeImageFromYoutube
{
    public partial class mainForm : System.Windows.Forms.Form
    {

        #region # Property

        /// <summary>
        /// 기입정보 파일 있는지 확인 
        /// </summary>
        /// <returns></returns>
        private bool ExistInfoFile
        {
            get
            {
                FileInfo fileInfo = new FileInfo(Application.StartupPath + "\\Info.csv");
                return fileInfo.Exists ? true : false;
            }
        }

        /// <summary>
        /// 기입 정보 클래스
        /// </summary>
        public Info info;

        /// <summary>
        /// 로그 클래스
        /// </summary>
        public Log log;

        private const int VIDEO_CONVERT_TO_IMAGES = 0;
        private const int ONLY_CONTVERT_TO_IMAGES = 1;

        #endregion

        #region # Constructor

        public mainForm()
        {
            InitializeComponent();

            // 로그 폴더 생성
            log = new Log();
            log.CreateLogFolder();

            // 파일 존재 O -> 처음 실행 X
            // 파일 존재 X -> 처음 실행 O
            InitializeControl(!ExistInfoFile);
        }

        #endregion

        #region # Event

        #region ## Button

        /// <summary>
        /// 영상 저장할 경로 선택 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findVideoLocationButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                storageLocationTextBox.Text = dialog.FileName;
            }
        }

        /// <summary>
        /// 다운로드 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void downloadButton_Click(object sender, EventArgs e)
        {
            // 폴더 경로 텍스트 및 Youtube 주소 유무 확인
            if (string.IsNullOrEmpty(storageLocationTextBox.Text) || string.IsNullOrEmpty(youtubeURLTextBox.Text) || string.IsNullOrEmpty(savedVideoFileNameTextBox.Text))
            {
                MessageBox.Show("기본 기능의 입력란에 작성하지 않은 곳이 있는지 확인해주세요.", "경고");
            }
            else
            {
                // 폴더 존재 유무 확인
                DirectoryInfo di = new DirectoryInfo(storageLocationTextBox.Text);
                if (!di.Exists)
                {
                    MessageBox.Show("영상을 저장시킬 경로를 다시 확인해주세요.", "경고");
                }
                else
                {
                    // 경로 중 공백이 들어있는 경로가 있는지 확인
                    // 공백이 있는 경우 youtube-dl.exe는 실행되지 않는다.
                    List<string> spacePathList = CheckWhiteSpaceInPath(storageLocationTextBox.Text);

                    if (spacePathList.Count != 0)
                    {
                        string spaceFolder = string.Empty;

                        foreach (string folder in spacePathList)
                        {
                            spaceFolder += folder;
                            spaceFolder += ", ";
                        }

                        spaceFolder = spaceFolder.Substring(0, spaceFolder.Length - 2);

                        MessageBox.Show("저장 경로에 공백때문에 정상작동하지 않습니다.\n(" + spaceFolder + ")", "경고");
                    }
                    else
                    {
                        // 고급기능 사용 O
                        if (info.checkedAdvanced)
                        {
                            MessageBox.Show("고급기능 개발중");
                        }
                        // 고급기능 사용 X
                        else
                        {
                            DownloadYoutubeVideo();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 변환할 동영상 경로 선택 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void videoPathButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                videoPathTextBox.Text = dialog.FileName;
            }
        }

        /// <summary>
        /// 저장할 이미지 경로 선택 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveImagePathButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                saveImagePathTextBox.Text = dialog.FileName;
            }
        }

        /// <summary>
        /// 이미지 변환 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeImageButton_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region ## TextBox

        /// <summary>
        /// 고급기능 -> 시 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hourTextBox_Leave(object sender, EventArgs e)
        {
            if (hourTextBox.Text.Length < 2)
            {
                MessageBox.Show("00h 형태로 입력해주세요.", "경고");
                hourTextBox.Focus();
            }
            else
            {
                info.hour = hourTextBox.Text;
            }
        }

        /// <summary>
        /// 고급기능 -> 분 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void minuteTextBox_Leave(object sender, EventArgs e)
        {
            if (minuteTextBox.Text.Length < 2)
            {
                MessageBox.Show("00m 형태로 입력해주세요.", "경고");
                minuteTextBox.Focus();
            }
            else
            {
                info.hour = minuteTextBox.Text;
            }
        }

        /// <summary>
        /// 고급기능 -> 초 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void secondTextBox_Leave(object sender, EventArgs e)
        {
            if (secondTextBox.Text.Length < 2)
            {
                MessageBox.Show("00s 형태로 입력해주세요.", "경고");
                secondTextBox.Focus();
            }
            else
            {
                info.hour = secondTextBox.Text;
            }
        }

        /// <summary>
        /// 시 텍스트 박스에서 키보드 눌렀을 때 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hourTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
                MessageBox.Show("숫자만 입력할 수 있습니다.", "경고");
            }
            // 숫자 개수 제한 (2개)
        }

        /// <summary>
        /// 분 텍스트 박스에서 키보드 눌렀을 때 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void minutTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            string minute = string.Empty;

            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
                MessageBox.Show("숫자만 입력할 수 있습니다.", "경고");
            }
            else if (char.IsDigit(e.KeyChar))
            {
                minute = minuteTextBox.Text + e.KeyChar;

                // 60분 이상 입력 제한
                if (Convert.ToInt32(minute) >= 60)
                {
                    e.Handled = true;
                    MessageBox.Show("60분 이상 입력 불가합니다.", "경고");
                }
            }
        }

        /// <summary>
        /// 초 텍스트 박스에서 키보드 눌렀을 때 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void secondTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            string second = string.Empty;

            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
                MessageBox.Show("숫자만 입력할 수 있습니다.", "경고");
            }
            else if (char.IsDigit(e.KeyChar))
            {
                second = secondTextBox.Text + e.KeyChar;

                // 60분 이상 입력 제한
                if (Convert.ToInt32(second) >= 60)
                {
                    e.Handled = true;
                    MessageBox.Show("60초 이상 입력 불가합니다.", "경고");
                }
            }
        }

        /// <summary>
        /// Frame Rate 값 변화 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frameRateUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (frameRateUpDown.Value < 1)
            {
                MessageBox.Show("프레임 레이트를 0으로 설정하면 이미지 변환이 어려울 수 있습니다.");
                frameRateUpDown.Value = 0;
            }
            else if (frameRateUpDown.Value > 5)
            {
                MessageBox.Show("프레임 레이트는 5가 넘을 필요가 없습니다.");
                frameRateUpDown.Value = 5;
            }
            else
            {
                info.frameRate = frameRateUpDown.Value;
            }
        }

        #endregion

        #region ## CheckBox & RadioButton

        /// <summary>
        /// Y 라디오버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void yesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            info.checkedAdvanced = true;

            // 고급기능 패널 활성화
            advancedPannel.Visible = true;
        }

        /// <summary>
        /// N 라디오버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void noRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            info.checkedAdvanced = false;

            // 고급기능 패널 비활성화
            advancedPannel.Visible = false;
        }



        /// <summary>
        /// 변환할 동영상 경로 체크박스 변화 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void videoPathCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            info.isCheckVideoPath = videoPathCheckBox.Checked;

            if (videoPathCheckBox.Checked)
            {
                videoPathTextBox.Enabled = true;
                videoPathButton.Enabled = true;
            }
            else
            {
                videoPathTextBox.Enabled = false;
                videoPathButton.Enabled = false;
            }
        }

        /// <summary>
        /// 저장할 이미지 경로 체크박스 변화 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveImagePathCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            info.isCheckSaveImagePath = saveImagePathCheckBox.Checked;

            if (saveImagePathCheckBox.Checked)
            {
                saveImagePathTextBox.Enabled = true;
                saveImagePathButton.Enabled = true;
            }
            else
            {
                saveImagePathTextBox.Enabled = false;
                saveImagePathButton.Enabled = false;
            }
        }

        #endregion

        #region ## Form

        /// <summary>
        /// 폼 종료 후 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 기입정보 저장
            log.CreateInfoLog(info);
        }

        #endregion

        #endregion

        #region # Method

        /// <summary>
        /// 저장될 영상 경로에 공백이 있는지 체크
        /// </summary>
        /// <param name="savePath"></param>
        /// <returns></returns>
        private List<string> CheckWhiteSpaceInPath(string savePath)
        {
            List<string> spacePathList = new List<string>();
            string[] pathArray = savePath.Split('\\');

            foreach (string path in pathArray)
            {
                if (path.IndexOf(" ") > 0)
                {
                    spacePathList.Add(path);
                }
            }

            return spacePathList;
        }

        /// <summary>
        /// 유튜브 영상 다운로드
        /// </summary>
        private void DownloadYoutubeVideo()
        {
            try
            {
                // 경로 저장
                string exePath = Application.StartupPath + @"\ffmpeg\bin\youtube-dl";
                string savedVideoName = storageLocationTextBox.Text + @"\" + savedVideoFileNameTextBox.Text;
                //string command = exePath + " -o " + savedVideoName + " " + youtubeURLTextBox.Text;
                string command = exePath + " -o " + savedVideoName + " -f bestvideo+bestaudio --merge-output-format mkv " + youtubeURLTextBox.Text;

                // Youtube 영상 저장경로 저장 (자동 불러오기 위해서)
                info.savedVideoPath = storageLocationTextBox.Text;

                ProcessStartInfo pri = new ProcessStartInfo();
                Process pro = new Process();

                // 실행할 파일명 입력
                pri.FileName = "cmd.exe";

                // cmd 창 띄우기
                pri.CreateNoWindow = false; // false -> 띄우기, true -> 안 띄우기
                pri.UseShellExecute = false;

                pri.RedirectStandardInput = true;
                pri.RedirectStandardOutput = true;
                pri.RedirectStandardError = true;

                pro.StartInfo = pri;
                pro.Start();

                // 명령어 실행
                pro.StandardInput.Write(command + Environment.NewLine);
                pro.StandardInput.Close();

                // 이 코드가 있어야 제대로 유튜브 영상을 다운받을 수 있다.
                string result = pro.StandardOutput.ReadToEnd();

                // 다운로드 로그 저장
                log.WriteLog(result);

                pro.WaitForExit();
                pro.Close();

                MessageBox.Show("유튜브 영상 다운 성공");
            }
            catch (Exception ex)
            {
                MessageBox.Show("유튜브 영상을 다운받는 중 에러가 발생했습니다.", "에러발생");
                log.WriteLog("[ERROR] : " + ex);
            }
        }

        /// <summary>
        /// 프로그램 실행 시 컨트롤 초기화 메서드
        /// isFirst = true -> 처음 실행 O(Info 정보 없음)
        /// isFirst = false -> 처음 실행 X(Info 정보 있음)
        /// </summary>
        /// <param name="isFirst"></param>
        private void InitializeControl(bool isFirst)
        {
            if (isFirst)
            {
                info = new Info();

                // No 라디오 버튼 활성화
                noRadioButton.Checked = true;

                // 변환할 동영상 경로 체크박스 비활성화
                videoPathCheckBox.Checked = info.isCheckVideoPath;
                videoPathTextBox.Enabled = info.isCheckVideoPath;
                videoPathButton.Enabled = info.isCheckVideoPath;

                // 저장할 이미지 경로 체크박스 비활성화
                saveImagePathCheckBox.Checked = info.isCheckSaveImagePath;
                saveImagePathTextBox.Enabled = info.isCheckSaveImagePath;
                saveImagePathButton.Enabled = info.isCheckSaveImagePath;

                // 시작시간 초기화
                hourTextBox.Text = info.hour;
                minuteTextBox.Text = info.minute;
                secondTextBox.Text = info.second;

                // 프레임 레이트 초기화
                frameRateUpDown.Value = info.frameRate;
            }
            else
            {
                // Info File이 있는 경우에는 정보 불러와서 Info 클래스에 값 저장 후 컨트롤에 적용
                info = log.GetInfoLog();

                // 고급기능 초기화
                if (info.checkedAdvanced)
                {
                    yesRadioButton.Checked = true;
                }
                else
                {
                    noRadioButton.Checked = true;
                }

                // 변환할 동영상 경로 체크박스 초기화
                videoPathCheckBox.Checked = info.isCheckVideoPath;
                videoPathTextBox.Enabled = info.isCheckVideoPath;
                videoPathButton.Enabled = info.isCheckVideoPath;

                // 저장할 이미지 경로 체크박스 초기화
                saveImagePathCheckBox.Checked = info.isCheckSaveImagePath;
                saveImagePathTextBox.Enabled = info.isCheckSaveImagePath;
                saveImagePathButton.Enabled = info.isCheckSaveImagePath;

                // 시작시간 초기화
                hourTextBox.Text = info.hour;
                minuteTextBox.Text = info.minute;
                secondTextBox.Text = info.second;

                // 프레임 레이트 초기화
                frameRateUpDown.Value = info.frameRate;
            }
        }

        private void ConvertToImages(int type)
        {
            // Youtube 다운 + 이미지 변환
            if (type == VIDEO_CONVERT_TO_IMAGES)
            {

            }
            // 원래 있던 영상을 이미지 변환만
            else if (type == ONLY_CONTVERT_TO_IMAGES)
            {

            }
        }

        #endregion

    }
}
