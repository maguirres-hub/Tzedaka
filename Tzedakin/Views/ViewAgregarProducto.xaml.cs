using Tzedaka.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tzedaka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewAgregarProducto : ContentPage
    {
        public ViewAgregarProducto()
        {
            InitializeComponent();
            BindingContext = new ViewModel_Tienda();
        }
    }
}