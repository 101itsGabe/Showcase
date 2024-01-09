using ShowcaseFullApp.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Avalonia.Media.Imaging;


namespace ShowcaseFullApp.Api;

public class TvShowClient
{

    private readonly HttpClient _client;
    public TvShowClient()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://www.episodate.com");
    }
    
    public async Task<string?> GetAllShowsAsync()
    {
        try
        {
            //REMEMBER TO CHANGE THIS to a search query or something
            const string apiUrl = "/api/show-details?q=keeping-up-with-the-kardashians";

            var response = await _client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            { 
                //Console.WriteLine(await response.Content.ReadAsStringAsync());
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"Failed to get show {response.StatusCode}");
            }
        }

        catch(HttpRequestException ex)
        {
            Console.WriteLine($"Response Exception: {ex.Message}");
            return null;
        }
    }
    
    public async Task<Bitmap?> DownloadImage(string imageUrl)
    {
            Bitmap? image = null;

            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync(new Uri(imageUrl));

                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    image = new Bitmap(stream);
                }
                else
                {
                    // Handle unsuccessful response
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
            }

            return image;
    }
        
    public tvShow ConvertToTvShow(string json)
    {
        var jparse = JObject.Parse(json);
        var name = jparse["tvShow"]?["name"]?.ToString();
        Console.WriteLine(name);
        tvShow temp = new tvShow();
        temp.TvShow.Title = name;
        return temp;
    }
    
    

}