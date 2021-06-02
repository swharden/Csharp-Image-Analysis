using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace Cima
{
    public static class IO
    {
        public static Bitmap LoadBitmap(string filename) => new(filename);

        public static byte[,] BitmapToBytes2D(Bitmap bmp)
        {
            // lock the image and copy all its bytes
            Rectangle rect = new(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            int byteCount = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] bytes = new byte[byteCount];
            Marshal.Copy(bmpData.Scan0, bytes, 0, byteCount);
            bmp.UnlockBits(bmpData);

            // copy data from bytes into 2D array
            int bytesPerPixel = System.Drawing.Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
            int bytesPerRow = bmpData.Stride;
            byte[,] data = new byte[bmp.Height, bmp.Width];
            for (int y = 0; y < bmp.Height; y++)
            {
                int rowOffset = bytesPerRow * y;
                for (int x = 0; x < bmp.Width; x++)
                {
                    int pos = rowOffset + x * bytesPerPixel;
                    data[y, x] = bytes[pos];
                }
            }

            return data;
        }

        public static byte[,,] BitmapToBytes3D(Bitmap bmp)
        {
            // lock the image and copy all its bytes
            Rectangle rect = new(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            int byteCount = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] bytes = new byte[byteCount];
            Marshal.Copy(bmpData.Scan0, bytes, 0, byteCount);
            bmp.UnlockBits(bmpData);

            // copy data from bytes into 2D array
            int bytesPerPixel = System.Drawing.Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
            int bytesPerRow = bmpData.Stride;
            byte[,,] data = new byte[bmp.Height, bmp.Width, bytesPerPixel];
            for (int y = 0; y < bmp.Height; y++)
            {
                int rowOffset = bytesPerRow * y;
                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int z = 0; z < bytesPerPixel; z++)
                    {
                        // pixel bytes are little endian (RGB and ARGB)
                        int pos = rowOffset + x * bytesPerPixel + (bytesPerPixel - z - 1);
                        data[y, x, z] = bytes[pos];
                    }
                }
            }

            return data;
        }
    }
}
