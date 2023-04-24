using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using StatsCounter.Services;

namespace StatsCounter.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddGitHubService(
            this IServiceCollection services,
            Uri baseApiUrl)
        {
            services.AddTransient<IGitHubService, GitHubService>( sp => new GitHubService(new HttpClient(), baseApiUrl));
            return services; // TODO: add your code here
        }
    }
}