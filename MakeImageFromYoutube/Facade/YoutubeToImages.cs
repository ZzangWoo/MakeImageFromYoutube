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

        #region # Method

        /// <summary>
        /// 유튜브 영상만 다운로드
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool DownloadYoutubeVideo(Info info)
        {
            return youtubeVideo.Download(info);
        }

        /// <summary>
        /// 유튜브 영상 다운 후 이미지 변환
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool ConvertYoutubeToImages(Info info)
        {
            return toImages.Convert(info);
        }

        /// <summary>
        /// 기존에 있는 영상을 이미지로 변환
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool ConvertVideoToImages(Info info)
        {
            return toImages.OnlyConvert(info);
        }

        /// <summary>
        /// 유튜브 음원만 다운로드
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool DownloadYoutubeAudio(Info info)
        {
            return youtubeAudio.Download(info);
        }

        #endregion
    }
}
