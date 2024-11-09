using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace TallerEF
{
    public static class BitmapImageConverter
    {
        public static BitmapImage Convert(byte[] byteArray)
        {
            if (byteArray == null)
            {
                return new BitmapImage();
            }
            using (var ms = new MemoryStream(byteArray))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = ms;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                return image;
            }
        }

        public static byte[] Convert(string filePath) 
        {
            return File.ReadAllBytes(filePath);
        }
    }
}
