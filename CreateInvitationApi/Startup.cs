using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Text;
using CreateInvitationApi;
using CreateInvitationApi.Interfaces;
using CreateInvitationApi.Models;
using CreateInvitationApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace CreateInvitationApi
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IInvitationService, InvitationService>();

            builder.Services.AddOptions<ApiConfiguration>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("AzureCliSettings")
                        .Bind(settings);
                });
        }

        public void ConfigureServices(IFunctionsConfigurationBuilder builder)
        {
            //local dev
            var buildConfig = builder.ConfigurationBuilder.Build();
            builder.ConfigurationBuilder
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}