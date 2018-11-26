
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public static class HttpTrigger
    {
        private static readonly HttpClient httpclient;
        static HttpTrigger ()
            {
            httpclient = new HttpClient ();
            }
        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> RunAsync (
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post","put","delete", Route = "func/{endpoint}")]
            HttpRequest req, 
            TraceWriter log,
            string endpoint)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
            
            //httpclient.BaseAddress= new System.Uri("https://jsonplaceholder.typicode.com/users");
            httpclient.DefaultRequestHeaders.Accept.Clear ();
            httpclient.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));
            var url = $"https://jsonplaceholder.typicode.com/{endpoint}";
            var newmessage = new HttpRequestMessage(HttpMethod.Get, url);
            
            HttpResponseMessage res =await httpclient.SendAsync(newmessage);
            return res;
        }
    }
}
