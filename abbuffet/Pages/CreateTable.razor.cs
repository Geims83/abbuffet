using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using abbuffet.Services;
using abbuffet.Models;

namespace abbuffet.Pages
{
    public class CreateTableBase : ComponentBase
    {
        [Inject] NavigationManager Navigation { get; set; }

        [Inject] iDBDataLayer _dl { get; set; }
        protected string Nome { get; set; }

        protected async Task Aggiungi()
        {
            var t = new Tavolo(Nome);
            await _dl.Tavoli_Add(t);
            Navigation.NavigateTo($"table/{t.IdTavolo.ToString()}");
        }

        protected void Back()
        {
            Navigation.NavigateTo("/");
        }
    }
}