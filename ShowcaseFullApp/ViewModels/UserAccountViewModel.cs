using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ShowcaseFullApp.Services;
using ShowcaseFullApp.Api;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShowcaseFullApp.ViewModels;

public sealed class UserAccountViewModel : INotifyPropertyChanged
{
    private UserService _userService;
    private FirebaseApi firebase;
    private List<idShow> baseShowList;
    public UserAccountViewModel()
    {
        firebase = new FirebaseApi();
        _userService = UserService.Current;
        InitList();
        OnPropertyChanged(nameof(ShowList));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public async void InitList()
    {
        baseShowList = new List<idShow>();
        baseShowList = await firebase.getShowList(_userService.email);
        Console.WriteLine(baseShowList.Count);
        OnPropertyChanged(nameof(ShowList));
    }

    public string Email => _userService.email;

    public ObservableCollection<idShow> ShowList
    {
        get => new ObservableCollection<idShow>(baseShowList);
    }
}