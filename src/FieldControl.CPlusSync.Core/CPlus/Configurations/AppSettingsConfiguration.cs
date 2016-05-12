using System.Configuration;

namespace FieldControl.CPlusSync.Core.CPlus.Configurations
{
    public class AppSettingsConfiguration : IConfiguration
    {
        public string ConnnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["cplus.connectionString"];
            }
        }
    }
}
