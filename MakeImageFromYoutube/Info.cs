using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeImageFromYoutube
{
    public class Info
    {
        /// <summary>
        /// 고급기능 선택 유무
        /// 선택 O => true
        /// 선택 X => false
        /// </summary>
        public bool checkedAdvanced { get; set; } = false;

        /// <summary>
        /// 고급기능 -> 시
        /// </summary>
        public string hour { get; set; } = "00";

        /// <summary>
        /// 고급기능 -> 분
        /// </summary>
        public string minute { get; set; } = "00";

        /// <summary>
        /// 고급기능 -> 초
        /// </summary>
        public string second { get; set; } = "00";

        /// <summary>
        /// 변환할 동영상 경로 체크박스 체크 여부
        /// </summary>
        public bool isCheckVideoPath { get; set; } = false;

        /// <summary>
        /// 저장할 이미지 경로 체크박스 체크 여부
        /// </summary>
        public bool isCheckSaveImagePath { get; set; } = false;

        #region # Facade Property

        #region ## For Download

        /// <summary>
        /// Youtube-dl.exe 경로
        /// </summary>
        public string youtubedlPath { get; set; }

        /// <summary>
        /// Youtube 영상 저장경로
        /// </summary>
        public string saveVideoPath { get; set; }

        /// <summary>
        /// Youtube 영상 URL
        /// </summary>
        public string youtubeURL { get; set; }

        #endregion

        #region ## For Convert

        /// <summary>
        /// 이미지 폴더 이름
        /// </summary>
        public string imageFolderName { get; set; }

        /// <summary>
        /// 저장될 이미지 경로
        /// </summary>
        public string saveImagePath { get; set; }

        /// <summary>
        /// 저장될 이미지 이름
        /// </summary>
        public string saveImageName { get; set; }

        /// <summary>
        /// 이미지로 변환할 영상 경로
        /// </summary>
        public string videoPath { get; set; }

        /// <summary>
        /// FFMPEG 경로
        /// </summary>
        public string ffmpegPath { get; set; }

        /// <summary>
        /// 이미지 변환할 영상 시작시간
        /// </summary>
        public string startTime { get; set; }

        /// <summary>
        /// 프레임 레이트 값
        /// </summary>
        public decimal frameRate { get; set; } = 1;

        #endregion

        #region ## For Audio

        /// <summary>
        /// 음악 저장할 경로
        /// </summary>
        public string saveAudioPath { get; set; }

        /// <summary>
        /// 저장할 음악 이름
        /// </summary>
        public string saveAudioName { get; set; }

        #endregion

        #region ## For Cut Video

        /// <summary>
        /// 자르기 할 영상 경로
        /// </summary>
        public string cutVideoPath { get; set; }

        /// <summary>
        /// 잘라낸 영상 경로
        /// </summary>
        public string cutTargetVideoPath { get; set; }

        /// <summary>
        /// 잘라낸 영상 이름
        /// </summary>
        public string cutVideoName { get; set; }

        /// <summary>
        /// 잘라낼 영상 종료시간
        /// </summary>
        public string finalTime { get; set; }

        #endregion

        #endregion
    }
}
