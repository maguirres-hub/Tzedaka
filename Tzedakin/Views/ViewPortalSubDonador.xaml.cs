using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tzedaka.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tzedaka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewPortalSubDonador : TabbedPage
    {
        public ViewPortalSubDonador ()
        {
            InitializeComponent();
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
            else if (this.CurrentPage is ViewSubscriptores)
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