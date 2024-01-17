using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Pipes;
using System.Linq;
using Google.Apis.Logging;
using Newtonsoft.Json.Linq;
using ShowcaseFullApp.Api;
using ShowcaseFullApp.Models;

namespace ShowcaseFullApp.Services;

public class TvShowService
{
    private static TvShowService? _instance;

    public List<TvShow> showList { get; set; }

    public TvShowService()
    {
        showList = new List<TvShow>();
    }

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
        
        //onsole.WriteLine(jparse);

        if (jparse["tvShow"]?.ToString() != null)
        {
            Console.WriteLine(jparse["tvshow"]?.ToString());
            //Show name
            var name = jparse["tvShow"]?["name"]?.ToString();
            if (name != null)
                show.Title = name;

            //Last Season
            var lastSeason = jparse["tvShow"]?["episodes"]?.Last?["season"]?.ToString();
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

            var desc = jparse["tvShow"]?["description"]?.ToString();
            if (desc != null)
                show.Description = desc;


        }
    }

    public void ConvertList(string json, ObservableCollection<string> names)
    {
        var jparse = JObject.Parse(json);
        //Console.WriteLine(jparse);
        var list = jparse["tv_shows"];
        if (list != null)
        {
            foreach (var show in list)
            {
                var showName = show["name"]?.ToString();
                if (showName != null)
                {
                    names.Add(showName);
                    //Console.WriteLine(showName);
                }
            }
        }
        
        

    }
    
    
}