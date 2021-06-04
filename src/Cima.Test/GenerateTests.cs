using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cima.Test
{
    class GenerateTests
    {
        [Test]
        public void Test_Generate_Random()
        {
            Random rand = new(0);
            double[,] data = Generate2D.Random(rand, 10, 6);
            Console.WriteLine(Display.Array(data));
            Assert.AreEqual(10, data.GetLength(1));
            Assert.AreEqual(6, data.GetLength(0));

            double mean1 = ImageMath.Mean(data);
            Assert.AreEqual(.5675501235764863, mean1);

            double mean2 = ImageMath.Mean(Operations.Flatten(data));
            Assert.AreEqual(mean1, mean2);
        }

        [Test]
        public void Test_Generate_RandomBig()
        {
            Random rand = new(0);
            double[,] data = Generate2D.Random(rand, 600, 400);
            Assert.AreEqual(600, data.GetLength(1));
            Assert.AreEqual(400, data.GetLength(0));

            double mean1 = ImageMath.Mean(data);
            Assert.AreEqual(.5, mean1, .001);

            double mean2 = ImageMath.Mean(Operations.Flatten(data));
            Assert.AreEqual(mean1, mean2);

            TestTools.SavePng(data);
        }

        [Test]
        public void Test_Generate_Black()
        {
            double[,] data = Generate2D.Black(10, 6);
            Console.WriteLine(Display.Array(data));
            Assert.AreEqual(10, data.GetLength(1));
            Assert.AreEqual(6, data.GetLength(0));
            Assert.AreEqual(0, ImageMath.Mean(data));
        }

        [Test]
        public void Test_Generate_White()
        {
            double[,] data = Generate2D.White(10, 6);
            Assert.AreEqual(1, ImageMath.Mean(data));
        }

        [Test]
        public void Test_Generate_Gray()
        {
            double[,] data = Generate2D.SolidGray(10, 6);
            Assert.AreEqual(.5, ImageMath.Mean(data));
        }

        [TestCase(-7, 0)]
        [TestCase(0, 0)]
        [TestCase(.3, 76)]
        [TestCase(.5, 128)]
        [TestCase(.7, 179)]
        [TestCase(1, 255)]
        [TestCase(7, 255)]
        public void Test_Generate_GrayDefined(double grayLevel, byte expectedMeanByte)
        {
            double[,] data = Generate2D.SolidGray(10, 6, grayLevel);
            double mean = ImageMath.Mean(data);
            Assert.AreEqual(grayLevel, mean, 1e-10);

            byte meanByte = IO.ValidByte(mean);
            Assert.AreEqual(expectedMeanByte, meanByte);
        }
    }
}
