using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using abbuffet.Models;
using System.Threading.Tasks;
using abbuffet.Services;
using System.Linq;
using Microsoft.AspNetCore.WebUtilities;

namespace abbuffet.Pages
{
    public class IndexBase : ComponentBase
    {
        protected List<Tavolo> _tavoli;
        [Inject] NavigationManager Navigation { get; set; }
        [Inject] iDBDataLayer _dl {get;set;}


        protected override async Task OnInitializedAsync()
        {
            var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("join", out var _joinId))
            {
                Navigation.NavigateTo($"/join/{_joinId}");
            }

            await RefreshTavoli();
        }

        protected void Aggiungi()
        {
            Navigation.NavigateTo("creatavolo");
        }

        protected async Task Cancella(string id)
        {
            var t = _tavoli.Where(t => t.IdTavolo.ToString() == id).First();
            await _dl.Tavoli_Delete(t);
            await RefreshTavoli();
        }

        protected async Task Edit(string id)
        {
            var t = _tavoli.Where(t => t.IdTavolo.ToString() == id).First();
            string page = ((t.Online ?? false) ? "onlinetable" : "table");
            Navigation.NavigateTo($"{page}/{id}");
        }

        protected async Task RefreshTavoli()
        {
            _tavoli = await _dl.Tavoli_GetAll();
        }
    }
}