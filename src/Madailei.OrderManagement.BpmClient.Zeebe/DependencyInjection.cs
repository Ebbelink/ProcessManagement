using Microsoft.Extensions.DependencyInjection;

namespace Madailei.ProcessManagement.BpmClient.Zeebe
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBpmClientZeebe(this IServiceCollection services)
        {
            return services
                .AddBpmClientBase()
                .AddSingleton<IBpmClient, ZeebeService>();
        }
    }
}
