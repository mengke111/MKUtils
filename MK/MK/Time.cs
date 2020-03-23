using System;

namespace XTPWPF
{
    public class Time
    {
        public static string Now()
        {
            return  System.DateTime.Now.ToString("yyyy_MM_dd");
        }
        public static string Nows()
        {
            return  System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
        }

        public static string Nowss()
        {
            return System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ssfff");
        }
    }
}