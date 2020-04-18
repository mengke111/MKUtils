/*
 *=====================================================================
 * Name    : MBL
 * Author  : LCFC RD SS
 * Copyright (c) 2012 - 2019, Hefei LCFC Information Technology Co.Ltd.
 *=====================================================================
 */

using MK;
using System;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;


namespace MBX
{
    public class XpsEx
    {
        public static void ExportXps(Uri path, FrameworkElement surface)
        {
            if (path == null) return;
            Transform transform = surface.LayoutTransform;
            surface.LayoutTransform = null;
            Size size = new Size(surface.Width, surface.Height);
            surface.Measure(size);
            surface.Arrange(new Rect(size));
            Package package = Package.Open(path.LocalPath, FileMode.Create);
            XpsDocument doc = new XpsDocument(package);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
            writer.Write(surface);
            doc.Close();
            package.Close();
            surface.LayoutTransform = transform;
        }

        public static bool GenerateXps(string path, Canvas canvas)
        {
            if (true)
            {
                try
                {
                    if (path == null)
                    {
                        LogHelper.Log(" ExportToXps path == null");
                        return false;
                    }
                    ExportXps(new Uri(path, UriKind.Absolute), canvas);
                    LogHelper.Log(" ExportToXps Finish " + path);
                    //PdfDocument doc = new PdfDocument();
                    //doc.LoadFromFile(path, FileFormat.XPS);
                    //doc.SaveToFile(path+".pdf", FileFormat.PDF);
                    //LogHelper.Log(" ExportTopdf Finish " + path);
                    return true;
                }
                catch (Exception eee)
                {
                    LogHelper.Log(" ExportToPic Exception " + eee.ToString());
                    return false;
                }
            }
            else
            {
                //新建PDF文档，添加一页
                //PdfDocument doc = new PdfDocument();
                //PdfPageBase page = doc.Pages.Add();
                //doc.Pages.RemoveAt(0);
                //page = doc.Pages.Add();
                //page.BackgroundColor = System.Drawing.Color.White;
                ////加载图片到PdfImage对象
                //Bitmap image = (Bitmap)Image.FromFile(x);
                //PdfImage pdfImage = PdfImage.FromImage(image);
                //page.Canvas.DrawImage(pdfImage, 0, 0);
                ////保存文档
                //doc.SaveToFile(x + ".pdf");
                //image.Dispose();
                //doc.Close();
                //return true;
            }
        }
    }
}
