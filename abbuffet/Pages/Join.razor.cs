using System.Threading.Tasks;
using abbuffet.Services;
using Microsoft.AspNetCore.Components;

namespace abbuffet.Pages
{
    public class JoinBase : ComponentBase
    {
        [Parameter] public string idTavolo {get;set;}
        [Inject] NavigationManager Navigation { get; set; }
        [Inject] iDBDataLayer _local {get;set;}
        [Inject] TableStorageDataLayer _remote {get;set;}

        protected override async Task OnInitializedAsync()
        {
            var table = await _remote.Tavoli_GetById(idTavolo);
            await _local.Tavoli_Add(table);

            Navigation.NavigateTo($"/onlinetable/{idTavolo}");
        }
    }
}