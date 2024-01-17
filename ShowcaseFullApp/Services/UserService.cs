using ShowcaseFullApp.Models;
using System;
namespace ShowcaseFullApp.Services;

public class UserService
{
    private static UserService? _instance;
    public string email { get; set; }

    public UserService()
    {
        
    }
    
    public static UserService Current
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UserService();
            }

            return _instance;
        }
        
    }
    
}