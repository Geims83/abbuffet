using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TG.Blazor.IndexedDB;
using abbuffet.Services;

namespace abbuffet
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            var baseAddress = builder.Configuration["APIEndpoint"] ?? builder.HostEnvironment.BaseAddress;
            builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(baseAddress + "api/v1/") });

            builder.Services.AddIndexedDB(dbStore =>
            {
                dbStore.DbName = "abbuffet";
                dbStore.Version = 1;

                dbStore.Stores.Add(new StoreSchema
                {
                    Name = "Tavoli",
                    PrimaryKey = new IndexSpec { Name = "IdTavolo", KeyPath = "IdTavolo", Unique = true },
                    Indexes = new List<IndexSpec>
                    {
                        new IndexSpec{ Name = "Descrizione", KeyPath = "Descrizione", Unique = true }
                    }
                });

                dbStore.Stores.Add(new StoreSchema
                {
                    Name = "Ordini",
                    PrimaryKey = new IndexSpec { Name = "IdOrdine", KeyPath = "IdOrdine", Unique = true },
                    Indexes = new List<IndexSpec>
                    {
                        new IndexSpec { Name = "Utente", KeyPath = "Utente" },
                        new IndexSpec { Name = "Piatto", KeyPath = "Piatto" },
                        new IndexSpec { Name = "TavoloId", KeyPath = "TavoloId" }
                    }
                });

                dbStore.Stores.Add(new StoreSchema
                {
                    Name = "Piatti",
                    PrimaryKey = new IndexSpec { Name = "Nome", KeyPath = "Nome", Unique = true }
                });
            });

            builder.Services.AddScoped<iDBDataLayer, iDBDataLayer>();

            builder.Services.AddScoped<TableStorageDataLayer, TableStorageDataLayer>();

            await builder.Build().RunAsync();
        }
    }
}
