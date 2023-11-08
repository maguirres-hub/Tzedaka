using Rg.Plugins.Popup.Services;
using System;
using Tzedaka.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tzedaka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewRegistro : ContentPage
    {
        public ViewRegistro()
        {
            InitializeComponent();
            BindingContext = new ViewModel_Registro();

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new ViewPoliticas());
        }
    }
}