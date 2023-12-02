using AdventOfCode.Core.Components;
using AdventOfCode.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace AdventOfCode.V2023.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Register2023Calendar(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            services.AddSingleton<IYearAccessor>(new DefaultYearAccessor(2023));

            foreach (var impl in typeof(ServiceCollectionExtensions).Assembly.GetDayImplementations())
            {
                services.Add(new(typeof(IDay), impl, lifetime));
            }

            return services;
        }
    }
}
