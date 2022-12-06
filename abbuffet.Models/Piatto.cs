using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.Azure.Cosmos.Table;

namespace abbuffet.Models
{
    public class Piatto
    {
        [JsonPropertyName("Nome")]
        public string Nome { get; set; }
        [JsonPropertyName("Descrizione")]
        public string Descrizione { get; set; }

        public Piatto() { }
    }
}