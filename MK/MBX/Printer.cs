/*
 *=====================================================================
 * Name    : MBX
 * Author  : LCFC RD SS
 * Copyright (c) 2012 - 2019, Hefei LCFC Information Technology Co.Ltd.
 *=====================================================================
 */

using MK;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace MBX
{
    public class Printer
    {
        public static string gbmp { get; set; }
        public static string SaveAndPrint( Canvas mainWindowMainCanvas, string sNData, string mTMData, double MBLConfigureDPITimes, string MBLConfigurePicPath, bool MBLConfigureXpsPrintOn, bool MBLConfigureMFGOn,double w,double h, bool isSizeAuto, bool IsLatest)
        {
           
            string x = SaveSn(MBLConfigurePicPath, MBLConfigureDPITimes,mainWindowMainCanvas, sNData, mTMData);
            Print(x, mainWindowMainCanvas, MBLConfigureXpsPrintOn, MBLConfigureMFGOn,w , h, isSizeAuto);
            return x;
        }

       

        internal static string SaveSn(string MBLConfigurePicPath,double MBLConfigureDPITimes, Canvas mainCanvas, string sNData, string mTMData = "")
        {
            string pathMTM = MBLConfigurePicPath + @"\MTM";
            if (Directory.Exists(pathMTM) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(pathMTM);
            }
            string tmp = MBLConfigurePicPath + @"\" + sNData + ".bmp";


            if (!MBXUtils.ExportToPic(tmp, mainCanvas, MBLConfigureDPITimes))
            {
                LogHelper.Log("duplicate: " + sNData);
            }
            if (!string.IsNullOrWhiteSpace(mTMData))
            {
                string tm = pathMTM + @"\" + mTMData + ".bmp";
                if (!File.Exists(tm))
                {
                    File.Copy(tmp, tm, true);
                    LogHelper.Log("New MTM: " + sNData);
                }
            }
            return tmp;
        }

        private static void Print(string x, Canvas mainWindowMainCanvas, bool MBLConfigureXpsPrintOn, bool MBLConfigureMFGOn, double w, double h,bool IsSizeAuto)
        { //加载图片到Image对象
            try
            {
                if (MBLConfigureXpsPrintOn)
                {
                    LogHelper.Log("XpsEx PrintOn");
                    XpsEx.GenerateXps(x + ".xps", mainWindowMainCanvas);
                }
                if (MBLConfigureMFGOn || !MBLConfigureXpsPrintOn)
                {
                    LogHelper.Log("MFG PrintOn ");
                    gbmp = x;
                    System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();//Refreshpd();//
                    pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pd_PrintPage);
                    pd.Print();
                }
            }
            catch (Exception ee)
            {
                LogHelper.Log("Print Exception" + ee.ToString());
            }
        }


        public static void SaveAndPrintProjectCountry(Canvas mainWindowMainCanvas, string Project, string Country, bool MBLConfigureXpsPrintOn, bool MBLConfigureMFGOn,string MBLConfigurePicPath,double MBLConfigureDPITimes, double w, double h,bool isSizeAuto, bool IsLatest)
        {
            
            string x = SaveSn(MBLConfigurePicPath, MBLConfigureDPITimes, mainWindowMainCanvas, Project + "_" + Country + "_" + System.DateTime.Now.ToString("yyyyMMddhhmmss"));
            Print(x, mainWindowMainCanvas,  MBLConfigureXpsPrintOn,  MBLConfigureMFGOn,w,h, isSizeAuto);
        }



        internal static void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            //  e.PageSettings.PaperSize = new PaperSize("First custom size", (int)(100 * 3.937008), (int)(30 * 3.937008));
            e.Graphics.DrawImage(System.Drawing.Image.FromFile(gbmp), 0, 0);
        }
        internal static void pd_PrintPagetest(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = 1;
            float topMargin = 0;
            String line = null;
            linesPerPage = ev.MarginBounds.Height / printFont.GetHeight(ev.Graphics);
            while (count < linesPerPage &&
            ((line = streamToPrint.ReadLine()) != null))
            {
                if (count == 0)
                {
                    yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
                    ev.Graphics.DrawString(line, titleFont, System.Drawing.Brushes.Black, leftMargin + 10, yPos, new StringFormat());
                }
                else
                {
                    yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
                    ev.Graphics.DrawString(line, printFont, System.Drawing.Brushes.Black, leftMargin, yPos, new StringFormat());
                }
                count++;
            }
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }
        static private Font printFont;
        static private Font titleFont;
        static private StringReader streamToPrint;
        // static private int leftMargin = 0;
        public static void TestPrint(string MBLConfigurePrinterName)
        {
            try
            {
                streamToPrint = new StringReader("MBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL TestMBL Test");
                printFont = new Font("宋体", 10);
                titleFont = new Font("宋体", 15);
                PaperSize pageSize = new PaperSize("First custom size", (int)(100 * 960 / 2.42), (int)(30 * 960 / 2.42));
                System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
                pd.DefaultPageSettings.PaperSize = pageSize;
                LogHelper.Log("PrintName： " + MBLConfigurePrinterName);
                if (!string.IsNullOrWhiteSpace(MBLConfigurePrinterName))
                {
                    pd.PrinterSettings.PrinterName = MBLConfigurePrinterName;
                }
                pd.DocumentName = pd.PrinterSettings.MaximumCopies.ToString();
                pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pd_PrintPagetest);

                pd.PrintController = new System.Drawing.Printing.StandardPrintController();
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }


}