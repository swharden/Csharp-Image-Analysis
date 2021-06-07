using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cima.Test
{
    class ConvolutionTests
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
            double[,] convolved = ImageMath.Convolve(original, kernel);
            double[,] contracted = Operations.Contract(original, 1);

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
            double[,] convolved = ImageMath.Convolve(original, kernel);

            Assert.Less(ImageMath.Mean(convolved), ImageMath.Mean(original));
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
            double[,] convolved = ImageMath.Convolve(original, kernel);

            Assert.Greater(ImageMath.Mean(convolved), ImageMath.Mean(original));
        }

        [Test]
        public void Test_Convolve_Blur()
        {
            double[,] kernel = Generate2D.SolidGray(5, 5, 1.0 / 25);
            double[,] original = IO.LoadImageGrayscaleFloating(Sample.Path.Camera);
            double[,] convolved = ImageMath.Convolve(original, kernel);

            TestTools.SavePng(original, "1");
            TestTools.SavePng(convolved, "2");

            Assert.AreEqual(ImageMath.Mean(convolved), ImageMath.Mean(original), .01);
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
            double[,] convolved = ImageMath.Convolve(original, kernel);
            Operations.AddInPlace(convolved, .5);

            TestTools.SavePng(original, "1");
            TestTools.SavePng(convolved, "2");
            Assert.AreEqual(.5, ImageMath.Mean(convolved), .01);
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
            double[,] convolved = ImageMath.Convolve(original, kernel);
            Operations.AddInPlace(convolved, .5);

            TestTools.SavePng(original, "1");
            TestTools.SavePng(convolved, "2");
            Assert.AreEqual(.5, ImageMath.Mean(convolved), .01);
        }

        [Test]
        public void Test_Convolve_Sobel()
        {
            // https://en.wikipedia.org/wiki/Sobel_operator

            double[,] original = IO.LoadImageGrayscaleFloating(Sample.Path.Camera);

            double[,] kernelHorizontal = { { -1, -1, -1 }, { 0, 0, 0 }, { 1, 1, 1 }, };
            double[,] convolvedHorizontal = ImageMath.Convolve(original, kernelHorizontal);

            double[,] kernelVertical = { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 }, };
            double[,] convolvedVertical = ImageMath.Convolve(original, kernelVertical);

            double[,] output = Operations.Magnitude(convolvedHorizontal, convolvedVertical);

            TestTools.SavePng(output, "2");
            Assert.NotZero(ImageMath.Mean(output));
            Assert.Less(ImageMath.Mean(output), .5);
        }
    }
}
