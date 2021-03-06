﻿using MakeImageFromYoutube.Facade;
using Microsoft.WindowsAPICodePack.Dialogs;
using NReco.VideoInfo;
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

        /// <summary>
        /// Facade 클래스
        /// </summary>
        private YoutubeToImages youtubeToImages;

        //private const int VIDEO_CONVERT_TO_IMAGES = 0;
        //private const int ONLY_CONTVERT_TO_IMAGES = 1;

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

            // Facade 클래스 동적할당
            youtubeToImages = new YoutubeToImages();
        }

        #endregion

        #region # Event

        #region ## Button

        #region ### Youtube Video + Convert Images

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

                    if (spacePathList.Count > 0)
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
                    else if (CheckWhiteSpaceInPath(savedVideoFileNameTextBox.Text).Count > 0)
                    {
                        MessageBox.Show("저장할 영상 이름에 공백이 있으면 안됩니다.", "경고");
                        savedVideoFileNameTextBox.Focus();
                    }
                    else
                    {
                        // 고급기능 사용 O
                        if (info.checkedAdvanced)
                        {
                            info.youtubedlPath = Application.StartupPath + @"\ffmpeg\bin\youtube-dl";
                            info.saveVideoPath = storageLocationTextBox.Text + @"\" + savedVideoFileNameTextBox.Text;
                            info.youtubeURL = youtubeURLTextBox.Text;

                            // 유튜브 영상 다운로드
                            bool result = youtubeToImages.DownloadYoutubeVideo(info);

                            if (result)
                            {
                                // 이미지로 변환할 영상 재생시간 + 시작시간 초로
                                string videoPath = storageLocationTextBox.Text + @"\" + savedVideoFileNameTextBox.Text + ".mkv";
                                FFProbe ffProbe;
                                double videoDuration;
                                double startTimeDuration;

                                // 영상있는지 확인
                                if (IsExistFile(videoPath))
                                {
                                    ffProbe = new FFProbe();
                                    var videoInfo = ffProbe.GetMediaInfo(videoPath);
                                    videoDuration = Math.Floor(videoInfo.Duration.TotalSeconds); // 소수점이 나오는 경우가 있다해서 버림 사용

                                    // 시작시간 초로 계산
                                    startTimeDuration = (Convert.ToDouble(hourTextBox.Text) * 60 * 60) + (Convert.ToDouble(minuteTextBox.Text) * 60) + Convert.ToDouble(secondTextBox.Text);
                                }
                                else
                                {
                                    MessageBox.Show("이미지로 변환할 영상이 존재하지 않습니다. 로그를 확인해주세요.", "오류");
                                    return;
                                }

                                // 저장할 이미지 이름에 띄어쓰기가 있는지 확인
                                if (CheckWhiteSpaceInPath(saveImageNameTextBox.Text).Count > 0)
                                {
                                    MessageBox.Show("저장할 이미지 이름에 띄어쓰기가 있으면 안됩니다.", "경고");
                                    saveImageNameTextBox.Focus();
                                }
                                // 이미지로 변환할 영상 길이와 시작시간 비교
                                else if (startTimeDuration >= videoDuration)
                                {
                                    MessageBox.Show("이미지로 변환할 영상의 길이보다 시작시간이 더 큽니다.", "경고");
                                    hourTextBox.Focus();
                                }
                                // 저장할 이미지 경로 빈 칸인지 확인
                                else if (info.isCheckSaveImagePath && string.IsNullOrEmpty(saveImagePathTextBox.Text))
                                {
                                    MessageBox.Show("저장할 이미지 경로를 입력해주세요", "경고");
                                    saveImagePathTextBox.Focus();
                                }
                                // 저장할 이미지 경로에 띄어쓰기 있는지 확인
                                else if (info.isCheckSaveImagePath && CheckWhiteSpaceInPath(saveImagePathTextBox.Text).Count > 0)
                                {
                                    MessageBox.Show("저장할 이미지 경로에 띄어쓰기가 있으면 안됩니다.", "경고");
                                    saveImagePathTextBox.Focus();
                                }
                                // 저장할 이미지 경로 유무 확인
                                else if (info.isCheckSaveImagePath && !IsExistDirectory(saveImagePathTextBox.Text))
                                {
                                    MessageBox.Show("저장할 이미지 경로가 없습니다. 다시 확인해주세요.", "경고");
                                    saveImagePathTextBox.Focus();
                                }
                                // 저장할 이미지 이름이 빈 칸인지 확인
                                else if (string.IsNullOrEmpty(saveImageNameTextBox.Text))
                                {
                                    MessageBox.Show("저장할 이미지 이름을 입력해주세요", "경고");
                                    saveImageNameTextBox.Focus();
                                }
                                else
                                {
                                    info.videoPath = videoPath;
                                    info.saveImagePath = saveImagePathTextBox.Text;
                                    info.saveImageName = saveImageNameTextBox.Text;
                                    info.startTime = hourTextBox.Text + ":" + minuteTextBox.Text + ":" + secondTextBox.Text + ".00";
                                    info.ffmpegPath = Application.StartupPath + @"\ffmpeg\bin\ffmpeg.exe";
                                    info.frameRate = frameRateUpDown.Value;

                                    result = youtubeToImages.ConvertYoutubeToImages(info);

                                    if (result)
                                    {
                                        MessageBox.Show("이미지 변환 성공", "알림");
                                    }
                                    else
                                    {
                                        MessageBox.Show("이미지 변환 실패", "알림");
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("유튜브 영상 다운 실패", "알림");
                            }
                        }
                        // 고급기능 사용 X
                        else
                        {
                            info.youtubedlPath = Application.StartupPath + @"\ffmpeg\bin\youtube-dl";
                            info.saveVideoPath = storageLocationTextBox.Text + @"\" + savedVideoFileNameTextBox.Text;
                            info.youtubeURL = youtubeURLTextBox.Text;

                            bool result = youtubeToImages.DownloadYoutubeVideo(info);
                            if (result)
                            {
                                if (mp4YesRadioButton.Checked)
                                {
                                    info.ffmpegPath = Application.StartupPath + @"\ffmpeg\bin\ffmpeg";

                                    result = youtubeToImages.ConvertMP4(info);
                                    if (result)
                                    {
                                        if (IsExistFile(info.saveVideoPath + ".mkv"))
                                        {
                                            try
                                            {
                                                System.IO.File.Delete(info.saveVideoPath + ".mkv");
                                            }
                                            catch (Exception ex)
                                            {
                                                log.WriteLog("[ERROR]_MP4변환 : " + ex);
                                                MessageBox.Show("유튜브 영상 변환 중 오류가 발생했습니다.", "오류");
                                            }
                                        }

                                        MessageBox.Show("유튜브 영상 다운 성공", "알림");
                                    }
                                    else
                                    {
                                        MessageBox.Show("유튜브 영상 다운 실패", "알림");
                                    }
                                }
                                else if (mp4NoRadioButton.Checked)
                                {
                                    MessageBox.Show("유튜브 영상 다운 성공", "알림");
                                }
                            }
                            else
                            {
                                MessageBox.Show("유튜브 영상 다운 실패", "알림");
                            }
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
            if (!videoPathCheckBox.Checked)
            {
                MessageBox.Show("변환할 동영상 경로의 체크박스를 선택해주세요.", "경고");
            }
            else if (string.IsNullOrEmpty(videoPathTextBox.Text))
            {
                MessageBox.Show("변환할 동영상 경로를 입력해주세요.", "경고");
                videoPathTextBox.Focus();
            }
            else if (!saveImagePathCheckBox.Checked)
            {
                MessageBox.Show("저장할 이미지 경로의 체크박스를 선택해주세요.", "경고");
            }
            else if (string.IsNullOrEmpty(saveImagePathTextBox.Text))
            {
                MessageBox.Show("저장할 이미지 경로를 입력해주세요.", "경고");
                saveImagePathTextBox.Focus();
            }
            //else if (!IsExistDirectory(videoPathTextBox.Text))
            //{
            //    MessageBox.Show("유효하지 않은 변환할 동영상 경로입니다.", "경고");
            //    videoPathTextBox.Focus();
            //}
            else if (!IsExistDirectory(saveImagePathTextBox.Text))
            {
                MessageBox.Show("유효하지 않은 저장할 이미지 경로입니다.", "경고");
                saveImagePathTextBox.Focus();
            }
            else if (CheckWhiteSpaceInPath(videoPathTextBox.Text).Count > 0)
            {
                MessageBox.Show("변환할 동영상 경로에 띄어쓰기가 있으면 이미지 변환을 할 수 없습니다.", "경고");
                videoPathTextBox.Focus();
            } 
            else if (CheckWhiteSpaceInPath(saveImagePathTextBox.Text).Count > 0)
            {
                MessageBox.Show("저장할 이미지 경로에 띄어쓰기가 있으면 이미지 변환을 할 수 없습니다.", "경고");
                saveImagePathTextBox.Focus();
            }
            else if (string.IsNullOrEmpty(saveImageNameTextBox.Text))
            {
                MessageBox.Show("저장할 이미지 이름을 입력해주세요.", "경고");
                saveImageNameTextBox.Focus();
            }
            else if (CheckWhiteSpaceInPath(saveImageNameTextBox.Text).Count > 0)
            {
                MessageBox.Show("저장할 이미지 이름에 띄어쓰기가 있으면 이미지 변환을 할 수 없습니다.", "경고");
                saveImageNameTextBox.Focus();
            }
            else
            {
                // 이미지로 변환할 영상 재생시간 + 시작시간 초로
                string videoPath = videoPathTextBox.Text;
                FFProbe ffProbe;
                double videoDuration;
                double startTimeDuration;

                // 영상있는지 확인
                if (IsExistFile(videoPath))
                {
                    ffProbe = new FFProbe();
                    var videoInfo = ffProbe.GetMediaInfo(videoPath);
                    videoDuration = Math.Floor(videoInfo.Duration.TotalSeconds); // 소수점이 나오는 경우가 있다해서 버림 사용

                    // 시작시간 초로 계산
                    startTimeDuration = (Convert.ToDouble(hourTextBox.Text) * 60 * 60) + (Convert.ToDouble(minuteTextBox.Text) * 60) + Convert.ToDouble(secondTextBox.Text);
                }
                else
                {
                    MessageBox.Show("이미지로 변환할 영상이 존재하지 않습니다. 로그를 확인해주세요.", "경고");
                    return;
                }

                if (startTimeDuration >= videoDuration)
                {
                    MessageBox.Show("이미지로 변환할 영상의 길이보다 시작시간이 더 큽니다.", "경고");
                    hourTextBox.Focus();
                }
                else
                {
                    info.videoPath = videoPath;
                    info.saveImagePath = saveImagePathTextBox.Text;
                    info.saveImageName = saveImageNameTextBox.Text;
                    info.startTime = hourTextBox.Text + ":" + minuteTextBox.Text + ":" + secondTextBox.Text + ".00";
                    info.ffmpegPath = Application.StartupPath + @"\ffmpeg\bin\ffmpeg.exe";
                    info.frameRate = frameRateUpDown.Value;

                    bool result = youtubeToImages.ConvertVideoToImages(info);

                    if (result)
                    {
                        MessageBox.Show("이미지 변환 성공", "알림");
                    }
                    else
                    {
                        MessageBox.Show("이미지 변환 실패", "알림");
                    }
                }
            }
        }

        #endregion

        #region ### YoutubeAudio

        /// <summary>
        /// 찾기 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findSaveAudioPathButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                saveAudioPathTextBox.Text = dialog.FileName;
            }
        }

        /// <summary>
        /// 다운로드 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void audioDownloadButton_Click(object sender, EventArgs e)
        {
            // 폴더 경로 텍스트 및 Youtube 주소 유무 확인
            if (string.IsNullOrEmpty(saveAudioPathTextBox.Text) || string.IsNullOrEmpty(saveAudioNameTextBox.Text) || string.IsNullOrEmpty(audioYoutubeURLTextBox.Text))
            {
                MessageBox.Show("기본 기능의 입력란에 작성하지 않은 곳이 있는지 확인해주세요.", "경고");
            }
            // 음악 저장할 경로에 띄어쓰기 유무 판별
            else if (CheckWhiteSpaceInPath(saveAudioPathTextBox.Text).Count > 0)
            {
                MessageBox.Show("음악 저장할 경로에 띄어쓰기가 포함되어있습니다. 경로를 다시 한 번 확인해주세요.", "경고");
                saveAudioPathTextBox.Focus();
            }
            // 저장할 음악 이름에 띄어쓰기 유무 판별
            else if (CheckWhiteSpaceInPath(saveAudioNameTextBox.Text).Count > 0)
            {
                MessageBox.Show("저장할 음악 이름에 띄어쓰기가 포함되어있습니다. 띄어쓰기가 포함되지 않게 입력해주세요.", "경고");
                saveAudioNameTextBox.Focus();
            }
            // 음악 저장할 경로 존재 여부 판별
            else if (!IsExistDirectory(saveAudioPathTextBox.Text))
            {
                MessageBox.Show("음악 저장할 경로는 없는 경로입니다. 경로를 다시 한 번 확인해주세요.", "경고");
                saveAudioPathTextBox.Focus();
            }
            else
            {
                info.youtubedlPath = Application.StartupPath + @"\ffmpeg\bin\youtube-dl";
                info.saveAudioPath = saveAudioPathTextBox.Text + @"\" + saveAudioNameTextBox.Text + ".mp3";
                info.youtubeURL = audioYoutubeURLTextBox.Text;

                bool result = youtubeToImages.DownloadYoutubeAudio(info);

                if (result)
                {
                    MessageBox.Show("유튜브 영상에서 음원 추출 성공", "알림");
                }
                else
                {
                    MessageBox.Show("유튜브 영상에서 음원 추출 실패", "알림");
                }
            }
        }

        #endregion

        #region ### Cut Video

        /// <summary>
        /// 자르기 할 영상 찾기 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findCutVideoPathButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                cutVideoPathTextBox.Text = dialog.FileName;
            }
        }

        /// <summary>
        /// 잘라낸 영상 경로 찾기 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findCutTargetVideoPathButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                cutTargetVideoPathTextBox.Text = dialog.FileName;
            }
        }

        /// <summary>
        /// 자르기 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cutVideoButton_Click(object sender, EventArgs e)
        {
            // 자르기 할 영상 빈 칸인지 판별
            if (string.IsNullOrEmpty(cutVideoPathTextBox.Text))
            {
                MessageBox.Show("자르기 할 영상을 선택해주세요.", "경고");
                cutVideoPathTextBox.Focus();
            }
            // 잘라낸 영상 경로 빈 칸인지 판별
            else if (string.IsNullOrEmpty(cutTargetVideoPathTextBox.Text))
            {
                MessageBox.Show("잘라낸 영상 경로를 선택해주세요.", "경고");
                cutTargetVideoPathTextBox.Focus();
            }
            // 잘라낸 영상 이름 빈 칸인지 판별
            else if (string.IsNullOrEmpty(cutVideoNameTextBox.Text))
            {
                MessageBox.Show("잘라낸 영상 이름을 입력해주세요.", "경고");
                cutVideoNameTextBox.Focus();
            }
            // 시작시간 '시' 빈 칸인지 판별
            else if (string.IsNullOrEmpty(startHourTextBox.Text))
            {
                MessageBox.Show("시작시간의 '시'를 입력해주세요.", "경고");
                startHourTextBox.Focus();
            }
            // 시작시간 '분' 빈 칸인지 판별
            else if (string.IsNullOrEmpty(startMinuteTextBox.Text))
            {
                MessageBox.Show("시작시간의 '분'을 입력해주세요.", "경고");
                startMinuteTextBox.Focus();
            }
            // 시작시간 '초' 빈 칸인지 판별
            else if (string.IsNullOrEmpty(startSecondTextBox.Text))
            {
                MessageBox.Show("시작시간의 '초'를 입력해주세요.", "경고");
                startSecondTextBox.Focus();
            }
            // 종료시간 '시' 빈 칸인지 판별
            else if (string.IsNullOrEmpty(finalHourTextBox.Text))
            {
                MessageBox.Show("종료시간의 '시'를 입력해주세요.", "경고");
                finalHourTextBox.Focus();
            }
            // 종료시간 '분' 빈 칸인지 판별
            else if (string.IsNullOrEmpty(finalMinuteTextBox.Text))
            {
                MessageBox.Show("종료시간의 '분'을 입력해주세요.", "경고");
                finalMinuteTextBox.Focus();
            }
            // 종료시간 '초' 빈 칸인지 판별
            else if (string.IsNullOrEmpty(finalSecondTextBox.Text))
            {
                MessageBox.Show("종료시간의 '초'를 입력해주세요.", "경고");
                finalSecondTextBox.Focus();
            }
            // 자르기 할 영상에 빈 칸이 들어있는지 판별
            else if (CheckWhiteSpaceInPath(cutVideoPathTextBox.Text).Count > 0)
            {
                MessageBox.Show("자르기 할 영상 경로에 빈 칸이 포함되어있습니다.", "경고");
                cutVideoPathTextBox.Focus();
            }
            // 잘라낸 영상 경로에 빈 칸이 들어있는지 판별
            else if (CheckWhiteSpaceInPath(cutTargetVideoPathTextBox.Text).Count > 0)
            {
                MessageBox.Show("잘라낸 영상 경로에 빈 칸이 포함되어 있습니다.", "경고");
                cutTargetVideoPathTextBox.Focus();
            }
            // 잘라낸 영상 이름에 빈 칸이 들어있는지 판별
            else if (CheckWhiteSpaceInPath(cutVideoNameTextBox.Text).Count > 0)
            {
                MessageBox.Show("잘라낸 영상 이름에 빈 칸이 포함되어 있습니다.", "경고");
                cutVideoNameTextBox.Focus();
            }
            // 자르기 할 영상 존재 유무 판별
            else if (!IsExistFile(cutVideoPathTextBox.Text))
            {
                MessageBox.Show("자르기 할 영상이 존재하지 않습니다.", "경고");
                cutVideoPathTextBox.Focus();
            }
            // 잘라낸 영상 경로 존재 유무 판별
            else if (!IsExistDirectory(cutTargetVideoPathTextBox.Text))
            {
                MessageBox.Show("잘라낸 영상 경로가 존재하지 않습니다.", "경고");
                cutTargetVideoPathTextBox.Focus();
            }
            else
            {
                FFProbe ffProbe = new FFProbe();
                var videoInfo = ffProbe.GetMediaInfo(cutVideoPathTextBox.Text);
                Double videoDuration = Math.Floor(videoInfo.Duration.TotalSeconds); // 소수점이 나오는 경우가 있다해서 버림 사용

                // 시작시간 초로 계산
                Double startTimeDuration = (Convert.ToDouble(startHourTextBox.Text) * 60 * 60) + (Convert.ToDouble(startMinuteTextBox.Text) * 60) + Convert.ToDouble(startSecondTextBox.Text);
                Double finalTimeDuration = (Convert.ToDouble(finalHourTextBox.Text) * 60 * 60) + (Convert.ToDouble(finalMinuteTextBox.Text) * 60) + Convert.ToDouble(finalSecondTextBox.Text);

                // 시작시간이 영상 길이보다 길 때
                if (startTimeDuration >= videoDuration)
                {
                    MessageBox.Show("영상 종료시간보다 시작시간이 더 큽니다.", "경고");
                    startHourTextBox.Focus();
                }
                // 종료시간이 영상 길이보다 길 때
                else if (finalTimeDuration >= videoDuration)
                {
                    MessageBox.Show("영상 종료시간보다 종료시간이 더 큽니다.", "경고");
                    finalHourTextBox.Focus();
                }
                // 시작시간과 종료시간이 같을 때
                else if (startTimeDuration == finalTimeDuration)
                {
                    MessageBox.Show("시작시간과 종료시간이 같습니다.", "경고");
                    startHourTextBox.Focus();
                }
                // 시작시간이 종료시간보다 클 때
                else if (startTimeDuration > finalTimeDuration)
                {
                    MessageBox.Show("시작시간이 종료시간보다 큽니다.", "경고");
                    startHourTextBox.Focus();
                }
                else
                {
                    info.ffmpegPath = Application.StartupPath + @"\ffmpeg\bin\ffmpeg.exe";
                    info.cutVideoPath = cutVideoPathTextBox.Text;
                    info.startTime = startHourTextBox.Text + ":" + startMinuteTextBox.Text + ":" + startSecondTextBox.Text;
                    info.finalTime = finalHourTextBox.Text + ":" + finalMinuteTextBox.Text + ":" + finalSecondTextBox.Text;
                    info.cutTargetVideoPath = cutTargetVideoPathTextBox.Text;
                    info.cutVideoName = cutVideoNameTextBox.Text;

                    bool result = youtubeToImages.CutVideo(info);

                    if (result)
                    {
                        MessageBox.Show("영상 자르기 성공", "알림");
                    }
                    else
                    {
                        MessageBox.Show("영상 자르기 실패", "알림");
                    }
                }
            }
        }

        #endregion

        #endregion

        #region ## TextBox

        #region ### Youtube Video + Convert Images

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
                info.minute = minuteTextBox.Text;
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
                info.second = secondTextBox.Text;
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
                frameRateUpDown.Focus();
            }
            else if (frameRateUpDown.Value > 5)
            {
                MessageBox.Show("프레임 레이트는 5가 넘을 필요가 없습니다.");
                frameRateUpDown.Value = 5;
                frameRateUpDown.Focus();
            }
            else
            {
                info.frameRate = frameRateUpDown.Value;
            }
        }

        /// <summary>
        /// 프레임 레이트에 빈칸이 들어가는 경우 예외처리 하기 위해 만든 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frameRateUpDown_Validated(object sender, EventArgs e)
        {
            var value = sender as NumericUpDown;

            if (string.IsNullOrEmpty(value.Text))
            {
                MessageBox.Show("프레임 레이트에 값을 입력해주세요", "경고");
                frameRateUpDown.Focus();
            }
        }

        #endregion

        #region ### YoutubeAudio



        #endregion

        #region ### Cut Video

        /// <summary>
        /// 시 텍스트 박스에서 키보드 눌렀을 때 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartHour_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
                MessageBox.Show("숫자만 입력할 수 있습니다.", "경고");
            }
        }

        /// <summary>
        /// 분 텍스트 박스에서 키보드 눌렀을 때 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartMinute_KeyPress(object sender, KeyPressEventArgs e)
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
                minute = startMinuteTextBox.Text + e.KeyChar;

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
        private void StartSecond_KeyPress(object sender, KeyPressEventArgs e)
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
                second = startSecondTextBox.Text + e.KeyChar;

                // 60분 이상 입력 제한
                if (Convert.ToInt32(second) >= 60)
                {
                    e.Handled = true;
                    MessageBox.Show("60초 이상 입력 불가합니다.", "경고");
                }
            }
        }

        /// <summary>
        /// 시 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartHour_Leave(object sender, EventArgs e)
        {
            if (startHourTextBox.Text.Length < 2)
            {
                MessageBox.Show("00h 형태로 입력해주세요.", "경고");
                startHourTextBox.Focus();
            }
        }
        
        /// <summary>
        /// 분 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartMinute_Leave(object sender, EventArgs e)
        {
            if (startMinuteTextBox.Text.Length < 2)
            {
                MessageBox.Show("00m 형태로 입력해주세요.", "경고");
                startMinuteTextBox.Focus();
            }
        }

        /// <summary>
        /// 초 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartSecond_Leave(object sender, EventArgs e)
        {
            if (startSecondTextBox.Text.Length < 2)
            {
                MessageBox.Show("00s 형태로 입력해주세요.", "경고");
                startSecondTextBox.Focus();
            }
        }

        /// <summary>
        /// 시 텍스트 박스에서 키보드 눌렀을 때 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FinalHour_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
                MessageBox.Show("숫자만 입력할 수 있습니다.", "경고");
            }
        }

        /// <summary>
        /// 분 텍스트 박스에서 키보드 눌렀을 때 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FinalMinute_KeyPress(object sender, KeyPressEventArgs e)
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
                minute = finalMinuteTextBox.Text + e.KeyChar;

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
        private void FinalSecond_KeyPress(object sender, KeyPressEventArgs e)
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
                second = finalSecondTextBox.Text + e.KeyChar;

                // 60분 이상 입력 제한
                if (Convert.ToInt32(second) >= 60)
                {
                    e.Handled = true;
                    MessageBox.Show("60초 이상 입력 불가합니다.", "경고");
                }
            }
        }

        /// <summary>
        /// 시 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FinalHour_Leave(object sender, EventArgs e)
        {
            if (finalHourTextBox.Text.Length < 2)
            {
                MessageBox.Show("00h 형태로 입력해주세요.", "경고");
                finalHourTextBox.Focus();
            }
        }

        /// <summary>
        /// 분 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FinalMinute_Leave(object sender, EventArgs e)
        {
            if (finalMinuteTextBox.Text.Length < 2)
            {
                MessageBox.Show("00m 형태로 입력해주세요.", "경고");
                finalMinuteTextBox.Focus();
            }
        }

        /// <summary>
        /// 초 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FinalSecond_Leave(object sender, EventArgs e)
        {
            if (finalSecondTextBox.Text.Length < 2)
            {
                MessageBox.Show("00s 형태로 입력해주세요.", "경고");
                finalSecondTextBox.Focus();
            }
        }

        #endregion

        #endregion

        #region ## CheckBox & RadioButton

        #region ### Youtube Video + Convert Images

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

            // 탭 컨트롤 크기 조정
            tabControl.Size = new System.Drawing.Size(796, 563);
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

            // 탭 컨트롤 크기 조정
            tabControl.Size = new System.Drawing.Size(788, 295);
        }

        /// <summary>
        /// MP4 변환 여부 Y 라디오버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mp4YesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            info.checkedMP4 = true;
        }

        /// <summary>
        /// MP4 변환 여부 N 라디오버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mp4NoRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            info.checkedMP4 = false;
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

        #region ### YoutubeAudio

        #endregion

        #endregion

        #region ## TabControl

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabPage1)
            {
                if (yesRadioButton.Checked)
                {
                    tabControl.Size = new System.Drawing.Size(796, 563);
                }
                else if (noRadioButton.Checked)
                {
                    tabControl.Size = new System.Drawing.Size(796, 295);
                }
            }
            else if (tabControl.SelectedTab == tabPage2)
            {
                tabControl.Size = new System.Drawing.Size(796, 255);
            }
            else if (tabControl.SelectedTab == tabPage3)
            {
                tabControl.Size = new System.Drawing.Size(796, 300);
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

                // mp4변환 No 라디오 버튼 활성화
                mp4NoRadioButton.Checked = true;

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

                // MP4 변환기능 초기화
                if (info.checkedMP4)
                {
                    mp4YesRadioButton.Checked = true;
                }
                else
                {
                    mp4NoRadioButton.Checked = true;
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

        /// <summary>
        /// 파일 유무 판별 메서드
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool IsExistFile(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            return fileInfo.Exists ? true : false;
        }

        /// <summary>
        /// 폴더 유무 판별 메서드
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool IsExistDirectory(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            return directoryInfo.Exists ? true : false;
        }



        #endregion

        #region # Trash Can

        /// <summary>
        /// 유튜브 영상 다운로드
        /// </summary>
        //private void DownloadYoutubeVideo()
        //{
        //    try
        //    {
        //        // 경로 저장
        //        string exePath = Application.StartupPath + @"\ffmpeg\bin\youtube-dl";
        //        string savedVideoName = storageLocationTextBox.Text + @"\" + savedVideoFileNameTextBox.Text;
        //        //string command = exePath + " -o " + savedVideoName + " " + youtubeURLTextBox.Text;
        //        string command = exePath + " -o " + savedVideoName + " -f bestvideo+bestaudio --merge-output-format mkv " + youtubeURLTextBox.Text;

        //        // Youtube 영상 저장경로 저장 (자동 불러오기 위해서)
        //        info.saveVideoPath = storageLocationTextBox.Text;

        //        ProcessStartInfo pri = new ProcessStartInfo();
        //        Process pro = new Process();

        //        // 실행할 파일명 입력
        //        pri.FileName = "cmd.exe";

        //        // cmd 창 띄우기
        //        pri.CreateNoWindow = false; // false -> 띄우기, true -> 안 띄우기
        //        pri.UseShellExecute = false;

        //        pri.RedirectStandardInput = true;
        //        pri.RedirectStandardOutput = true;
        //        pri.RedirectStandardError = true;

        //        pro.StartInfo = pri;
        //        pro.Start();

        //        // 명령어 실행
        //        pro.StandardInput.Write(command + Environment.NewLine);
        //        pro.StandardInput.Close();

        //        // 이 코드가 있어야 제대로 유튜브 영상을 다운받을 수 있다.
        //        string result = pro.StandardOutput.ReadToEnd();

        //        // 다운로드 로그 저장
        //        log.WriteLog(result);

        //        pro.WaitForExit();
        //        pro.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("유튜브 영상을 다운받는 중 에러가 발생했습니다.", "에러발생");
        //        log.WriteLog("[ERROR] : " + ex);
        //    }
        //}

        /// <summary>
        /// 이미지 변환 메서드
        /// </summary>
        /// <param name="type"></param>
        //private void ConvertToImages(int type)
        //{
        //    // Youtube 다운 + 이미지 변환
        //    if (type == VIDEO_CONVERT_TO_IMAGES)
        //    {
        //        // 이미지로 변환할 영상 재생시간 + 시작시간 초로
        //        string videoPath = storageLocationTextBox.Text + @"\" + savedVideoFileNameTextBox.Text + ".mkv";
        //        FFProbe ffProbe;
        //        double videoDuration;
        //        double startTimeDuration;

        //        // 영상있는지 확인
        //        if (IsExistFile(videoPath))
        //        {
        //            ffProbe = new FFProbe();
        //            var videoInfo = ffProbe.GetMediaInfo(videoPath);
        //            videoDuration = Math.Floor(videoInfo.Duration.TotalSeconds); // 소수점이 나오는 경우가 있다해서 버림 사용

        //            // 시작시간 초로 계산
        //            startTimeDuration = (Convert.ToDouble(hourTextBox.Text) * 60 * 60) + (Convert.ToDouble(minuteTextBox.Text) * 60) + Convert.ToDouble(secondTextBox.Text);
        //        }
        //        else
        //        {
        //            MessageBox.Show("이미지로 변환할 영상이 존재하지 않습니다. 로그를 확인해주세요.", "오류");
        //            return;
        //        }

        //        // 저장할 이미지 이름에 띄어쓰기가 있는지 확인
        //        if (CheckWhiteSpaceInPath(saveImageNameTextBox.Text).Count > 0)
        //        {
        //            MessageBox.Show("저장할 이미지 이름에 띄어쓰기가 있으면 안됩니다.", "경고");
        //            saveImageNameTextBox.Focus();
        //        }
        //        // 이미지로 변환할 영상 길이와 시작시간 비교
        //        else if (startTimeDuration >= videoDuration)
        //        {
        //            MessageBox.Show("이미지로 변환할 영상의 길이보다 시작시간이 더 큽니다.", "경고");
        //            hourTextBox.Focus();
        //        }
        //        // 저장할 이미지 경로 빈 칸인지 확인
        //        else if (info.isCheckSaveImagePath && string.IsNullOrEmpty(saveImagePathTextBox.Text))
        //        {
        //            MessageBox.Show("저장할 이미지 경로를 입력해주세요", "경고");
        //            saveImagePathTextBox.Focus();
        //        }
        //        // 저장할 이미지 경로에 띄어쓰기 있는지 확인
        //        else if (info.isCheckSaveImagePath && CheckWhiteSpaceInPath(saveImagePathTextBox.Text).Count > 0)
        //        {
        //            MessageBox.Show("저장할 이미지 경로에 띄어쓰기가 있으면 안됩니다.", "경고");
        //            saveImagePathTextBox.Focus();
        //        }
        //        // 저장할 이미지 경로 유무 확인
        //        else if (info.isCheckSaveImagePath && !IsExistDirectory(saveImagePathTextBox.Text))
        //        {
        //            MessageBox.Show("저장할 이미지 경로가 없습니다. 다시 확인해주세요.", "경고");
        //            saveImagePathTextBox.Focus();
        //        }

        //        // 예외처리 끝
        //        else
        //        {
        //            try
        //            {
        //                // 체크박스 체크 여부에 따라서 이미지 저장 경로 저장
        //                string saveImagePath = info.isCheckSaveImagePath == true ? saveImagePathTextBox.Text : Application.StartupPath + @"\" + savedVideoFileNameTextBox.Text;
        //                saveImagePath = saveImagePath + @"\" + savedVideoFileNameTextBox.Text;
        //                // 이미지 저장할 폴더 생성
        //                DirectoryInfo di = new DirectoryInfo(saveImagePath);
        //                if (!di.Exists)
        //                {
        //                    di.Create();
        //                }

        //                saveImagePath = saveImagePath + @"\" + saveImageNameTextBox.Text;

        //                string startTime = hourTextBox.Text + ":" + minuteTextBox.Text + ":" + secondTextBox.Text + ".00";
        //                string ffmpegPath = Application.StartupPath + @"\ffmpeg\bin\ffmpeg.exe";
        //                string command = " -ss " + startTime + " -i " + videoPath + " -r " + frameRateUpDown.Text + " " + saveImagePath + "_%05d.jpg";

        //                ProcessStartInfo startInfo = new ProcessStartInfo();
        //                startInfo.CreateNoWindow = false;
        //                startInfo.UseShellExecute = false;
        //                startInfo.FileName = ffmpegPath;
        //                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        //                startInfo.Arguments = command;

        //                // 밑의 코드를 활성화시키면 버퍼에 무리가 가서 도중에 멈추게 됨
        //                //startInfo.RedirectStandardOutput = true;
        //                //startInfo.RedirectStandardError = true;

        //                using (Process process = Process.Start(startInfo))
        //                {
        //                    process.WaitForExit();
        //                }

        //                //ProcessStartInfo pri = new ProcessStartInfo();
        //                //Process pro = new Process();

        //                //// 실행할 파일명 입력
        //                //pri.FileName = "cmd.exe";

        //                //// cmd 창 띄우기
        //                //pri.CreateNoWindow = false; // false -> 띄우기, true -> 안 띄우기
        //                //pri.UseShellExecute = false;

        //                //pri.RedirectStandardInput = true;
        //                //pri.RedirectStandardOutput = true;
        //                //pri.RedirectStandardError = true;

        //                //pro.StartInfo = pri;
        //                //pro.Start();

        //                //// 명령어 실행
        //                //pro.StandardInput.Write(command + Environment.NewLine);
        //                //pro.StandardInput.Close();


        //                ////string result = pro.StandardOutput.ReadToEnd();

        //                //pro.WaitForExit();
        //                //pro.Close();

        //                //// 다운로드 로그 저장
        //                ////log.WriteLog(result);
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("이미지 변환 중 에러가 발생했습니다.", "에러");
        //                log.WriteLog("[ERROR] : " + ex);
        //            }
        //        }
        //    }
        //    // 원래 있던 영상을 이미지 변환만
        //    else if (type == ONLY_CONTVERT_TO_IMAGES)
        //    {
        //        try
        //        {
        //            string saveImagePath = saveImagePathTextBox.Text + @"\" + saveImageNameTextBox.Text;
        //            // 이미지 저장할 폴더 생성
        //            DirectoryInfo di = new DirectoryInfo(saveImagePath);
        //            if (!di.Exists)
        //            {
        //                di.Create();
        //            }

        //            saveImagePath = saveImagePath + @"\" + saveImageNameTextBox.Text;

        //            string startTime = hourTextBox.Text + ":" + minuteTextBox.Text + ":" + secondTextBox.Text + ".00";
        //            string ffmpegPath = Application.StartupPath + @"\ffmpeg\bin\ffmpeg.exe";
        //            string command = " -ss " + startTime + " -i " + videoPathTextBox.Text + " -r " + frameRateUpDown.Text + " " + saveImagePath + "_%05d.jpg";

        //            ProcessStartInfo startInfo = new ProcessStartInfo();
        //            startInfo.CreateNoWindow = false;
        //            startInfo.UseShellExecute = false;
        //            startInfo.FileName = ffmpegPath;
        //            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        //            startInfo.Arguments = command;

        //            // 밑의 코드를 활성화시키면 버퍼에 무리가 가서 도중에 멈추게 됨
        //            //startInfo.RedirectStandardOutput = true;
        //            //startInfo.RedirectStandardError = true;

        //            using (Process process = Process.Start(startInfo))
        //            {
        //                process.WaitForExit();
        //            }

        //            MessageBox.Show("이미지 변환 성공", "알림");
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("이미지 변환 중 에러가 발생했습니다.", "에러");
        //            log.WriteLog("[ERROR] : " + ex);
        //        }
        //    }
        //}

        #endregion

    }
}
