using System;
using System.Threading.Tasks;
using StatsCounter.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StatsCounter.Services
{
    public interface IStatsService
    {
        Task<RepositoryStats> GetRepositoryStatsByOwnerAsync(string owner);
    }

    public class StatsService : IStatsService
    {
        private readonly IGitHubService _gitHubService;

        public StatsService(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        public async Task<RepositoryStats> GetRepositoryStatsByOwnerAsync(string owner)
        {
            if (string.IsNullOrWhiteSpace(owner))
            {
                throw new ArgumentException("Owner cannot be null or empty.", nameof(owner));
            }

            var repositoryInfos = await _gitHubService.GetRepositoryInfosByOwnerAsync(owner);
            var repositories = repositoryInfos.ToList();

            if (repositories.Count == 0)
            {
                return new RepositoryStats
                {
                    Owner = owner,
                    Letters = new Dictionary<char, int>(),
                    AvgStargazers = 0,
                    AvgWatchers = 0,
                    AvgForks = 0,
                    AvgSize = 0
                };
            }

            var names = new StringBuilder();
            var repositoryStats = new RepositoryStats
            {
                Owner = owner,
                Letters = new Dictionary<char, int>(),
                AvgStargazers = repositories.Average(r => r.StargazersCount),
                AvgWatchers = repositories.Average(r => r.WatchersCount),
                AvgForks = repositories.Average(r => r.ForksCount),
                AvgSize = repositories.Average(r => r.Size)
            };

            foreach (var repo in repositories)
            {
                names.Append(repo.Name.Replace(" ", "").Trim());
            }

            repositoryStats.Letters = CalculateLetterFrequencies(names.ToString());

            return repositoryStats;
        }

        private Dictionary<char, int> CalculateLetterFrequencies(string str)
        {
            var frequencies = new Dictionary<char, int>();

            foreach (var c in str)
            {
                if (char.IsLetter(c))
                {
                    var letter = char.ToLower(c);
                    if (frequencies.ContainsKey(letter))
                    {
                        frequencies[letter]++;
                    }
                    else
                    {
                        frequencies[letter] = 1;
                    }
                }
            }

            return frequencies;
        }


    }
}