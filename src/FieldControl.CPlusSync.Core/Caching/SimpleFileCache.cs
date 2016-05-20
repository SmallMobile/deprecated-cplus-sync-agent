using System;
using System.IO;

namespace FieldControl.CPlusSync.Core.Caching
{
    public class SimpleFileCache
    {
        private string cacheFilePath = null;

        public SimpleFileCache(string cacheName)
        {
            cacheFilePath = cacheName + ".cache";
        }

        private void CreateFileCache()
        {
            if (!File.Exists(cacheFilePath))
            {
                var f = File.Create(cacheFilePath);
                f.Close();
                f.Dispose();
            }
        }

        public void CheckIfExpired()
        {
            if (File.Exists(cacheFilePath))
            {
                var createdAt = File.GetCreationTime(cacheFilePath);
                if (createdAt.Date != DateTime.Today.Date)
                {
                    File.Delete(cacheFilePath);
                }
            }
        }

        public string GetOrPut(Func<string> putAction)
        {
            CheckIfExpired();
            CreateFileCache();

            var cacheString = File.ReadAllText(cacheFilePath);

            if (string.IsNullOrEmpty(cacheString))
            {
                using (var sw = new StreamWriter(cacheFilePath))
                {
                    cacheString = putAction();
                    sw.WriteLine(cacheString);
                }
            }

            return cacheString;
        }
    }
}
