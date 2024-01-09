using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ShowcaseFullApp.Views;


    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            
        }
        
        public void LoginClick(object sender, RoutedEventArgs e)
        {
            this.Content = new TvShowView();
        }
        
    }