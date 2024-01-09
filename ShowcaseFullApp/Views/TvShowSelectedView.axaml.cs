using System;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using ShowcaseFullApp.Models;
using ShowcaseFullApp.ViewModels;

namespace ShowcaseFullApp.Views;

public partial class TvShowSelectedView : UserControl, INotifyPropertyChanged
{
    private TvShowSelectedViewModel tvsviewmodel;

    public TvShowSelectedView(TvShow show)
    {
        this.InitializeComponent();
        tvsviewmodel = new TvShowSelectedViewModel(show);
        this.DataContext = tvsviewmodel;
    }

    public void UpdateEpisode(object sender, RoutedEventArgs e)
    {
        tvsviewmodel.updateEpisode();
    }
    
}