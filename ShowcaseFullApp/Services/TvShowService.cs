using System;
using Newtonsoft.Json.Linq;
using ShowcaseFullApp.Models;

namespace ShowcaseFullApp.Services;

public class TvShowService
{
    private static TvShowService? _instance;

    public static TvShowService Current
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TvShowService();
            }

            return _instance;
        }
        
    }

    public void Convert(string json, TvShow show)
    {
        var jparse = JObject.Parse(json);

        //Show name
        var name = jparse["tvShow"]?["name"]?.ToString();
        show.Title = name;
        
        //Last Season
        var lastSeason =  jparse["tvShow"]?["episodes"]?.Last?["season"]?.ToString();
        if (int.TryParse(lastSeason, out var num))
        {
            show.SeasonCount = num;
        }
        
        //Id
        var id = jparse["tvShow"]?["id"]?.ToString();
        if (int.TryParse(lastSeason, out var num2))
        {
            show.id = num2;
        }
        
        
        //Rating
        var rating = jparse["tvShow"]?["rating"]?.ToString();
        if (decimal.TryParse(rating, out var dec))
        {
            show.Rating = dec;
        }
        
        //Hey








    }
    
    
}