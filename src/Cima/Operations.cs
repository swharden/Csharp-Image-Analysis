using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cima
{
    public static class Operations
    {
        public static byte[] Flatten(byte[,] input)
        {
            int height = input.GetLength(0);
            int width = input.GetLength(1);
            byte[] output = new byte[height * width];

            ulong pos = 0;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                     output[pos++] = input[y, x];

            return output;
        }

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
    }
}
