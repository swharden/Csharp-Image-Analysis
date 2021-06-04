using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cima.Test
{
    class DisplayTests
    {
        [Test]
        public void Test_Display_IsNotNull()
        {
            double[,] data = Sample.Data.Random_5x4;
            string disp = Display.Array(data);
            Assert.IsNotNull(disp);
            Console.WriteLine(disp);
        }
    }
}
