using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GHE.Domain.Interfaces;
using GHE.Extensions;

namespace GHE.ViewModels.Login;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    string? email;

    [ObservableProperty]
    string? password;

    [ObservableProperty]
    bool isBusy;

    private readonly IUserRepository _userRepository;
    public LoginViewModel()
    {
        _userRepository = App.Current.Handler.MauiContext.Services.GetService<IUserRepository>();
    }

    [RelayCommand]
    public async Task Login()
    {
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
        {
            await App.Current.MainPage.DisplayAlert("Error", "Email e senha são obrigatório", "Ok");
            return;
        }

        var user = await _userRepository.Login(Email.ToLower(), Password.ToLower());

        if (user != null)
        {
            if (isBusy)
                UserAuth.SetUserAuth(user);

            await Shell.Current.GoToAsync("criarghe");
        }
        else
            await App.Current.MainPage.DisplayAlert("Error", "Email ou senha invalidos", "Ok");
    }

    [RelayCommand]
    public async Task GoToRegister()
    {
        await Shell.Current.GoToAsync("register");
    }

    [RelayCommand]
    private async Task GoToSearchGhe()
    {
        await Shell.Current.GoToAsync("ghe");
    }
}
