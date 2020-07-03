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

        #region Property

        /// <summary>
        /// 고급기능 선택 유무 
        /// 선택 O => true
        /// 선택 X => false
        /// </summary>
        public bool checkedAdvanced { get; set; }

        #endregion

        #region # Constructor

        public mainForm()
        {
            InitializeComponent();

            // Yes 라디오 버튼 활성화
            noRadioButton.Checked = true;
        }

        #endregion

        #region # Event

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
                        DownloadYoutubeVideo();
                    }
                }
            }
        }

        /// <summary>
        /// Y 라디오버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void yesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            checkedAdvanced = true;

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
            checkedAdvanced = false;

            // 고급기능 패널 비활성화
            advancedPannel.Visible = false;

            // 폼 크기 조절
            
        }

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
                string exePath = System.Windows.Forms.Application.StartupPath + @"\ffmpeg\bin\youtube-dl";
                string savedVideoName = storageLocationTextBox.Text + @"\" + savedVideoFileNameTextBox.Text;
                string command = exePath + " -o " + savedVideoName + " " + youtubeURLTextBox.Text;

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

                pro.WaitForExit();
                pro.Close();

                MessageBox.Show("유튜브 영상 다운 성공");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "에러발생");
            }
        }

        #endregion

    }
}
