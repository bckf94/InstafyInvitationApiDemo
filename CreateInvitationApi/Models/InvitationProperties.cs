using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CreateInvitationApi.Models
{
    public class InvitationProperties
    {
        [JsonPropertyName("provider")]
        public string Provider { get; set; }

        [JsonPropertyName("domain")]
        public string Domain { get; set; }

        [JsonPropertyName("properties")]
        public string Roles { get; set; }

        [JsonPropertyName("userDetails")]
        public string UserDetails { get; set; }

        [JsonPropertyName("numHoursToExpiration")]
        public int NumHoursToExpiration { get; set; }
    }
}