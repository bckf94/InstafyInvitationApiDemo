using System;
using System.Collections.Generic;
using System.Text;
using CreateInvitationApi;
using CreateInvitationApi.Service;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace CreateInvitationApi
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IHttpClientService, HttpClientService>();
        }
    }
}