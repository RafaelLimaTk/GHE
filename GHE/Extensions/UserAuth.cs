using GHE.Domain.Entities;
using System.Text.Json;

namespace GHE.Extensions;

public static class UserAuth
{
    public static void SetUserAuth(User user)
    {
        var userJson = JsonSerializer.Serialize(user);
        Preferences.Default.Set("UserAuth", userJson);
    }

    public static User? GetUserAuth()
    {
        var userJson = Preferences.Default.Get<string>("UserAuth", string.Empty);
        if (string.IsNullOrEmpty(userJson))
            return null;

        return JsonSerializer.Deserialize<User>(userJson);
    }

    public static void RemoveUserAuth()
    {
        if (Preferences.Default.ContainsKey("UserAuth"))
            Preferences.Default.Remove("UserAuth");
    }
}
