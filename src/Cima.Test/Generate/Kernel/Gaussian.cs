using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cima.Test.Generate.Kernel
{
    class Gaussian
    {
        [Test]
        public void Test_Gaussian1d_VsPython()
        {
            // values generated with Python and scipy
            // https://docs.scipy.org/doc/scipy/reference/generated/scipy.signal.windows.gaussian.html

            // signal.gaussian(25, 3)
            double[] expected =
            {
                0.00033546262790251185, 0.001203859994828203, 0.0038659201394728076, 0.011108996538242306,
                0.028565500784550377, 0.06572852861653047, 0.1353352832366127, 0.24935220877729622,
                0.41111229050718745, 0.6065306597126334, 0.8007374029168081, 0.9459594689067654, 1.0,
                0.9459594689067654, 0.8007374029168081, 0.6065306597126334, 0.41111229050718745, 0.24935220877729622,
                0.1353352832366127, 0.06572852861653047, 0.028565500784550377, 0.011108996538242306,
                0.0038659201394728076, 0.001203859994828203, 0.00033546262790251185
            };

            Assert.AreEqual(expected, Cima.Generate.Kernel.Gaussian1D(25, 3));
        }

        [Test]
        public void Test_Gaussian2d_VsPython()
        {
            double[,] expected =
            {
                {0.36787944, 0.53526143, 0.60653066, 0.53526143, 0.36787944},
                {0.53526143, 0.77880078, 0.8824969,  0.77880078, 0.53526143},
                {0.60653066, 0.8824969,  1.0,        0.8824969,  0.60653066},
                {0.53526143, 0.77880078, 0.8824969,  0.77880078, 0.53526143},
                {0.36787944, 0.53526143, 0.60653066, 0.53526143, 0.36787944}
            };
            double[] expectedFlat = Cima.Operations.Flatten(expected);

            double[,] got = Cima.Generate.Kernel.Gaussian2D(5, 2, normalize: false);
            double[] gotFlat = Cima.Operations.Flatten(got);

            for (int i = 0; i < gotFlat.Length; i++)
                Assert.AreEqual(expectedFlat[i], gotFlat[i], 1e-7);
        }
    }
}
