using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBX
{
    public class MesRoutin
    {
        /// <summary>
        /// 写MES信息上传文件
        /// </summary>
        /// <param name="newFilePath"></param>
        /// <param name="mProFile"></param>
        /// <param name="bmppath"></param>
        /// <param name="profilepath"></param>
        /// <param name="errorMsg"></param>
        public static void SetNewFileAndUpload(string newFilePath, string mProFileVersion, string bmppath = "", string profilepath = "", string errorMsg = "")
        {
            string tmp = "";
            tmp += "DateTime:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine;
            tmp += "AppName:" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString() + Environment.NewLine;
            tmp += "AppVersion:" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() +
                   Environment.NewLine;
           // if (mProFile != null)
            {
                tmp += "ProfileVersion:" + mProFileVersion + Environment.NewLine;
            }
            if (!string.IsNullOrWhiteSpace(profilepath))
            {
                tmp += "ProfilePath:" + profilepath;
            }
            if (!string.IsNullOrWhiteSpace(errorMsg))
            {
                tmp += "ErrorMessage:" + errorMsg;
            }
            Trace.WriteLine(tmp);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(newFilePath, false))//不追加，覆盖之前的数据
            {
                file.WriteLine(tmp);
                file.Flush();
                file.Close();
            }
            string fhFile = newFilePath + ".fh";
            if (File.Exists(fhFile))
            {
                File.Delete(fhFile);
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fhFile, true))
            {
                file.WriteLine("");// 直接追加文件末尾，换行 
                file.Flush();
                file.Close();
            }
        }
    }
}
