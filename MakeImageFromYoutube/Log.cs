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
        string infoFile = "info.csv";

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
            string infoPath = Application.StartupPath + "\\";

            try
            {
                using (StreamWriter streamWriter = new StreamWriter(infoPath + infoFile, false, Encoding.UTF8))
                {
                    streamWriter.WriteLine("{0},{1}", "checkedAdvanced", info.checkedAdvanced);
                    streamWriter.WriteLine("{0},{1}", "hour", info.hour);
                    streamWriter.WriteLine("{0},{1}", "minute", info.minute);
                    streamWriter.WriteLine("{0},{1}", "second", info.second);
                    streamWriter.WriteLine("{0},{1}", "frameRate", info.frameRate);
                    streamWriter.WriteLine("{0},{1}", "isCheckVideoPath", info.isCheckVideoPath);
                    streamWriter.WriteLine("{0},{1}", "isCheckSaveImagePath", info.isCheckSaveImagePath);
                }
            }
            catch (IOException iOException)
            {
                MessageBox.Show("info 파일이 현재 실행중입니다. 기입정보가 제대로 저장되지 않습니다.", "오류");
                WriteLog("[ERROR] info 파일 실행 중...");
            }
            catch (Exception e)
            {
                MessageBox.Show("기입정보를 저장하는 도중 오류가 발생했습니다.", "오류");
                WriteLog("[ERROR] : " + e);
            }
        }

        /// <summary>
        /// 기입정보 가져오기
        /// </summary>
        /// <returns></returns>
        public Info GetInfoLog()
        {
            Info info = new Info();
            string infoPath = Application.StartupPath + "\\";
            
            try
            {
                using (StreamReader streamReader = new StreamReader(infoPath + infoFile))
                {
                    string list = string.Empty;
                    while ((list = streamReader.ReadLine()) != null)
                    {
                        string[] listArray = list.Split(',');

                        switch (listArray[0])
                        {
                            case "checkedAdvanced":
                                info.checkedAdvanced = listArray[1] == "True" ? true : false;
                                break;
                            case "hour":
                                info.hour = listArray[1];
                                break;
                            case "minute":
                                info.minute = listArray[1];
                                break;
                            case "second":
                                info.second = listArray[1];
                                break;
                            case "frameRate":
                                info.frameRate = Convert.ToDecimal(listArray[1]);
                                break;
                            case "isCheckVideoPath":
                                info.isCheckVideoPath = listArray[1] == "True" ? true : false;
                                break;
                            case "isCheckSaveImagePath":
                                info.isCheckSaveImagePath = listArray[1] == "True" ? true : false;
                                break;
                        }
                    }
                }
            }
            catch (IOException iOException)
            {
                MessageBox.Show("info 파일이 현재 실행중입니다. info 파일을 종료하시고 다시 프로그램을 실행해주세요.", "오류");
                WriteLog("[ERROR] info 파일 실행 중...");
            }
            catch (Exception e)
            {
                MessageBox.Show("기입 정보를 가져오는 중 오류가 발생했습니다.", "오류");
                WriteLog("[ERROR] : " + e);
            }

            return info;
        }
    }
}
