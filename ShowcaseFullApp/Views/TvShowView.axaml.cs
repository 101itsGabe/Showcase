using System.Runtime.Loader;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ShowcaseFullApp.ViewModels;
using ShowcaseFullApp.Models;

namespace ShowcaseFullApp.Views;

public partial class TvShowView : UserControl
{
    public TVShowViewModel tvshowviewmodel;
    public TvShowView()
    {
        this.InitializeComponent();
        tvshowviewmodel = new TVShowViewModel();
        DataContext = tvshowviewmodel;
    }

    public void SelectedTvShow(object sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: TvShow show })
        {
            tvshowviewmodel.selectedTvShow = show;
        }

        this.Content = new TvShowSelectedView(tvshowviewmodel.selectedTvShow);
    }
    
    
}