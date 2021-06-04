using System;
using System.Collections.Generic;
using System.Text;

namespace Cima
{
    public static class Display
    {
        public static string Array(double[,] vals)
        {
            int width = vals.GetLength(1);
            int height = vals.GetLength(0);
            int count = width * height;

            StringBuilder sb = new();
            sb.Append($"2D array with {count} values (w={width}, h={height})");
            for (int y = 0; y < height; y++)
            {
                sb.AppendLine();
                for (int x = 0; x < width; x++)
                {
                    double val = vals[y, x];
                    sb.Append($"{val, 7:#.0000}");
                }
            }
            return sb.ToString();
        }
    }
}
