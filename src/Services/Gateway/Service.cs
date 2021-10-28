using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLib.Models.ErrorModels;
using Microsoft.AspNetCore.Http;

namespace Gateway
{
    public class Service
    {
        private string _hostName;
        private int _port;

        public Service(string hostName, int port)
        {
            _hostName = hostName;
            _port = port;
        }
        
        public async Task<(JsonDocument doc, HttpStatusCode StatusCode)> Route(HttpContext httpContext, string path = null, object bodyArg = null)
        {
            path ??= httpContext.Request.Path;
            var sendingRequest = new HttpRequestMessage();
            sendingRequest.RequestUri = new Uri($"http://{_hostName}:{_port}{path}");
            if (!string.IsNullOrEmpty(httpContext.Request.Headers["Authorization"]))
                sendingRequest.Headers.Add("Authorization",httpContext.Request.Headers["Authorization"].ToString());
            if (bodyArg != null) sendingRequest.Content = JsonContent.Create(bodyArg, bodyArg.GetType());
            sendingRequest.Method = new HttpMethod(httpContext.Request.Method);
            var client = new HttpClient();
            var result =  client.SendAsync(sendingRequest).Result;
            httpContext.Response.Headers.Add("Content-Type", "application/json");
            return result.StatusCode switch
            {
                HttpStatusCode.Unauthorized => throw new UnAuthorized(),
                HttpStatusCode.BadRequest => throw new BadRequest(),
                HttpStatusCode.Conflict => throw new Conflict(),
                HttpStatusCode.NotFound => throw new NotFound(),
                _ =>  (await JsonDocument.ParseAsync(result.Content.ReadAsStreamAsync().Result), result.StatusCode)
            };
        }


    }
}