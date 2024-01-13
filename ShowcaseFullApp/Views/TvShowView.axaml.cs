using System;
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
        /*
        if (sender is Button { DataContext: TvShow show })
        {
            tvshowviewmodel.selectedTvShow = show;
            this.Content = new TvShowSelectedViewModel(tvshowviewmodel.selectedTvShow.Title);
        }
        */
        
        if (sender is Button { DataContext: string title })
        {
            //Console.WriteLine("help");
            //Console.WriteLine(title);
            this.Content = new TvShowSelectedView(title);
        }
    }

    public void nextPage(object sender, RoutedEventArgs e)
    {
        tvshowviewmodel.updatePageup();
        //Console.WriteLine(tvshowviewmodel._curPage);
        tvshowviewmodel.searchedList(tvshowviewmodel.searchString);
    }

    public void prevPage(object sender, RoutedEventArgs e)
    {
        tvshowviewmodel.updatePagedown();
        //Console.WriteLine(tvshowviewmodel._curPage);
        tvshowviewmodel.searchedList(tvshowviewmodel.searchString);
    }
    
    
}