using System;
using Microsoft.Extensions.DependencyInjection;
using StatsCounter.Services.GitHubService;
namespace StatsCounter.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddGitHubService(
            this IServiceCollection services,
            Uri baseApiUrl)
        {
            services.AddService<IGitHubService>().AsTransient();
            return services; // TODO: add your code here
        }
    }
}