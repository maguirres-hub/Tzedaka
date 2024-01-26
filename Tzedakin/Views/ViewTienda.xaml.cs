using System.Collections.ObjectModel;
using System.Linq;
using Tzedaka.Model;
using Tzedaka.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tzedaka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewTienda : ContentPage
    {
        public ViewTienda()
        {
            InitializeComponent();
        }

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var _container = BindingContext as ViewModel_Tienda;
            if (string.IsNullOrEmpty(e.NewTextValue)) 
            {
                listaProductos.ItemsSource = _container.Productos_;
            }
            else
            {
                listaProductos.ItemsSource = _container.Productos_.Where(p => p.Nombre.Contains(e.NewTextValue)).ToList();
            }
        }
    }
}