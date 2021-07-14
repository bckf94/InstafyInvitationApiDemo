using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CreateInvitationApi.Models;
using Microsoft.Azure.Management.WebSites;
using Microsoft.Azure.Management.WebSites.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CreateInvitationApi
{
    public interface IManagementService
    {
        StaticSiteUserInvitationRequestResource SetInvitationRequestResource(InvitationProperties invitationProperties);

        Task<Invitation> GetUserInvitationLinkAync(
            StaticSiteUserInvitationRequestResource requestResource, string token, string url);
    }

    public class ManagementService : IManagementService
    {
        private readonly ApiConfiguration _azureCliConfiguration;

        public ManagementService(ApiConfiguration azureCliConfiguration)
        {
            _azureCliConfiguration = azureCliConfiguration;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="invitationProperties"></param>
        /// <returns></returns>
        public async Task<Invitation> RunAsync(InvitationProperties invitationProperties)
        {
            var token = await AuthenticationHelper.AcquireTokenAsync(
                tenantId: _azureCliConfiguration.TenantId,
                clientId: _azureCliConfiguration.ClientId,
                clientSecret: _azureCliConfiguration.ClientSecret);

            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("Token is null or empty.");
                return await Task.FromResult<Invitation>(null);
            }

            var requestResource = SetInvitationRequestResource(invitationProperties);

            if (requestResource == null)
            {
                Console.WriteLine("Error by setting StaticSiteUserInvitationResponseResource.");
                return await Task.FromResult<Invitation>(null);
            }

            var url = GetAzureCliEndpoint();

            if (string.IsNullOrWhiteSpace(url))
            {
                Console.WriteLine("Error by getting url");
                return await Task.FromResult<Invitation>(null);
            }

            return await GetUserInvitationLinkAync(requestResource, token, url);
        }

        /// <summary>
        /// Set the invitation request body
        /// </summary>
        /// <param name="invitationProperties"></param>
        /// <returns></returns>
        public StaticSiteUserInvitationRequestResource SetInvitationRequestResource(InvitationProperties invitationProperties)
        {
            try
            {
                return new StaticSiteUserInvitationRequestResource
                {
                    Domain = invitationProperties.Domaine,
                    UserDetails = invitationProperties.UserDetails,
                    Roles = invitationProperties.Roles,
                    NumHoursToExpiration = invitationProperties.NumHoursToExpiration,
                    Provider = invitationProperties.Provider,
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// Sends Http Post Request and Gets the UserInvitationLink
        /// </summary>
        /// <param name="requestResource"></param>
        /// <param name="token"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<Invitation> GetUserInvitationLinkAync(StaticSiteUserInvitationRequestResource requestResource, string token, string url)
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(_azureCliConfiguration.Resource);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var body = JsonSerializer.Serialize(new { properties = requestResource },
                    new JsonSerializerOptions { IgnoreNullValues = true });
                var content = new StringContent(body, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Status:  {0}", response.StatusCode);
                    Console.WriteLine("Content: {0}", await response.Content.ReadAsStringAsync());
                }

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Invitation>(responseContent);
            }
            catch (JsonSerializationException e)
            {
                Console.WriteLine($"Error by deserializing Invitation. Reason: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Get the Azure static endpoint
        /// </summary>
        /// <returns></returns>
        private string GetAzureCliEndpoint()
        {
            if (_azureCliConfiguration is null)
                return string.Empty;

            return
                $"/subscriptions/{_azureCliConfiguration.SubscriptionId}" +
                $"/resourcegroups/{_azureCliConfiguration.ResourceGroupName}" +
                $"/providers/Microsoft.Web/staticSites/{_azureCliConfiguration.StaticSiteName}" +
                $"/createUserInvitation?api-version={_azureCliConfiguration.ApiVersion}";
        }
    }
}