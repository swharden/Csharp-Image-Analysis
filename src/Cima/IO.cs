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
        /// Load a bitmap with System.Drawing.Common so the caller doesn't have to.
        /// System.Drawing always returns a 32-bit ARGB regardless of the input file format.
        /// </summary>
        public static Bitmap LoadARGB(string filename) => new(filename);

        /// <summary>
        /// Save an image as a PNG by interacting with System.Drawing so the caller doesn't have to.
        /// </summary>
        public static void SavePng(Bitmap bmp, string path) => bmp.Save(path, ImageFormat.Png);

        /// <summary>
        /// Return the stride of a bitmap with the given dimensions.
        /// The stride will be padded as needed to ensure it is a multiple of 4.
        /// </summary>
        /// <returns></returns>
        public static int Stride(int width, int bytesPerPixel)
        {
            int stride = width * bytesPerPixel;
            while (stride % 4 > 0)
                stride += 1;
            return stride;
        }

        /// <summary>
        /// Convert a bitmap to a 3D byte array (height, width, channel).
        /// The number of channels is dynamic based on the input image format.
        /// </summary>
        public static byte[,,] LoadImage3D(Bitmap bmp)
        {
            // TODO: check for all possible bitmap pixel formats and throw for the weird ones
            int bpp = System.Drawing.Image.GetPixelFormatSize(bmp.PixelFormat) / 8;

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
        /// Convert a 3D array to a multichannel bitmap
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

            int stride = Stride(width, bpp);
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
