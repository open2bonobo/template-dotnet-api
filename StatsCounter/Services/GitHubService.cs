using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using StatsCounter.Models;
using System.Text.Json.Serialization;


namespace StatsCounter.Services
{
    public interface IGitHubService
    {
        Task<IEnumerable<RepositoryInfo>> GetRepositoryInfosByOwnerAsync(string owner);
    }
    
    public class GitHubService : IGitHubService
    {
        private readonly HttpClient _httpClient;

        public GitHubService(HttpClient httpClient, IGitHubService git)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<RepositoryInfo>> GetRepositoryInfosByOwnerAsync(string owner)
        {
            using var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("User-Agent", "MyApp"); 

var repo = "docs";

var response = await httpClient.GetAsync($"https://api.github.com/repos/{owner}/{repo}");
var content = await response.Content.ReadAsStringAsync();
            var output = JsonSerializer.Deserialize<List<RepositoryInfo>>(content);

            return output; // TODO: add your code here
        }
    }
}
