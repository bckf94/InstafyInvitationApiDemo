using System;
using System.Collections.Generic;
using System.Text;

namespace CreateInvitationApi.Service
{
    public class AzureCliSettings
    {
        public string ResourceGroupName { get; set; }
        public string StaticSiteName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TenantId { get; set; }
        public string SubscriptionId { get; set; }
        public string Resource { get; set; }
        public string ApiVersion { get; set; }
    }
}