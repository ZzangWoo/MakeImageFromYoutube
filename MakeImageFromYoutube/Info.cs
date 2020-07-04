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
        /// Youtube 영상 저장경로
        /// </summary>
        public string savedVideoPath { get; set; }

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
        /// 프레임 레이트 값
        /// </summary>
        public decimal frameRate { get; set; } = 6;

        /// <summary>
        /// 변환할 동영상 경로 체크박스 체크 여부
        /// </summary>
        public bool isCheckVideoPath { get; set; } = false;

        /// <summary>
        /// 저장할 이미지 경로 체크박스 체크 여부
        /// </summary>
        public bool isCheckSaveImagePath { get; set; } = false;
    }
}
