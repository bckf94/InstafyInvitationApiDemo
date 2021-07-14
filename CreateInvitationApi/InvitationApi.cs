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

        [FunctionName("InvitationApi")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string name = req.Query["name"];

                if (string.IsNullOrEmpty(name))
                {
                    var responseMessage = "Email is not specified.";
                    return new BadRequestErrorMessageResult(responseMessage);
                }

                var response = await _invitationService.GetInvitationAsync(name);

                if (response == null)
                    return new NoContentResult();

                return new OkObjectResult(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new InternalServerErrorResult();
            }
        }
    }
}