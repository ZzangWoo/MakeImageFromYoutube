using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeImageFromYoutube.Facade.Functions
{
    class ToImages
    {
        #region # Property

        private Log log;

        #endregion

        #region # Constructor

        public ToImages()
        {
            log = new Log();
        }

        #endregion

        #region # Method

        /// <summary>
        /// 유튜브 영상 다운 후 이미지 변환
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Convert(Info info)
        {
            try
            {
                // 체크박스 체크 여부에 따라서 이미지 저장 경로 저장
                info.saveImagePath = info.saveImagePath + @"\" + info.saveImageName;
                // 이미지 저장할 폴더 생성
                DirectoryInfo di = new DirectoryInfo(info.saveImagePath);
                if (!di.Exists)
                {
                    di.Create();
                }

                info.saveImagePath = info.saveImagePath + @"\" + info.saveImageName;

                string command = " -ss " + info.startTime + " -i " + info.videoPath + " -r " + info.frameRate.ToString() + " " +info.saveImagePath + "_%05d.jpg";

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = false;
                startInfo.UseShellExecute = false;
                startInfo.FileName = info.ffmpegPath;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.Arguments = command;

                // 밑의 코드를 활성화시키면 버퍼에 무리가 가서 도중에 멈추게 됨
                //startInfo.RedirectStandardOutput = true;
                //startInfo.RedirectStandardError = true;

                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                }

                log.WriteLog("[SUCCESS] : 이미지변환 성공");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteLog("[ERROR] : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 기존에 있는 영상을 이미지로 변환
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool OnlyConvert(Info info)
        {
            try
            {
                info.saveImagePath = info.saveImagePath + @"\" + info.saveImageName;
                // 이미지 저장할 폴더 생성
                DirectoryInfo di = new DirectoryInfo(info.saveImagePath);
                if (!di.Exists)
                {
                    di.Create();
                }

                info.saveImagePath = info.saveImagePath + @"\" + info.saveImageName;

                string command = " -ss " + info.startTime + " -i " + info.videoPath + " -r " + info.frameRate.ToString() + " " + info.saveImagePath + "_%05d.jpg";

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = false;
                startInfo.UseShellExecute = false;
                startInfo.FileName = info.ffmpegPath;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.Arguments = command;

                // 밑의 코드를 활성화시키면 버퍼에 무리가 가서 도중에 멈추게 됨
                //startInfo.RedirectStandardOutput = true;
                //startInfo.RedirectStandardError = true;

                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                }

                log.WriteLog("[SUCCESS] : 이미지 변환 성공");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteLog("[ERROR] : " + ex);
                return false;
            }
        }

        #endregion
    }
}
