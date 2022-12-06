using abbuffet.Models;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Threading.Tasks;
using WebAppServiceLibrary.Attributes;

namespace abbuffet.Backend.Services.Implementations
{
    [Service(typeof(ITablesService), ServiceLifetime.Transient)]
    public class TablesService : ITablesService
    {
        private string _connString => Environment.GetEnvironmentVariable("AzureTableStorage");
        private static CloudTable _tables;

        public TablesService()
        {
            CloudStorageAccount storageAcc = CloudStorageAccount.Parse(_connString);
            var client = storageAcc.CreateCloudTableClient(new TableClientConfiguration());
            _tables = client.GetTableReference("tavoli");
        }
      
        public async Task<bool> AddTablesAsync(Tavolo table)
        {
            TableOperation insertOperation = TableOperation.Insert(table);
            var result = await _tables.ExecuteAsync(insertOperation);
            return result.HttpStatusCode == (int)HttpStatusCode.OK;
        }

        public async Task<Tavolo> GetTableAsync(string id)
        {
            var getOperation = TableOperation.Retrieve<Tavolo>("Tavoli", id);
            var result = await _tables.ExecuteAsync(getOperation);
            var table = result.Result as Tavolo;

            return table;
        }

        public async Task<bool> UpdateTableAsync(Tavolo table)
        {
            var updateOperation = TableOperation.InsertOrReplace(table);
            var result = await _tables.ExecuteAsync(updateOperation);

            return result.HttpStatusCode == (int)HttpStatusCode.OK;
        }

        public async Task<bool> DeleteTableAsync(Tavolo table)
        {
            TableOperation deleteOperation = TableOperation.Delete(table);
            var result = await _tables.ExecuteAsync(deleteOperation);

            return result.HttpStatusCode == (int)HttpStatusCode.NoContent;
        }
    }
}