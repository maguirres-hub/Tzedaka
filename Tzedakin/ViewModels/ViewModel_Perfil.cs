using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tzedaka.Model;
using Tzedaka.Views;
using Xamarin.Forms;

namespace Tzedaka.ViewModels
{
    public class ViewModel_Perfil : INotifyPropertyChanged
    {
        HttpClient cliente;

        private Clientes _cliente = new Clientes();
        private ObservableCollection<Ciudades> Ciudad_ = new ObservableCollection<Ciudades>();
        public Ciudades CiudadSeleccionada { get; set; }
        public Command Btn_Modificar_Perfil { get; set; }

        public Command Btn_ir_mision { get; set; }
        public Command Btn_ir_vision { get; set; }
        public Command Btn_ir_quinesSomso { get; set; }

        private bool _isLoading;

        public string Bloque_Actual => $"Tú bloque actual es: { Cliente.Bloque }";

        public string Nombre_Completo { get; set; }

        public ViewModel_Perfil()
        {
            if (Application.Current.Properties.ContainsKey("Cliente"))
            {
                Cliente = (Clientes)Application.Current.Properties["Cliente"];

            }
            Cargar_Cliente();
            Cargar_Ciudades();
            Btn_Modificar_Perfil = new Command(() => { Actualizar_Cliente(); });
            Btn_ir_mision = new Command(() => { Ir_Mision(); });
            Btn_ir_vision = new Command(() => { Ir_Vision(); });
            Btn_ir_quinesSomso = new Command(() => { Ir_QuienesSomos(); });

            Nombre_Completo = "¡Hola " + Cliente.Nombres + " " + Cliente.Apellidos + "!";


        }

        private async void Ir_Mision()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ViewMision());
        }

        private async void Ir_Vision()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ViewVision());
        }

        private async void Ir_QuienesSomos()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ViewQuienesSomos());
        }


        private async void Cargar_Ciudades()
        {
            await Get_Ciudades();
        }

        private async void Cargar_Cliente()
        {
            await Get_Cliente();
        }

        private async void Actualizar_Cliente()
        {
            await Put_Cliente();
        }


        private async Task Get_Cliente()
        {
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);

                var url = Settings.Url + "tzedakin/api/cliente_email/" + Cliente.Correo.ToString();
                HttpResponseMessage respuesta = await cliente.GetAsync(url);

                if (respuesta.IsSuccessStatusCode)
                {

                    var clienteJson = await respuesta.Content.ReadAsStringAsync();
                    var cliente = JsonConvert.DeserializeObject<List<Clientes>>(clienteJson);

                    Cliente = cliente[0];
                    Application.Current.Properties.Remove("Correo_Cliente");
                    await Application.Current.SavePropertiesAsync();

                    Application.Current.Properties["Cliente"] = Cliente;
                    await Application.Current.SavePropertiesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }

        }

        private async Task Put_Cliente()
        {
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);

                if (CiudadSeleccionada == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Seleccione una ciudad primero", "ok");
                }
                else
                {
                    if (!string.IsNullOrEmpty(Cliente.Nombres) && !string.IsNullOrEmpty(Cliente.Apellidos) && !string.IsNullOrEmpty(Cliente.Correo) && CiudadSeleccionada.Id_Ciudad != 0 && !string.IsNullOrEmpty(Cliente.Password))
                    {

                        Clientes actualizarCliente = new Clientes
                        {
                            Nombres = Cliente.Nombres,
                            Apellidos = Cliente.Apellidos,
                            Telefono = Cliente.Telefono,
                            Direccion = Cliente.Direccion,
                            Password = Cliente.Password,
                            Id_Ciudad = CiudadSeleccionada.Id_Ciudad,

                        };

                        var json = JsonConvert.SerializeObject(actualizarCliente);
                        var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpResponseMessage respuesta = await cliente.PutAsync(Settings.Url + $"tzedakin/api/clientes_actualizacion_app/{Cliente.Id_Cliente}", contenido);

                        if (respuesta.IsSuccessStatusCode)
                        {
                            await Application.Current.MainPage.DisplayAlert("Actualizacion exitosa", "Se han actualizado tus datos correctamente.!", "ok");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Actualizacion fallido", "Se ha producido un fallo durante el registro", "ok");
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Actualizacion fallido", "Llene todos los campos", "ok");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }


        }

        private async Task Get_Ciudades()
        {
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(60);

                HttpResponseMessage respuesta = await cliente.GetAsync("https://desfrlopez.me/jhernandez/api/ciudades");
                if (respuesta.IsSuccessStatusCode)
                {
                    var ciudadesJson = await respuesta.Content.ReadAsStringAsync();
                    var ciudades = JsonConvert.DeserializeObject<ObservableCollection<Ciudades>>(ciudadesJson);
                    Ciudad = new ObservableCollection<Ciudades>(ciudades);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo cargar la lista de ciudades.!", "Ok");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
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

        public ObservableCollection<Ciudades> Ciudad
        {
            get { return Ciudad_; }
            set
            {
                if (Ciudad_ != value)
                {
                    Ciudad_ = value;
                    OnPropertyChanged(nameof(Ciudad));
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
