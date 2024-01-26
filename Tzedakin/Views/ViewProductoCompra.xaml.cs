using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tzedaka.Model;
using Tzedaka.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tzedaka.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewProductoCompra : ContentPage
	{
		public ViewProductoCompra (Producto p)
		{
			InitializeComponent ();
            BindingContext = new VMProductoCompra(p);
        }
	}
}