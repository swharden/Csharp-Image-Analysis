using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cima.Test.Generate.Array
{
    class Grays
    {
        [Test]
        public void Test_Generate_Black()
        {
            double[,] data = Generate2D.Black(10, 6);
            Console.WriteLine(Cima.Display.Array(data));
            Assert.AreEqual(10, data.GetLength(1));
            Assert.AreEqual(6, data.GetLength(0));
            Assert.AreEqual(0, Cima.Statistics.Mean(data));
        }

        [Test]
        public void Test_Generate_White()
        {
            double[,] data = Generate2D.White(10, 6);
            Assert.AreEqual(1, Cima.Statistics.Mean(data));
        }

        [Test]
        public void Test_Generate_Gray()
        {
            double[,] data = Generate2D.SolidGray(10, 6);
            Assert.AreEqual(.5, Cima.Statistics.Mean(data));
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
            double mean = Cima.Statistics.Mean(data);
            Assert.AreEqual(grayLevel, mean, 1e-10);

            byte meanByte = IO.ValidByte(mean);
            Assert.AreEqual(expectedMeanByte, meanByte);
        }
    }
}
