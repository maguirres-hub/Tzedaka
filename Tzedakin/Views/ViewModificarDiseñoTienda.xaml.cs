using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tzedaka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewModificarDiseñoTienda : ContentPage
    {
        public ViewModificarDiseñoTienda()
        {
            InitializeComponent();
        }

        private async void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            uint duracion = 700;
            //await Task.WhenAll(
            //        menuOpciones.FadeTo(0, duracion),
            //        tienda.TranslateTo(0,100,duracion+200,Easing.CubicIn),
            //        opciones.TranslateTo(0,100,duracion,Easing.CubicIn)
            //    );
            opciones.IsVisible = true;
        }

        private void SwipeGestureRecognizer_Swiped_1(object sender, SwipedEventArgs e)
        {
            opciones.IsVisible = false;
        }
    }
}