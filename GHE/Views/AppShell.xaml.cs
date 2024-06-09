namespace GHE;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("login", typeof(Views.Login.LoginPage));
        Routing.RegisterRoute("register", typeof(Views.Login.RegisterPage));
        Routing.RegisterRoute("ghe", typeof(Views.Ghes.GhePage));
        Routing.RegisterRoute("criarghe", typeof(Views.Ghes.CreateGhePage));
    }
}
