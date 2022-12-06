using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace abbuffet.Services
{
    public static class ExtensionMethods
    {
        public static async Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient _client, string url, T content) where T : class
        {
            var jsonContent = JsonContent.Create<T>(content);
            return await _client.PatchAsync(url, jsonContent);
        }
    }
}
