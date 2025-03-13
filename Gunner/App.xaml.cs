namespace Gunner
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AccountPage.LoginPage();
        }
    }
}
