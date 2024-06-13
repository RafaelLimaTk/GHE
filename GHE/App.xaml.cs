using GHE.Extensions;

namespace GHE
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            OnSleep();
        }

        protected override void OnSleep()
        {
            UserAuth.RemoveUserAuth();
        }
    }
}
