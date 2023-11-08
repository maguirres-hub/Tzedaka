using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tzedaka.Model;
using Xamarin.Forms;

namespace Tzedaka.ViewModels
{
    public class ViewModel_Retiro : INotifyPropertyChanged
    {
        HttpClient cliente;
        private Reporte_Billetera Reporte_Billetera_ = new Reporte_Billetera();
        private Reporte_Retiros Reporte_Retiros_ = new Reporte_Retiros();
        private Clientes Cliente_ = new Clientes();



        private bool _isLoading;
        public string Descripcion_Banco { get; set; }
        public float Cantidad_Retiro { get; set; }

        public Command Btn_Retirar { get; set; }







        public ViewModel_Retiro()
        {
            if (Application.Current.Properties.ContainsKey("Cliente"))
            {
                Cliente = (Clientes)Application.Current.Properties["Cliente"];

            }
            Obtener_Billetera();
            Btn_Retirar = new Command(() => { Retirar(); });
        }

        private async void Obtener_Billetera()
        {
            await Get_Billetera();
        }

        private async void Retirar()
        {
            if (Cantidad_Retiro != 0 && !string.IsNullOrEmpty(Descripcion_Banco))
            {
                if (Cantidad_Retiro >= 0)
                {
                    if (Cantidad_Retiro <= Cliente_Billetera.Total)
                    {
                        IsLoading = true;
                        await Post_Retiro();
                        IsLoading = false;
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "La cantidad de retiro supera a la actual.!", "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "La cantidad de retiro no puede ser negativa.", "OK");
                }

            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe llenar todos los campos.!", "ok");
            }

        }

        private async Task Get_Billetera()
        {
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(60);
                var Url = Settings.Url + $"tzedakin/api/billetera_virtual/{Cliente.Id_Cliente}";

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


        public async Task Actualizar_Billetera()
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);

                string Fecha_Compra = DateTime.Now.ToString("yyyy-MM-dd");
                Reporte_Billetera nuevaCarga = new Reporte_Billetera
                {
                    fecha = Fecha_Compra,
                    cantidad = Cantidad_Retiro,
                    total = Cantidad_Retiro,
                    id_cliente = Cliente.Id_Cliente,
                    motivo = "Retiro Pendiente.",
                    codigo_producto = 0,

                };
                var json = JsonConvert.SerializeObject(nuevaCarga);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await cliente.PostAsync(Settings.Url + $"tzedakin/api/reportes_billetera", contenido);

                if (respuesta.IsSuccessStatusCode)
                {
                    await Actualizar_Billetera_Cliente();

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
        public async Task Post_Retiro()
        {
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(60);
                var url = Settings.Url + "tzedakin/api/reportes_retiro/";

                IsLoading = true;
                string Fecha_Retiro = DateTime.Now.ToString("yyyy-MM-dd");
                Reporte_Retiros producto = new Reporte_Retiros
                {
                    Fecha = Fecha_Retiro,
                    Cantidad = Cantidad_Retiro,
                    Id_Estado = 1,
                    Id_Cliente = Cliente.Id_Cliente,
                    Descripcion = Descripcion_Banco

                };
                var Json = JsonConvert.SerializeObject(producto);
                var Contenido = new StringContent(Json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await cliente.PostAsync(url, Contenido);

                if (respuesta.IsSuccessStatusCode)
                {
                    await Actualizar_Billetera();

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }

            IsLoading = false;
        }

        public async Task Actualizar_Billetera_Cliente()
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);


                float NuevoTotal = Cliente_Billetera.Total - Cantidad_Retiro;
                Billetera_Virtual nuevaCarga = new Billetera_Virtual
                {
                    Total = NuevoTotal
                };
                var json = JsonConvert.SerializeObject(nuevaCarga);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await cliente.PutAsync(Settings.Url + $"tzedakin/api/billetera_virtual/{Cliente.Id_Cliente}", contenido);

                if (respuesta.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Retiro Realizado", "Se ha realizado el retiro, Este pendiente de la confirmación vía correo electronico, presione el boton recargar.!", "ok");
                    await Get_Billetera();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Registro fallido", "Se ha producido un fallo durante el registro", "ok");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }

            IsLoading = false;
        }

        private Billetera_Virtual Cliente_Billetera_ = new Billetera_Virtual();
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
        public Clientes Cliente
        {
            get { return Cliente_; }
            set
            {
                if (Cliente_ != value)
                {
                    Cliente_ = value;
                    OnPropertyChanged(nameof(Cliente));
                }
            }
        }

        public Reporte_Retiros Reporte_Retiros
        {
            get { return Reporte_Retiros_; }
            set
            {
                if (Reporte_Retiros_ != value)
                {
                    Reporte_Retiros_ = value;
                    OnPropertyChanged(nameof(Reporte_Retiros));
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
