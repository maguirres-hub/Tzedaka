using Tzedaka.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tzedaka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewRetiro : ContentPage
    {
        public ViewRetiro()
        {
            InitializeComponent();
            BindingContext = new ViewModel_Retiro();
        }
    }
}