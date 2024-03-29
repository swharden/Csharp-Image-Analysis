﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cima.Test
{
    public static class Sample
    {
        private static readonly string ImageFolder = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(
                    TestContext.CurrentContext.TestDirectory,
                    "../../../../../data/images"));

        public static class Path
        {
            public static string Astronaut => System.IO.Path.Combine(ImageFolder, "astronaut.png");
            public static string Cat => System.IO.Path.Combine(ImageFolder, "cat.png");
            public static string CatSmall => System.IO.Path.Combine(ImageFolder, "cat-124x82.png");
            public static string Camera => System.IO.Path.Combine(ImageFolder, "camera.png");
            public static string Coins => System.IO.Path.Combine(ImageFolder, "coins.png");
            public static string Coffee => System.IO.Path.Combine(ImageFolder, "coffee.png");
            public static string Colorwheel => System.IO.Path.Combine(ImageFolder, "colorwheel.png");

            [Test]
            public static void Test_Files_Exist()
            {
                Assert.That(System.IO.File.Exists(Cat));
                Assert.That(System.IO.File.Exists(CatSmall));
                Assert.That(System.IO.File.Exists(Camera));
                Assert.That(System.IO.File.Exists(Coins));
                Assert.That(System.IO.File.Exists(Astronaut));
                Assert.That(System.IO.File.Exists(Coffee));
                Assert.That(System.IO.File.Exists(Colorwheel));
            }
        }

        public static class Bitmap
        {
            public static System.Drawing.Bitmap Cat => new(Path.Cat);
            public static System.Drawing.Bitmap CatSmall => new(Path.CatSmall);
            public static System.Drawing.Bitmap Camera => new(Path.Camera);
            public static System.Drawing.Bitmap Astronaut => new(Path.Astronaut);
            public static System.Drawing.Bitmap Coins => new(Path.Coins);
            public static System.Drawing.Bitmap Coffee => new(Path.Coffee);
            public static System.Drawing.Bitmap Colorwheel => new(Path.Colorwheel);

            [Test]
            public static void Test_Bitmaps_AreValid()
            {
                Assert.IsNotNull(Cat);
                Assert.IsNotNull(CatSmall);
                Assert.IsNotNull(Camera);
                Assert.IsNotNull(Astronaut);
                Assert.IsNotNull(Coffee);
                Assert.IsNotNull(Colorwheel);
                Assert.IsNotNull(Coins);
            }
        }

        public static class Data
        {
            public static double[,] Random_5x4 => new double[,]
            {
                {9.7,1.4,7.9,3.5,3.1},
                {6.4,2.0,6.3,4.8,4.2},
                {4.5,2.4,3.0,9.2,2.4},
                {5.0,7.6,2.1,5.8,5.4},
            };
        }
    }
}
