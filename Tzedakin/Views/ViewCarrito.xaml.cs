using Tzedaka.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tzedaka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewCarrito : ContentPage
    {
        public ViewCarrito()
        {
            InitializeComponent();
            BindingContext = new ViewModel_Tienda();
        }
    }
}