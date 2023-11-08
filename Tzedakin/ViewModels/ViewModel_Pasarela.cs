using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tzedaka.Model;
using Xamarin.Forms;

namespace Tzedaka.ViewModels
{
    public class ViewModel_Pasarela : INotifyPropertyChanged
    {

        HttpClient Http_Cliente;
        public Command Btn_Recargar { get; set; }
        public int CV { get; set; }
        public string Nombre_Tarjeta { get; set; }
        public float Cantidad { get; set; }

        private DateTime _fechaSeleccionadaTarjeta;
        private Reporte_Billetera _Reporte_Billetera = new Reporte_Billetera();
        private List<Billetera_Virtual> _Virtual_Billetera = new List<Billetera_Virtual>();
        private bool _isLoading;
        private Clientes _cliente = new Clientes();


        public ViewModel_Pasarela()
        {
            Btn_Recargar = new Command(() => Billetera());
            FechaSeleccionadaTarjeta = DateTime.Now;
            if (Application.Current.Properties.ContainsKey("Cliente"))
            {
                Cliente = (Clientes)Application.Current.Properties["Cliente"];
            }
            Cargar_Datos_Billetera();
        }

        private async void Cargar_Datos_Billetera()
        {
            await Obtener_Billetera();
        }
        private async void Billetera()
        {
            await Actualizar_Billetera();
        }

        public async Task Obtener_Billetera()
        {
            try
            {
                Http_Cliente = new HttpClient();
                Http_Cliente.Timeout = TimeSpan.FromSeconds(120);
                var Url = Settings.Url + $"tzedakin/api/billetera_virtual/{Cliente.Id_Cliente}";

                HttpResponseMessage respuesta = await Http_Cliente.GetAsync(Url);
                if (respuesta.IsSuccessStatusCode)
                {
                    var Billeter = await respuesta.Content.ReadAsStringAsync();
                    var BilleteraConvert = JsonConvert.DeserializeObject<List<Billetera_Virtual>>(Billeter);
                    Virtual_Billetera = BilleteraConvert;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }


        }

        public async Task Actualizar_Billetera()
        {
            IsLoading = true;
            try
            {
                Http_Cliente = new HttpClient();
                Http_Cliente.Timeout = TimeSpan.FromSeconds(120);

                if (!string.IsNullOrEmpty(Nombre_Tarjeta) && FechaSeleccionadaTarjeta != null && CV != 0 || CV.ToString().Length == 3 && Cantidad != 0)
                {


                    float NuevoTotal = Virtual_Billetera[0].Total + Cantidad;
                    Billetera_Virtual nuevaCarga = new Billetera_Virtual
                    {
                        Total = NuevoTotal
                    };
                    var json = JsonConvert.SerializeObject(nuevaCarga);
                    var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage respuesta = await Http_Cliente.PutAsync(Settings.Url + $"tzedakin/api/billetera_virtual/{Cliente.Id_Cliente}", contenido);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        await Reportar_Billetera();

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Registro fallido", "Se ha producido un fallo durante el registro", "ok");
                    }

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Recarga Fallida", "Llene todos los campos", "ok");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }

            IsLoading = false;
        }

        public async Task Reportar_Billetera()
        {
            IsLoading = true;
            try
            {
                Http_Cliente = new HttpClient();
                Http_Cliente.Timeout = TimeSpan.FromSeconds(120);


                float NuevoTotal = Virtual_Billetera[0].Total + Cantidad;
                Reporte_Billetera nuevaCarga = new Reporte_Billetera
                {
                    fecha = FechaSeleccionadaTarjeta.ToString("yyyy-MM-dd"),
                    cantidad = Cantidad,
                    total = NuevoTotal,
                    id_cliente = Cliente.Id_Cliente,
                    motivo = "Recarga Pasarela",
                    codigo_producto = 0,

                };
                var json = JsonConvert.SerializeObject(nuevaCarga);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await Http_Cliente.PostAsync(Settings.Url + $"tzedakin/api/reportes_billetera", contenido);

                if (respuesta.IsSuccessStatusCode)
                {
                    Cargar_Datos_Billetera();
                    await Application.Current.MainPage.DisplayAlert("Recarga exitosa.!", "Has recargado " + Cantidad.ToString() + " tienes un total de " + NuevoTotal.ToString(), "ok");
                    // await Application.Current.MainPage.Navigation.PushAsync(new ViewPortal());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Registro fallido", "Se ha producido un fallo durante el registro", "ok");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            IsLoading = false;
        }

        public Clientes Cliente
        {
            get { return _cliente; }
            set
            {
                if (_cliente != value)
                {
                    _cliente = value;
                    OnPropertyChanged(nameof(Cliente));
                }
            }
        }

        public DateTime FechaSeleccionadaTarjeta
        {
            get { return _fechaSeleccionadaTarjeta; }
            set
            {
                if (_fechaSeleccionadaTarjeta != value)
                {
                    _fechaSeleccionadaTarjeta = value;
                    OnPropertyChanged(nameof(FechaSeleccionadaTarjeta));
                }
            }
        }

        public Reporte_Billetera Reporte_Pasarela
        {
            get { return _Reporte_Billetera; }
            set
            {
                if (_Reporte_Billetera != value)
                {
                    _Reporte_Billetera = value;
                    OnPropertyChanged(nameof(Reporte_Pasarela));
                }
            }
        }

        public List<Billetera_Virtual> Virtual_Billetera
        {
            get { return _Virtual_Billetera; }
            set
            {
                if (_Virtual_Billetera != value)
                {
                    _Virtual_Billetera = value;
                    OnPropertyChanged(nameof(Virtual_Billetera));
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
