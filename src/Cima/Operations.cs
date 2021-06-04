using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cima
{
    public static class Operations
    {
        /// <summary>
        /// Reshape a 2D array into a 1D array
        /// </summary>
        public static T[] Flatten<T>(T[,] input)
        {
            int height = input.GetLength(0);
            int width = input.GetLength(1);
            T[] output = new T[height * width];

            ulong pos = 0;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    output[pos++] = input[y, x];

            return output;
        }

        /// <summary>
        /// Reshape a 3D array into a 1D array
        /// </summary>
        public static byte[] Flatten(byte[,,] input)
        {
            int height = input.GetLength(0);
            int width = input.GetLength(1);
            int depth = input.GetLength(2);
            byte[] output = new byte[height * width * depth];

            ulong pos = 0;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    for (int z = 0; z < depth; z++)
                        output[pos++] = input[y, x, z];

            return output;
        }

        public static string MD5(byte[] bytes)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            return string.Join("", md5.ComputeHash(bytes).Select(x => x.ToString("x2")).ToArray());
        }

        public static string MD5(double[,] bytes) => MD5(Flatten(bytes).SelectMany(x => BitConverter.GetBytes(x)).ToArray());

        public static string MD5(byte[,] bytes) => MD5(Flatten(bytes));

        public static string MD5(byte[,,] bytes) => MD5(Flatten(bytes));

        /// <summary>
        /// Fill all values of an array with the given value IN PLACE
        /// </summary>
        public static void FillInPlace(double[,] input, double value)
        {
            for (int i = 0; i < input.GetLength(0); i++)
                for (int j = 0; j < input.GetLength(1); j++)
                    input[i, j] = value;
        }

        /// <summary>
        /// Add a fixed value to every element in place
        /// </summary>
        public static void AddInPlace(double[,] input, double value)
        {
            for (int i = 0; i < input.GetLength(0); i++)
                for (int j = 0; j < input.GetLength(1); j++)
                    input[i, j] += value;
        }

        /// <summary>
        /// Expand the image by N pixels on all sides.
        /// </summary>
        public static double[,] Expand(double[,] input, int n, double edgeColor = 0)
        {
            int width = input.GetLength(1);
            int height = input.GetLength(0);
            int newWidth = width + n * 2;
            int newHeight = height + n * 2;

            double[,] output = new double[newHeight, newWidth];
            if (edgeColor != 0)
                FillInPlace(output, edgeColor);

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    output[i + n, j + n] = input[i, j];

            return output;
        }

        /// <summary>
        /// Remove N pixels from all sides of the image.
        /// </summary>
        public static double[,] Contract(double[,] input, int n)
        {
            int width = input.GetLength(1);
            int height = input.GetLength(0);
            int newWidth = width - n * 2;
            int newHeight = height - n * 2;

            double[,] output = new double[newHeight, newWidth];

            for (int i = 0; i < newHeight; i++)
                for (int j = 0; j < newWidth; j++)
                    output[i, j] = input[i + n, j + n];

            return output;
        }
    }
}