using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tzedaka.Model;
using Tzedaka.Views;
using Xamarin.Forms;

namespace Tzedaka.ViewModels
{
    public class ViewModel_Billetera : INotifyPropertyChanged
    {
        HttpClient cliente;

        private Clientes Cliente_ = new Clientes();
        private ObservableCollection<Reporte_Billetera> Reportes_Billetera_ = new ObservableCollection<Reporte_Billetera>();
        private Reporte_Billetera Reporte_Billetera_ = new Reporte_Billetera();
        private Billetera_Virtual Cliente_Billetera_ = new Billetera_Virtual();
        public Command Comando_Btn { get; set; }
        public Command Comando_Btn_Cargar { get; set; }
        public Command Btn_ir_retiro { get; set; }

        private bool _isLoading;



        public ViewModel_Billetera()
        {
            if (Application.Current.Properties.ContainsKey("Cliente"))
            {
                Cliente = (Clientes)Application.Current.Properties["Cliente"];
                Comando_Btn = new Command(() => _ = Ir_Cargar_Billetera());
                Comando_Btn_Cargar = new Command(() => Cargar_Billetera());
                Btn_ir_retiro = new Command(() => Ir_Retiro());
                Cargar_Billetera();
                Cargar_Reportes_Billetera();
            }
        }

        private async void Ir_Retiro()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ViewRetiro());
        }

        private async void Cargar_Billetera()
        {
            await Get_Billetera();
            Cargar_Reportes_Billetera();
        }

        private async Task Ir_Cargar_Billetera()
        {
            var viewModelPasarela = new ViewModel_Pasarela();
            var viewPasarela = new ViewPasarela() { BindingContext = viewModelPasarela };
            await Application.Current.MainPage.Navigation.PushAsync(viewPasarela);
        }

        private async void Cargar_Reportes_Billetera()
        {
            await Get_Reportes_Billetera();
        }

        private async Task Get_Billetera()
        {
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(60);
                var Url = $"https://berajotweb.com/tzedakin/api/billetera_virtual/{Cliente.Id_Cliente}";

                HttpResponseMessage respuesta = await cliente.GetAsync(Url);
                if (respuesta.IsSuccessStatusCode)
                {
                    var Billeter = await respuesta.Content.ReadAsStringAsync();
                    var BilleteraConvert = JsonConvert.DeserializeObject<List<Billetera_Virtual>>(Billeter);
                    if (BilleteraConvert.Count > 0)
                    {
                        Cliente_Billetera = BilleteraConvert.LastOrDefault();
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }

        }
        private async Task Get_Reportes_Billetera()
        {
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(60);
                var Url = $"https://berajotweb.com/tzedakin/api/reportes_billetera_inner/{Cliente.Id_Cliente}";

                HttpResponseMessage respuesta = await cliente.GetAsync(Url);
                if (respuesta.IsSuccessStatusCode)
                {
                    var Billeter = await respuesta.Content.ReadAsStringAsync();
                    var BilleteraConvert = JsonConvert.DeserializeObject<ObservableCollection<Reporte_Billetera>>(Billeter);
                    if (BilleteraConvert.Count > 0)
                    {
                        Billetera_Reportes = new ObservableCollection<Reporte_Billetera>(BilleteraConvert);

                        foreach (var item in Billetera_Reportes)
                        {
                            if (DateTime.TryParse(item.fecha, out DateTime parsedDate))
                            {
                                item.fecha = parsedDate.ToString("yyyy-MM-dd");
                            }
                        }
                        Billetera_Reporte = Billetera_Reportes.LastOrDefault();

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }

        }


        public Clientes Cliente
        {
            get { return Cliente_; }
            set
            {
                if (Cliente_ != null)
                {
                    Cliente_ = value;
                    OnPropertyChanged(nameof(Cliente));
                }
            }
        }
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
        public ObservableCollection<Reporte_Billetera> Billetera_Reportes
        {
            get { return Reportes_Billetera_; }
            set
            {
                if (Reportes_Billetera_ != value)
                {
                    Reportes_Billetera_ = value;
                    OnPropertyChanged(nameof(Billetera_Reportes));
                }
            }
        }

        public Reporte_Billetera Billetera_Reporte
        {
            get { return Reporte_Billetera_; }
            set
            {
                if (Reporte_Billetera_ != value)
                {
                    Reporte_Billetera_ = value;
                    OnPropertyChanged(nameof(Billetera_Reporte));
                }
            }
        }

        public Billetera_Virtual Cliente_Billetera
        {
            get
            {
                return Cliente_Billetera_;
            }
            set
            {
                if (Cliente_Billetera_ != value)
                {
                    Cliente_Billetera_ = value;
                    OnPropertyChanged(nameof(Cliente_Billetera));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
