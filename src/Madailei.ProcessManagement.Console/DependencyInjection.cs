using System;
using Madailei.ProcessManagement.BpmClient.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Madailei.ProcessManagement.Console
{
    public static class DependencyInjection
    {
        internal static IServiceCollection AddConfiguration(this IServiceCollection services)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                .Build();

            var BpmConnectSettings = new BpmServerConnectionSettings();
            config.GetSection(typeof(BpmServerConnectionSettings).Name).Bind(BpmConnectSettings);

            return services.AddSingleton<IConfiguration>(config)
                .AddSingleton(BpmConnectSettings);
        }
    }
}
