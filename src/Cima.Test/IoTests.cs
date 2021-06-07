using NUnit.Framework;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Cima.Test
{
    public class IoTests
    {
        [Test]
        public void Test_Dimensions_MatchExpected()
        {
            byte[,,] bytes3d = IO.LoadImage(Sample.Path.Cat);
            Assert.AreEqual(300, bytes3d.GetLength(0));
            Assert.AreEqual(451, bytes3d.GetLength(1));
            Assert.AreEqual(3, bytes3d.GetLength(2));
        }

        [Test]
        public void Test_Dimensions_Gray()
        {
            var bytes2d = IO.LoadImage(Sample.Path.Coins);
            Assert.AreEqual(303, bytes2d.GetLength(0));
            Assert.AreEqual(384, bytes2d.GetLength(1));
        }

        [Test]
        public void Test_Hash_RGB()
        {
            // compare hash to that calculated with Python/PIL
            var bytes3d = IO.LoadImage(Sample.Path.Cat);
            var bytes1d = Cima.Operations.Flatten(bytes3d);
            string hash = Cima.Operations.MD5(bytes1d);
            Assert.AreEqual("4cbc8458da90b6c4b2dcf19e51656619", hash);
        }

        [Test]
        public void Test_Hash_Gray()
        {
            // compare hash to that calculated with Python/PIL
            byte[,,] bytes3d = IO.LoadImage(Sample.Path.Coins);
            byte[,] bytes2d = IO.GetChannel(bytes3d);
            byte[] bytes1d = Cima.Operations.Flatten(bytes2d);
            Console.WriteLine(bytes1d.Length);
            string hash = Cima.Operations.MD5(bytes1d);
            Assert.AreEqual("651a9e413b9cc780d6ae9c5eca027c76", hash);
        }

        [Test]
        public void Test_Save_RGB()
        {
            string saveFilePath = TestTools.GetOutputFilePath("test_save_rgb.png");

            byte[,,] originalBytes = IO.LoadImage(Sample.Path.Cat);
            string originalHash = Cima.Operations.MD5(originalBytes);
            Assert.AreEqual("4cbc8458da90b6c4b2dcf19e51656619", originalHash);
            IO.SavePng(originalBytes, saveFilePath);

            byte[,,] newBytes = IO.LoadImage(saveFilePath);
            string newHash = Cima.Operations.MD5(newBytes);
            Assert.AreEqual(originalHash, newHash);
        }

        [Test]
        public void Test_Save_Gray()
        {
            string saveFilePath = TestTools.GetOutputFilePath("test_save_gray.png");

            byte[,,] bytes1 = IO.LoadImage(Sample.Path.Coins);
            IO.SavePng(bytes1, saveFilePath);

            byte[,,] bytes3d = IO.LoadImage(saveFilePath);
            byte[,] bytes2d = IO.GetChannel(bytes3d);
            byte[] bytes1d = Cima.Operations.Flatten(bytes2d);
            string hash = Cima.Operations.MD5(bytes1d);
            Assert.AreEqual("651a9e413b9cc780d6ae9c5eca027c76", hash);
        }

        [TestCase("cat-119x79.png", 119, 79)]
        [TestCase("cat-120x80.png", 120, 80)]
        [TestCase("cat-121x80.png", 121, 80)]
        [TestCase("cat-122x81.png", 122, 81)]
        [TestCase("cat-123x82.png", 123, 82)]
        [TestCase("cat-124x82.png", 124, 82)]
        [TestCase("rgb-321x217.png", 321, 217)]
        [TestCase("rgba-321x217.png", 321, 217)]
        public void Test_Save_MatchesOriginal(string filename, int width, int height)
        {
            // Even though bitmap images can be any width their bytes in memory always use widths that are multiples of 4.
            // This means there will be empty bytes in memory for bitmaps with widths that aren't an even multiple of 4.
            // A bad span (image width in memory) presents itself as an image that slants.
            // These tests ensure span calculations are accurate by loading, saving, and comparing images of different widths.

            string imageFolderPath = Path.Combine(TestContext.CurrentContext.TestDirectory, $"../../../../../data/images/");
            string loadFilePath = Path.Combine(imageFolderPath, filename);

            // load an input image of known size
            byte[,,] bytes1 = IO.LoadImage(loadFilePath);
            Assert.AreEqual(bytes1.GetLength(0), height);
            Assert.AreEqual(bytes1.GetLength(1), width);
            string originalHash = Cima.Operations.MD5(bytes1);

            // save the image to a test file and re-load it
            string saveFilePath = TestTools.GetOutputFilePath($"test_{filename}.png");
            IO.SavePng(bytes1, saveFilePath);
            byte[,,] newBitmap = IO.LoadImage(saveFilePath);
            string newHash = Cima.Operations.MD5(newBitmap);

            // ensure the loaded data is identical
            Assert.AreEqual(originalHash, newHash);
        }
    }
}