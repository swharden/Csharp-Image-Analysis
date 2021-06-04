using System;
using System.Collections.Generic;
using System.Text;

namespace Cima
{
    public class ImageMath
    {
        public static double Mean(double[,] vals)
        {
            int height = vals.GetLength(0);
            int width = vals.GetLength(1);

            double sum = 0;
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    sum += vals[i, j];

            return sum / (height * width);
        }

        public static double Mean(double[] vals)
        {
            double sum = 0;

            for (int i = 0; i < vals.Length; i++)
                sum += vals[i];

            return sum / vals.Length;
        }

        public static double[,] Convolve(double[,] input, double[,] kernel)
        {
            if (kernel.GetLength(0) != kernel.GetLength(1))
                throw new ArgumentException("kernel must be square");
            if (kernel.GetLength(0) % 2 == 0)
                throw new ArgumentException("kernel width must be odd");

            int height = input.GetLength(0);
            int width = input.GetLength(1);

            int kernelWidth = kernel.GetLength(1);
            int deadSpace = (kernelWidth - 1) / 2;
            int finalHeight = height - 2 * deadSpace;
            int finalWidth = width - 2 * deadSpace;

            double GetNewPixel(int inputY, int inputX)
            {
                double sum = 0;
                for (int ky = 0; ky < kernelWidth; ky++)
                {
                    int dy = kernelWidth / 2 - ky;
                    for (int kx = 0; kx < kernelWidth; kx++)
                    {
                        int dx = kernelWidth / 2 - kx;
                        sum += input[inputY + dy, inputX + dx] * kernel[ky, kx];
                    }
                }
                return sum;
            }

            double[,] output = new double[finalHeight, finalWidth];
            for (int y = 0; y < finalHeight; y++)
                for (int x = 0; x < finalWidth; x++)
                    output[y, x] = GetNewPixel(y + deadSpace, x + deadSpace);

            return output;
        }
    }
}
