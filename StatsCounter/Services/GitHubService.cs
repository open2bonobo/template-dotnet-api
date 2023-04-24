using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using StatsCounter.Models;


namespace StatsCounter.Services
{
    public interface IGitHubService
    {
        Task<IEnumerable<RepositoryInfo>> GetRepositoryInfosByOwnerAsync(string owner);
    }

    public class GitHubService : IGitHubService
    {
        private readonly HttpClient _httpClient;
        private readonly Uri _baseApiUrl;

        public GitHubService(HttpClient httpClient, Uri baseApiUrl)
        {
            _httpClient = httpClient;
            _baseApiUrl = baseApiUrl;
        }

        public async Task<IEnumerable<RepositoryInfo>> GetRepositoryInfosByOwnerAsync(string owner)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(owner);

                var response = await _httpClient.GetAsync($"{_baseApiUrl}users/{owner}/repos").ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var output = JsonSerializer.Deserialize<IEnumerable<RepositoryInfo>>(content);
                return output;
            }
            catch (HttpRequestException ex)
            {
               
                Console.WriteLine($"HTTP request failed: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON deserialization failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return null;
        }
    }
}
