using Microsoft.Extensions.DependencyInjection;

namespace Madailei.ProcessManagement.BpmClient.LogOutput
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBpmClientZeebe(this IServiceCollection services)
        {
            return services
                .AddBpmClientBase()
                .AddSingleton<IBpmClient, BpmLogOutputService>();
        }
    }
}
