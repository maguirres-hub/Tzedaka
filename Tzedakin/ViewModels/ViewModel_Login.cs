using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tzedaka.Model;
using Tzedaka.Views;
using Xamarin.Forms;

namespace Tzedaka.ViewModels
{
    public class ViewModel_Login : INotifyPropertyChanged
    {
        private Clientes _Cliente = new Clientes();
        private HttpClient cliente;
        private bool _isLoading;
        public Command Loguearse { get; set; }
        public Command Recuperar { get; set; }

        public ViewModel_Login()
        {
            Loguearse = new Command(() => Iniciar_Sesion());
            Recuperar = new Command(() => Recuperar_Password());
        }


        private async void Recuperar_Password()
        {
            try
            {
                if (!string.IsNullOrEmpty(Cliente.Correo))
                {
                    await Put_Cliente_Password();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Debe ingresar el correo electronico en la casilla luego presionar nuevamente, para recuperar su contraseña", "ok");
                    //await Application.Current.MainPage.DisplayAlert("Aviso", "Necesi tzedaka.tzadikim@gmail.com.!", "ok");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }

        }

        public Clientes Cliente
        {
            get { return _Cliente; }
            set
            {
                if (_Cliente != value)
                {
                    _Cliente = value;
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
        public async void Iniciar_Sesion()
        {
            IsLoading = true;
            try
            {

                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);

                var url = $"https://berajotweb.com/tzedakin/api/login_clientes/{Cliente.Correo}/{Cliente.Password}";
                HttpResponseMessage respuesta = await cliente.GetAsync(url);
                if (respuesta.IsSuccessStatusCode)
                {
                    var clienteJson = await respuesta.Content.ReadAsStringAsync();
                    var cliente = JsonConvert.DeserializeObject<List<Clientes>>(clienteJson);
                    Cliente.Is_Valid = cliente[0].Is_Valid;

                    if (Cliente.Is_Valid == 1)
                    {
                        Cliente.Active = cliente[0].Active;
                        if (Cliente.Active == 1)
                        {
                            Cliente.Id_Subscripcion = cliente[0].Id_Subscripcion;
                            if (Cliente.Id_Subscripcion == 0)
                            {
                                await ObtenerCliente();
                                await Application.Current.MainPage.Navigation.PushAsync(new ViewPortal());
                            }
                            else if (Cliente.Id_Subscripcion == 1)
                            {
                                await ObtenerCliente();
                                 if(Cliente.Bloque == 1)
                                  {
                                      await Application.Current.MainPage.Navigation.PushAsync(new ViewPortalSub());
                                  }
                                  else if(Cliente.Bloque == 2 || Cliente.Bloque == 3)
                                  {
                                      await Application.Current.MainPage.Navigation.PushAsync(new ViewPortalSubDonador());
                                }
                                else
                                {
                                    await Application.Current.MainPage.Navigation.PushAsync(new ViewPortalSubEspera());
                                }

                            }
                        }
                        else if (Cliente.Active == 0)
                        {
                            await Application.Current.MainPage.DisplayAlert("Inicio de sesion fallido", "Has sido bloqueado, envia un correo a tzedaka.tzadikim@gmail.com, para mayor informacion.!", "ok");
                        }

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Inicio de sesion fallido", "Credenciales Incorrectas.!", "ok");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Inicio de sesion fallido", "Ocurrio un problema con la solicitud.!", "ok");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
            IsLoading = false;
        }

        private async Task ObtenerCliente()
        {
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(60);

                var url = "https://berajotweb.com/tzedakin/api/cliente_email/" + Cliente.Correo.ToString();
                HttpResponseMessage respuesta = await cliente.GetAsync(url);

                if (respuesta.IsSuccessStatusCode)
                {
                    var clienteJson = await respuesta.Content.ReadAsStringAsync();
                    var cliente = JsonConvert.DeserializeObject<List<Clientes>>(clienteJson);
                    Cliente = cliente[0];
                    Application.Current.Properties["Cliente"] = Cliente;
                    await Application.Current.SavePropertiesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }

        }



        private async Task Put_Cliente_Password()
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(60);
                var URL = $"https://berajotweb.com/tzedakin/api/recuperar_password/{Cliente.Correo}";
                string password = GenerateRandomPassword(8);
                Clientes nuevoPass = new Clientes
                {
                    Password = password
                };

                var Json = JsonConvert.SerializeObject(nuevoPass);
                var Contenido = new StringContent(Json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await cliente.PutAsync(URL, Contenido);
                Debug.WriteLine(respuesta.StatusCode);
                if (respuesta.StatusCode != HttpStatusCode.NotFound)
                {
                    await Envio_Email_Recuperacion_Pass(password);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No existe usuario con ese correo electronico.!", "cerrar");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
            IsLoading = false;
        }

        private async Task Envio_Email_Recuperacion_Pass(string pass)
        {
            try
            {
                cliente = new HttpClient();
                var URL = "https://berajotweb.com/tzedakin/api/correo_rg_gmail";
                var Email_RG = new
                {
                    from = "berajot@jorgehernandezr.dev",
                    to = Cliente.Correo.ToString(),
                    subject = "Recuperacion de Contraseña Tzedakim-Tzedaká",
                    text = "Tu nueva contraseña es: " + pass.ToString()
                };

                var Json = JsonConvert.SerializeObject(Email_RG);
                var Contenido = new StringContent(Json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await cliente.PostAsync(URL, Contenido);
                if (respuesta.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Recuperacion Contraseña", "Se envio un correo electronico con tu nueva contraseña.!", "ok");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }

        }

        static string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
