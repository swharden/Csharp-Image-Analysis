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

        public static int Stride(int width, int bytesPerPixel)
        {
            int padding = 0;
            int bytesPerRow = width * bytesPerPixel;
            if (bytesPerRow % 4 > 0)
                padding = 4 - (bytesPerRow % 4);
            return width * bytesPerPixel + padding;
        }

        public static byte[,] GetBytes2D(Bitmap bmp)
        {
            int bytesPerPixel = System.Drawing.Image.GetPixelFormatSize(bmp.PixelFormat) / 8;

            Rectangle rect = new(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);
            int byteCount = bmpData.Stride * bmp.Height;
            byte[] input = new byte[byteCount];
            Marshal.Copy(bmpData.Scan0, input, 0, byteCount);
            bmp.UnlockBits(bmpData);

            byte[,] output = new byte[bmp.Height, bmp.Width];
            for (int y = 0; y < bmp.Height; y++)
            {
                int rowOffset = bmpData.Stride * y;
                for (int x = 0; x < bmp.Width; x++)
                {
                    int pos = rowOffset + x * bytesPerPixel;
                    output[y, x] = input[pos];
                }
            }

            return output;
        }

        public static byte[,,] GetBytes3D(Bitmap bmp)
        {
            // lock the image and copy all its bytes
            Rectangle rect = new(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            int byteCount = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] input = new byte[byteCount];
            Marshal.Copy(bmpData.Scan0, input, 0, byteCount);
            bmp.UnlockBits(bmpData);

            // copy data from bytes into 2D array
            int bytesPerPixel = System.Drawing.Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
            int bytesPerRow = bmpData.Stride;
            byte[,,] output = new byte[bmp.Height, bmp.Width, bytesPerPixel];
            for (int y = 0; y < bmp.Height; y++)
            {
                int rowOffset = bytesPerRow * y;
                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int z = 0; z < bytesPerPixel; z++)
                    {
                        // pixel bytes are little endian (RGB and ARGB)
                        int pos = rowOffset + x * bytesPerPixel + (bytesPerPixel - z - 1);
                        output[y, x, z] = input[pos];
                    }
                }
            }

            return output;
        }

        public static Bitmap BitmapFromBytes2D(byte[,] input)
        {
            int height = input.GetLength(0);
            int width = input.GetLength(1);

            byte[] pixelsOutput = new byte[height * width];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    pixelsOutput[y * width + x] = input[y, x];

            Bitmap bmp = new(width, height, PixelFormat.Format8bppIndexed);
            ApplyPalette_Grayscale(bmp);
            var rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            Marshal.Copy(pixelsOutput, 0, bmpData.Scan0, pixelsOutput.Length);
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        public static Bitmap BitmapFromBytes3D(byte[,,] input)
        {
            int height = input.GetLength(0);
            int width = input.GetLength(1);
            int bpp = input.GetLength(2);

            PixelFormat bmpFormat = bpp switch
            {
                3 => PixelFormat.Format24bppRgb,
                4 => PixelFormat.Format32bppArgb,
                _ => throw new NotImplementedException($"unsupported bytes per pixel ({bpp})"),
            };

            int padding = 0;
            int bytesPerRow = width * bpp;
            if (bytesPerRow % 4 > 0)
                padding = 4 - (bytesPerRow % 4);
            int stride = width * bpp + padding;
            byte[] pixelsOutput = new byte[height * stride];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    for (int z = 0; z < bpp; z++)
                        pixelsOutput[y * stride + x * bpp + (bpp - z - 1)] = input[y, x, z];

            Bitmap bmp = new(width, height, bmpFormat);
            var rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmpFormat);
            Marshal.Copy(pixelsOutput, 0, bmpData.Scan0, pixelsOutput.Length);
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        private static void ApplyPalette_Grayscale(Bitmap bmp)
        {
            ColorPalette pal = bmp.Palette;
            for (int i = 0; i < 256; i++)
                pal.Entries[i] = Color.FromArgb(255, i, i, i);
            bmp.Palette = pal;
        }

        public static void SavePng(Bitmap bmp, string path) => bmp.Save(path, ImageFormat.Png);
    }
}
