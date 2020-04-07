using MK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MBX
{
  public  class UIMainUtils
    {
        public static void AddTextBlock(List<TextBlock> mTextBlockList, StackPanel sp)
        {
            foreach (UIElement element in sp.Children)
            {
                if (element is TextBlock)
                {
                    TextBlock current = ((TextBlock)element);
                    mTextBlockList.Add((TextBlock)element);
                }
            }
        }

        public static void BMWorkSVG(DrawingImage bm, double qRWH, StackPanel spLower, double topmargin)
        {
            if (bm != null)
            {
                Image image = new Image();
                image.Source = bm;
                image.Width = qRWH;
                image.Height = qRWH;
                image.Margin = new Thickness(4, topmargin, 0, 0);
                ImageBrush ib = new ImageBrush();
                ib.ImageSource = image.Source;
                ib.Stretch = Stretch.Uniform;

                StackPanel x = GetNewStackPanelV(spLower);
                StackPanel x1 = GetNewStackPanelH(x);
                x1.Height = topmargin;
                // x1.Background = Brushes.IndianRed;
                x.Children.Add(image);
            }
        }


        public static TextBlock StrWork(string v, double fontSize, StackPanel mStackPanel)
        {
            TextBlock a = new TextBlock();
            a.FontSize = fontSize;
            a.Text = v;
            mStackPanel.Children.Add(a);
            return a;
        }

        public static TextBlock StrWorkWithMargin(string v, double fontSize, StackPanel mStackPanel, Thickness thickness)
        {
            TextBlock a = StrWork(v, fontSize, mStackPanel);
            a.Margin = thickness;
            return a;
        }

        public static TextBlock StrWorkVCenter(string v, double fontSize, StackPanel mStackPanel)
        {
            TextBlock a = StrWork(v, fontSize, mStackPanel);
            a.VerticalAlignment = VerticalAlignment.Center;
            return a;
        }

        public static TextBlock StrWorkTextCenter(string v, double fontSize, StackPanel mStackPanel)
        {
            TextBlock a = StrWork(v, fontSize, mStackPanel);
            a.TextAlignment = TextAlignment.Center;
            return a;
        }

        public static Border GetNewBorder(int v, StackPanel mStackPanel)
        {
            Border mBorder = new Border();
            mBorder.BorderBrush = new SolidColorBrush(Colors.Black);
            mBorder.BorderThickness = new Thickness(v);
            mStackPanel.Children.Add(mBorder);
            return mBorder;
        }

        public static void StrBorderWork(string v, double fontSize, Border mBorder)
        {
            TextBlock a = new TextBlock();
            a.FontWeight = FontWeights.Bold;
            a.FontSize = fontSize;
            a.Text = v;
            mBorder.Child = a;
        }

        public static StackPanel GetNewStackPanelHWithMargin(StackPanel spparent, Thickness thickness)
        {
            StackPanel x = new StackPanel();
            x.Orientation = Orientation.Horizontal;
            x.Margin = thickness;
            spparent.Children.Add(x);
            return x;
        }

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

        public static void SmallIconWork(BitmapImage asyncsfdfElec, StackPanel mStackPanel)
        {
            Image image = new Image();
            image.Source = asyncsfdfElec;
            image.Margin = new Thickness(3, 0, 0, 0);
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = image.Source;
            ib.Stretch = Stretch.None;
            mStackPanel.Children.Add(image);
        }

        public static void SmallIconWorkWithMargin(BitmapImage asyncsfdfElec, double fontSize, StackPanel mStackPanel, Thickness thickness)
        {
            Image image = new Image();
            image.Source = asyncsfdfElec;
            image.Margin = thickness;
            image.Height = fontSize;
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = image.Source;
            ib.Stretch = Stretch.None;
            mStackPanel.Children.Add(image);
        }

        public static void StrWorkRight(string v, double fontSize, StackPanel mStackPanel)
        {
            TextBlock a = new TextBlock();
            a.HorizontalAlignment = HorizontalAlignment.Center;
            a.FontSize = fontSize;
            a.Text = v;
            a.TextAlignment = TextAlignment.Right;
            mStackPanel.Children.Add(a);
        }

        public static TextBlock StrWorkHCenter(string v, double fontSize, StackPanel mStackPanel)
        {
            TextBlock a = StrWork(v, fontSize, mStackPanel);
            a.HorizontalAlignment = HorizontalAlignment.Center;
            return a;
        }

        public static TextBlock StrModelWorkWithNull(string mModelCode, double fontSize, StackPanel mStackPanel)
        {
            TextBlock a = null;
            if (mModelCode == null || mModelCode == "")
            {
                a = StrWork(mModelCode, fontSize, mStackPanel);
            }
            else
            {
                a = StrWork("", fontSize, mStackPanel);
            }
            return a;
        }

        public static TextBlock StrModelWork(string mModelCode, double fontSize, StackPanel mStackPanel)
        {
            TextBlock a = null;

            a = StrWork(mModelCode, fontSize, mStackPanel);
            return a;
        }

        public static void ModelCodeWork(string mModelCode, double size, StackPanel sp)
        {
          
            {
                StrWork("    " + mModelCode, size, sp);
            }
        }

        public static void SystemWork(string str, double size, StackPanel mStackPanelV2)
        {
            if (str != null)
            {
                StrWork("    " + str, size, mStackPanelV2);
            }
            else
            {
                MessageBox.Show("No SystemWork str");
            }
        }

        public static StackPanelMBX GetNewStackPanelMBXV(StackPanel spparent)
        {
            StackPanelMBX x = new StackPanelMBX();
            x.Orientation = Orientation.Vertical;
            spparent.Children.Add(x);
            return x;
        }

        public static StackPanelMBX GetNewStackPanelMBXH(StackPanel spparent)
        {
            StackPanelMBX x = new StackPanelMBX();
            x.Orientation = Orientation.Horizontal;
            spparent.Children.Add(x);
            return x;
        }
        public class StackPanelMBX : StackPanel
        {
        }

        public static void NullCheck(TextBlock mTextBlock)
        {
            if (!String.IsNullOrWhiteSpace(mTextBlock.Text))
            {
                mTextBlock.Text += "   ";
            }
        }

        public static double iconadd(BitmapImage bitmapImage, StackPanel mWrapPanel1)
        {
            Image image = new Image();
            image.Source = bitmapImage;
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = image.Source;
            ib.Stretch = Stretch.Uniform;
            image.Margin = new Thickness(0, 0, MBXUtils.Type, 0);
            mWrapPanel1.Children.Add(image);
            return bitmapImage.Width;
        }

        internal static void Error_(string v)
        {
           
                LogHelper.Error(v);

        }
    }
}
