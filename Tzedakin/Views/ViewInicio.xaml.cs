using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tzedaka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewInicio : ContentPage
    {
        public ViewInicio()
        {
            InitializeComponent();
            Application.Current.UserAppTheme = OSAppTheme.Dark;
        }
    }
}