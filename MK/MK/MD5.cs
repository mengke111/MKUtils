using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MK
{
    public class MD5 { 
        static public string GetMD5WithFilePath(string filePath)
        {
             FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
             MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
             byte[] hash_byte = md5.ComputeHash(file);
             string str = System.BitConverter.ToString(hash_byte);
             str = str.Replace("-", "");
             return str;
         }
    }
}
