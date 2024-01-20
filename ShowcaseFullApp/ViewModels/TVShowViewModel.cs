using System.Collections.ObjectModel;
using ShowcaseFullApp.Services;
using ShowcaseFullApp.Api;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShowcaseFullApp.ViewModels;
using Models;

public struct idShow
{
    public string name { get; set; }
    public int id { get; set; }
}

public class TVShowViewModel : ViewModelBase, INotifyPropertyChanged
{
    public int _curPage { get; set; }
    private TvShowClient tvclient { get; set; }
    private TvShowService tvservice { get; set; }
    public ObservableCollection<idShow> tvshowlist { get; set;  }
    
    public string searchString { get; set; }
    
    public TvShow selectedTvShow { get; set; }

    public TVShowViewModel()
    {
        //when you get the users tvshow probably put it in the constructor
        tvshowlist = new ObservableCollection<idShow>();
        tvclient = new TvShowClient();
        tvservice = TvShowService.Current;
        searchString = "search";
        _curPage = 1;
        foreach (var show in tvservice.showList)
        {
            idShow curShow = new idShow();
            curShow.name = show.Title;
            tvshowlist.Add(curShow);
        }
        searchedList(searchString);

    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public ObservableCollection<idShow> TvShowList
    {
        get { return tvshowlist; }
        set { tvshowlist = value; }
    }

    public string SearchString
    {
        get => searchString;
        set
        {
            searchString = value;
            searchedList(searchString);
            OnPropertyChanged(SearchString);
            OnPropertyChanged(nameof(TvShowList));
        }
    }

    public async void searchedList(string s)
    {
        tvshowlist.Clear();
        if (s == "" || s == "search" || s == " ")
        {
            var json = await tvclient.GetPopularShow(_curPage);
            if (json != null)
            {
                tvservice.ConvertList(json, tvshowlist);
                OnPropertyChanged(nameof(TvShowList));
            }
        }
        else
        {
            var json = await tvclient.GetShowSearch(s, _curPage);
            if (json != null)
            {
                tvservice.ConvertList(json, tvshowlist);
                OnPropertyChanged(nameof(TvShowList));
            }
            
        }
        
    }

    public void updatePageup()
    {
        _curPage += 1;
    }

    public void updatePagedown()
    {
        if (_curPage > 1)
        {
            _curPage -= 1;
        }
    }
    
    
    
    
}