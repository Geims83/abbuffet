using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using abbuffet.Models;

namespace abbuffet.Services
{
    public interface IAbbuffetDataLayer
    {
        Task Tavoli_Add(Tavolo t);
        Task Tavoli_Update(Tavolo t);
        Task Tavoli_Delete(Tavolo t);
        Task<List<Tavolo>> Tavoli_GetAll();
        Task<Tavolo> Tavoli_GetById(string id);

        Task Ordini_Add(Ordine o);
        Task Ordini_Update(Ordine o);
        Task Ordine_Delete(Ordine o);

        //Task<List<Ordine>> Ordine_GetAll();

        //Task<Ordine> Ordine_GetById(string id, string tavoloId);

        Task<IEnumerable<Ordine>> Ordine_GetByTavoloId(string tavoloId);


    }
}