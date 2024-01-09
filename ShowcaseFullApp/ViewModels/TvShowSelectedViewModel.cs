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
    public TvShow curTvShow;
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
    
    public TvShowSelectedViewModel(TvShow show)
    {
        tvclient = new TvShowClient();
        searchedtvshow = new TvShow();
        //firebase = new FirebaseApi();
        tvservice = TvShowService.Current;
        curTvShow = show;
        searchedtvshow.Title = "Loading...";
        InitTvSearchTitle();
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
    
/*
    public Bitmap? TvShowImage
    {
        get=> tvShowBitmap;
        set
        {
            if (tvShowBitmap != value)
            {
                tvShowBitmap = value;
                OnPropertyChanged();
            }
        }
    }
    */
    


    public int CurEpisode
    {
        
        get => curTvShow.CurEpisode;
        set => curTvShow.CurEpisode = value;
    }
    
    
    

    public void updateEpisode()
    {
        curTvShow.CurEpisode += 1;
        OnPropertyChanged(nameof(CurEpisode));
    }

    private async void InitTvSearchTitle()
    {
        var json = await tvclient.GetAllShowsAsync();
        if (json != null)
        {
            tvservice.Convert(json, searchedtvshow);
        }

        Console.WriteLine("well ill be");
        //var snapshot = firebase.GetDocumentAsync();
        OnPropertyChanged(nameof(TvShowTitle));
        OnPropertyChanged(nameof(SeasonCount));
        OnPropertyChanged(nameof(Rating));

    }
}