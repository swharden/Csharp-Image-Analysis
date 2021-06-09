using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cima.Test
{
    class QrssTests
    {
        public void Test_QRSS_LoadSystemDrawing()
        {
            string qrssImageFolder = @"C:\Users\scott\Documents\GitHub\QRSSplus\dev\sample-images";
            foreach (string filePath in System.IO.Directory.GetFiles(qrssImageFolder, "*.*"))
            {
                Cima.IO.LoadImage(filePath);
            }
        }

        public void Test_QRSS_LoadImageMagick()
        {
            string qrssImageFolder = @"C:\Users\scott\Documents\GitHub\QRSSplus\dev\sample-images";
            foreach (string filePath in System.IO.Directory.GetFiles(qrssImageFolder, "*.*"))
            {
                Cima.IOMagick.LoadImage(filePath);
            }
        }

        public void Test_Load_Grayscale()
        {
            string qrssImageFolder = @"C:\Users\scott\Documents\GitHub\QRSSplus\dev\sample-images";

            double[,] kernel =
            {
                { -1, 0, 1 },
                { -1, 0, 1 },
                { -1, 0, 1 },
            };

            foreach (string filePath in System.IO.Directory.GetFiles(qrssImageFolder, "*.*"))
            {
                Console.WriteLine(filePath);
                double[,] data = IOSystemDrawing.LoadGrayscale(filePath);
                double[,] convolved = Cima.Operations.Convolve(data, kernel);
                Cima.Statistics.AbsoluteInPlace(convolved);
                double[] means = Cima.Statistics.MeanByColumn(convolved);

                var plt = new ScottPlot.Plot();
                plt.AddSignal(means);
                plt.Title(System.IO.Path.GetFileNameWithoutExtension(filePath));
                plt.SaveFig($"test-{System.IO.Path.GetFileNameWithoutExtension(filePath)}-graph.png");
                Cima.IO.SavePng(data, $"test-{System.IO.Path.GetFileNameWithoutExtension(filePath)}-data.png");
            }
        }
    }
}
