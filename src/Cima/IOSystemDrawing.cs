using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;

namespace Cima
{
    public static class IOSystemDrawing
    {
        private static double[,] GrayscaleFromBmp(Bitmap bmp, int bytesPerPixel)
        {
            (byte[] bytes, int stride) = GetImageBytes(bmp);

            double[,] output = new double[bmp.Height, bmp.Width];
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    int offset = y * stride + x * bytesPerPixel;
                    output[y, x] += bytes[offset];
                    output[y, x] += bytes[offset + 1];
                    output[y, x] += bytes[offset + 2];
                    output[y, x] /= (255 * 3);
                }
            }
            return output;
        }

        private static (byte[] bytes, int stride) GetImageBytes(Bitmap bmp)
        {
            Rectangle rect = new(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            int byteCount = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] bytes = new byte[byteCount];
            Marshal.Copy(bmpData.Scan0, bytes, 0, byteCount);
            bmp.UnlockBits(bmpData);
            return (bytes, bmpData.Stride);
        }

        public static double[,] LoadGrayscale(string filePath)
        {
            using Bitmap bmp = new(filePath);

            if (bmp.PixelFormat.ToString().Contains("24bpp"))
                return GrayscaleFromBmp(bmp, 3);
            else if (bmp.PixelFormat.ToString().Contains("32bpp"))
                return GrayscaleFromBmp(bmp, 4);
            else
                throw new NotImplementedException($"unsupported pixel format: {bmp.PixelFormat}");
        }
    }
}
