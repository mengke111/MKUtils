using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MK
{
    public class LogHelper
    {
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");//这里的 loginfo 和 log4net.config 里的名字要一样
        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");//这里的 logerror 和 log4net.config 里的名字要一样
        static string strYMD = null;
        static string path = null;
        static string ErrorStr { get; set; }
        public static TextBox gTextBox { get; private set; }

        public static void Log(string line)
        {
            if (gTextBox != null)
            {
                try
                {
                    gTextBox.Dispatcher.Invoke(new Action(delegate
                    {
                        if (gTextBox.Text.Length > 20000)
                        {
                            gTextBox.Text = line + "\r\n";
                        }
                        else
                        {
                            gTextBox.Text += line + "\r\n";
                        }
                        gTextBox.SelectionStart = gTextBox.Text.Length;
                        gTextBox.ScrollToEnd(); //自动滚动到底

                        }));
                }
                catch (Exception)
                {

                }
            }
            OnlyLog(line);
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(line);
            }

        }

        public static void Warning(string v)
        {
            Log("Warning!!! " + v);
        }

        internal static void ClearError()
        {
            ErrorStr = "";
        }

        internal static string GetErrorStr()
        {
            return ErrorStr;
        }
        public static void Error(string v)
        {
            v = "ERROR!!! " + v;
            Log(v);
            MessageBox.Show(v);
        }

        internal static void ErrorMFG(string v)
        {
            v = "ERROR!!! " + v;
            Log(v);
        }

        public static void SetListBoxLog(TextBox listBoxLog)
        {
            gTextBox = listBoxLog;
        }

        internal static void OnlyLog(string line)
        {
            line = DateTime.Now.ToString() + " " + line;
            Trace.WriteLine(line);
            strYMD = System.DateTime.Now.ToString("yyyy_MM_dd");
            path = AppDomain.CurrentDomain.BaseDirectory + @"Logs";

            if (Directory.Exists(path) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(path);
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path + @"\" + strYMD + ".txt", true))
            {
                file.WriteLine(line);// 直接追加文件末尾，换行 
            }
            if (uppath != "")
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(uppath + @"\" + strYMD + ".txt", true))
                {
                    file.WriteLine(line);// 直接追加文件末尾，换行 
                }
            }
        }

        public static void Log(string line, TextBox listBoxLog)
        {
            if (listBoxLog != null)
            {
                try
                {
                    listBoxLog.Dispatcher.Invoke(new Action(delegate
                    {
                        if (gTextBox.Text.Length > 20000)
                        {
                            listBoxLog.Text = line + "\r\n";
                        }
                        else
                        {
                            listBoxLog.Text += line + "\r\n";
                        }
                        listBoxLog.SelectionStart = listBoxLog.Text.Length;
                        listBoxLog.ScrollToEnd(); //自动滚动到底

                        }));
                }
                catch (Exception)
                {

                }
            }
            OnlyLog(line);
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(line);
            }

        }

        public static string uppath = "";

        internal static void Init(string picPath)
        {
            uppath = picPath + @"\Logs";
            if (Directory.Exists(uppath) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(uppath);
            }
        }
    }
}