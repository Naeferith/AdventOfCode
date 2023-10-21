using AdventOfCode.Core;
using AdventOfCode.Core.Components;
using AdventOfCode.V2022.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace AdventOfCode
{
    /// <summary>
    /// Application configurator
    /// </summary>
    internal static class Startup
    {
        public static IHostBuilder ConfigureApplication(this IHostBuilder builder)
        {
            return builder
                .ConfigureAppConfiguration(RegisterConfigurations)
                .ConfigureServices(RegisterServices);
        }

        private static void RegisterConfigurations(IConfigurationBuilder configuration)
        {
            // Configure application
        }

        private static void RegisterServices(HostBuilderContext builder, IServiceCollection services)
        {
            services.AddTransient<ICalendar, Calendar>();
            services.Register2022Calendar(ServiceLifetime.Singleton);

            services.TryAddSingleton<IInputAccessor, DefaultInputAccessor>();
        }
    }
}
