using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cima.Test.Generate.Array
{
    class Noise
    {
        [Test]
        public void Test_Generate_Random()
        {
            Random rand = new(0);
            double[,] data = Generate2D.Random(rand, 10, 6);
            Console.WriteLine(Cima.Display.Array(data));
            Assert.AreEqual(10, data.GetLength(1));
            Assert.AreEqual(6, data.GetLength(0));

            double mean1 = Cima.Statistics.Mean(data);
            Assert.AreEqual(.5675501235764863, mean1);

            double mean2 = Cima.Statistics.Mean(Cima.Operations.Flatten(data));
            Assert.AreEqual(mean1, mean2);
        }

        [Test]
        public void Test_Generate_RandomBig()
        {
            Random rand = new(0);
            double[,] data = Generate2D.Random(rand, 600, 400);
            Assert.AreEqual(600, data.GetLength(1));
            Assert.AreEqual(400, data.GetLength(0));

            double mean1 = Cima.Statistics.Mean(data);
            Assert.AreEqual(.5, mean1, .001);

            double mean2 = Cima.Statistics.Mean(Cima.Operations.Flatten(data));
            Assert.AreEqual(mean1, mean2);

            TestTools.SavePng(data);
        }
    }
}
