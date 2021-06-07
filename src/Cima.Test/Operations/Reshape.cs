using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cima.Test.Operations
{
    class Reshape
    {
        [Test]
        public void Test_Expand_IncreasesDarkSpace()
        {
            Random rand = new(0);
            double[,] data = Generate2D.Random(rand, 300, 200);
            double[,] expanded = Cima.Operations.Expand(data, 10);
            Assert.Less(Cima.Statistics.Mean(expanded), Cima.Statistics.Mean(data));
            Assert.AreEqual(320, expanded.GetLength(1));
            Assert.AreEqual(220, expanded.GetLength(0));
        }

        [Test]
        public void Test_Expand_CustomFill()
        {
            Random rand = new(0);
            double[,] data = Generate2D.Random(rand, 300, 200);
            double[,] expanded = Cima.Operations.Expand(data, 10, 1);
            Assert.Greater(Cima.Statistics.Mean(expanded), Cima.Statistics.Mean(data));
        }

        [Test]
        public void Test_Contract_IsNonDestructive()
        {
            Random rand = new(0);
            double[,] original = Generate2D.Random(rand, 300, 200);
            double[,] expanded = Cima.Operations.Expand(original, 10);
            double[,] contracted = Cima.Operations.Contract(expanded, 10);
            Assert.AreEqual(original, contracted);
        }
    }
}
