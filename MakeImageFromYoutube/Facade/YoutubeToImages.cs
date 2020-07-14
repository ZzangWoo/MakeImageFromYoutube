using MakeImageFromYoutube.Facade.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeImageFromYoutube.Facade
{
    class YoutubeToImages
    {
        #region # Property

        // 사용할 기능(클래스) 전역변수로 선언
        private YoutubeVideo youtubeVideo;
        private YoutubeAudio youtubeAudio;
        private ToImages toImages;

        #endregion

        #region # Constructor

        public YoutubeToImages()
        {
            youtubeVideo = new YoutubeVideo();
            youtubeAudio = new YoutubeAudio();
            toImages = new ToImages();
        }

        #endregion

        #region Method

        /// <summary>
        /// 유튜브 영상만 다운로드
        /// </summary>
        /// <param name="exePath"></param>
        /// <param name="saveVideoName"></param>
        /// <param name="youtubeURL"></param>
        /// <returns></returns>
        public string DownloadYoutubeVideo(string yotubedlPath, string saveVideoName, string youtubeURL)
        {
            return youtubeVideo.Download(yotubedlPath, saveVideoName, youtubeURL);
        }

        public string ConvertYoutubeToImages(string youtubedlPath, string saveVideoName, string youtubeURL, string imageFolderName, 
            string saveImagePath, string saveImageName, string videoPath, string ffmpegPath, string startTime, string frameRate)
        {
            string result = youtubeVideo.Download(youtubedlPath, saveVideoName, youtubeURL);
            if (result.Substring(1, 5) == "Error")
            {

            }
        }

        #endregion
    }
}
