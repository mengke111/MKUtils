/*
 *=====================================================================
 * Name    : MBL
 * Author  : LCFC RD SS
 * Copyright (c) 2012 - 2019, Hefei LCFC Information Technology Co.Ltd.
 *=====================================================================
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MK;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MBX
{
    public class MBXUtils
    {
        public static double Type = 3.7808;
        public static void SetQRCodeImage(DrawingImage mBMPDataQRDrawingImage, Canvas mainCanvas, Image QRImage, double MBLConfigureQRWH, double MBLConfigureQRRight, double MBLConfigureBigLeft, double MBLConfigureQRDown, double MBLConfigureBigDown)
        {
            if (mBMPDataQRDrawingImage != null)
            {
                QRImage.Source = mBMPDataQRDrawingImage;
                QRImage.Width = MBLConfigureQRWH * MBXUtils.Type;
                QRImage.Height = MBLConfigureQRWH * MBXUtils.Type;
                ImageBrush ib = new ImageBrush();
                ib.ImageSource = QRImage.Source;
                ib.Stretch = Stretch.Uniform;
                Canvas.SetLeft(QRImage, mainCanvas.Width - MBLConfigureQRRight * MBXUtils.Type - MBLConfigureBigLeft * MBXUtils.Type);
                Canvas.SetTop(QRImage, mainCanvas.Height - MBLConfigureQRDown * MBXUtils.Type - MBLConfigureBigDown * MBXUtils.Type);
            }
        }

        public static void SetSN(FontFamily fontFamily, Canvas mainCanvas, string sSN,string ConstStrSN, double MBLConfigureSNFontSize, double MBLConfigureSNRight, double MBLConfigureSNDown,double SNWidth)
        {
            if (!string.IsNullOrWhiteSpace(sSN))
            {
                StackPanel xStackPanel = new StackPanel();
                xStackPanel.Orientation = Orientation.Vertical;
                xStackPanel.Width = SNWidth;// MBLConfigure.SNLength * Utils.Type;
                TextBlock a = new TextBlock();
                a.Text = ConstStrSN + sSN;
                a.TextAlignment = TextAlignment.Right;
                a.FontFamily = fontFamily;
                a.FontSize = MBLConfigureSNFontSize;
                xStackPanel.Children.Add(a);
                Canvas.SetLeft(xStackPanel, mainCanvas.Width - xStackPanel.Width - MBLConfigureSNRight * MBXUtils.Type);
                Canvas.SetTop(xStackPanel, mainCanvas.Height - MBLConfigureSNDown * MBXUtils.Type);
                mainCanvas.Children.Add(xStackPanel);
            }
        }

        public static bool ExportToPic(string tmp, Canvas surface,double MBLConfigureDPITimes)
        {

            Uri xUri = new Uri(tmp, UriKind.Absolute);
            try
            {
                if (xUri == null)
                {
                    LogHelper.Log("ExportToPic path null");
                    return false;
                }
                double x = MBLConfigureDPITimes;
                Size size = new Size(surface.ActualWidth, surface.ActualHeight);
                surface.Measure(size);
                surface.Arrange(new Rect(size));
                RenderTargetBitmap renderBitmap =
                  new RenderTargetBitmap(
                     (int)(surface.ActualWidth * x),
                    (int)(surface.ActualHeight * x),
                    96 * x,
                    96 * x,
                    PixelFormats.Pbgra32);

                renderBitmap.Render(surface);

                using (FileStream outStream = new FileStream(xUri.LocalPath, FileMode.Create))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                    encoder.Save(outStream);
                }
                LogHelper.Log(" ExportToPic Finish " + xUri);
                return true;
            }
            catch (Exception eee)
            {
                LogHelper.Log(" ExportToPic Exception " + eee.ToString());
                return false;
            }
        }

        public static string CombineImages(List<string> files, string toPath, string gProFiledata)
        {

            int fontheight = (int)(20* Type);
            //change the location to store the final image.
            var finalImage = toPath;
            var imgs = files.Select(f => System.Drawing.Image.FromFile(f));
            var finalWidth =
                imgs.Max(img => img.Width);
            var finalHeight =
                imgs.Sum(img => (img.Height + fontheight));
            var finalImg = new System.Drawing.Bitmap(finalWidth, finalHeight);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImg);
            g.Clear(System.Drawing.SystemColors.AppWorkspace);

            var width = finalWidth;
            var height = finalHeight;
            var nIndex = 0;
            foreach (string file in files)
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(file);
                if (nIndex == 0)
                {
                    g.DrawString("Info: " + gProFiledata, new System.Drawing.Font("Verdana", 20), System.Drawing.Brushes.Red, 0, 0);
                    g.DrawImage(img, new System.Drawing.Rectangle(0, fontheight, img.Width, img.Height));
                    g.DrawString(file, new System.Drawing.Font("Verdana", 20), System.Drawing.Brushes.Black, 0, fontheight + img.Height);
                    nIndex++;
                    width = img.Width;
                    height = img.Height + fontheight + fontheight;
                }
                else
                {
                    g.DrawImage(img, new System.Drawing.Rectangle(0, height, img.Width, img.Height));
                    g.DrawString(file, new System.Drawing.Font("Verdana", 20), System.Drawing.Brushes.Black, 0, height + img.Height);
                    height += img.Height + fontheight;
                }
                img.Dispose();
            }
            g.Dispose();
            finalImg.Save(finalImage, System.Drawing.Imaging.ImageFormat.Png);
            finalImg.Dispose();
            LogHelper.Log("大图已经生成: " + toPath);
            OpenPic(toPath);
            return "大图已经生成: " + toPath;
        }
        public static void OpenPic(string fileName)
        {
            try
            {
                Process.Start(fileName);
            }
            catch (Exception)
            {
                try
                {
                    string arg =
                        string.Format(
                            "\"{0}\\Windows Photo Viewer\\PhotoViewer.dll\", ImageView_Fullscreen  {1} ",
                            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
                            , fileName);
                    var dllExe = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System),
                        "rundll64.exe");
                    // LogHelper.WriteLog(string.Format("调用系统默认的图片查看器打开图片，参数为：{0} {1}", dllExe, arg));
                    System.Diagnostics.Process.Start(dllExe, arg);
                }
                catch (Exception)
                {
                    //打开文件夹并选中文件
                    var argment = string.Format(@"/select,""{0}""", fileName);
                    System.Diagnostics.Process.Start("Explorer", argment);
                }
            }
        }
       static double gtypes = 1;
        public static void ZoomInit(Canvas MainCanvas, Label ZoomPercent) {
            TransformGroup tg = MainCanvas.RenderTransform as TransformGroup;
            if (tg == null)
                tg = new TransformGroup();

            tg.Children.Add(new ScaleTransform(1 / gtypes, 1 / gtypes, MainCanvas.Width / 2, MainCanvas.Height / 2));
            gtypes = 1;
            MainCanvas.RenderTransform = tg;
            ZoomPercent.Content = Math.Round(gtypes * 100, 2).ToString() + "%";
        }
        public static void Zoom(double x, object CBCountrySelectedValue, Canvas MainCanvas,Label ZoomPercent) {
            if (CheckValue(CBCountrySelectedValue))
            {
                TransformGroup tg = MainCanvas.RenderTransform as TransformGroup;
                if (tg == null)
                    tg = new TransformGroup();
                gtypes *= (x);
                tg.Children.Add(new ScaleTransform(x, x, MainCanvas.Width / 2, MainCanvas.Height / 2));
                MainCanvas.RenderTransform = tg;
                ZoomPercent.Content = Math.Round(gtypes * 100, 2).ToString() + "%";
            }
        }

        public static bool CheckValue(object selectedValue)
        {
            if (selectedValue != null && !string.IsNullOrWhiteSpace(selectedValue.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
