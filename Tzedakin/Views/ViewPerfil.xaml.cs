using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tzedaka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewPerfil : ContentPage
    {
        public ViewPerfil()
        {
            InitializeComponent();
        }

        private void ShowPassword(object sender, EventArgs e)
        {
            Pass.IsPassword = !Pass.IsPassword;
        }

    }
}