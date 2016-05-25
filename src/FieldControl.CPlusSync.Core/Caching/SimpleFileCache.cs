using FieldControl.CPlusSync.Core.Logging;
using Newtonsoft.Json;
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
                FileLog.WriteLineIf("Arquivo de cache criado.");
                var f = File.Create(cacheFilePath);
                f.Close();
                f.Dispose();
            }
        }

        public void DeleteFileIfExpired()
        {
            if (File.Exists(cacheFilePath))
            {
                var content = File.ReadAllText(cacheFilePath);
                if (!string.IsNullOrEmpty(content))
                {
                    var cacheModel = JsonConvert.DeserializeObject<CacheModel>(content);
                    FileLog.WriteLineIf("Arquivo de cache carregado");
                    if (cacheModel.CreatedAt.Date != DateTime.Now.Date)
                    {
                        FileLog.WriteLineIf("Token de autenticação expirado, invalidando cache");
                        File.Delete(cacheFilePath);
                    }
                    else
                    {
                        FileLog.WriteLineIf("Cache ainda válido");
                    }
                }
            }
        }

        public string GetOrPut(Func<string> putAction)
        {
            DeleteFileIfExpired();
            CreateFileCache();

            var content = File.ReadAllText(cacheFilePath);
            var accessToken = string.Empty;

            if (string.IsNullOrEmpty(content))
            {
                using (var sw = new StreamWriter(cacheFilePath))
                {
                    accessToken = putAction();
                    FileLog.WriteLineIf("Token de autenticação criado");
                    var cacheModel = new CacheModel(accessToken);
                    sw.WriteLine(JsonConvert.SerializeObject(cacheModel));
                }
            }
            else
            {
                var cacheModel = JsonConvert.DeserializeObject<CacheModel>(content);
                accessToken = cacheModel.AccessToken;
                FileLog.WriteLineIf("Token de autenticação carregado pelo cache");
            }

            return accessToken;
        }
    }
}
