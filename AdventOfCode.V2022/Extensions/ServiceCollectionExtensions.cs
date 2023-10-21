using AdventOfCode.Core.Components;
using AdventOfCode.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace AdventOfCode.V2022.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Register2022Calendar(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            services.AddSingleton<IYearAccessor>(new DefaultYearAccessor(2022));

            foreach (var impl in typeof(ServiceCollectionExtensions).Assembly.GetDayImplementations())
            {
                services.Add(new(typeof(IDay), impl, lifetime));
            }

            return services;
        }
    }
}
