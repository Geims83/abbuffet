using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.Azure.Cosmos.Table;

namespace abbuffet.Models
{
    public class Ordine : TableEntity
    {
        [JsonPropertyName("IdOrdine")]
        public Guid IdOrdine { get; set; }
        [JsonPropertyName("TavoloId")]
        public Guid TavoloId { get; set; }
        [JsonPropertyName("Utente")]
        public string Utente { get; set; }
        [JsonPropertyName("Piatto")]
        public string Piatto { get; set; }
        [JsonPropertyName("Evaso")]
        public bool? Evaso { get; set; }
        public Ordine() 
        {
            this.IdOrdine = Guid.NewGuid();
         }

         public Ordine(Guid tavoloId)
         {
             this.IdOrdine = Guid.NewGuid();
             this.TavoloId = tavoloId;
             
             this.PartitionKey = tavoloId.ToString();
             this.RowKey = this.IdOrdine.ToString();
         }
    }
}