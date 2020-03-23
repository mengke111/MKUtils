using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MK
{
    public class ST
    {
        public static StackPanel GetNewStackPanelH(StackPanel spparent)
        {
            StackPanel x = new StackPanel();
            x.Orientation = Orientation.Horizontal;
            spparent.Children.Add(x);
            return x;
        }

        public static StackPanel GetNewStackPanelV(StackPanel spparent)
        {
            StackPanel x = new StackPanel();
            x.Orientation = Orientation.Vertical;
            spparent.Children.Add(x);
            return x;
        }
    }
}
