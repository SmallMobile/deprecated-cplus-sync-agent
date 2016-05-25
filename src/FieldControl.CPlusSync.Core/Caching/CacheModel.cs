using System;

namespace FieldControl.CPlusSync.Core.Caching
{
    public class CacheModel
    {
        public CacheModel()
        {
            CreatedAt = DateTime.Now.Date;
        }

        public CacheModel(string accessToken) : this()
        {
            AccessToken = accessToken;
        }

        public string AccessToken { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
