using MK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MK
{
    public class FilesEx
    {

        public static bool MakeFolder(string FolderPath)
        {
            try
            {
                if ((!string.IsNullOrWhiteSpace(FolderPath)))//如果不存在就创建file文件夹
                {
                    if (!Directory.Exists(FolderPath))
                    {
                       // LogHelper.Warning(FolderName + ": " + FolderPath + " not Exists");
                        Directory.CreateDirectory(FolderPath);
                    }
                    else
                    {
                      //  LogHelper.Log("【" + FolderName + "】:" + FolderPath);
                    }
                }
                else
                {
                   // LogHelper.Warning(FolderName + ":  Null");
                }
                return true;
            }
            catch (Exception ee)
            {
                //MessageBox.Show(errstr + " " + FolderName + "!!配置文件" + FolderPath + "出错" + ee.ToString());

                return false;
            }
        }

        public static bool MakeFolder(string FolderPath, string FolderName, string errstr)
        {
            try
            {
                if ((!string.IsNullOrWhiteSpace(FolderPath)))//如果不存在就创建file文件夹
                {
                    if (!Directory.Exists(FolderPath))
                    {
                        LogHelper.Warning(FolderName + ": " + FolderPath + " not Exists");
                        Directory.CreateDirectory(FolderPath);
                    }
                    else
                    {
                        LogHelper.Log("【" + FolderName + "】:" + FolderPath);
                    }
                }
                else
                {
                    LogHelper.Warning(FolderName + ":  Null");
                }
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(errstr + " " + FolderName + "!!配置文件" + FolderPath + "出错" + ee.ToString());

                return false;
            }
        }
    }
}
