using System;
using System.Runtime.Loader;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MongoDB.Driver.Linq;
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
        Console.WriteLine("HELP");
        if (sender is Button { DataContext: idShow curShow })
        {
            //Console.WriteLine("help");
            Console.WriteLine(curShow.name);
            foreach (var show in tvshowviewmodel.tvshowlist)
            {
                if (show.name == curShow.name)
                {
                    this.Content = new TvShowSelectedView(curShow.name, curShow.id);
                }
                
            }
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


    private void backbutton(object? sender, RoutedEventArgs e)
    {
        this.Content = new MainView();
    }

    private void AccountViewClick(object? sender, RoutedEventArgs e)
    {
        this.Content = new UserAccountView();
    }
}