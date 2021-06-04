using System;
using System.Collections.Generic;
using System.Text;

namespace Cima.Test
{
    public static class TestTools
    {
        public static void SavePng(double[,] data, string suffix = "")
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            string callingMethod = stackTrace.GetFrame(1).GetMethod().Name;
            suffix = string.IsNullOrWhiteSpace(suffix) ? "" : "-" + suffix;
            string path = System.IO.Path.GetFullPath($"{callingMethod}{suffix}.png");
            IO.SavePng(data, path);
            Console.WriteLine($"Saved:\n{path}");
        }
    }
}
