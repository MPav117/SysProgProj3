using SP3.Models;
using System.Net;
using System.Text;

namespace SP3
{
    public class RepositoryObserver(HttpListenerResponse response) : IObserver<Repository>
    {
        private readonly StringBuilder sb = new();
        private readonly HttpListenerResponse response = response;

        public void OnCompleted()
        {
            _ = SendResponse("Uspesna obrada zahteva", 200, "OK",
                sb.Length > 0 ? sb.ToString() : "No such repositories found");
        }

        public void OnError(Exception error)
        {
            _ = SendResponse(error.Message, 500, "Internal server error");
        }

        public void OnNext(Repository value)
        {
            sb.AppendLine(value + "\n");
        }

        public async Task SendResponse(string toPrint, int code, string desc, string body = "")
        {
            Console.Write(toPrint + "\n\n");
            response.StatusCode = code;
            response.StatusDescription = desc;
            string responseString;

            if (code != 200)
            {
                responseString = $"<HTML><BODY>{code} {desc}</BODY></HTML>";
            }
            else
            {
                responseString = $"<!DOCTYPE html><HTML><BODY style=white-space:pre-line;>{body}</BODY></HTML>";
            }

            await response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes(responseString));
            response.Close();
        }
    }
}
