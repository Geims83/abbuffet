using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using abbuffet.Models;

namespace abbuffet.Backend.Services
{
    public interface IOrdersService
    {
        Task<bool> AddOrdineAsync(Ordine ordine);
        Task<Ordine> GetOrdineAsync(string tableId, string orderId);
        Task<List<Ordine>> GetAllOrdineByTavoloAsync(string tableId);
        Task<bool> UpdateOrdineAsync(Ordine ordine);
        Task<bool> DeleteOrdineAsync(Ordine ordine);
    }
}