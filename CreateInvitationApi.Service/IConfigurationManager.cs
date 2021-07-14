using System;
using System.Collections.Generic;
using System.Text;

namespace CreateInvitationApi.Service
{
    public interface IConfigurationManager
    {
        AzureCliSettings GetAzureCLISettings();
    }
}