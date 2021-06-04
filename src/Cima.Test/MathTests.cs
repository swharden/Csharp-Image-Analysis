using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cima.Test
{
    public class MathTests
    {
        [Test]
        public void Test_Math_Mean()
        {
            double[,] data = Sample.Data.Random_5x4;
            double dataMean = ImageMath.Mean(data);
            Assert.AreEqual(4.835, dataMean, delta: 1e-10);

            double[] flat = Operations.Flatten(data);
            double flatMean = ImageMath.Mean(flat);
            Assert.AreEqual(flatMean, dataMean);
        }
    }
}
