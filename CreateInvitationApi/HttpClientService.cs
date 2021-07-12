using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CreateInvitationApi.Service;

namespace CreateInvitationApi
{
    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientService _httpClient;

        public HttpClientService(IHttpClientService httpClientHelper)
        {
            _httpClient = httpClientHelper;
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        /// Todo: Spaeter nach Providers filtern
        /// </remarks>
        /// <returns></returns>
        public async Task<Invitation> GetInvitation(string email)
        {
            try
            {
                if (email == null)
                    throw new ArgumentNullException(nameof(email));

                if (IsValidEmail(email))
                {
                }

                return new Invitation();
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