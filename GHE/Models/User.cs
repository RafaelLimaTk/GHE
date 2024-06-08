using GHE.Models.Base;

namespace GHE.Models;

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
