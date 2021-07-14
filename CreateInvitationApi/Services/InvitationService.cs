using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CreateInvitationApi.Controller;
using CreateInvitationApi.Interfaces;
using CreateInvitationApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace CreateInvitationApi.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiConfiguration _azureCliConfiguration;

        public InvitationService(
            IOptions<ApiConfiguration> azureCliConfiguration,
            HttpClient httpClient)
        {
            _azureCliConfiguration = azureCliConfiguration.Value;
            _httpClient = httpClient;
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        /// Todo: Spaeter nach Providers filtern
        /// </remarks>
        /// <returns></returns>
        public async Task<Invitation> GetInvitationAsync(string email)
        {
            try
            {
                if (email == null)
                    throw new ArgumentNullException(nameof(email));

                if (!IsValidEmail(email))
                    return new Invitation();

                if (_azureCliConfiguration is null)
                    throw new Exception("Error by getting the azureCliConfiguration");

                var invitationProperties = new InvitationProperties
                {
                    Provider = "add",
                    Roles = "reader",
                    UserDetails = email,
                    Domain = _azureCliConfiguration.Domaine,
                    NumHoursToExpiration = 1
                };

                var controller = new WebSiteManagementController(_azureCliConfiguration);
                var result = await controller.CreateUserInvitationLinkAsync(invitationProperties);

                return result;
            }
            catch (ArgumentNullException)
            {
                const string error = "The email is not valid";
                Console.WriteLine(error);
                return null;
            }
            catch (Exception e)
            {
                var error =
                    $"An Error has occured while getting Invitation.\n" +
                    $"Reason: {e.InnerException?.GetBaseException()}";
                Console.WriteLine(error);
                return null;
            }
        }

        /// <summary>
        /// Checks if the given address is valid
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                Console.WriteLine("Email is not Valid and returned false.");
                return false;
            }
        }
    }
}