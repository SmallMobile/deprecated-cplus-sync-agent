using System;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;

namespace FieldControl.CPlusSync.Core.Logging
{
    public static class FileLog
    {
        private static string _fileName = null;
        private static string _filePath = ConfigurationManager.AppSettings["logging.filePath"];
        private static string _fullFilePath = null;
        public static bool Verbose { get; set; }

        private static void CreateName()
        {
            _fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + ".txt";
            _fullFilePath = Path.Combine(_filePath, _fileName);
        }

        public static void WriteLine(string message)
        {
            if (_fullFilePath == null)
            {
                CreateName();
            }

            using (StreamWriter sw = new StreamWriter(_fullFilePath, true))
            {
                sw.WriteLine(message);
            }
        }

        public static void WriteJson(object obj)
        {
            if (Verbose) { 
                WriteLine(JsonConvert.SerializeObject(obj, Formatting.None));
            }
        }
    }
}
