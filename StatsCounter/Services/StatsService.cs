using System;
using System.Threading.Tasks;
using StatsCounter.Models;

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

        public Task<RepositoryStats> GetRepositoryStatsByOwnerAsync(string owner)
        {

            throw new NotImplementedException(); // TODO: add your code here
        }
        private IDictionary<char, int> CountLetters(IEnumerable<RepositoryDto> repositories)
{
    var letters = new Dictionary<char, int>();

    foreach (var repository in repositories)
    {
        foreach (var letter in repository.Name.ToLower().Where(char.IsLetter))
        {
            if (letters.ContainsKey(letter))
            {
                letters[letter]++;
            }
            else
            {
                letters[letter] = 1;
            }
        }
    }

    return letters;
}
    }
}