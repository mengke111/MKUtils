using MK;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace MBX
{
    public class SelfRAR
    {

        public static bool RAR(string fullPath, string folder, Version thisversion, Version fileversopm)
        {

      
            //  string fullPath = @"C:\MBL\MBLUpgrade\MBLpack.exe";
            if (fileversopm > thisversion)
            {
                LogHelper.Log("fullPath+ folder+ thisversion+ fileversopm" + fullPath + folder + thisversion + fileversopm);
                Process p = new Process();
                p.StartInfo.FileName = fullPath;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WorkingDirectory = folder;
                p.Start();
                p.WaitForExit();
                return true;
            }
            else
            {
                return false;
            }

           
        }
    }
}