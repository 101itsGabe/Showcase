using System.Collections.ObjectModel;

namespace ShowcaseFullApp.ViewModels;
using Models;

public class TVShowViewModel : ViewModelBase
{
    public TvShow tvshow { get; set; }
    public ObservableCollection<TvShow> tvshowlist { get; }
    
    public TvShow selectedTvShow { get; set; }

    public TVShowViewModel()
    {
        //when you get the users tvshow probably put it in the constructor
        tvshowlist = new ObservableCollection<TvShow>();
        for (int i = 0; i < 3; i++)
        {
            tvshow = new TvShow();
            tvshow.Title = "Jujustu Kaisen " + i;
            tvshow.SeasonCount = 2;
            tvshow.EpisodeCount = [35, 36];
            tvshow.CurEpisode = 3;
            tvshowlist.Add(tvshow);
        }

    }

    public ObservableCollection<TvShow> TvShowList
    {
        get { return tvshowlist; }
        set { TvShowList = value; }
    }
    
    
}