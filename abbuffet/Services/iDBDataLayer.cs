using TG.Blazor.IndexedDB;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using abbuffet.Models;

namespace abbuffet.Services
{
    public class iDBDataLayer : IDisposable, IAbbuffetDataLayer
    {
        private readonly IndexedDBManager _dbManager;
        public iDBDataLayer(IndexedDBManager dbManager)
        {
            this._dbManager = dbManager;
            _dbManager.ActionCompleted += OnIndexedDbNotification;
        }

        protected void OnIndexedDbNotification(object sender, IndexedDBNotificationArgs args)
        {
            Console.WriteLine(args.Message);
        }

        public void Dispose()
        {
            _dbManager.ActionCompleted -= OnIndexedDbNotification;
        }

        public async Task Tavoli_Add(Tavolo t)
        {
            var row = new StoreRecord<Tavolo>
            {
                Storename = "Tavoli",
                Data = t
            };

            await _dbManager.AddRecord(row);
        }

        public async Task Tavoli_Update(Tavolo t)
        {
            var row = new StoreRecord<Tavolo>
            {
                Data = t,
                Storename = "Tavoli"
            };

            await _dbManager.UpdateRecord<Tavolo>(row);
        }

        public async Task Tavoli_Delete(Tavolo t)
        {
            await _dbManager.DeleteRecord("Tavoli", t.IdTavolo.ToString());
        }

        public async Task<List<Tavolo>> Tavoli_GetAll()
        {
            return await _dbManager.GetRecords<Tavolo>("Tavoli");
        }

        public async Task<Tavolo> Tavoli_GetById(string id)
        {
            return await _dbManager.GetRecordById<string, Tavolo>("Tavoli", id);
        }

        public async Task Ordini_Add(Ordine o)
        {
            var row = new StoreRecord<Ordine>
            {
                Storename = "Ordini",
                Data = o
            };

            await _dbManager.AddRecord(row);
        }

         public async Task Ordini_Update(Ordine o)
        {
            var row = new StoreRecord<Ordine>
            {
                Data = o,
                Storename = "Ordini"
            };

            await _dbManager.UpdateRecord<Ordine>(row);
        }

        public async Task Ordine_Delete(Ordine o)
        {
            await _dbManager.DeleteRecord("Ordini", o.IdOrdine.ToString());
        }

        public async Task<Ordine> Ordine_GetById(Ordine o)
        {
            return await _dbManager.GetRecordById<string, Ordine>("Ordini", o.IdOrdine.ToString());
        }

        public async Task<IEnumerable<Ordine>> Ordine_GetByTavoloId(string tavoloId)
        {
            var indexSearch = new StoreIndexQuery<string>
            {
                Storename = "Ordini",
                IndexName = "TavoloId",
                QueryValue = tavoloId
            };

            return await _dbManager.GetAllRecordsByIndex<string, Ordine>(indexSearch);
        }


    }
}