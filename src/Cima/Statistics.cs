﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Cima
{
    public class Statistics
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
    }
}