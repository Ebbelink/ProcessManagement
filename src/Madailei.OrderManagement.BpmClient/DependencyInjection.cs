using Microsoft.Extensions.DependencyInjection;

namespace Madailei.ProcessManagement.BpmClient
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBpmClientBase(this IServiceCollection services)
        {
            return services;
        }
    }
}
