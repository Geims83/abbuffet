using System.Linq;
using System.Threading.Tasks;
using abbuffet.Services;
using Microsoft.AspNetCore.Components;

namespace abbuffet.Pages
{
    public class MoveOnlineBase : ComponentBase
    {
        [Parameter] public string idTavolo {get;set;}
        [Inject] NavigationManager Navigation { get; set; }
        [Inject] iDBDataLayer _local {get;set;}
        [Inject] TableStorageDataLayer _remote {get;set;}


        protected override async Task OnInitializedAsync()
        {
            var table = await _local.Tavoli_GetById(idTavolo);
            var ordiniTask = _local.Ordine_GetByTavoloId(idTavolo);

            await _remote.Tavoli_Add(table);
            var ordini = await ordiniTask;
            var oTasks = ordini.Select(o => _remote.Ordini_Add(o));
            await Task.WhenAll(oTasks);

            table.Online = true;
            await Task.WhenAll(_local.Tavoli_Update(table), _remote.Tavoli_Update(table));

            Navigation.NavigateTo($"/onlinetable/{idTavolo}");
        }
    }
}