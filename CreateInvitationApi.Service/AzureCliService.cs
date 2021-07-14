using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CreateInvitationApi.Service
{
    public class AzureCliService : IAzureCliService
    {
        private readonly IAzureCliService _azureCliService;
        private readonly HttpClient _httpClient;

        public AzureCliService(IAzureCliService azureCliService, HttpClient httpClient)
        {
            _azureCliService = azureCliService;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("");
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