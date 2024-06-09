using FluentValidation;
using GHE.Domain.Entities.Base;

namespace GHE.Domain.Entities;

public class User : EntityBase
{
    public string Email { get; private set; }
    public string Password { get; private set; }

    protected User() { }

    public User(string email, string password)
    {
        Email = email;
        Password = password;
    }
}

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(p => p.Email).NotEmpty().WithMessage("O nome é obrigatório.");
        RuleFor(p => p.Password).NotEmpty().WithMessage("A senha é obrigatória.");
    }
}