using NUnit.Framework;
using System;
using System.IO;

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

        public static readonly string PATH_L =
            Path.GetFullPath(
                Path.Combine(
                    TestContext.CurrentContext.TestDirectory,
                    "../../../../../data/images/coins.png")
                );

        [Test]
        public void Test_SampleImages_Exist()
        {
            Assert.That(File.Exists(PATH_RGB), PATH_RGB);
            Assert.That(File.Exists(PATH_L), PATH_L);
        }

        [Test]
        public void Test_Dimensions_RGB()
        {
            var bmp = IO.LoadBitmap(PATH_RGB);
            var bytes3d = IO.BitmapToBytes3D(bmp);
            Assert.AreEqual(300, bytes3d.GetLength(0));
            Assert.AreEqual(451, bytes3d.GetLength(1));
            Assert.AreEqual(3, bytes3d.GetLength(2));
        }

        [Test]
        public void Test_Dimensions_G()
        {
            var bmp = IO.LoadBitmap(PATH_L);
            var bytes2d = IO.BitmapToBytes2D(bmp);
            Assert.AreEqual(303, bytes2d.GetLength(0));
            Assert.AreEqual(384, bytes2d.GetLength(1));
        }

        [Test]
        public void Test_Flatten_RGB()
        {
            var bmp = IO.LoadBitmap(PATH_RGB);
            var bytes3d = IO.BitmapToBytes3D(bmp);
            var bytes1d = Operations.Flatten(bytes3d);
            Assert.AreEqual(bmp.Width * bmp.Height * 3, bytes1d.Length);
        }

        [Test]
        public void Test_Flatten_L()
        {
            var bmp = IO.LoadBitmap(PATH_L);
            var bytes2d = IO.BitmapToBytes2D(bmp);
            var bytes1d = Operations.Flatten(bytes2d);
            Assert.AreEqual(bmp.Width * bmp.Height, bytes1d.Length);
        }

        [Test]
        public void Test_Hash_RGB()
        {
            // compare hash to that calculated with Python/PIL
            var bmp = IO.LoadBitmap(PATH_RGB);
            var bytes3d = IO.BitmapToBytes3D(bmp);
            var bytes1d = Operations.Flatten(bytes3d);
            string hash = Operations.MD5(bytes1d);
            Assert.AreEqual("4cbc8458da90b6c4b2dcf19e51656619", hash);
        }

        [Test]
        public void Test_Hash_L()
        {
            // compare hash to that calculated with Python/PIL
            var bmp = IO.LoadBitmap(PATH_L);
            var bytes2d = IO.BitmapToBytes2D(bmp);
            var bytes1d = Operations.Flatten(bytes2d);
            string hash = Operations.MD5(bytes1d);
            Assert.AreEqual("651a9e413b9cc780d6ae9c5eca027c76", hash);
        }
    }
}