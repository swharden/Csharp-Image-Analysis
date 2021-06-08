using System;
using System.Collections.Generic;
using System.Text;

namespace Cima.Generate
{
    public static class Kernel
    {
        /// <summary>
        /// Return a square kernel with all values the same
        /// </summary>
        /// <param name="value">set every element in the array to this value</param>
        /// <param name="size">number of pixels between the center pixel and each edge</param>
        /// <returns></returns>
        public static double[,] Uniform(int size, double value)
        {
            if (size < 0)
                throw new ArgumentException("size must be positive");

            int width = size * 2 + 1;
            double[,] kernel = new double[width, width];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < width; j++)
                    kernel[i, j] = value;
            return kernel;
        }

        /// <summary>
        /// Return a Gaussian window
        /// </summary>
        /// <param name="size">Number of points in the output window</param>
        /// <param name="sigma">The standard deviation</param>
        /// <returns>The window, with the maximum value normalized to 1</returns>
        public static double[] Gaussian1D(int size, double sigma)
        {
            double[] ys = new double[size];
            double d = 2 * sigma * sigma;
            for (int i = 0; i < size; i++)
            {
                double x = i - (size - 1.0) / 2.0;
                ys[i] = Math.Exp(-x * x / d);
            }
            return ys;
        }

        public static double[,] Gaussian2D(int size, double sigma = 1, bool normalize = true)
        {
            if (sigma == 0)
            {
                double[,] preserve = new double[size, size];
                preserve[size / 2, size / 2] = 1;
                return preserve;
            }
            
            double[] window = Gaussian1D(size, sigma);
            double[,] data = ProductSqure(window);
            if (normalize)
                Operations.NormalizeInPlace(data);
            return data;
        }


        /// <summary>
        /// Compute the product (2D) of a vector (1D) times itself
        /// </summary>
        private static double[,] ProductSqure(double[] input)
        {
            double[,] output = new double[input.Length, input.Length];

            for (int i = 0; i < input.Length; i++)
                for (int j = 0; j < input.Length; j++)
                    output[i, j] = input[i] * input[j];

            return output;
        }

        public static double[,] Gaussian(int size)
        {
            throw new NotImplementedException();
        }
    }
}
