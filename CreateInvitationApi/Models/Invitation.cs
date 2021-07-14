using Newtonsoft.Json;

namespace CreateInvitationApi.Models
{
    public class Invitation
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "properties")]
        public Properties Properties { get; set; }
    }

    public class Properties
    {
        [JsonProperty(PropertyName = "expiresOn")]
        public string ExpiresOn { get; set; }

        [JsonProperty(PropertyName = "invitationUrl")]
        public string InvitationUrl { get; set; }
    }
}