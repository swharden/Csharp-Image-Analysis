using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cima.Test.Operations
{
    class Convolve
    {
        [Test]
        public void Test_Convolve_Preserve()
        {
            double[,] kernel =
            {
                { 0, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 0 },
            };

            double[,] original = IO.LoadImageGrayscaleFloating(Sample.Path.Camera);
            double[,] convolved = Cima.Operations.Convolve(original, kernel);
            double[,] contracted = Cima.Operations.Contract(original, 1);

            Assert.AreEqual(contracted, convolved);
        }

        [Test]
        public void Test_Convolve_Darken()
        {
            double[,] kernel =
            {
                { 0, 0, 0 },
                { 0, .5, 0 },
                { 0, 0, 0 },
            };

            double[,] original = IO.LoadImageGrayscaleFloating(Sample.Path.Camera);
            double[,] convolved = Cima.Operations.Convolve(original, kernel);

            Assert.Less(Cima.Statistics.Mean(convolved), Cima.Statistics.Mean(original));
        }

        [Test]
        public void Test_Convolve_Lighten()
        {
            double[,] kernel =
            {
                { 0, 0, 0 },
                { 0, 2, 0 },
                { 0, 0, 0 },
            };

            double[,] original = IO.LoadImageGrayscaleFloating(Sample.Path.Camera);
            double[,] convolved = Cima.Operations.Convolve(original, kernel);

            Assert.Greater(Cima.Statistics.Mean(convolved), Cima.Statistics.Mean(original));
        }

        [Test]
        public void Test_Convolve_BoxBlur()
        {
            double[,] kernel = Generate2D.SolidGray(5, 5, 1.0 / 25);
            double[,] original = IO.LoadImageGrayscaleFloating(Sample.Path.Camera);
            double[,] convolved = Cima.Operations.Convolve(original, kernel);

            TestTools.SavePng(original, "original");
            TestTools.SavePng(convolved, "convolved");

            Assert.AreEqual(Cima.Statistics.Mean(convolved), Cima.Statistics.Mean(original), .01);
        }

        [TestCase(5, 0)]
        [TestCase(5, 1)]
        [TestCase(9, 3)]
        //[TestCase(25, 5)]
        public void Test_Convolve_GaussianBlur(int size, int sigma)
        {
            double[,] kernel = Cima.Generate.Kernel.Gaussian2D(size, sigma);
            double[,] original = IO.LoadImageGrayscaleFloating(Sample.Path.Camera);
            double[,] convolved = Cima.Operations.Convolve(original, kernel);

            TestTools.SavePng(convolved, $"blur-{sigma:D2}");

            Assert.AreEqual(Cima.Statistics.Mean(convolved), Cima.Statistics.Mean(original), .1);
        }

        [Test]
        public void Test_Convolve_LeftEdge()
        {
            double[,] kernel =
            {
                { -1, 0, 1 },
                { -1, 0, 1 },
                { -1, 0, 1 },
            };

            double[,] original = IO.LoadImageGrayscaleFloating(Sample.Path.Camera);
            double[,] convolved = Cima.Operations.Convolve(original, kernel);
            Cima.Operations.AddInPlace(convolved, .5);

            TestTools.SavePng(original, "original");
            TestTools.SavePng(convolved, "convolved");

            Assert.AreEqual(.5, Cima.Statistics.Mean(convolved), .01);
        }

        [Test]
        public void Test_Convolve_TopEdge()
        {
            double[,] kernel =
            {
                { -1, -1, -1 },
                { 0, 0, 0 },
                { 1, 1, 1 },
            };

            double[,] original = IO.LoadImageGrayscaleFloating(Sample.Path.Camera);
            double[,] convolved = Cima.Operations.Convolve(original, kernel);
            Cima.Operations.AddInPlace(convolved, .5);

            TestTools.SavePng(original, "original");
            TestTools.SavePng(convolved, "convolved");

            Assert.AreEqual(.5, Cima.Statistics.Mean(convolved), .01);
        }

        [Test]
        public void Test_Convolve_Sobel()
        {

            double[,] original = IO.LoadImageGrayscaleFloating(Sample.Path.Camera);

            // https://en.wikipedia.org/wiki/Sobel_operator
            double[,] kernelHorizontal =
            {
                { 1, 2, 1 },
                { 0, 0, 0 },
                { -1, -2, -1 },
            };

            double[,] kernelVertical =
            {
                { 1, 0, -1 },
                { 2, 0, -2 },
                { 1, 0, -1 },
            };

            double[,] convolvedHorizontal = Cima.Operations.Convolve(original, kernelHorizontal);
            double[,] convolvedVertical = Cima.Operations.Convolve(original, kernelVertical);
            double[,] output = Cima.Operations.Magnitude(convolvedHorizontal, convolvedVertical);

            TestTools.SavePng(original, "original");
            TestTools.SavePng(output, "output");

            Assert.NotZero(Cima.Statistics.Mean(output));
            Assert.Less(Cima.Statistics.Mean(output), .5);
        }
    }
}
