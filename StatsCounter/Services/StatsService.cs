using System;
using System.Threading.Tasks;
using StatsCounter.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

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

            var data = await _gitHubService.GetRepositoryInfosByOwnerAsync(owner);

            var repo = data.ToList();
            var repositoryStats = new RepositoryStats()
            {
                Owner = owner,
                Letters = new Dictionary<char, int>(),
                AvgStargazers = 0,
                AvgWatchers = 0,
                AvgForks = 0,
                AvgSize = 0
            };
            string names = String.Empty;
            for (int i = 0; i < repo.Count; i++)
            {
                names += repo[i].Name.Replace(" ", "").Trim();
                //repositoryStats.Id = repo[i].Id;
                repositoryStats.AvgStargazers += repo[i].StargazersCount;
                repositoryStats.AvgWatchers += repo[i].WatchersCount;
                repositoryStats.AvgForks += repo[i].ForksCount;
                repositoryStats.AvgSize += repo[i].Size;
            }
            repositoryStats.Letters = CountLetters(names);
            repositoryStats.AvgStargazers /= repo.Count;
            repositoryStats.AvgWatchers /= repo.Count;
            repositoryStats.AvgForks /= repo.Count;
            repositoryStats.AvgSize /= repo.Count;



            return repositoryStats;
        }
        private Dictionary<char, int> CountLetters(string str)
        {
            Dictionary<char, int> frequencies = new Dictionary<char, int>();

            foreach (char c in str)
            {
                if (char.IsLetter(c))
                {
                    char letter = char.ToLower(c);
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