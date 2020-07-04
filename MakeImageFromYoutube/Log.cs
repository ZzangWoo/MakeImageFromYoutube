using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MakeImageFromYoutube
{
    public class Log
    {
        /// <summary>
        /// 로그 폴더 생성 메서드
        /// </summary>
        public void CreateLogFolder()
        {
            string logPath = Application.StartupPath + "\\Logs";
            DirectoryInfo di = new DirectoryInfo(logPath);
            if (!di.Exists)
            {
                di.Create();
            }
        }

        /// <summary>
        /// 로그 작성
        /// </summary>
        /// <param name="message"></param>
        public void WriteLog(string message)
        {
            string logPath = Application.StartupPath + "\\Logs\\";
            DirectoryInfo di = new DirectoryInfo(logPath);
            if (!di.Exists)
            {
                di.Create();
            }
            else
            {
                string logFileName = DateTime.Now.ToString("yyyyMMdd");

                using (StreamWriter streamWriter = new StreamWriter(logPath + "[" + logFileName + "]log.txt", true, Encoding.UTF8))
                {
                    streamWriter.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]");
                    streamWriter.WriteLine(message);
                    streamWriter.WriteLine();
                }
            }
        }

        /// <summary>
        /// 기입정보 저장
        /// </summary>
        /// <param name="info"></param>
        public void CreateInfoLog(Info info)
        {
            
        }

        /// <summary>
        /// 기입정보 가져오기
        /// </summary>
        /// <returns></returns>
        public Info GetInfoLog()
        {
            Info info = new Info();
            return info;
        }
    }
}
