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
        /// <summary>
        /// Save an image as a PNG.
        /// </summary>
        public static void SavePng(byte[,,] bytes, string path) => BitmapFromBytes3D(bytes).Save(path, ImageFormat.Png);

        /// <summary>
        /// Save 2D image data as a grayscale PNG.
        /// </summary>
        public static void SavePng(double[,] data, string path) => BitmapFromBytes2D(data).Save(path, ImageFormat.Png);

        /// <summary>
        /// Load the image file as a 3D byte array (height, width, channel).
        /// The number of channels is dynamic based on the input image format.
        /// </summary>
        public static byte[,,] LoadImage(string filePath)
        {
            Bitmap bmp = new(filePath);
            return LoadImage(bmp);
        }

        /// <summary>
        /// Load the image file as a 2D byte array taking just the red channel.
        /// </summary>
        public static double[,] LoadImageGrayscaleFloating(string filePath)
        {
            Bitmap bmp = new(filePath);
            byte[,,] bytes3d = LoadImage(bmp);
            byte[,] bytes2d = GetChannel(bytes3d);
            return BytesToFloating(bytes2d);
        }

        public static double[,] BytesToFloating(byte[,] input)
        {
            int height = input.GetLength(0);
            int width = input.GetLength(1);

            double[,] output = new double[height, width];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    output[y, x] = (double)input[y, x] / 255;

            return output;
        }

        /// <summary>
        /// Convert a bitmap to a 3D byte array (height, width, channel).
        /// The number of channels is dynamic based on the input image format.
        /// </summary>
        public static byte[,,] LoadImage(Bitmap bmp)
        {
            int bpp = bmp.PixelFormat switch
            {
                PixelFormat.Format24bppRgb => 3,
                PixelFormat.Format32bppRgb => 4,
                PixelFormat.Format32bppArgb => 4,
                PixelFormat.Format32bppPArgb => 4,
                _ => throw new NotImplementedException($"unsupported pixel format: {bmp.PixelFormat}")
            };

            Rectangle rect = new(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            int byteCount = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] input = new byte[byteCount];
            Marshal.Copy(bmpData.Scan0, input, 0, byteCount);
            bmp.UnlockBits(bmpData);

            byte[,,] output = new byte[bmp.Height, bmp.Width, bpp];
            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                    for (int z = 0; z < bpp; z++)
                        output[y, x, z] = input[bmpData.Stride * y + x * bpp + (bpp - z - 1)];

            return output;
        }

        /// <summary>
        /// Given a 3d array of imaging data (height, width, channel), return a 2D array of the Nth channel.
        /// Recall that channels are little-endian encoded (A, B, G, R)
        /// </summary>
        public static byte[,] GetChannel(byte[,,] input, int c = 3)
        {
            int height = input.GetLength(0);
            int width = input.GetLength(1);

            byte[,] output = new byte[height, width];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    output[y, x] = input[y, x, c];

            return output;
        }

        /// <summary>
        /// Convert a double into a valid intensity byte
        /// </summary>
        public static byte ValidByte(double input)
        {
            int value = (int)(input * 256);
            if (value <= 0)
                return 0;
            if (value >= 255)
                return 255;
            return (byte)value;
        }

        /// <summary>
        /// Convert a 2D array (Grayscale) to a Bitmap
        /// </summary>
        public static Bitmap BitmapFromBytes2D(double[,] data)
        {
            // TODO: make this more effecient by using indexed color
            int height = data.GetLength(0);
            int width = data.GetLength(1);
            byte[,,] bytes = new byte[height, width, 3];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    byte b = ValidByte(data[i, j]);
                    bytes[i, j, 0] = b;
                    bytes[i, j, 1] = b;
                    bytes[i, j, 2] = b;
                }
            }

            return BitmapFromBytes3D(bytes);
        }

        /// <summary>
        /// Convert a 3D array (RGB) to a Bitmap
        /// </summary>
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

            int stride = 4 * ((width * bpp + 3) / 4);
            byte[] output = new byte[height * stride];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    for (int z = 0; z < bpp; z++)
                        output[y * stride + x * bpp + (bpp - z - 1)] = input[y, x, z];

            Bitmap bmp = new(width, height, bmpFormat);
            var rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmpFormat);
            Marshal.Copy(output, 0, bmpData.Scan0, output.Length);
            bmp.UnlockBits(bmpData);

            return bmp;
        }
    }
}
