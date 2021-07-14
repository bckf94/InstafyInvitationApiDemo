using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace CreateInvitationApi.Service
{
    public class ConfigurationManagerService : IConfigurationManager
    {
        public AzureCliSettings AzureCliSettings { get; set; }

        public AzureCliSettings GetAzureCLISettings()
        {
            AzureCliSettings = 
        }
    }
}