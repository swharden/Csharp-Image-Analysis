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

        public static byte[,] LoadGrayscale(Bitmap bmp)
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
            byte[,] output = new byte[bmp.Height, bmp.Width];
            for (int y = 0; y < bmp.Height; y++)
            {
                int rowOffset = bytesPerRow * y;
                for (int x = 0; x < bmp.Width; x++)
                {
                    int pos = rowOffset + x * bytesPerPixel;
                    output[y, x] = input[pos];
                }
            }

            return output;
        }

        public static byte[,,] LoadRGB(Bitmap bmp)
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

        public static Bitmap GetBitmap(byte[,] input)
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

        public static Bitmap GetBitmap(byte[,,] input)
        {
            int height = input.GetLength(0);
            int width = input.GetLength(1);
            int depth = input.GetLength(2);

            PixelFormat bmpFormat = depth switch
            {
                3 => PixelFormat.Format24bppRgb,
                4 => PixelFormat.Format32bppArgb,
                _ => throw new NotImplementedException($"unsupported bytes per pixel ({depth})"),
            };

            int extraBytesPerRow = width % 4; // TODO: additional image width tests
            int bytesPerRow = width * depth + extraBytesPerRow; // stride
            byte[] pixelsOutput = new byte[height * bytesPerRow];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    for (int z = 0; z < depth; z++)
                        pixelsOutput[y * bytesPerRow + x * depth + (depth - z - 1)] = input[y, x, z];

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
