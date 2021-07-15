namespace CreateInvitationApi.Models
{
    public class ApiConfiguration
    {
        public string ResourceGroupName { get; set; }
        public string StaticSiteName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TenantId { get; set; }
        public string SubscriptionId { get; set; }
        public string Resource { get; set; }
        public string ApiVersion { get; set; }
        public string Domain { get; set; }
    }
}