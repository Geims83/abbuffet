using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using abbuffet.Models;
using abbuffet.Services;
using Microsoft.AspNetCore.Components;

namespace abbuffet.Pages
{
    public class TableBase : ComponentBase
    {
        [Parameter] public string IdTavolo { get; set; }
        [Inject] iDBDataLayer _dl { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        protected string _utente { get; set; }
        protected string _piatto { get; set; }

        protected Tavolo _tavolo { get; set; }

        public List<Ordine> _ordini { get; set; }

        // protected readonly IAbbuffetDataLayer _dl;
        // public TableBase(IAbbuffetDataLayer dl)
        // {
        //     _dl = dl;
        // }

        protected override async Task OnInitializedAsync()
        {
            _tavolo = await _dl.Tavoli_GetById(IdTavolo);

            await RefreshOrdini();
        }

        protected async Task AggiungiOrdine()
        {
            if (_ordini == null)
                _ordini = new List<Ordine>();

            var o = new Ordine(_tavolo.IdTavolo)
            {
                Utente = _utente,
                Piatto = _piatto
            };

           await _dl.Ordini_Add(o);

            await RefreshOrdini();

            _piatto = "";
        }

        protected async Task EvadiOrdine(string idOrdine)
        {
            var o = _ordini.Where(o => o.IdOrdine.ToString() == idOrdine).First();

            if (o.Evaso.HasValue)
                o.Evaso = !o.Evaso.Value;
            else
                o.Evaso = true;

            await _dl.Ordini_Update(o);

            await RefreshOrdini();
        }

        protected async Task CancellaOrdine(string idOrdine)
        {

            var o = _ordini.Where(o => o.IdOrdine.ToString() == idOrdine).First();
            await _dl.Ordine_Delete(o);

            await RefreshOrdini();
        }

        private async Task RefreshOrdini()
        {
            var res = await _dl.Ordine_GetByTavoloId(_tavolo.IdTavolo.ToString());
            if (res != null)
                _ordini = res.ToList();

            StateHasChanged();
        }

        protected async Task GoOnline()
        {
            //first things first: devo caricare i dati su redis!


            //secondo step: devo navigare alla pagina giusta
            Navigation.NavigateTo($"/moveonline/{_tavolo.IdTavolo}");
        }
    }
}