using ImageMagick;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cima
{
    public static class IOMagick
    {
        public static byte[,,] LoadImage(string filePath)
        {
            MagickImage image = new(filePath);
            byte[] pixelValues = image.GetPixels().GetValues();
            int bytesPerPixel = 3;
            byte[,,] data = new byte[image.Height, image.Width, bytesPerPixel];
            for (int y = 0; y < image.Height; y++)
                for (int x = 0; x < image.Width; x++)
                    for (int c = 0; c < bytesPerPixel; c++)
                        data[y, x, c] = pixelValues[y * image.Width * bytesPerPixel + x * bytesPerPixel + c];

            return data;
        }
    }
}
