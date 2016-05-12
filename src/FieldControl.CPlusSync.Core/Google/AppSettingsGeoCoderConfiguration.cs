using System.Configuration;

namespace FieldControl.CPlusSync.Core.Google
{
    public class AppSettingsGeoCoderConfiguration : IGeoCoderConfiguration
    {
        public string GoogleKey
        {
            get
            {
                return ConfigurationManager.AppSettings["google.geoCoderKey"];
            }
        }
    }
}
