using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Net;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static MBX.UIMainUtils;

namespace MBX
{
   public class SizeCheck
    {
        public static bool CheckSize(StackPanel spBig, StackPanel spSpecial, double width, double height, Image qRImage)
        {
           

            if (qRImage != null && spSpecial != null)
            {
                GeneralTransform generalTransform = qRImage.TransformToVisual(Application.Current.MainWindow as UIElement);
                Point point = generalTransform.Transform(new Point(0, 0));
                Rect qRImagerect = new Rect(point, new Size(qRImage.ActualWidth, qRImage.ActualHeight));
                //  LogHelper.Log("qRImage相对于屏幕原点的位置：" + qRImagerect.ToString());
                Cul(spSpecial, qRImagerect);
            }

            spBig.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            if (spBig.DesiredSize.Width > width)
            {
                spBig.Background = Brushes.IndianRed;
                string x = " Need more " + ((spBig.RenderSize.Width - width) / (3.8) + 0.5) + "mm";
                UIMainUtils.Error_("Width Too lang!!! Design:" + width + " Now:" + spBig.RenderSize.Width + x);
                return false;
            }
            if (spBig.DesiredSize.Height > height)
            {
                spBig.Background = Brushes.IndianRed;
                string x = " Need more " + ((spBig.RenderSize.Height - height) / (3.8) + 0.5) + "mm";
                UIMainUtils.Error_("Height Too lang!!! Design:" + height + " Now:" + spBig.RenderSize.Height + x);
                return false;
            }
            return true;
        }
        private static void Cul(StackPanel sp, Rect qRImagerect)
        {
            foreach (UIElement itemx in sp.Children)
            {
                if (itemx is StackPanelMBX)
                {
                    GeneralTransform generalTransform = itemx.TransformToVisual(Application.Current.MainWindow as UIElement);
                    Point point = generalTransform.Transform(new Point(0, 0));
                    Rect rect = new Rect(point, new Size(((FrameworkElement)itemx).ActualWidth, ((FrameworkElement)itemx).ActualHeight));
                    if (rect.IntersectsWith(qRImagerect))
                    {
                        ColorDebug.ColorInit((StackPanel)itemx);
                        UIMainUtils.Error_(qRImagerect.ToString() + "  IntersectsWith" + rect.ToString());
                    }
                }
                else if (itemx is StackPanel)
                {
                    Cul((StackPanel)itemx, qRImagerect);
                }
            }
        }
    }
}
