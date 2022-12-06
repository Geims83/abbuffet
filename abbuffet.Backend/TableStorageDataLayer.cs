using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using abbuffet.Backend.Services;
using abbuffet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace abbuffet.Backend 
{
    public class TableStorageDataLayer 
    {
        private readonly IOrdersService _orderService;
        private readonly ITablesService _tableService;

        public TableStorageDataLayer (IOrdersService orderService,
            ITablesService tablesService) 
        {
            _orderService = orderService;
            _tableService = tablesService;
        }

        #region Tavoli

        [FunctionName (nameof (Table_Create))]
        public async Task<IActionResult> Table_Create (
            [HttpTrigger (AuthorizationLevel.Function, "post", Route = "v1/tables/")] HttpRequest req,
            ILogger log
        ) 
        {
            var content = await req.ReadAsStringAsync();
            var tavolo = JsonConvert.DeserializeObject<Tavolo>(content);
            var insert = await _tableService.AddTablesAsync(tavolo);

            return new OkResult ();
        }

         [FunctionName (nameof (Table_Read))]
        public async Task<IActionResult> Table_Read (
            [HttpTrigger (AuthorizationLevel.Function, "get", Route = "v1/tables/{tableId}")] HttpRequest req,
            string tableId,
            ILogger log
        ) 
        {
            var tavolo = await _tableService.GetTableAsync(tableId);

            if (tavolo is null)
                return new NotFoundResult ();
            else
                return new OkObjectResult (tavolo);
        }

        [FunctionName (nameof (Table_Update))]
        public async Task<IActionResult> Table_Update (
            [HttpTrigger (AuthorizationLevel.Function, "patch", Route = "v1/tables/{tableId}")] HttpRequest req,
            string tableId,
            ILogger log
        ) 
        {
            var body = await req.ReadAsStringAsync ();
            var tavolo = JsonConvert.DeserializeObject<Tavolo> (body);

            var insert = await _tableService.UpdateTableAsync(tavolo);

            return new OkResult ();
        }

        [FunctionName (nameof (Table_Delete))]
        public async Task<IActionResult> Table_Delete (
            [HttpTrigger (AuthorizationLevel.Function, "delete", Route = "v1/tables/{tableId}")] HttpRequest req,
            string tableId,
            ILogger log
        ) 
        {
            var tavolo = await _tableService.GetTableAsync (tableId);
            if (tavolo is null) {
                log.LogError ($"queryed tables with id {tableId}, returned NULL");
                return new NotFoundObjectResult (HttpStatusCode.NotFound);
            }

            var deleted = await _tableService.DeleteTableAsync (tavolo);
            if (!deleted)
                return new BadRequestResult ();
            log.LogInformation ($"deleted Table with id {tableId}");

            return new OkResult ();
        }

        #endregion

        #region Ordini

        [FunctionName(nameof(Ordine_Create))]
        public async Task<IActionResult> Ordine_Create (
            [HttpTrigger (AuthorizationLevel.Function, "post", Route = "v1/tables/{tableId}/orders/")] HttpRequest req,
            string tableId,
            ILogger log
        ) 
        {
            var body = await req.ReadAsStringAsync ();
            var ordine = JsonConvert.DeserializeObject<Ordine> (body);

            var insert = await _orderService.AddOrdineAsync (ordine);

            return new OkResult ();
        }

        [FunctionName (nameof (Ordine_Read))]
        public async Task<IActionResult> Ordine_Read (
            [HttpTrigger (AuthorizationLevel.Function, "get", Route = "v1/tables/{tableId}/orders/{orderId}")] HttpRequest req,
            string tableId,
            string orderId,
            ILogger log
        ) 
        {
            var ordine = await _orderService.GetOrdineAsync (tableId, orderId);

            if (ordine is null)
                return new NotFoundResult ();
            else
                return new OkObjectResult (ordine);
        }

        [FunctionName (nameof (Ordine_ReadAllByTable))]
        public async Task<IActionResult> Ordine_ReadAllByTable (
            [HttpTrigger (AuthorizationLevel.Function, "get", Route = "v1/tables/{tableId}/orders")] HttpRequest req,
            string tableId,
            ILogger log
        ) 
        {
            var ordine = await _orderService.GetAllOrdineByTavoloAsync(tableId);

            if (ordine is null)
                return new NotFoundResult ();
            else
                return new OkObjectResult (ordine);
        }

        [FunctionName (nameof (Ordine_Update))]
        public async Task<IActionResult> Ordine_Update (
            [HttpTrigger (AuthorizationLevel.Function, "patch", Route = "v1/tables/{tableId}/orders/{orderId}")] HttpRequest req,
            string tableId,
            string orderId,
            ILogger log
        ) 
        {
            var body = await req.ReadAsStringAsync ();
            var ordine = JsonConvert.DeserializeObject<Ordine> (body);

            var insert = await _orderService.UpdateOrdineAsync (ordine);

            return new OkResult ();
        }

        [FunctionName (nameof (Ordine_Delete))]
        public async Task<IActionResult> Ordine_Delete (
            [HttpTrigger (AuthorizationLevel.Function, "delete", Route = "v1/tables/{tableId}/orders/{orderId}")] HttpRequest req,
            string tableId,
            string orderId,
            ILogger log
        ) 
        {
            var ordine = await _orderService.GetOrdineAsync (tableId, orderId);
            if (ordine is null) {
                log.LogError ($"queryed tables with id {orderId}, returned NULL");
                return new NotFoundObjectResult (HttpStatusCode.NotFound);
            }

            var deleted = await _orderService.DeleteOrdineAsync (ordine);
            if (!deleted)
                return new BadRequestResult ();
            log.LogInformation ($"deleted order with id {orderId}");

            return new OkResult ();
        }
        #endregion
    }
}