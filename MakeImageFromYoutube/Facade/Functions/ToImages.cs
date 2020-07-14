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
        public string Convert(string imageFolderName, string saveImagePath, string saveImageName, string videoPath, string ffmpegPath, string startTime, string frameRate)
        {
            try
            {
                // 체크박스 체크 여부에 따라서 이미지 저장 경로 저장
                saveImagePath = saveImagePath + @"\" + saveImageName;
                // 이미지 저장할 폴더 생성
                DirectoryInfo di = new DirectoryInfo(saveImagePath);
                if (!di.Exists)
                {
                    di.Create();
                }

                saveImagePath = saveImagePath + @"\" + saveImageName;

                string command = " -ss " + startTime + " -i " + videoPath + " -r " + frameRate + " " + saveImagePath + "_%05d.jpg";

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = false;
                startInfo.UseShellExecute = false;
                startInfo.FileName = ffmpegPath;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.Arguments = command;

                // 밑의 코드를 활성화시키면 버퍼에 무리가 가서 도중에 멈추게 됨
                //startInfo.RedirectStandardOutput = true;
                //startInfo.RedirectStandardError = true;

                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                }

                return "Success";
            }
            catch (Exception ex)
            {
                return "[Error] : " + ex;
            }
        }
    }
}
