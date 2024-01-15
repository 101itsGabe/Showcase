using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Newtonsoft.Json.Linq;
using ShowcaseFullApp.Models;
using ShowcaseFullApp.Api;
using Avalonia.Media.Imaging;
using Google.Cloud.Firestore;
using ShowcaseFullApp.Services;

namespace ShowcaseFullApp.ViewModels;

public class TvShowSelectedViewModel : ViewModelBase, INotifyPropertyChanged
{
    public TvShow searchedtvshow;
    private TvShowClient tvclient;
    private TvShowService tvservice;
    private FirebaseApi firebase;

    public string imageUrl = "https://static.episodate.com/images/tv-show/full/29560.jpg";
    //private Bitmap? tvShowBitmap;
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public TvShowSelectedViewModel()
    {
    }
    
    /*
    public TvShowSelectedViewModel(TvShow show)
    {
        tvclient = new TvShowClient();
        searchedtvshow = new TvShow();
        //firebase = new FirebaseApi();
        tvservice = TvShowService.Current;
        curTvShow = show;
        searchedtvshow.Title = "Loading...";
        //InitTvSearchTitle();
    }
    */

    public TvShowSelectedViewModel(string showName)
    {
        tvclient = new TvShowClient();
        searchedtvshow = new TvShow();
        //sqlapi = new SqlApi();
        //mongodb = new MongoDBApi();
        firebase = new FirebaseApi();
        tvservice = TvShowService.Current;
        searchedtvshow.Title = "Loading...";
        searchedtvshow.Description = "loading...";
        InitTvSearchTitle(showName);
    }
    

    public string TvShowTitle
    {
        get
        {
            return searchedtvshow.Title;

        }
        
        set
        {
            searchedtvshow.Title = value;
        }
        
    }

    public string SeasonCount
    {
        get => searchedtvshow.SeasonCount.ToString();
        set
        {
            SeasonCount = value;
        }
        
    }


    public string Rating
    {
        get => searchedtvshow.Rating.ToString("0.##");
        set
        {
            Rating = value;
        }
    }
    

    public string ImageUrl
    {
        get => imageUrl;
    }
    


    public int CurEpisode
    {
        
        get => searchedtvshow.CurEpisode;
        set => searchedtvshow.CurEpisode = value;
    }

    public string Desc
    {
        get => searchedtvshow.Description;
        set => searchedtvshow.Description = value;
    }
    
    
    

    public void updateEpisode()
    {
        searchedtvshow.CurEpisode += 1;
        OnPropertyChanged(nameof(CurEpisode));
    }

    public async void AddTvShow()
    {
        //string showName = curTvShow.Title;
            Console.WriteLine("Wee");
            Console.WriteLine();
            await firebase.AddTvShow(searchedtvshow.Title);
            Console.WriteLine("woo");
        
    }
    

    private async void InitTvSearchTitle(string showName)
    {
        //Console.WriteLine("ello there");
        var json = await tvclient.GetAShowsAsync(showName);
        if (json != null)
        {
            tvservice.Convert(json, searchedtvshow);
        }

        await firebase.GetDocumentAsync();

        //var snapshot = firebase.GetDocumentAsync();
        //Console.WriteLine(searchedtvshow.Description);
        OnPropertyChanged(nameof(TvShowTitle));
        OnPropertyChanged(nameof(SeasonCount));
        OnPropertyChanged(nameof(Rating));
        OnPropertyChanged(nameof(Desc));

    }
    
    
}