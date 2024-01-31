using RestSharp.Deserializers;
using ShowcaseFullApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShowcaseFullApp.Services
{
    public class RestService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;

        public RestService()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<List<TvShow>> RefershList()
        {
            var Shows = new List<TvShow>();
            Uri uri = new Uri(string.Empty);

            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if(response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Shows = JsonSerializer.Deserialize<List<TvShow>>(content, _serializerOptions);
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }

            return Shows;
        }
    }
}
