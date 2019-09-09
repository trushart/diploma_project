using System;
using System.IO;
using System.Linq;

namespace FIT.Diploma.TestingFramework
{
    public class FileLoader
    {
        public static string TestDataPath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\TestData");

        public static string GetFileContent(string fileName)
        {
            var files = GetFiles(fileName);

            if (files.Count() > 1)
                throw new Exception($"Multiple files with name '{fileName}' found.");
            if (files.Count() == 0)
                throw new Exception($"Files with name '{fileName}' not found.");
            return ReadFile(files.Single());
        }

        public static string[] GetFiles(string fileNamePattern)
        {
            return Directory.GetFiles(TestDataPath, fileNamePattern, SearchOption.AllDirectories);
        }

        public static string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
