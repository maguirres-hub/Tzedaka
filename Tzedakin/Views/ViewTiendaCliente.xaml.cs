using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Tzedaka.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tzedaka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewTiendaCliente : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public ViewTiendaCliente()
        {
            InitializeComponent();
        }
        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var _container = BindingContext as ViewModel_Tienda;
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                listaProductos.ItemsSource = _container.MISProductos;
            }
            else
            {
                listaProductos.ItemsSource = _container.MISProductos.Where(p => p.Nombre.Contains(e.NewTextValue)).ToList();
            }
        }
    }
}
