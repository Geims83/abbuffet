using System;
using abbuffet.Models;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using System.Net;
using System.Collections.Generic;
using WebAppServiceLibrary.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace abbuffet.Backend.Services.Implementations
{
    [Service(typeof(IOrdersService), ServiceLifetime.Transient)]
    public class OrdersService : IOrdersService
    {
        private string _connString => Environment.GetEnvironmentVariable("AzureTableStorage");
        private static CloudTable _orders;

        public OrdersService()
        {
            CloudStorageAccount storageAcc = CloudStorageAccount.Parse(_connString);
            var client = storageAcc.CreateCloudTableClient(new TableClientConfiguration());
            _orders = client.GetTableReference("ordini");
        }

        public async Task<bool> AddOrdineAsync(Ordine ordine)
        {
            TableOperation insertOperation = TableOperation.Insert(ordine);
            var result = await _orders.ExecuteAsync(insertOperation);
            return result.HttpStatusCode == (int)HttpStatusCode.OK;
        }

        public async Task<Ordine> GetOrdineAsync(string tableId, string orderId)
        {
            var getOperation = TableOperation.Retrieve<Ordine>(tableId, orderId);
            var result = await _orders.ExecuteAsync(getOperation);
            var ordine = result.Result as Ordine;

            return ordine;
        }

        public async Task<List<Ordine>> GetAllOrdineByTavoloAsync(string tableId)
        {
            List<Ordine> lista = new List<Ordine>();
            TableQuery<Ordine> rangeQuery = new TableQuery<Ordine>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, tableId));
            TableContinuationToken token = null;
            rangeQuery.TakeCount = 50;
            do
            {
                TableQuerySegment<Ordine> segment = await _orders.ExecuteQuerySegmentedAsync(rangeQuery, token);
                token = segment.ContinuationToken;
                lista.AddRange(segment);
            }
            while (token != null);
            return lista;
        }


        public async Task<bool> UpdateOrdineAsync(Ordine ordine)
        {
            var updateOperation = TableOperation.InsertOrReplace(ordine);
            var result = await _orders.ExecuteAsync(updateOperation);

            return result.HttpStatusCode == (int)HttpStatusCode.OK;
        }

        public async Task<bool> DeleteOrdineAsync(Ordine ordine)
        {
            TableOperation deleteOperation = TableOperation.Delete(ordine);
            var result = await _orders.ExecuteAsync(deleteOperation);

            return result.HttpStatusCode == (int)HttpStatusCode.NoContent;
        }
    }
}