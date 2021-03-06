﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeImageFromYoutube.Facade.Functions
{
    class YoutubeAudio
    {
        #region # Property

        private Log log;

        #endregion

        #region # Constructor

        public YoutubeAudio()
        {
            log = new Log();
        }

        #endregion

        #region # Method

        public bool Download(Info info)
        {
            try
            {
                string command = " -o " + info.saveAudioPath + " -i --extract-audio --audio-format mp3 --audio-quality 0 " + info.youtubeURL;

                ProcessStartInfo pri = new ProcessStartInfo();
                pri.CreateNoWindow = false;
                pri.UseShellExecute = false;
                pri.FileName = info.youtubedlPath;
                pri.WindowStyle = ProcessWindowStyle.Hidden;
                pri.Arguments = command;

                using (Process pro = Process.Start(pri))
                {
                    pro.WaitForExit();
                }

                log.WriteLog("[SUCCESS] : 유튜브 영상에서 음원 추출 성공");

                return true;

                //// 실행할 파일명 입력
                //pri.FileName = "cmd.exe";

                //// cmd 창 띄우기
                //pri.CreateNoWindow = false; // false -> 띄우기, true -> 안 띄우기
                //pri.UseShellExecute = false;

                //pri.RedirectStandardInput = true;
                //pri.RedirectStandardOutput = true;
                //pri.RedirectStandardError = true;

                //pro.StartInfo = pri;
                //pro.Start();

                //// 명령어 실행
                //pro.StandardInput.Write(command + Environment.NewLine);
                //pro.StandardInput.Close();

                //// 이 코드가 있어야 제대로 유튜브 영상을 다운받을 수 있다.
                //string result = pro.StandardOutput.ReadToEnd();

                //pro.WaitForExit();
                //pro.Close();

                //log.WriteLog(result);
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
