using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cima.Test
{
    class ReshapeTests
    {
        [Test]
        public void Test_Expand_IncreasesDarkSpace()
        {
            Random rand = new(0);
            double[,] data = Generate2D.Random(rand, 300, 200);
            double[,] expanded = Operations.Expand(data, 10);
            Assert.Less(ImageMath.Mean(expanded), ImageMath.Mean(data));
            Assert.AreEqual(320, expanded.GetLength(1));
            Assert.AreEqual(220, expanded.GetLength(0));
        }

        [Test]
        public void Test_Expand_CustomFill()
        {
            Random rand = new(0);
            double[,] data = Generate2D.Random(rand, 300, 200);
            double[,] expanded = Operations.Expand(data, 10, 1);
            Assert.Greater(ImageMath.Mean(expanded), ImageMath.Mean(data));
        }
    }
}
