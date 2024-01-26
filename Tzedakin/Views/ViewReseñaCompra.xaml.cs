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
	public partial class ViewReseñaCompra : ContentPage
	{
		public ViewReseñaCompra ()
		{
			InitializeComponent ();
		}
        void CambiarPuntuacionTienda(object sender, CheckedChangedEventArgs e)
        {
			var radio = sender as RadioButton;
			if(radio.IsChecked)
			{
                VMMisCompras vMMisCompras = BindingContext as VMMisCompras;
				vMMisCompras.ComentarioTienda.puntuacion = int.Parse((string)radio.Value);
            }			

        }
        void CambiarPuntuacionProducto(object sender, CheckedChangedEventArgs e)
        {
            var radio = sender as RadioButton;
            if (radio.IsChecked)
            {
                VMMisCompras vMMisCompras = BindingContext as VMMisCompras;
                vMMisCompras.ComentarioProducto.puntuacion = int.Parse((string)radio.Value);
            }
        }
    }
}