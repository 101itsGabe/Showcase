using ReactiveUI;
using ShowcaseFullApp.ViewModels;

namespace ShowcaseFullApp.ViewModels;

public class MainViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
    public string Greeting => "Hehe!";
    private ViewModelBase _contentViewModel;
    
    
     public MainViewModel()
     {
     }
    
    public TVShowViewModel TvShowView { get; }
    
    public ViewModelBase ContentViewModel
    {
        get => _contentViewModel;
        private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
    }
    
    public void TvShow_Click()
    {
        ContentViewModel = new TVShowViewModel();
    }

#pragma warning restore CA1822 // Mark members as static
}
