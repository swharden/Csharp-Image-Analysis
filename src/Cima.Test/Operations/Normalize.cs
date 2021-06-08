using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cima.Test.Operations
{
    class Normalize
    {
        [Test]
        public void Test_Normalize_SumIsOne()
        {
            double[,] data = Cima.Generate2D.Random(new Random(0), 7, 5);
            Assert.AreNotEqual(1, Cima.Statistics.Sum(data));

            Cima.Operations.NormalizeInPlace(data);
            Assert.AreEqual(1, Cima.Statistics.Sum(data));
        }
    }
}
