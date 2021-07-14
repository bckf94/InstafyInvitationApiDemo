using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CreateInvitationApi.Models;
using Microsoft.Azure.Management.WebSites;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.Rest;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using ClientCredential = Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential;

using Azure.Identity;
using Microsoft.Azure.Management.WebSites.Models;
using Microsoft.Rest.Azure;
using Newtonsoft.Json;

namespace CreateInvitationApi.Controller
{
    public class WebSiteManagementController
    {
        private readonly WebSiteManagementClient _client;
        private readonly ApiConfiguration _options;
        private string _authResult;

        public WebSiteManagementController(ApiConfiguration options)
        {
            _options = options;
            _client =
                new WebSiteManagementClient(GetCredsFromServicePrincipal(
                    tenantId: _options.TenantId,
                    clientId: _options.ClientId,
                    clientSecret: _options.ClientSecret))
                { SubscriptionId = _options.SubscriptionId };
        }

        private TokenCredentials GetCredsFromServicePrincipal(string tenantId, string clientId, string clientSecret)
        {
            var authority = @"https://login.microsoftonline.com/" + tenantId;
            var authContext = new AuthenticationContext(authority);
            var credential = new ClientCredential(clientId, clientSecret);
            var authResult = authContext.AcquireTokenAsync("https://management.azure.com", credential).GetAwaiter().GetResult();
            _authResult = authResult.AccessToken;
            return new TokenCredentials(authResult.AccessToken);
        }

        public async Task<Invitation> CreateUserInvitationLinkAsync(InvitationProperties properties)
        {
            try
            {
                var headers = new Dictionary<string, List<string>>();

                var response = await _client.StaticSites.CreateUserRolesInvitationLinkWithHttpMessagesAsync(
                    resourceGroupName: _options.ResourceGroupName,
                    name: _options.StaticSiteName,
                    staticSiteUserRolesInvitationEnvelope: SetInvitationRequestResource(properties)

                );

                var resultContent = await response.Response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Invitation>(resultContent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public StaticSiteUserInvitationRequestResource SetInvitationRequestResource(InvitationProperties invitationProperties)
        {
            try
            {
                return new StaticSiteUserInvitationRequestResource
                {
                    Domain = invitationProperties.Domain,
                    UserDetails = invitationProperties.UserDetails,
                    Roles = invitationProperties.Roles,
                    NumHoursToExpiration = invitationProperties.NumHoursToExpiration,
                    Provider = invitationProperties.Provider
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}