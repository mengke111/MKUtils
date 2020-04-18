using MK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MBX
{
  public  class PicUtils
    {
        public static BitmapImage GetImage(string imagePath)
        {
            BitmapImage bitmap = new BitmapImage();
            if (File.Exists(imagePath))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                using (Stream ms = new MemoryStream(File.ReadAllBytes(imagePath)))
                {
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    bitmap.Freeze();
                }
            }
            return bitmap;
        }
        public enum OutFileType
        {
            Picture,
            Pdf,
            Xps
        }
        private static void addwaterprint(Canvas mainWindowMainCanvas)
        {
            TextBlock a = new TextBlock();
            a.FontSize = 18;
            a.Text = "UnOfficial";
            Canvas.SetLeft(a, mainWindowMainCanvas.Width / 2);
            Canvas.SetTop(a, mainWindowMainCanvas.Height / 2);
            mainWindowMainCanvas.Children.Add(a);
        }
        public static string ExportUI(string path, string name, Canvas mainCanvas, OutFileType mOutFileType,double MBLConfigureDPITimes,bool IsLatest)
        {
            if (!IsLatest)
            {
                addwaterprint(mainCanvas);
            }


            if (mOutFileType == OutFileType.Picture)
            {
                string tmppath = path + @"\" + name + ".bmp";

                if (MBXUtils.ExportToPic(tmppath, mainCanvas, MBLConfigureDPITimes))
                {
                    return tmppath;
                }
                else
                {
                    return null;
                }
            }
            else if (mOutFileType == OutFileType.Pdf)
            {
                string tmppdf = path + @"\" + name + ".pdf";
                if (ExportToPdf(tmppdf, mainCanvas))
                {
                    return tmppdf;
                }
                else
                {
                    return null;
                }
            }
            else if (mOutFileType == OutFileType.Xps)
            {
                string tmpxps = path + @"\" + name + ".xps";
                if (ExportToXps(tmpxps, mainCanvas))
                {
                    return tmpxps;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public static bool ExportToXps(string path, Canvas surface)
        {
            return XpsEx.GenerateXps(path, surface);
        }

        public static bool ExportToPdf(string path, Canvas surface)
        {
            try
            {
                if (path == null)
                {
                    LogHelper.Log("ExportToPdf path null");
                    return false;
                }
                ExportPdf(path, surface);
                LogHelper.Log("ExportToPdf Finish " + path);
                return true;
            }
            catch (Exception eee)
            {
                LogHelper.Log("ExportToPic Exception " + eee.ToString());
                return false;
            }
        }

        private static void ExportPdf(string path, Canvas surface)
        {
            try
            {
                PrintDialog dialog = new PrintDialog();
                if (dialog.ShowDialog() != true)
                {
                    return;
                }
                dialog.PrintVisual(surface, "IFMS Print Screen");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Print Screen", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
