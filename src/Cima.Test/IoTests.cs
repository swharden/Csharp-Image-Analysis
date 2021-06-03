using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace Cima.Test
{
    public class Tests
    {
        public static readonly string PATH_RGB =
            Path.GetFullPath(
                Path.Combine(
                    TestContext.CurrentContext.TestDirectory,
                    "../../../../../data/images/cat.png")
                );

        public static readonly string PATH_GRAY =
            Path.GetFullPath(
                Path.Combine(
                    TestContext.CurrentContext.TestDirectory,
                    "../../../../../data/images/coins.png")
                );

        private static string GetFileHash(string filePath)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            using var stream = File.OpenRead(filePath);
            byte[] bytes = md5.ComputeHash(stream);
            return string.Join("", md5.ComputeHash(bytes).Select(x => x.ToString("x2")).ToArray()); ;
        }

        [Test]
        public void Test_SampleImages_Exist()
        {
            Assert.That(File.Exists(PATH_RGB), PATH_RGB);
            Assert.That(File.Exists(PATH_GRAY), PATH_GRAY);
        }

        [Test]
        public void Test_Dimensions_RGB()
        {
            var bmp = IO.LoadBitmap(PATH_RGB);
            var bytes3d = IO.LoadRGB(bmp);
            Assert.AreEqual(300, bytes3d.GetLength(0));
            Assert.AreEqual(451, bytes3d.GetLength(1));
            Assert.AreEqual(3, bytes3d.GetLength(2));
        }

        [Test]
        public void Test_Dimensions_Gray()
        {
            var bmp = IO.LoadBitmap(PATH_GRAY);
            var bytes2d = IO.LoadGrayscale(bmp);
            Assert.AreEqual(303, bytes2d.GetLength(0));
            Assert.AreEqual(384, bytes2d.GetLength(1));
        }

        [Test]
        public void Test_Flatten_RGB()
        {
            var bmp = IO.LoadBitmap(PATH_RGB);
            var bytes3d = IO.LoadRGB(bmp);
            var bytes1d = Operations.Flatten(bytes3d);
            Assert.AreEqual(bmp.Width * bmp.Height * 3, bytes1d.Length);
        }

        [Test]
        public void Test_Flatten_Gray()
        {
            var bmp = IO.LoadBitmap(PATH_GRAY);
            var bytes2d = IO.LoadGrayscale(bmp);
            var bytes1d = Operations.Flatten(bytes2d);
            Assert.AreEqual(bmp.Width * bmp.Height, bytes1d.Length);
        }

        [Test]
        public void Test_Hash_RGB()
        {
            // compare hash to that calculated with Python/PIL
            var bmp = IO.LoadBitmap(PATH_RGB);
            var bytes3d = IO.LoadRGB(bmp);
            var bytes1d = Operations.Flatten(bytes3d);
            string hash = Operations.MD5(bytes1d);
            Assert.AreEqual("4cbc8458da90b6c4b2dcf19e51656619", hash);
        }

        [Test]
        public void Test_Hash_Gray()
        {
            // compare hash to that calculated with Python/PIL
            var bmp = IO.LoadBitmap(PATH_GRAY);
            var bytes2d = IO.LoadGrayscale(bmp);
            var bytes1d = Operations.Flatten(bytes2d);
            string hash = Operations.MD5(bytes1d);
            Assert.AreEqual("651a9e413b9cc780d6ae9c5eca027c76", hash);
        }

        [Test]
        public void Test_Save_RGB()
        {
            string saveFilePath = $"test_{Path.GetFileNameWithoutExtension(PATH_RGB)}.png";
            var bmp1 = IO.LoadBitmap(PATH_RGB);
            var bytes1 = IO.LoadRGB(bmp1);
            var bmp2 = IO.GetBitmap(bytes1);
            IO.SavePng(bmp2, saveFilePath);

            var bmp = IO.LoadBitmap(saveFilePath);
            var bytes2d = IO.LoadRGB(bmp);
            var bytes1d = Operations.Flatten(bytes2d);
            string hash = Operations.MD5(bytes1d);
            Assert.AreEqual("4cbc8458da90b6c4b2dcf19e51656619", hash);
        }

        [Test]
        public void Test_Save_Gray()
        {
            string saveFilePath = $"test_{Path.GetFileNameWithoutExtension(PATH_GRAY)}.png";
            var bmp1 = IO.LoadBitmap(PATH_GRAY);
            var bytes1 = IO.LoadGrayscale(bmp1);
            var bmp2 = IO.GetBitmap(bytes1);
            IO.SavePng(bmp2, saveFilePath);

            var bmp = IO.LoadBitmap(saveFilePath);
            var bytes2d = IO.LoadGrayscale(bmp);
            var bytes1d = Operations.Flatten(bytes2d);
            string hash = Operations.MD5(bytes1d);
            Assert.AreEqual("651a9e413b9cc780d6ae9c5eca027c76", hash);
        }

        [TestCase("cat-119x79.png", 119, 79)]
        [TestCase("cat-120x80.png", 120, 80)]
        [TestCase("cat-121x80.png", 121, 80)]
        [TestCase("cat-122x81.png", 122, 81)]
        [TestCase("cat-123x82.png", 123, 82)]
        [TestCase("cat-124x82.png", 124, 82)]
        public void Test_SpanCalculation_IsAccurate(string filename, int width, int height)
        {
            // Even though bitmap images can be any width their bytes in memory always use widths that are multiples of 4.
            // This means there will be empty bytes in memory for bitmaps with widths that aren't an even multiple of 4.
            // A bad span (image width in memory) presents itself as an image that slants.
            // These tests ensure span calculations are accurate by loading, saving, and comparing images of different widths.

            string imageFolderPath = Path.Combine(TestContext.CurrentContext.TestDirectory, $"../../../../../data/images/");
            string loadFilePath = Path.Combine(imageFolderPath, filename);

            // load an input image of known size
            var originalBitmap = IO.LoadBitmap(loadFilePath);
            var originalBytes2d = IO.LoadGrayscale(originalBitmap);
            Assert.AreEqual(originalBytes2d.GetLength(0), height);
            Assert.AreEqual(originalBytes2d.GetLength(1), width);
            string originalHash = Operations.MD5(originalBytes2d);

            // save the image to a test file and re-load it
            string saveFilePath = $"test_{filename}.png";
            IO.SavePng(originalBitmap, saveFilePath);
            var newBitmap = IO.LoadBitmap(saveFilePath);
            var newBytes2d = IO.LoadGrayscale(newBitmap);
            string newHash = Operations.MD5(newBytes2d);

            // ensure the loaded data is identical
            Assert.AreEqual(originalHash, newHash);
        }
    }
}