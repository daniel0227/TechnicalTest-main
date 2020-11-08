using Microsoft.Extensions.Configuration;

namespace TechnicalTest.Web.Helpers
{
    public class ConfigurationServiceHelper : IConfigurationServiceHelper
    {
        private readonly IConfiguration _configuration;

        public ConfigurationServiceHelper(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetToken()
        {
            return _configuration["ConfigurationServices:Token"];
        }

        public string GetUrlBaseService()
        {
            return _configuration["ConfigurationServices:UrlBase"];
        }
    }
}
