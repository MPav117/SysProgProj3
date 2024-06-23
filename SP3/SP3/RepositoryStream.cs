using SP3.Models;
using SP3.Services;
using System.Reactive.Subjects;

namespace SP3
{
    public class RepositoryStream : IObservable<Repository>
    {
        private readonly Subject<Repository> subject = new();
        
        public async Task GetRepositories(List<string> topics)
        {
            try
            {
                var repos = await GitHubSearchService.FetchRepositories(topics);

                if (repos != null)
                {
                    foreach (var repo in repos)
                    {
                        subject.OnNext(repo);
                    }
                }

                subject.OnCompleted();
            }
            catch (Exception e)
            {
                subject.OnError(e);
            }
        }
        public IDisposable Subscribe(IObserver<Repository> observer)
        {
            return subject.Subscribe(observer);
        }
    }
}
