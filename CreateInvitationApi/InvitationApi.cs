using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using CreateInvitationApi.Controller;
using CreateInvitationApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CreateInvitationApi
{
    public class InvitationApi
    {
        private readonly IInvitationService _invitationService;

        public InvitationApi(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        /// <summary>
        /// Get Request to get the invitation uri
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <example>api/invitation?email=bck_florian@outlook.fr&provider=aad&roles=Administrator&numHoursToExpiration=10
        /// </example>
        /// <returns></returns>
        [FunctionName("InvitationApi")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "invitation")] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string email = req.Query["email"];
                string provider = req.Query["provider"];
                string roles = req.Query["roles"];
                var numHoursToExpiration = Convert.ToInt32(req.Query["numHoursToExpiration"]);

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(roles) || string.IsNullOrWhiteSpace(provider) || numHoursToExpiration < 1)
                    throw new ArgumentNullException(nameof(email));

                var response = await _invitationService.GetInvitationAsync
                (
                    email: email,
                    roles: roles,
                    provider: provider,
                    numHoursToExpiration: numHoursToExpiration);

                if (response == null)
                    return new NoContentResult();

                return new OkObjectResult(response);
            }
            catch (ArgumentNullException e)
            {
                const string error = "Invalid properties values. Check the request.";
                Console.WriteLine("invalid Argument: " + e.Message);
                return new BadRequestErrorMessageResult(error);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new InternalServerErrorResult();
            }
        }
    }
}