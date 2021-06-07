using System;
using System.Collections.Generic;
using System.Text;

namespace Cima.Test
{
    public static class TestTools
    {
        public const string TEST_SUBFOLDER = "TEST_OUTPUT";

        public static string GetOutputFilePath(string filename) =>
            System.IO.Path.Combine(GetOutputFolder(), filename);

        public static string GetOutputFolder()
        {
            string outputFolder = System.IO.Path.GetFullPath(TEST_SUBFOLDER);
            if (!System.IO.Directory.Exists(outputFolder))
                System.IO.Directory.CreateDirectory(outputFolder);
            return outputFolder;
        }

        public static void SavePng(double[,] data, string suffix = "")
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            string callingMethod = stackTrace.GetFrame(1).GetMethod().Name;
            suffix = string.IsNullOrWhiteSpace(suffix) ? "" : "-" + suffix;
            string path = System.IO.Path.Combine(GetOutputFolder(), $"{callingMethod}{suffix}.png");
            IO.SavePng(data, path);
            Console.WriteLine($"Saved:\n{path}");
        }
    }
}
