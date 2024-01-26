using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Tzedaka.Model;
using Tzedaka.Views;
using Xamarin.Forms;

namespace Tzedaka.ViewModels
{
    public class ViewModel_Club : INotifyPropertyChanged
    {
        HttpClient cliente;

        private Clientes Cliente_ = new Clientes();
        private Clientes Clientes_Donar_ = new Clientes();
        private Billetera_Virtual Cliente_Billetera_ = new Billetera_Virtual();
        private Billetera_Virtual Cliente_Billetera_Donatorio_ = new Billetera_Virtual();
        private Detalles_Subscripcion Detalles_Subscripcion_ = new Detalles_Subscripcion();
        private Reporte_Billetera Reporte_Billetera_ = new Reporte_Billetera();

        private ObservableCollection<Subscripcion> Subscripcion_ = new ObservableCollection<Subscripcion>();
        private ObservableCollection<Solicitudes> Solicitudes_ = new ObservableCollection<Solicitudes>();

        private Solicitudes Solicitud_ = new Solicitudes();
        private Subscripcion Subscripcion_Cliente_ = new Subscripcion();

        public Command Btn_Comprar_Subs { get; set; }
        public Command Btn_Solicitar_Subs { get; set; }
        public Command Comando_Btn_Cargar_Billetera { get; set; }
        public Command Comando_Btn_Club { get; set; }
        public Command SelectItem { get; set; }

        public Command Btn_Ir_Cursos { get; set; }

        public Command Btn_Donar { get; set; }

        private Timer Tiempo;

        private bool _isLoading;

        private bool TimerLoading;

        public float Cantidad_Donacion { get; set; }

        public ViewModel_Club()
        {
            if (Application.Current.Properties.ContainsKey("Cliente"))
            {
                Cliente = (Clientes)Application.Current.Properties["Cliente"];
                Application.Current.SavePropertiesAsync();
            }

            Tiempo = new Timer();
            Tiempo.Enabled = true;

            Btn_Comprar_Subs = new Command(() => { Compra_Subscripcion(); });
            Btn_Solicitar_Subs = new Command(() => { Solicitar_Subs(); });
            SelectItem = new Command<Solicitudes>(Solicitud => Regalar_Solicitud(Solicitud.id_cliente));
            Btn_Ir_Cursos = new Command(() => Ir_Cursos());
            Comando_Btn_Cargar_Billetera = new Command(() => Cargar_Billetera());
            Comando_Btn_Club = new Command(() => { Cargar_Todo(); });
            Btn_Donar = new Command(() => { Enviar_Donacion(); });

            Cargar_Billetera();
            Cargar_Reportes_Billetera();
            Cargar_Detalles_Subs();
            Cargar_Solicitudes();
            Cargar_Subscripciones();
            Cargar_Primer_Subs();

            try
            {
                Tiempo.Interval = 20000;
                Tiempo.Elapsed += Timer_Subscripcion;
                TimerLoading = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

        }

        private async void Enviar_Donacion()
        {
            if (Cantidad_Donacion > 0)
            {

            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Cantidad invalida.!", "Aceptar");
            }
        }

        private async void Cargar_Primer_Subs()
        {

            await Get_Primer_Subs();


        }

        private async void Timer_Subscripcion(object sender, ElapsedEventArgs e)
        {
            await Check_Soli_Subscripcion();
        }

        private async void Cargar_Todo()
        {

            Cargar_Solicitudes();
            Cargar_Subscripciones();
            Cargar_Billetera();
            Cargar_Primer_Subs();
            await Task.Delay(100);

        }

        private async void Ir_Cursos()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ViewCursos());
        }

        private async void Cargar_Billetera()
        {
            await Get_Billetera();
        }



        private async void Regalar_Solicitud(int id)
        {
            if (Detalles_Subscripcion.precio > 0)
            {
                if (Cliente_Billetera.Total < Detalles_Subscripcion.precio)
                {
                    await Application.Current.MainPage.DisplayAlert("Subscribirse", "No puedes regalar, no tiene saldo suficiente el precio de la subscripcion es de USD." + Detalles_Subscripcion.precio.ToString() + ", puede recargar ingresando a su Wallet", "OK");
                }
                else
                {
                    bool resultado = await Application.Current.MainPage.DisplayAlert("Subscribirse", "El precio de la subscripcion es de USD. " + Detalles_Subscripcion.precio.ToString() + " Su saldo es de USD." + Cliente_Billetera.Total.ToString(), "Regalar", "Salir");
                    if (resultado == true)
                    {
                        await Get_Subscripciones_bloques_Regalo(id);

                    }
                    else
                    {
                        Debug.WriteLine("No Acepto");
                    }

                }
            }
            else
            {
                Cargar_Subscripciones();
            }

        }

        private async void Cargar_Solicitudes()
        {
            if (Soli != null)
            {
                Soli.Clear();
            }
            await Get_Solicitudes_Subscripciones();
        }

        private async void Solicitar_Subs()
        {
            await Post_Solicitud_Subs();
        }

        private async void Cargar_Detalles_Subs()
        {
            await Get_Detalles_Subscripciones();
        }

        private async void Cargar_Reportes_Billetera()
        {
            await Get_Reportes_Billetera();
        }
        private async void Cargar_Subscripciones()
        {
            await Get_Subscripciones_Cliente();

        }
        private async void Compra_Subscripcion()
        {
            if (Detalles_Subscripcion.precio > 0)
            {
                if (Cliente_Billetera.Total < Detalles_Subscripcion.precio)
                {
                    await Application.Current.MainPage.DisplayAlert("Subscribirse", "No tiene saldo suficiente el precio de la subscripcion es de USD." + Detalles_Subscripcion.precio.ToString() + ", puede recargar ingresando a su Wallet", "OK");
                }
                else
                {
                    bool resultado = await Application.Current.MainPage.DisplayAlert("Subscribirse", "El precio de la subscripcion es de USD. " + Detalles_Subscripcion.precio.ToString() + " Su saldo es de USD." + Cliente_Billetera.Total.ToString(), "Comprar", "Salir");
                    if (resultado == true)
                    {
                        await Get_Subscripciones_bloques();

                    }
                    else
                    {
                        Debug.WriteLine("No Acepto");
                    }

                }
            }
            else
            {
                Cargar_Subscripciones();
            }


        }

        private async Task Check_Soli_Subscripcion()
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);
                var Url = Settings.Url + $"tzedakin/api/solicitudes_cliente/{Cliente.Id_Cliente}";
                HttpResponseMessage respuesta = await cliente.GetAsync(Url);
                if (respuesta.IsSuccessStatusCode == true)
                {
                    var StringRespuesta = await respuesta.Content.ReadAsStringAsync();
                    var Json = JsonConvert.DeserializeObject<List<Solicitudes>>(StringRespuesta);
                    if (Json.Count > 0)
                    {
                        foreach (var item in Json)
                        {
                            if (item.Activado == 1)
                            {
                                await Get_Subscripciones_bloques();
                                await Eliminar_Subscripciones_Solicitada(Cliente.Id_Cliente);
                                Tiempo.Enabled = false;
                            }
                        };
                    }
                }
            }
            catch (TaskCanceledException)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "La solicitud se ha cancelado debido a un tiempo de espera agotado con el servidor.", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
            IsLoading = false;
        }

        // obtiene los bloque sy posiciones
        private async Task Get_Subscripciones_bloques()
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);

                var url = Settings.Url + "tzedakin/api/subscripciones";
                HttpResponseMessage respuesta = await cliente.GetAsync(url);

                if (respuesta.IsSuccessStatusCode)
                {
                    var SubsJson = await respuesta.Content.ReadAsStringAsync();
                    var Subs = JsonConvert.DeserializeObject<ObservableCollection<Subscripcion>>(SubsJson);
                    Subscripcion = Subs;

                    int nuevaposicion = 0;
                    int ultima_posicion = 0;

                    if (Subscripcion.Count > 0)
                    {
                        ultima_posicion = Subscripcion[Subscripcion.Count - 1].Posicion_Bloque;
                        nuevaposicion = ultima_posicion + 1;
                    }
                    else
                    {
                        nuevaposicion = 1;
                    }

                    int grupo = (nuevaposicion - 1) / 2 + 1;

                    await RegistrarSubscripcion(grupo, nuevaposicion);

                }
            }
            catch (TaskCanceledException)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "La solicitud se ha cancelado debido a un tiempo de espera agotado con el servidor.", "Ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Ocurrio un error procesando la solicitud", "Ok");
                Debug.WriteLine(ex.Message.ToString());
            }

        }
        // post subscripcion al comprar
        private async Task RegistrarSubscripcion(int nuevobloque, int nuevaposicion)
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);

                DateTime fechaActual = DateTime.Now;

                // Agregar 30 días a la fecha actual
                DateTime fechaFinal = fechaActual.AddDays(30);

                // Si la fecha final es el día 31 y la fecha actual es el día 30, ajustar la fecha final a 30 días
                if (fechaActual.Day == 30 && fechaFinal.Day == 31)
                {
                    fechaFinal = fechaFinal.AddDays(-1);
                }

                Subscripcion NuevaSubscripcion = new Subscripcion
                {
                    Id_Detalles_Sub = Detalles_Subscripcion.id_detalles_sub,
                    Id_Cliente = Cliente.Id_Cliente,
                    Fecha_Inicio = fechaActual.ToString("yyyy-MM-dd"),
                    Fecha_Final = fechaFinal.ToString("yyyy-MM-dd"),
                    Bloque = nuevobloque,
                    Posicion_Bloque = nuevaposicion,
                    Ultima_Donacion = 0,
                    Vueltas = 0,
                    Donacion_Activa = 0

                };
                var json = JsonConvert.SerializeObject(NuevaSubscripcion);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                var url = Settings.Url + "tzedakin/api/subscripciones";
                HttpResponseMessage respuesta = await cliente.PostAsync(url, contenido);
                if (respuesta.IsSuccessStatusCode == true)
                {
                    await Put_Billetera_Virtual();
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        Application.Current.Properties.Remove("Cliente");
                        await Application.Current.SavePropertiesAsync();
                        await Application.Current.MainPage.Navigation.PushAsync(new MainPage());
                        await Application.Current.MainPage.DisplayAlert("Subscripcion", "Bienvenido a club Tzedaka-Tzadikim inicia sesion nuevamente.!!", "Completada");
                    });


                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
            IsLoading = false;

        }

        /* private async Task Get_Posicion_Uno()
         {
             IsLoading = true;
             try
             {
                 cliente = new HttpClient();
                 cliente.Timeout = TimeSpan.FromSeconds(60);
                 var Url = Settings.Url + $"tzedakin/api/solicitudes/{id}";

                 HttpResponseMessage respuesta = await cliente.DeleteAsync(Url);
                 if (respuesta.IsSuccessStatusCode)
                 {
                     Cargar_Solicitudes();
                     Debug.WriteLine("Solicitud Eliminada");
                 }
             }
             catch (Exception ex)
             {
                 Debug.WriteLine(ex.Message.ToString());
             }
             IsLoading = false;
         }*/

        private async Task Get_Subscripciones_bloques_Regalo(int id)
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);

                var url = Settings.Url + "tzedakin/api/subscripciones";
                HttpResponseMessage respuesta = await cliente.GetAsync(url);

                if (respuesta.IsSuccessStatusCode)
                {
                    var SubsJson = await respuesta.Content.ReadAsStringAsync();
                    var Subs = JsonConvert.DeserializeObject<ObservableCollection<Subscripcion>>(SubsJson);
                    Subscripcion = Subs;

                    int nuevaposicion = 0;
                    int ultima_posicion = 0;

                    if (Subscripcion.Count > 0)
                    {
                        ultima_posicion = Subscripcion[Subscripcion.Count - 1].Posicion_Bloque;
                        nuevaposicion = ultima_posicion + 1;
                    }
                    else
                    {
                        nuevaposicion = 1;
                    }

                    int grupo = (nuevaposicion - 1) / 2 + 1;

                    await RegistrarSubscripcionRegalo(id, grupo, nuevaposicion);

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
        }

        private async Task RegistrarSubscripcionRegalo(int id, int nuevobloque, int nuevaposicion)
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);

                DateTime fechaActual = DateTime.Now;

                // Agregar 30 días a la fecha actual
                DateTime fechaFinal = fechaActual.AddDays(30);

                // Si la fecha final es el día 31 y la fecha actual es el día 30, ajustar la fecha final a 30 días
                if (fechaActual.Day == 30 && fechaFinal.Day == 31)
                {
                    fechaFinal = fechaFinal.AddDays(-1);
                }

                Subscripcion NuevaSubscripcion = new Subscripcion
                {
                    Id_Detalles_Sub = Detalles_Subscripcion.id_detalles_sub,
                    Id_Cliente = id,
                    Fecha_Inicio = fechaActual.ToString("yyyy-MM-dd"),
                    Fecha_Final = fechaFinal.ToString("yyyy-MM-dd"),
                    Bloque = nuevobloque,
                    Posicion_Bloque = nuevaposicion,
                    Ultima_Donacion = 0,
                    Vueltas = 0

                };

                var json = JsonConvert.SerializeObject(NuevaSubscripcion);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");

                var url = Settings.Url + "tzedakin/api/subscripciones";
                HttpResponseMessage respuesta = await cliente.PostAsync(url, contenido);

                if (respuesta.IsSuccessStatusCode)
                {
                    await Put_Billetera_Virtual_Regalo();
                    await Eliminar_Subscripciones_Solicitada(id);
                    await Application.Current.MainPage.DisplayAlert("Subscripcion", "Has Regalado una subscripcion gracias por colaborar.!!", "Completada", "Salir");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
            IsLoading = false;

        }


        private async Task Eliminar_Subscripciones_Solicitada(int id)
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(60);
                var Url = Settings.Url + $"tzedakin/api/solicitudes/{id}";

                HttpResponseMessage respuesta = await cliente.DeleteAsync(Url);
                if (respuesta.IsSuccessStatusCode)
                {
                    Cargar_Solicitudes();
                    Debug.WriteLine("Solicitud Eliminada");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
            IsLoading = false;

        }

        public async Task Put_Billetera_Virtual()
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);


                float NuevoTotal = Cliente_Billetera.Total - Detalles_Subscripcion.precio;
                Billetera_Virtual nuevaCarga = new Billetera_Virtual
                {
                    Total = NuevoTotal
                };
                var json = JsonConvert.SerializeObject(nuevaCarga);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await cliente.PutAsync(Settings.Url + $"tzedakin/api/billetera_virtual/{Cliente.Id_Cliente}", contenido);

                if (respuesta.IsSuccessStatusCode)
                {
                    await Put_Reporte_Billetera();
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

        public async Task Put_Reporte_Billetera()
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);

                float NuevoTotal = Billetera_Reporte.total - Detalles_Subscripcion.precio;
                string Fecha_Compra = DateTime.Now.ToString("yyyy-MM-dd");
                Reporte_Billetera nuevaCarga = new Reporte_Billetera
                {
                    fecha = Fecha_Compra,
                    cantidad = Detalles_Subscripcion.precio,
                    total = NuevoTotal,
                    id_cliente = Cliente.Id_Cliente,
                    motivo = "Compra Subscripción",
                    codigo_producto = 0,

                };
                var json = JsonConvert.SerializeObject(nuevaCarga);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await cliente.PostAsync(Settings.Url + $"tzedakin/api/reportes_billetera", contenido);

                if (respuesta.IsSuccessStatusCode)
                {

                    await Application.Current.MainPage.DisplayAlert("Subscripción exitosa.!", "Revisa tu billetera. Recuerda utilizar el boton recargar para actualizar.!", "ok");

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
        public async Task Put_Billetera_Virtual_Regalo()
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);


                float NuevoTotal = Cliente_Billetera.Total - Detalles_Subscripcion.precio;
                Billetera_Virtual nuevaCarga = new Billetera_Virtual
                {
                    Total = NuevoTotal
                };
                var json = JsonConvert.SerializeObject(nuevaCarga);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await cliente.PutAsync(Settings.Url + $"tzedakin/api/billetera_virtual/{Cliente.Id_Cliente}", contenido);

                if (respuesta.IsSuccessStatusCode)
                {
                    await Put_Reporte_Billetera_Regalo();
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

        public async Task Put_Reporte_Billetera_Regalo()
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);

                float NuevoTotal = Billetera_Reporte.total - Detalles_Subscripcion.precio;
                string Fecha_Compra = DateTime.Now.ToString("yyyy-MM-dd");
                Reporte_Billetera nuevaCarga = new Reporte_Billetera
                {
                    fecha = Fecha_Compra,
                    cantidad = Detalles_Subscripcion.precio,
                    total = NuevoTotal,
                    id_cliente = Cliente.Id_Cliente,
                    motivo = "Regalo Subscripción",
                    codigo_producto = 0,

                };
                var json = JsonConvert.SerializeObject(nuevaCarga);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await cliente.PostAsync(Settings.Url + $"tzedakin/api/reportes_billetera", contenido);

                if (respuesta.IsSuccessStatusCode)
                {

                    await Application.Current.MainPage.DisplayAlert("Billetera actualizada!", "Revisa tu billetera. Recuerda utilizar el boton recargar para actualizar.! ", "ok");

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
        private async Task Get_Detalles_Subscripciones()
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(60);
                var Url = Settings.Url + "tzedakin/api/detalles_subscripcion/";

                HttpResponseMessage respuesta = await cliente.GetAsync(Url);
                if (respuesta.IsSuccessStatusCode)
                {
                    var DetallesSub = await respuesta.Content.ReadAsStringAsync();
                    var DetallesSubsConvert = JsonConvert.DeserializeObject<List<Detalles_Subscripcion>>(DetallesSub);
                    if (DetallesSubsConvert.Count > 0)
                    {
                        foreach (var item in DetallesSubsConvert)
                        {
                            Detalles_Subscripcion = item;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
            IsLoading = false;
        }

        private async Task Get_Solicitudes_Subscripciones()
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(60);
                var Url = Settings.Url + "tzedakin/api/solicitudes_inner/";

                HttpResponseMessage respuesta = await cliente.GetAsync(Url);
                if (respuesta.IsSuccessStatusCode)
                {
                    var SolicitudesSub = await respuesta.Content.ReadAsStringAsync();
                    var SolicitudesSubConvert = JsonConvert.DeserializeObject<ObservableCollection<Solicitudes>>(SolicitudesSub);
                    if (SolicitudesSubConvert.Count > 0)
                    {
                        Soli = new ObservableCollection<Solicitudes>(SolicitudesSubConvert);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
            IsLoading = false;

        }

        private async Task Get_Subscripciones_Cliente()
        {
            if (TimerLoading == true)
            {
                IsLoading = false;
                TimerLoading = false;
            }
            else
            {
                IsLoading = true;
            }
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(60);
                var Url = Settings.Url + $"tzedakin/api/subscripcion_cliente/{Cliente.Id_Cliente}";
                HttpResponseMessage respuesta = await cliente.GetAsync(Url);
                if (respuesta.IsSuccessStatusCode)
                {
                    var Sub = await respuesta.Content.ReadAsStringAsync();
                    var SubsConvert = JsonConvert.DeserializeObject<List<Subscripcion>>(Sub);
                    if (SubsConvert.Count > 0)
                    {
                        foreach (var item in SubsConvert)
                        {
                            Subscripcion_Cliente = item;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
            IsLoading = false;

        }
        private async Task Get_Billetera()
        {
            IsLoading = true;
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
                        Cliente_Billetera = BilleteraConvert[0];
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
            IsLoading = false;
        }
        private async Task Get_Reportes_Billetera()
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(60);
                var Url = Settings.Url + $"tzedakin/api/reportes_billetera_inner/{Cliente.Id_Cliente}";

                HttpResponseMessage respuesta = await cliente.GetAsync(Url);
                if (respuesta.IsSuccessStatusCode)
                {
                    var Billeter = await respuesta.Content.ReadAsStringAsync();
                    var BilleteraConvert = JsonConvert.DeserializeObject<List<Reporte_Billetera>>(Billeter);
                    if (BilleteraConvert.Count > 0)
                    {
                        foreach (var rep in BilleteraConvert)
                        {
                            Billetera_Reporte = rep;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
            IsLoading = false;
        }


        private async Task Post_Solicitud_Subs()
        {
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(60);
                var url = Settings.Url + "tzedakin/api/solicitudes";

                IsLoading = true;

                Solicitudes solicitud = new Solicitudes
                {
                    id_cliente = Cliente.Id_Cliente
                };

                Application.Current.Properties.Remove("Cliente");
                await Application.Current.SavePropertiesAsync();

                var Json = JsonConvert.SerializeObject(solicitud);
                var Contenido = new StringContent(Json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await cliente.PostAsync(url, Contenido);

                if (respuesta.IsSuccessStatusCode)
                {
                    var StringRespuesta = await respuesta.Content.ReadAsStringAsync();
                    Debug.WriteLine(StringRespuesta.ToString());
                    if (StringRespuesta.ToString() == "1")
                    {
                        Application.Current.Properties["Cliente"] = Cliente;
                        await Application.Current.SavePropertiesAsync();
                        await Application.Current.MainPage.DisplayAlert("Solicitud Enviada.!", "Se envio su solicitud para obtener premium, si es aceptada por alguien el proximo logueo sera como subscriptor.!", "ok");

                    }
                    else
                    {
                        var JsonRes = JsonConvert.DeserializeObject<List<Solicitudes>>(StringRespuesta);
                        if (JsonRes.Count > 0)
                        {
                            foreach (var item in JsonRes)
                            {
                                if (item.Activado == 0)
                                {
                                    await Application.Current.MainPage.DisplayAlert("Notificación.!", "Ya tienes una solicitud enviada espera que alguien la acepte.!", "Ok");
                                }
                            };
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }

            IsLoading = false;
        }

        private async Task Get_Primer_Subs()
        {
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(60);
                var url = Settings.Url + "tzedakin/api/primer_subscriptor";

                HttpResponseMessage respuesta = await cliente.GetAsync(url);
                if (respuesta.IsSuccessStatusCode)
                {
                    var JsonRespuesta = await respuesta.Content.ReadAsStringAsync();
                    var ListaClientes = JsonConvert.DeserializeObject<List<Clientes>>(JsonRespuesta);
                    if (ListaClientes.Count > 0)
                    {
                        foreach (var item in ListaClientes)
                        {
                            Clientes_Donar = item;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }

        }


        public Solicitudes Solicitud
        {
            get { return Solicitud_; }
            set
            {
                if (Solicitud_ != value)
                {
                    Solicitud_ = value;
                    OnPropertyChanged(nameof(Solicitud));
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

        public Clientes Clientes_Donar
        {
            get { return Clientes_Donar_; }
            set
            {
                if (Clientes_Donar_ != value)
                {
                    Clientes_Donar_ = value;

                    OnPropertyChanged(nameof(Clientes_Donar));
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

        public Billetera_Virtual Cliente_Billetera_Donatorio
        {
            get
            {
                return Cliente_Billetera_Donatorio_;
            }
            set
            {
                if (Cliente_Billetera_Donatorio_ != value)
                {
                    Cliente_Billetera_Donatorio_ = value;
                    OnPropertyChanged(nameof(Cliente_Billetera_Donatorio));
                }
            }
        }
        public Detalles_Subscripcion Detalles_Subscripcion
        {
            get
            {
                return Detalles_Subscripcion_;
            }
            set
            {
                if (Detalles_Subscripcion_ != value)
                {
                    Detalles_Subscripcion_ = value;
                    OnPropertyChanged(nameof(Detalles_Subscripcion));
                }
            }
        }
        public Subscripcion Subscripcion_Cliente
        {
            get { return Subscripcion_Cliente_; }
            set
            {
                if (Subscripcion_Cliente_ != value)
                {
                    Subscripcion_Cliente_ = value;
                    OnPropertyChanged(nameof(Subscripcion_Cliente));
                }
            }
        }

        public ObservableCollection<Subscripcion> Subscripcion
        {
            get { return Subscripcion_; }
            set
            {
                if (Subscripcion_ != value)
                {
                    Subscripcion_ = value;
                    OnPropertyChanged(nameof(Subscripcion));
                }
            }
        }
        public ObservableCollection<Solicitudes> Soli
        {
            get { return Solicitudes_; }
            set
            {
                if (Solicitudes_ != value)
                {
                    Solicitudes_ = value;
                    OnPropertyChanged(nameof(Soli));
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
