using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.Azure.Cosmos.Table;

namespace abbuffet.Models
{
    public class Tavolo : TableEntity
    {
        [JsonPropertyName("IdTavolo")]
        public Guid IdTavolo { get; set; }
        [JsonPropertyName("Descrizione")]
        public string Descrizione { get; set; }
        [JsonPropertyName("Online")]
        public bool? Online { get; set; }

        public Tavolo() 
        {
            this.IdTavolo = Guid.NewGuid();

            this.PartitionKey = "Tavoli";
            this.RowKey = this.IdTavolo.ToString();
        }
        
        public Tavolo (string _desc)
        {
            this.IdTavolo = Guid.NewGuid();
            this.Descrizione = _desc;
            this.Online = false;

            this.PartitionKey = "Tavoli";
            this.RowKey = this.IdTavolo.ToString();
        }
    }
}