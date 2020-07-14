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
    }
}
