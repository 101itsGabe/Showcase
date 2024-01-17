using Avalonia.Controls;
using Avalonia.Interactivity;
using ShowcaseFullApp.ViewModels;

namespace ShowcaseFullApp.Views;


    public partial class LoginView : UserControl
    {
        public LoginViewModel loginviewmodel;
        public LoginView()
        {
            this.InitializeComponent();
            loginviewmodel = new LoginViewModel();
            DataContext = loginviewmodel;
        }


        public void SignupClick(object sender, RoutedEventArgs e)
        {
            loginviewmodel.SignUp();
            this.Content = new TvShowView();
        }

        public void LoginClick(object sender, RoutedEventArgs e)
        {
            loginviewmodel.Login();
            this.Content = new TvShowView();
        }
        
    }