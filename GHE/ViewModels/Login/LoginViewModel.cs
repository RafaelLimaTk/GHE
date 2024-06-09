using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GHE.ViewModels.Login;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    string? email;

    [ObservableProperty]
    string? password;

    [RelayCommand]
    public async Task GoToRegister()
    {
        await Shell.Current.GoToAsync("register");
    }
}
