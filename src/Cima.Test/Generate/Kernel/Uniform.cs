using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cima.Test.Generate.Kernel
{
    class Uniform
    {
        [TestCase(0, 1)]
        [TestCase(1, 3)]
        [TestCase(2, 5)]
        [TestCase(3, 7)]
        public void Test_Shape_IsCorrect(int size, int width)
        {
            double[,] kernel = Cima.Generate.Kernel.Uniform(size, 123);
            Assert.AreEqual(width, kernel.GetLength(0));
            Assert.AreEqual(width, kernel.GetLength(1));
        }

        [TestCase(123)]
        [TestCase(-321)]
        public void Test_Values_MatchThoseGiven(int val)
        {
            double[,] kernel = Cima.Generate.Kernel.Uniform(3, val);
            foreach (double v in Cima.Operations.Flatten(kernel))
                Assert.AreEqual(val, v);
        }

        public void Test_Size_CannotBeNegative(int val)
        {
            Assert.Throws<ArgumentException>(() => Cima.Generate.Kernel.Uniform(3, val));
        }
    }
}
