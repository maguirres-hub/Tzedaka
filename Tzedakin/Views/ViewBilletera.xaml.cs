using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tzedaka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewBilletera : ContentPage
    {
        public ViewBilletera()
        {
            InitializeComponent();
            //BindingContext = new ViewModel_Billetera();

        }
    }
}