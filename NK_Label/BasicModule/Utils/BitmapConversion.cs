using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BasicModule.Utils
{
    public static class BitmapConversion
    {
        public static BitmapSource BitmapToBitmapSource(Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                          source.GetHbitmap(),
                          IntPtr.Zero,
                          Int32Rect.Empty,
                          BitmapSizeOptions.FromEmptyOptions());
        }

        public static void GetLabelToImage()
        {
            // 라벨 화면 이미지화
        }
    }
}
