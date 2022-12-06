using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using abbuffet.Models;
using abbuffet.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http;

namespace abbuffet.Pages
{
    public class OnlineTableBase : ComponentBase
    {
        [Parameter] public string IdTavolo { get; set; }
        [Inject] TableStorageDataLayer _dl { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        [Inject] IJSRuntime JsRuntime { get; set; }
        [Inject] HttpClient _client { get; set; }
        protected string _utente { get; set; }
        protected string _piatto { get; set; }
        protected Tavolo _tavolo { get; set; }
        public List<Ordine> _ordini { get; set; }
        protected HubConnection hubConnection;
        protected readonly string functionsUrl = "https://abbuffetbackend.azurewebsites.net/api";
        protected string hubTavolo => "t" + IdTavolo.Replace("-", "_");

        protected override async Task OnInitializedAsync()
        {
            _tavolo = await _dl.Tavoli_GetById(IdTavolo);

            //signalr
            hubConnection = new HubConnectionBuilder()
            .WithUrl($"{functionsUrl}/v1/table/{hubTavolo}")
            .Build();

            hubConnection.On<string>("refresh", pars =>
            {
                RemoteRefresh();
            });

            await hubConnection.StartAsync();


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

            _ordini.Add(o);

            _piatto = "";

            await SendRefresh();
            StateHasChanged();
        }

        protected async Task EvadiOrdine(string idOrdine)
        {
            var o = _ordini.Where(o => o.IdOrdine.ToString() == idOrdine).First();

            if (o.Evaso.HasValue)
                o.Evaso = !o.Evaso.Value;
            else
                o.Evaso = true;

            await _dl.Ordini_Update(o);

            await SendRefresh();
            StateHasChanged();
        }

        protected async Task CancellaOrdine(string idOrdine)
        {

            var o = _ordini.Where(o => o.IdOrdine.ToString() == idOrdine).First();
            await _dl.Ordine_Delete(o);

            _ordini.Remove(o);

            await SendRefresh();
            StateHasChanged();
        }

        private async Task RefreshOrdini()
        {
            var res = await _dl.Ordine_GetByTavoloId(_tavolo.IdTavolo.ToString());
            if (res != null)
                _ordini = res.ToList();
            
            StateHasChanged();
        }

        protected async Task Share()
        {
            var link = $"https://abbuffet.giacomocastellani.dev/?join={IdTavolo}";
            await JsRuntime.InvokeVoidAsync("clipboardCopy.copyText", link);
        }

        Task RemoteRefresh() => RefreshOrdini();

        public bool IsConnected =>
            hubConnection.State == HubConnectionState.Connected;

        protected async Task SendRefresh() 
        {
            await _client.PostAsync($"{functionsUrl}/v1/table/{hubTavolo}/update", new StringContent("ma bella"));
        }
    }
}