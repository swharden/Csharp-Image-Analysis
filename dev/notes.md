## ImageMagick

```cs
using ImageMagick;
using NUnit.Framework;
using System;
using System.Linq;

namespace TestProject1
{
    public class Tests
    {
        const string TestImageFolder = @"C:\Users\scott\Documents\GitHub\Csharp-Image-Analysis\data\images";

        [TestCase("8bit-02.tif", new float[] { 40, 28, 28, 32, 32, 38, 32, 19, 34, })]
        [TestCase("16bit-01.tif", new float[] { 1321, 1380, 1472, 1526, 1640, 1751, 1791, 1898, 1958, })]
        [TestCase("16bit-02.png", new float[] { 400, 376, 376, 383, 383, 396, 383, 357, 388, })]
        [TestCase("16bit-02.tif", new float[] { 400, 376, 376, 383, 383, 396, 383, 357, 388, })]
        [TestCase("16bit-04.tif", new float[] { 125, 275, 139, 108, 108, 121, 200, 194, 109, })]
        [TestCase("32bit-02.tif", new float[] { 28000000, 26320000, 26320000, 26810000, 26810000, 27720000, 26810000, 24990000, 27160000, })]
        [TestCase("camera.png", new float[] { 200, 200, 200, 200, 199, 200, 199, 198, 199, })]
        [TestCase("checkerboard.png", new float[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, })]
        [TestCase("coins.png", new float[] { 47, 123, 133, 129, 137, 132, 138, 135, 134, })]
        [TestCase("horse.png", new float[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, })]
        public void Test_SampleImage_FirstValues(string filename, float[] expected)
        {
            string path = System.IO.Path.Combine(TestImageFolder, filename);
            MagickImage image = new(path);
            Console.WriteLine($"{filename} (Depth={image.Depth}, ColorSpace={image.ColorSpace}, ColorType={image.ColorType})");

            float[] pixels = image.GetPixels().GetValues();

            if (image.ColorSpace == ColorSpace.Gray)
            {
                if (image.Depth == 8)
                {
                    for (int i = 0; i < pixels.Length; i++)
                        pixels[i] = (int)pixels[i] / 256;
                }
                else if (image.Depth == 16)
                {
                    // already good because we are using the Q16 package
                }
                else if (image.Depth == 32)
                {
                    for (int i = 0; i < pixels.Length; i++)
                        pixels[i] = (long)pixels[i] / 65535;
                }
                else
                {
                    throw new NotImplementedException($"Unsupported color depth: {image.Depth}");
                }
            }

            if (image.ColorType == ColorType.Palette)
            {
                // every other value is the mask
                pixels = Enumerable.Range(0, pixels.Length / 2).Select(x => pixels[x * 2]).ToArray();
            }

            float[] got = pixels.Take(expected.Length).ToArray();
            Console.WriteLine(string.Join(", ", got.Select(x => x.ToString())));
            Assert.AreEqual(expected, got);
        }

        [TestCase("16bit-01.tif", new double[] { 1321, 1380, 1472, 1526, 1640, 1751, 1791, 1898, 1958, })]
        [TestCase("16bit-02.tif", new double[] { 400, 376, 376, 383, 383, 396, 383, 357, 388, })]
        [TestCase("16bit-04.tif", new double[] { 125, 275, 139, 108, 108, 121, 200, 194, 109, })]
        public void Test_LibTiff_FirstValues(string filename, double[] expected)
        {
            string path = System.IO.Path.Combine(TestImageFolder, filename);
            using Tiff image = Tiff.Open(path, "r");

            int width = image.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
            int height = image.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
            int bytesPerPixel = image.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt() / 8;

            int numberOfStrips = image.NumberOfStrips();
            byte[] bytes = new byte[numberOfStrips * image.StripSize()];
            for (int i = 0; i < numberOfStrips; ++i)
                image.ReadRawStrip(i, bytes, i * image.StripSize(), image.StripSize());

            double[] data = new double[bytes.Length / bytesPerPixel];

            if (bytesPerPixel != 2)
                throw new NotImplementedException("this routine only works for 16-bit TIFs");
            for (int i = 0; i < data.Length; i++)
            {
                if (image.IsBigEndian())
                    data[i] = bytes[i * 2 + 1] + (bytes[i * 2] << 8);
                else
                    data[i] = bytes[i * 2] + (bytes[i * 2 + 1] << 8);
            }

            double[] got = data.Take(expected.Length).ToArray();
            Console.WriteLine(string.Join(", ", got.Select(x => x.ToString())));
            Assert.AreEqual(expected, got);
        }
    }
}
```
