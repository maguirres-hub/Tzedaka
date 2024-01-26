using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tzedaka.Model;
using Tzedaka.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tzedaka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewChat : ContentPage
    {
        public ViewChat(Compras compra)
        {
            BindingContext = new VMChat(compra);
            InitializeComponent();
            
        }
    }
}