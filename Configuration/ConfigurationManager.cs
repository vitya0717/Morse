using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morse.Configuration 
{
    public class ConfigurationManager
    {
        public ConfigurationManager() { 
            
        }

        public IConfigurationRoot GetConfig()
        {
            var config = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                  .Build();

            return config;
        }
    }
}
