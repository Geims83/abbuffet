using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using abbuffet.Models;

namespace abbuffet.Backend.Services
{
    public interface ITablesService
    {
        Task<bool> AddTablesAsync(Tavolo table);
        Task<Tavolo> GetTableAsync(string id);
        Task<bool> UpdateTableAsync(Tavolo table);
        Task<bool> DeleteTableAsync(Tavolo table);
    }
}