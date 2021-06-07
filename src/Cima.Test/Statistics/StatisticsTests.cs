using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cima.Test.Statistics
{
    public class StatisticsTests
    {
        [Test]
        public void Test_Math_Mean()
        {
            double[,] data = Sample.Data.Random_5x4;
            double dataMean = Cima.Statistics.Mean(data);
            Assert.AreEqual(4.835, dataMean, delta: 1e-10);

            double[] flat = Cima.Operations.Flatten(data);
            double flatMean = Cima.Statistics.Mean(flat);
            Assert.AreEqual(flatMean, dataMean);
        }
    }
}
