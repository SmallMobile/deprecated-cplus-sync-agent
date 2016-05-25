using System;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;

namespace FieldControl.CPlusSync.Core.Logging
{
    public static class FileLog
    {
        private static string _filePath = ConfigurationManager.AppSettings["logging.folderPath"];
        private static string _fullFilePath = Path.Combine(_filePath, "cplus-sync-ok.txt");
        private static string _fullFileErrorPath = null;

        public static bool Verbose { get; set; }

        private static void GenerateLogErrorFileName()
        {
            var fileErrorName = "cplus-sync-errors-" + DateTime.Now.ToString("yyyy.MM.dd-hhmmss") + ".txt";
            _fullFileErrorPath = Path.Combine(_filePath, fileErrorName);
        }

        private static void CreateDirectoryIf()
        {
            if (!Directory.Exists(_filePath))
            {
                Directory.CreateDirectory(_filePath);
            }
        }

        public static void WriteLineIf(string message)
        {
            if (Verbose)
            {
                CreateDirectoryIf();

                using (StreamWriter sw = new StreamWriter(_fullFilePath, true))
                {
                    sw.WriteLine(message);
                }
            }
        }

        public static void WriteLine(string message)
        {
            CreateDirectoryIf();

            using (StreamWriter sw = new StreamWriter(_fullFilePath, true))
            {
                sw.WriteLine(message);
            }
        }

        public static void WriteError(string message)
        {
            CreateDirectoryIf();

            if (string.IsNullOrEmpty(_fullFileErrorPath))
            {
                GenerateLogErrorFileName();
            }

            using (StreamWriter sw = new StreamWriter(_fullFileErrorPath, true))
            {
                sw.WriteLine(message);
            }
        }

        public static void WriteJson(object obj)
        {
            if (Verbose)
            {
                WriteLine(JsonConvert.SerializeObject(obj, Formatting.None));
            }
        }
    }
}
