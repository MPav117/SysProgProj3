using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace SP3
{
    public class RequestHandler
    {
        public static async Task HandleRequest(HttpListenerContext context)
        {
            Console.WriteLine("Pocela obrada zahteva");

            HttpListenerResponse response = context.Response;
            RepositoryObserver observer = new(response);
            RepositoryStream stream = new();

            if (context == null
                || context.Request == null
                || context.Request.QueryString == null
                || context.Request.QueryString.Count != 1)
            {
                await observer.SendResponse("Greska u request-u", 400, "Bad Request");
                return;
            }

            var query = context.Request.QueryString;

            if (query.Keys[0] != "topic" || query[0] == null || query[0] == "")
            {
                await observer.SendResponse("Greska u request-u", 400, "Bad Request");
                return;
            }

            string[] topics = query[0]!.Split(',');
            stream.ObserveOn(TaskPoolScheduler.Default).Subscribe(observer);

            await stream.GetRepositories(topics.Distinct().ToList());
        }
    }
}
