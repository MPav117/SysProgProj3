using System.Net;

namespace SP3
{
    internal class GitHubSearchServer
    {
        static async Task Main()
        {
            HttpListener listener = new();

            try
            {
                listener.Prefixes.Add("http://localhost:7777/");
                listener.Start();
            }
            catch (HttpListenerException ex)
            {
                Console.WriteLine(ex.ErrorCode + "\n");
            }

            if (listener.IsListening)
            {
                Console.WriteLine("Server podignut. Pocinje slusanje zahteva\n");

                while (true)
                {
                    try
                    {
                        HttpListenerContext context = await listener.GetContextAsync();
                        Console.WriteLine("Primljen zahtev");
                        
                        _ = Task.Run(() => RequestHandler.HandleRequest(context));
                    }
                    catch (HttpListenerException ex)
                    {
                        Console.WriteLine(ex.ErrorCode + "\n");
                    }
                }
            }
        }
    }
}
