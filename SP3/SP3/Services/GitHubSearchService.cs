using Newtonsoft.Json.Linq;
using SP3.Models;
using System.Text;

namespace SP3.Services
{
    public static class GitHubSearchService
    {
        private static readonly HttpClient client;
        private static readonly string url = "https://api.github.com/search/repositories?per_page=100&q=";
        private static readonly string apikey = "";

        static GitHubSearchService()
        {
            client = new();
            client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
            client.DefaultRequestHeaders.Add("User-Agent", "GitHubSearch");
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
            client.DefaultRequestHeaders.Authorization = new("Bearer", apikey);
        }

        public static async Task<IEnumerable<Repository>> FetchRepositories(List<string> topics)
        {
            StringBuilder sb = new(url + $"topic:{topics[0].ToLower()}");

            if (topics.Count > 1)
            {
                topics.RemoveAt(0);

                foreach (string topic in topics)
                {
                    sb.Append($" topic:{topic}");
                }
            }

            JToken? repos = null;

            try
            {
                var res = await client.GetAsync(sb.ToString());

                repos = JObject.Parse(await res.Content.ReadAsStringAsync())["items"];
            }
            catch (Exception)
            {
                throw;
            }

            if (repos == null)
            {
                return [];
            }

            return repos.Select(repo => new Repository
            {
                Name = (string?)repo["full_name"] ?? "N/A",
                URL = (string?)repo["html_url"] ?? "N/A",
                Size = (int?)repo["size"] ?? 0,
                NumForks = (int?)repo["forks_count"] ?? 0,
                NumStars = (int?)repo["stargazers_count"] ?? 0
            });
        }
    }
}
