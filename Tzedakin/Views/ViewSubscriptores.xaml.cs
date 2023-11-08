using Tzedaka.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tzedaka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewSubscriptores : ContentPage
    {
        public ViewSubscriptores()
        {
            InitializeComponent();
            BindingContext = new ViewModel_Club();
        }
    }
}