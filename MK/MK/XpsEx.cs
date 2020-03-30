using System;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
namespace MK
{
  public  class XpsEx
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

        internal static bool GenerateXps(string path, Canvas canvas)
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
               
            }
        }
    }
}
