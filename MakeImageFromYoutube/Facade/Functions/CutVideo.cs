using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeImageFromYoutube.Facade.Functions
{
    class CutVideo
    {
        #region # Property

        private Log log;

        #endregion

        #region # Constructor

        public CutVideo()
        {
            log = new Log();
        }

        #endregion

        #region # Method

        public bool Cut(Info info)
        {
            try
            {
                string command = " -i " + info.cutVideoPath + " -ss " + info.startTime + " -to " + info.finalTime + " " + info.cutTargetVideoPath + @"\" + info.cutVideoName + ".mkv";

                ProcessStartInfo pri = new ProcessStartInfo();
                pri.CreateNoWindow = false;
                pri.UseShellExecute = false;
                pri.FileName = info.ffmpegPath;
                pri.WindowStyle = ProcessWindowStyle.Hidden;
                pri.Arguments = command;

                using (Process pro = Process.Start(pri))
                {
                    pro.WaitForExit();
                }

                log.WriteLog("[SUCCESS] : 영상 자르기 성공");

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
