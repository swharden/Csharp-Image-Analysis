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

        public static double[,] Gaussian(int size)
        {
            throw new NotImplementedException();
        }
    }
}
