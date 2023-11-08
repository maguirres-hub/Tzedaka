using System;
using Tzedaka.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tzedaka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewPortal : TabbedPage
    {
        public ViewPortal()
        {
            InitializeComponent();
            Application.Current.UserAppTheme = OSAppTheme.Light;
            NavigationPage.SetHasBackButton(this, false);
        }

        private void OnCurrentPageChanged(object sender, EventArgs e)
        {
            if (this.CurrentPage.BindingContext != null)
            {
                return;
            }

            if (this.CurrentPage is ViewInicio)
            {

            }
            else if (this.CurrentPage is ViewClubTzedaka)
            {
                this.CurrentPage.BindingContext = new ViewModel_Club();
            }
            else if (this.CurrentPage is ViewBilletera)
            {
                this.CurrentPage.BindingContext = new ViewModel_Billetera();
            }
            else if (this.CurrentPage is ViewPerfil)
            {
                this.CurrentPage.BindingContext = new ViewModel_Perfil();
            }
            else if (this.CurrentPage is ViewTienda)
            {
                this.CurrentPage.BindingContext = new ViewModel_Tienda();
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Application.Current.Properties.Remove("Cliente");
            await Application.Current.SavePropertiesAsync();
            Application.Current.Properties.Remove("Correo_Cliente");
            await Application.Current.SavePropertiesAsync();
            await Application.Current.MainPage.Navigation.PushAsync(new MainPage());
        }
    }
}