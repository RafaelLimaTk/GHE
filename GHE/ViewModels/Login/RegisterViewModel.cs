using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GHE.Domain.Entities;
using GHE.Domain.Interfaces;
using GHE.Extensions;

namespace GHE.ViewModels.Login;

public partial class RegisterViewModel : ObservableObject
{
    [ObservableProperty]
    string? email;

    [ObservableProperty]
    string? password;

    [ObservableProperty]
    string? confirmPassword;

    private readonly IUserRepository _userRepository;
    public RegisterViewModel()
    {
        _userRepository = App.Current.Handler.MauiContext.Services.GetService<IUserRepository>();
    }

    [RelayCommand]
    public async Task Register()
    {
        var currentPageErro = Shell.Current.CurrentPage;
        if (password != confirmPassword)
        {
            await currentPageErro.DisplayAlert("Erro",
                                  $"As senhas não conferem", "Ok");
            return;
        }

        var validateEmail = EmailValidate.IsValidEmail(Email);
        if (!validateEmail)
        {
            await App.Current.MainPage.DisplayAlert("Error", "Email invalido", "Ok");
            return;
        }

        var user = new User(email.ToLower() ?? string.Empty, password.ToLower() ?? string.Empty);

        var validationResult = new UserValidator().Validate(user);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("\n", validationResult.Errors.Select(e => e.ErrorMessage));
            await currentPageErro.DisplayAlert("Erro de Validação", errors, "Ok");
            return;
        }

        await _userRepository.AddAsync(user);

        bool isConfirmed = await currentPageErro.DisplayAlert("Sucesso",
         $"Usuário criado com sucesso", "Ok", "Fechar");

        if (isConfirmed)
            await Shell.Current.GoToAsync("login");
    }
}
