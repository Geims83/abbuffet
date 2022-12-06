using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace abbuffet.Backend
{
    public class SignalR
    {
        [FunctionName(nameof(Negotiate))]
        public async Task<SignalRConnectionInfo> Negotiate(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/table/{tableId}/negotiate")] HttpRequest req,
           //string tableId,
           [SignalRConnectionInfo(HubName = "{tableId}")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }


        [FunctionName(nameof(Update))]
        public async Task Update(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/table/{tableId}/update")] HttpRequest req,
            [SignalR(HubName = "{tableId}")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "refresh",
                    Arguments = new[] { content }
                });
        }
    }
}
