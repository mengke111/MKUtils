using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace MBX
{
  public  class ColorDebug
    {
        static List<SolidColorBrush> SolidColorBrusharray = new List<SolidColorBrush>() { Brushes.PaleVioletRed, Brushes.OrangeRed, Brushes.DarkSlateGray, Brushes.LightYellow, Brushes.Tomato, Brushes.Beige };
        static int i = 0;
        public static void ColorInit(StackPanel sp)
        {
            sp.Background = SolidColorBrusharray[i];
            i = (i + 1) % SolidColorBrusharray.Count;
        }
    }
}
