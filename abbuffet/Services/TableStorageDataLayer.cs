using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using abbuffet.Models;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace abbuffet.Services
{
    public class TableStorageDataLayer : IAbbuffetDataLayer, IDisposable
    {
        //private readonly AppConfiguration _config;

        private HttpClient _client;

        public TableStorageDataLayer(//AppConfiguration config, 
            HttpClient client)
        {
            //this._config = config;
            this._client = client;
        }

        public void Dispose()
        {
        }

        #region Tavoli

        public async Task Tavoli_Add(Tavolo t)
        {
            var url = "tables";
            await _client.PostAsJsonAsync(url, t);
            Log(nameof(this.Tavoli_Add), $" added table with id {t.IdTavolo}");
        }

        public async Task Tavoli_Update(Tavolo t)
        {
            var url = $"tables/{t.IdTavolo}";
            await _client.PatchAsJsonAsync<Tavolo>(url, t);
            Log(nameof(this.Tavoli_Update), $" updated table with id {t.IdTavolo}");
        }

        public async Task Tavoli_Delete(Tavolo t)
        {
            var url = $"tables/{t.IdTavolo}";
            await _client.DeleteAsync(url);
            Log(nameof(this.Tavoli_Delete), $" deleted table with id {t.IdTavolo}");
        }

        public Task<List<Tavolo>> Tavoli_GetAll()
        {
            throw new System.NotImplementedException();
        }

        public async Task<Tavolo> Tavoli_GetById(string id)
        {
            var url = $"tables/{id}";
            var t = await _client.GetFromJsonAsync<Tavolo>(url);
            if (t != null)
                Log(nameof(this.Tavoli_GetById), $" queryed tables with id {id}, returned valid table");
            else
                Log(nameof(this.Tavoli_GetById), $" queryed tables with id {id}, returned NULL");
            return t;
        }

        #endregion

        #region Ordini
        public async Task Ordini_Add(Ordine o)
        {
            var url = $"tables/{o.TavoloId}/orders";
            await _client.PostAsJsonAsync(url, o);
            Log(nameof(this.Ordini_Add), $" added order with id {o.IdOrdine}");

        }

        public async Task Ordini_Update(Ordine o)
        {
            var url = $"tables/{o.TavoloId}/orders/{o.IdOrdine}";
            await _client.PatchAsJsonAsync(url, o);
            Log(nameof(this.Ordini_Update), $" updated order with id {o.IdOrdine}");
        }

        public async Task Ordine_Delete(Ordine o)
        {
            var url = $"tables/{o.TavoloId}/orders/{o.IdOrdine}";
            var resp = await _client.DeleteAsync(url);
            if (resp.IsSuccessStatusCode)
                Log(nameof(this.Ordine_Delete), $" deleted order with id {o.IdOrdine}");
            else
                Log(nameof(this.Ordine_Delete), $" ERROR deleting order with id {o.IdOrdine}");
        }

        public async Task<IEnumerable<Ordine>> Ordine_GetByTavoloId(string tavoloId)
        {
            var url = $"tables/{tavoloId}/orders";
            var lista = await _client.GetFromJsonAsync<List<Ordine>>(url);
            Log(nameof(this.Ordine_GetByTavoloId), $"returned {lista.Count} records");
            return lista;
        }

        #endregion

        private void Log(string method, string message)
        {
            Console.WriteLine($"[TableStorageDataLayer] {method} {message}");
        }
    }
}