using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Firebase.Auth;
using ShowcaseFullApp.ViewModels;

namespace ShowcaseFullApp.Views;

public partial class UserAccountView : UserControl
{
    public UserAccountViewModel uacvm;
    public UserAccountView()
    {
        this.InitializeComponent();
        uacvm = new UserAccountViewModel();
        DataContext = uacvm;
    }

    private void backButton(object? sender, RoutedEventArgs e)
    {
        this.Content = new TvShowView();
    }

    private void showSelected(object? sender, RoutedEventArgs e)
    {

        if (sender is Button { DataContext: idShow curShow })
        {
            string[] parts = curShow.name.Split(',');
            if (parts.Length > 1)
            {
                string partsbefore = parts[0].Trim();
                Console.WriteLine(partsbefore);
                this.Content = new TvShowSelectedView(partsbefore, curShow.id);
            }
        }
    }
}