using Avalonia.Controls;
using System.Linq;
using ReactiveUI;
using ShowcaseFullApp.ViewModels;
using System.Reactive;
using Avalonia.Interactivity;

namespace ShowcaseFullApp.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        this.InitializeComponent();
        DataContext = new MainViewModel();
    }

    public void TvShowClick(object sender, RoutedEventArgs e)
    {
        this.Content = new TvShowView();
    }

    public void LoginClick(object sender, RoutedEventArgs e)
    {
        this.Content = new LoginView();
    }
    
    
}