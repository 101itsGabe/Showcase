using System;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using ShowcaseFullApp.Api;
using ShowcaseFullApp.Models;
using ShowcaseFullApp.ViewModels;

namespace ShowcaseFullApp.Views;

public partial class TvShowSelectedView : UserControl, INotifyPropertyChanged
{
    private TvShowSelectedViewModel tvsviewmodel;

    /*
    public TvShowSelectedView(TvShow show)
    {
        this.InitializeComponent();
        tvsviewmodel = new TvShowSelectedViewModel(show);
        this.DataContext = tvsviewmodel;
    }
    */

    public TvShowSelectedView(string showName, int id)
    {
        this.InitializeComponent();
        tvsviewmodel = new TvShowSelectedViewModel(showName, id);
        this.DataContext = tvsviewmodel;
    }
    

    public void UpdateEpisode(object sender, RoutedEventArgs e)
    {
        tvsviewmodel.updateEpisode();
    }

    public void AddShow(object sender, RoutedEventArgs e)
    {
        tvsviewmodel.AddTvShow();
        tvsviewmodel.hasBeenAdded = false;
    }

    public void backButton(object sender, RoutedEventArgs e)
    {
        this.Content = new TvShowView();
    }
    
    
}