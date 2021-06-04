using System;
using System.Collections.Generic;
using System.Text;

namespace Cima
{
    public static class Generate2D
    {
        public static double[,] Black(int width, int height) => SolidGray(width, height, 0);

        public static double[,] White(int width, int height) => SolidGray(width, height, 1);

        public static double[,] SolidGray(int width, int height, double value = 0.5)
        {
            double[,] data = new double[height, width];
            for (int i = 0; i < data.GetLength(0); i++)
                for (int j = 0; j < data.GetLength(1); j++)
                    data[i, j] = value;
            return data;
        }

        public static double[,] Random(Random rand, int width, int height)
        {
            double[,] data = new double[height, width];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    data[i, j] = rand.NextDouble();
            return data;
        }
    }
}
