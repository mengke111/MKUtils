using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK
{
    public class FileMK
    {
        public static int GetFileSizeK(string localPath)
        {
            FileInfo fileInfo = new FileInfo(localPath);
            return (int)(fileInfo.Length / (long)1024);
        }
    }
}
