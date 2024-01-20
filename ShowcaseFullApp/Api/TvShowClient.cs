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
    
    public async Task<string?> GetAShowsAsync(int id)
    {
        /*
        string search = s.ToLower();

        for (int i = 0; i < search.Length; i++)
        {
            if (s[i] == ' ')
            {
                search = search.Replace(s[i], '-');
                Console.WriteLine(search);
            }
            
        }
        */
        try
        {
            //REMEMBER TO CHANGE THIS to a search query or something
            string apiUrl = $"/api/show-details?q={id}";

            
            var response = await _client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            { 
                //Console.WriteLine("Guys hes");
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

    public async Task<string?> GetPopularShow(int page)
    {
        try
        {
            string apiUrl = $"https://www.episodate.com/api/most-popular?page={page}";

            var response = await _client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"Failed to get show {response.StatusCode}");
            }
            
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Repsonse Exception: {ex.Message}");
            return null;
        }
        
    }
    

    public async Task<string?> GetShowSearch(string search, int page)
    {
        try
        {
            string apiUrl = $"/api/search?q=:{search}&page=:{page}";

            var response = await _client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                return await (response).Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"Failed to get show {response.StatusCode}");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Repsonse Exception: {ex.Message}");
            return null;
        }
    }
    
    
    
/*
    
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
        */

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