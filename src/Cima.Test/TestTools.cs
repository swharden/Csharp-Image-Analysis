using System;
using System.Collections.Generic;
using System.Text;

namespace Cima.Test
{
    public static class TestTools
    {
        public static void SavePng(double[,] data)
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            string callingMethod = stackTrace.GetFrame(1).GetMethod().Name;
            string path = System.IO.Path.GetFullPath($"{callingMethod}.png");
            IO.SavePng(data, path);
            Console.WriteLine($"Saved: {path}");
        }
    }
}
