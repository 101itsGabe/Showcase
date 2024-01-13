using System.Collections.ObjectModel;
using ShowcaseFullApp.Services;
using ShowcaseFullApp.Api;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShowcaseFullApp.ViewModels;
using Models;

public class TVShowViewModel : ViewModelBase, INotifyPropertyChanged
{
    public int _curPage { get; set; }
    private TvShowClient tvclient { get; set; }
    private TvShowService tvservice { get; set; }
    public ObservableCollection<string> tvshowlist { get; set;  }
    
    public string searchString { get; set; }
    
    public TvShow selectedTvShow { get; set; }

    public TVShowViewModel()
    {
        //when you get the users tvshow probably put it in the constructor
        tvshowlist = new ObservableCollection<string>();
        tvclient = new TvShowClient();
        tvservice = new TvShowService();
        searchString = "search";
        _curPage = 1;
        foreach (var show in tvservice.showList)
        {
            tvshowlist.Add(show.Title);
        }
        searchedList(searchString);

    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public ObservableCollection<string> TvShowList
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
        }
    }

    public async void searchedList(string s)
    {
        tvshowlist.Clear();
        if (s == "" || s == "search")
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