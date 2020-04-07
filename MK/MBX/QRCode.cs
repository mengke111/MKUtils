/*
 *=====================================================================
 * Name    : MBX
 * Author  : LCFC RD SS
 * Copyright (c) 2012 - 2019, Hefei LCFC Information Technology Co.Ltd.
 *=====================================================================
 */
using MBX;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;


using System.Linq;

using System.Threading.Tasks;

namespace MBX
{
  public  class QRCode
    {
        public static System.Windows.Media.DrawingImage WorkForSVG(string text,double MBLConfigureQRWH)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.H);
            XamlQRCode qrCode = new XamlQRCode(qrCodeData);
            System.Windows.Size size = new System.Windows.Size((int)MBLConfigureQRWH * MBXUtils.Type, (int)MBLConfigureQRWH * MBXUtils.Type);
            System.Windows.Media.DrawingImage qrCodeAsXaml = qrCode.GetGraphic(size, false);
            return qrCodeAsXaml;
        }

        public static BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            Bitmap bitmapSource = new Bitmap(bitmap.Width, bitmap.Height);
            int i, j;
            for (i = 0; i < bitmap.Width; i++)
                for (j = 0; j < bitmap.Height; j++)
                {
                    Color pixelColor = bitmap.GetPixel(i, j);
                    Color newColor = Color.FromArgb(pixelColor.R, pixelColor.G, pixelColor.B);
                    bitmapSource.SetPixel(i, j, newColor);
                }
            MemoryStream ms = new MemoryStream();
            bitmapSource.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(ms.ToArray());
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}
