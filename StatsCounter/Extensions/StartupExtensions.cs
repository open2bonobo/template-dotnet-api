using System;
using Microsoft.Extensions.DependencyInjection;
using StartupExtensions;

namespace StatsCounter.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddGitHubService(
            this IServiceCollection services,
            Uri baseApiUrl)
        {
            services.AddTransient<IGitHubService>();
            return services; // TODO: add your code here
        }
    }
}