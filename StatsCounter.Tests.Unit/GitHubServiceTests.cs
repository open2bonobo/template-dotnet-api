using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using FluentAssertions;
using Moq;
using Moq.Protected;
using StatsCounter.Models;
using StatsCounter.Services;
using Xunit;

namespace StatsCounter.Tests.Unit
{
    public class GitHubServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandler;
        private readonly IGitHubService _gitHubService;

        public GitHubServiceTests()
        {
            _httpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("open2bonobo");
            _gitHubService = new GitHubService(httpClient, new Uri("https://api.github.com/"));
        }
        
        [Fact]
        public async Task ShouldDeserializeResponse()
        {
            // given
            _httpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{\"id\":1,\"name\":\"name\",\"stargazers_count\":2,\"watchers_count\":3,\"forks_count\":4,\"size\":5}]")
                });
            var httpClient = new HttpClient(_httpMessageHandler.Object);

            // when
            var result = await new GitHubService(httpClient, new Uri("https://api.github.com/"))
                .GetRepositoryInfosByOwnerAsync("owner");
            
            // then
            result.Should().BeEquivalentTo(
                new List<RepositoryInfo>
                {
                    new RepositoryInfo
                    {
                        Id = 1,
                        Name = "name",
                        StargazersCount = 2,
                        WatchersCount = 3,
                        ForksCount = 4,
                        Size = 5
                    }
                }.AsEnumerable());
        }
    }
}