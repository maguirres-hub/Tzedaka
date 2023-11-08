using System;
using Tzedaka.ViewModels;
using Tzedaka.Views;
using Xamarin.Forms;

namespace Tzedaka
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new ViewModel_Login();
        }
        public async void Ir_Registro(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewRegistro());
        }
        private void ShowPassword(object sender, EventArgs e)
        {
            Pass.IsPassword = !Pass.IsPassword;
        }
    }
}
