using System;
using System.Threading.Tasks;
using ShowcaseFullApp.Api;
using ShowcaseFullApp.Models;
using ShowcaseFullApp.Services;

namespace ShowcaseFullApp.ViewModels;

public class LoginViewModel
{
    private string _email { get; set; }
    private string _password { get; set; }
    private FirebaseApi firebase;
    private UserService _userService;

    public LoginViewModel()
    {
        firebase = new FirebaseApi();
        _userService = UserService.Current;
    }
    

    public string Email
    {
        get => _email;
        set => _email = value;
    }


    public string Password
    {
        get => _password;
        set => _password = value;
    }


    public async void SignUp()
    {
        var token = await firebase.Signup(_email, _password);
        if (token != null)
        {
            _userService.email = _email;
        }

    }

    public async Task<bool> Login()
    {
        var token = await firebase.LoginToFirebase(_email, _password);
        if (token != null)
        {
            _userService.email = _email;
            return true;
        }
        return false;
        
    }


}