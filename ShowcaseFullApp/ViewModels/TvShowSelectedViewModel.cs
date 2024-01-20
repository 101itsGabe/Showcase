using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Newtonsoft.Json.Linq;
using ShowcaseFullApp.Models;
using ShowcaseFullApp.Api;
using Avalonia.Media.Imaging;
using Firebase.Auth;
using Google.Cloud.Firestore;
using ShowcaseFullApp.Services;

namespace ShowcaseFullApp.ViewModels;





public class TvShowSelectedViewModel : ViewModelBase, INotifyPropertyChanged
{
    public TvShow searchedtvshow;
    private TvShowClient tvclient;
    private TvShowService tvservice;
    private FirebaseApi firebase;
    private UserService _userService;

    private int curId { get; set; }
    public bool hasBeenAdded { get; set; }

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

    public TvShowSelectedViewModel(string showName, int id)
    {
        tvclient = new TvShowClient();
        searchedtvshow = new TvShow();
        _userService = UserService.Current;
        //sqlapi = new SqlApi();
        //mongodb = new MongoDBApi();
        firebase = new FirebaseApi();
        tvservice = TvShowService.Current;
        searchedtvshow.Title = "Loading...";
        searchedtvshow.Description = "loading...";
        hasBeenAdded = false;
        curId = id;
        InitTvSearchTitle(showName, id);
        InitBool(showName);
    }

    private async void InitBool(string showname)
    {
        
        if (firebase != null)
        {
            var help = await firebase.isAdded(_userService.email, showname);
            if (help)
            {
                hasBeenAdded = false;
            }
            else
            {
                hasBeenAdded = true;
                OnPropertyChanged(nameof(hasBeenAdded));
            }
        }

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

    public int CurSeason
    {
        get => searchedtvshow.CurSeason;
    }

    public string EpTitle
    {
        get => searchedtvshow.CurEpTitle;
    }
    
    
    
    

    public async void updateEpisode()
    {
        string sshowName = searchedtvshow.Title.Replace("(", "").Replace(")", "").ToLower();
        var json = await tvclient.GetAShowsAsync(curId);
        if (searchedtvshow.LastEpSeasonNum > searchedtvshow.CurEpisode)
        {
            searchedtvshow.CurEpisode += 1;
            searchedtvshow.CurEpTitle = tvservice.GetCurEpname(json, searchedtvshow.CurEpisode, searchedtvshow.CurSeason);
            if (int.TryParse(tvservice.GetCurSeason(json, searchedtvshow.CurSeason), out int val))
            {
                searchedtvshow.CurSeason = val;
            }
            firebase.updateEp(_userService.email, searchedtvshow.Title, searchedtvshow.LastEpSeasonNum, searchedtvshow.CurSeason, searchedtvshow.CurEpTitle);
            OnPropertyChanged(nameof(EpTitle));
            OnPropertyChanged(nameof(CurEpisode));
        }
        else
        {
            searchedtvshow.CurSeason = searchedtvshow.CurSeason + 1;
            searchedtvshow.CurEpisode = 1;
            OnPropertyChanged(nameof(CurEpisode));
            OnPropertyChanged(nameof(CurSeason));
            firebase.updateEp(_userService.email, searchedtvshow.Title, searchedtvshow.LastEpSeasonNum, searchedtvshow.CurSeason, searchedtvshow.CurEpTitle);
            searchedtvshow.LastEpSeasonNum = tvservice.GetLastEpInSeason(json, searchedtvshow.CurSeason);
        }
    }

    public async void AddTvShow()
    {
        hasBeenAdded = false;
        OnPropertyChanged(nameof(hasBeenAdded));
        var json = await tvclient.GetAShowsAsync(curId);
        searchedtvshow.CurEpTitle = tvservice.GetCurEpname(json, 1, 1);
        //string showName = curTvShow.Title;
        await firebase.AddTvShow(searchedtvshow.Title, searchedtvshow.CurEpTitle, searchedtvshow.id);
        searchedtvshow.CurSeason = await firebase.getCurSeason(_userService.email, searchedtvshow.Title);
        searchedtvshow.CurEpisode = await firebase.getCurEp(_userService.email, searchedtvshow.Title);
        OnPropertyChanged(nameof(CurSeason));
        OnPropertyChanged(nameof(CurEpisode));
        OnPropertyChanged(nameof(EpTitle));

    }
    

    private async void InitTvSearchTitle(string showName, int id)
    {
        var json = await tvclient.GetAShowsAsync(curId);
        if (json != null)
        {
            TvShow curShow = tvservice.Convert(json);
            searchedtvshow = curShow;
            /*
            searchedtvshow.Title = curShow.Title;
            searchedtvshow.Description = curShow.Description;
            searchedtvshow.SeasonCount = curShow.SeasonCount;
            searchedtvshow.id = curShow.id;
            searchedtvshow.EpisodeCount = curShow.EpisodeCount;
            searchedtvshow.Rating = curShow.Rating;
            searchedtvshow.CurEpTitle = curShow.CurEpTitle;
            */
            searchedtvshow.CurSeason = await firebase.getCurSeason(_userService.email, showName);
            searchedtvshow.CurEpisode = await firebase.getCurEp(_userService.email, showName);
            searchedtvshow.CurEpTitle = tvservice.GetCurEpname(json, searchedtvshow.CurEpisode, searchedtvshow.CurSeason);
            searchedtvshow.LastEpSeasonNum = tvservice.GetLastEpInSeason(json, searchedtvshow.CurSeason);
            OnPropertyChanged(nameof(CurSeason));
            OnPropertyChanged(nameof(CurEpisode));
        }

        await firebase.GetDocumentAsync();

        //var snapshot = firebase.GetDocumentAsync();
        //Console.WriteLine(searchedtvshow.Description);
        OnPropertyChanged(nameof(EpTitle));
        OnPropertyChanged(nameof(TvShowTitle));
        OnPropertyChanged(nameof(SeasonCount));
        OnPropertyChanged(nameof(Rating));
        OnPropertyChanged(nameof(Desc));

    }
    
    
}