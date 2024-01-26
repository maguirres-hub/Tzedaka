using Tzedaka.Views;
using Tzedaka.Views;
using Xamarin.Forms;

namespace Tzedaka
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            App.Current.UserAppTheme = OSAppTheme.Light;

            MainPage = new NavigationPage(new MainPage());
        }
        protected override void OnSleep()
        {
            Properties.Remove("Cliente");
            SavePropertiesAsync();
        }
    }
}
