using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BasicModule.Utils
{
    public static class BitmapConversion
    {
        public static BitmapSource BitmapToBitmapSource(Bitmap source)
        {
            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                              source.GetHbitmap(),
                              IntPtr.Zero,
                              Int32Rect.Empty,
                              BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Source + " : " + e.Message);
                return null;
            }
        }

        #region Zebra Converter

        public static string ConvertImageToZPLString(UserControl view)
        {
            RenderTargetBitmap renderedScreen = RenderScreenAndSave(view);
            Bitmap bitmap = RenderTargetBitmapToBitmap(renderedScreen);
            bitmap = BitmapTo1Bpp(bitmap);

            return ZebraImage.GetASCIIhexadecimalData(bitmap);
        }

        private static RenderTargetBitmap RenderScreenAndSave(UserControl view)
        {
            System.Windows.Size size = new System.Windows.Size(view.RenderSize.Width, view.RenderSize.Height);
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, System.Windows.Media.PixelFormats.Pbgra32);

            System.Windows.Shapes.Rectangle vRect = new System.Windows.Shapes.Rectangle()
            {
                Width = size.Width,
                Height = size.Height,
                Fill = System.Windows.Media.Brushes.White
            };
            vRect.Measure(size);
            vRect.Arrange(new Rect(size));
            rtb.Render(vRect);

            view.Measure(size);
            view.Arrange(new Rect(size));
            rtb.Render(view);

            return rtb;
        }

        private static Bitmap RenderTargetBitmapToBitmap(object obj)
        {
            var bitmapSource = obj as BitmapSource;

            var bitmapImage = new BitmapImage();
            var bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));

            using (var stream = new MemoryStream())
            {
                bitmapEncoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }

            Bitmap bitmap;
            using (MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(stream);

                bitmap = new Bitmap(stream);
            }

            return bitmap;
        }

        public static Bitmap BitmapTo1Bpp(Bitmap img)
        {
            int w = img.Width;
            int h = img.Height;

            Bitmap bmp = new Bitmap(w, h, PixelFormat.Format1bppIndexed);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);

            for (int y = 0; y < h; y++)
            {
                byte[] scan = new byte[(w + 7) / 8];
                for (int x = 0; x < w; x++)
                {
                    Color c = img.GetPixel(x, y);
                    if (c.GetBrightness() >= 0.5)
                        scan[x / 8] |= (byte)(0x80 >> (x % 8));
                }
                System.Runtime.InteropServices.Marshal.Copy(scan, 0, (IntPtr)((int)data.Scan0 + data.Stride * y), scan.Length);
            }
            bmp.UnlockBits(data);

            return bmp;
        }

        #endregion Zebra Converter

    }
}
