using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Pipes;
using System.Linq;
using Google.Apis.Logging;
using Newtonsoft.Json.Linq;
using ShowcaseFullApp.Api;
using ShowcaseFullApp.Models;
using ShowcaseFullApp.ViewModels;
using ZstdSharp.Unsafe;

namespace ShowcaseFullApp.Services;

public class TvShowService
{
    private static TvShowService? _instance;

    public List<TvShow> showList { get; set; }
    public TvShow curShow { get; set; }

    public TvShowService()
    {
        showList = new List<TvShow>();
    }

    public TvShowService(TvShow s)
    {
        curShow = s;
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

    public string GetCurEpname(string json, int epNum, int season)
    {
        string epName = "";
        var jparse = JObject.Parse(json);
        //Console.WriteLine("HEHHEEHEHEHEHEHE");
        foreach (var jarr in jparse["tvShow"]?["episodes"])
        {
            //Console.WriteLine(jarr.ToString());
            if (int.TryParse(jarr["season"].ToString(), out int s))
            {
                if (int.TryParse(jarr["episode"].ToString(), out int ep))
                {

                    if (s == season && epNum == ep)
                    {
                        epName = jarr["name"]?.ToString();
                    }
                }
            }
        }

        return epName;
    }
    public string GetCurSeason(string json, int seasonNum)
    {
        string epName = "";
        var jparse = JObject.Parse(json);
        if (jparse["tvShow"]?.ToString() != null || jparse["tvShow"]?.ToString() != "[]")
        {
            epName = jparse["tvShow"]?["season"]?[seasonNum - 1].ToString();
        }

        return epName;
    }

    public int GetLastEpInSeason(string json, int season)
    {
        bool found = false;
        string epName = "";
        var jparse = JObject.Parse(json);
        //Console.WriteLine(jparse);
        var prevInt = 0;
        foreach (var jarr in jparse["tvShow"]?["episodes"])
        {
            if (int.TryParse(jarr["season"].ToString(), out int s))
            {
                if (int.TryParse(jarr["episode"].ToString(), out int ep))
                {

                    if (s == season)
                    {
                        found = true;
                        prevInt = ep;

                    }

                    if (found && s > season)
                    {
                        return prevInt;
                        break;
                    }
                }
            }
        }
        
        
        

        return prevInt;
    }



//https://www.tvmaze.com/api
    public TvShow Convert(string json)
    {
        TvShow curShow = new TvShow();
        var jparse = JObject.Parse(json);

        if (jparse["tvShow"]?.ToString() != "[]")
        {
            //Console.WriteLine(jparse["tvShow"]?.ToString());
            //Show name
            var name = jparse["tvShow"]?["name"]?.ToString();
            if (name != null)
                curShow.Title = name;

            //Last Season
            var lastSeason = jparse["tvShow"]?["episodes"]?.Last?["season"]?.ToString();
            if (int.TryParse(lastSeason, out var num))
            {
                curShow.SeasonCount = num;
            }

            //Id
            var id = jparse["tvShow"]?["id"]?.ToString();
            if (int.TryParse(lastSeason, out var num2))
            {
                curShow.id = num2;
            }


            //Rating
            var rating = jparse["tvShow"]?["rating"]?.ToString();
            if (decimal.TryParse(rating, out var dec))
            {
                curShow.Rating = dec;
            }

            //Desc
            var desc = jparse["tvShow"]?["description"]?.ToString();
            if (desc != null)
                curShow.Description = desc;
            
            //Set to first ep
            curShow.CurEpisode = 1;

            var eptitle = jparse["tvShow"]?["episodes"]?[0]["name"]?.ToString();
            if (eptitle != null)
            {
                curShow.CurEpTitle = eptitle;
            }
            


        }

        return curShow;
    }

    public void ConvertList(string json, ObservableCollection<idShow> names)
    {
        int id = 0;
        var jparse = JObject.Parse(json);
        var list = jparse["tv_shows"];
        if (list != null)
        {
            foreach (var show in list)
            {
                idShow curShow = new idShow();
                var showName = show["name"]?.ToString();
                if (showName != null)
                {
                    curShow.name = showName;
                    //Console.WriteLine(showName);
                }
                if(int.TryParse(show["id"]?.ToString(), out int curID))
                {
                    curShow.id = curID;
                    Console.WriteLine(id);
                    names.Add(curShow);
                }
                
            }
        }

    }
    
    
    
    
}