using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tzedaka.Model;
using Xamarin.Forms;

namespace Tzedaka.ViewModels
{
    public class ViewModel_Registro : INotifyPropertyChanged
    {
        private Billetera_Virtual _Billetera = new Billetera_Virtual();
        private Clientes _Cliente = new Clientes();
        private HttpClient HttpCliente_;
        public Ciudades CiudadSeleccionada { get; set; }
        public Pais PaisSeleccionado_ = new Pais();

        private ObservableCollection<Ciudades> Ciudades_ = new ObservableCollection<Ciudades>();
        private ObservableCollection<Pais> Pais_ = new ObservableCollection<Pais>();
        public Resultados _insertId = new Resultados();
        public Command Registrar { get; set; }


        private bool _aceptarTerminos;
        private bool _activarBtnRG;
        private bool _isLoading;

        private int InsertId_cliente;

        public ViewModel_Registro()
        {
            ActivarBtnRG = false;
            Registrar = new Command(() => Registrar_Cliente_AL());
            Cargar_Paises();
        }

        private async void Cargar_Paises()
        {
            await Get_Paises();
        }
        private async void Registrar_Cliente_AL()
        {
            IsLoading = true;
            await Registrar_Cliente();
        }

        private async Task Get_Ciudades()
        {
            IsLoading = true;
            try
            {

                HttpCliente_ = new HttpClient();
                HttpCliente_.Timeout = TimeSpan.FromSeconds(120);
                var Url = $"https://berajotweb.com/tzedakin/api/ciudades_inner_pais/{PaisSeleccionado.Id_Pais}";

                HttpResponseMessage respuesta = await HttpCliente_.GetAsync(Url);
                Debug.WriteLine(Url);
                if (respuesta.IsSuccessStatusCode)
                {
                    var ciudadesJson = await respuesta.Content.ReadAsStringAsync();
                    var ciudades = JsonConvert.DeserializeObject<ObservableCollection<Ciudades>>(ciudadesJson);
                    Debug.WriteLine(ciudadesJson.ToString());
                    foreach (var ciudade in ciudades)
                    {
                        Ciudades ListaCiudad = new Ciudades
                        {
                            Id_Ciudad = ciudade.Id_Ciudad,
                            Ciudad = ciudade.Ciudad
                        };
                        Ciudad.Add(ListaCiudad);
                    }
                }

                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo cargar la lista de ciudades.!", "Ok");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            IsLoading = false;
        }


        private async Task Get_Paises()
        {
            IsLoading = true;
            try
            {
                HttpCliente_ = new HttpClient();
                HttpResponseMessage respuesta = await HttpCliente_.GetAsync("https://berajotweb.com/tzedakin/api/paises");
                if (respuesta.IsSuccessStatusCode)
                {
                    var paisesJson = await respuesta.Content.ReadAsStringAsync();
                    var paises = JsonConvert.DeserializeObject<ObservableCollection<Pais>>(paisesJson);
                    if (paises != null || paises.Count > 0)
                    {
                        Pais.Clear();
                        foreach (var item in paises)
                        {
                            Pais listaPais = new Pais
                            {
                                Id_Pais = item.Id_Pais,
                                Nombre_Pais = item.Nombre_Pais,
                            };
                            Pais.Add(listaPais);
                        }
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo cargar la lista de ciudades.!", "Ok");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            IsLoading = false;
        }



        private async Task Registrar_Cliente()
        {
            try
            {
                HttpCliente_ = new HttpClient();
                if (CiudadSeleccionada == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Seleccione una ciudad primero", "ok");
                }
                else
                {
                    if (!string.IsNullOrEmpty(Cliente.Nombres) && !string.IsNullOrEmpty(Cliente.Apellidos) && !string.IsNullOrEmpty(Cliente.Correo) && CiudadSeleccionada.Id_Ciudad != 0 && !string.IsNullOrEmpty(Cliente.Password))
                    {
                        if (Cliente.Correo.Contains("@") && Cliente.Correo != "correo@correo.com")
                        {
                            var Fecha = DateTime.Now;
                            var Fecha_Registro = Fecha.ToString("yyyy-MM-dd");
                            Clientes nuevoCliente = new Clientes
                            {
                                Nombres = Cliente.Nombres,
                                Apellidos = Cliente.Apellidos,
                                Correo = Cliente.Correo,
                                Password = Cliente.Password,
                                Id_Ciudad = CiudadSeleccionada.Id_Ciudad,
                                Fecha_Regitrocl = Fecha_Registro,
                                Active = 1
                            };

                            var json = JsonConvert.SerializeObject(nuevoCliente);
                            var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                            HttpResponseMessage respuesta = await HttpCliente_.PostAsync("https://berajotweb.com/tzedakin/api/clientes/", contenido);


                            if (respuesta.IsSuccessStatusCode)
                            {
                                var error = respuesta.Content.ReadAsStringAsync();
                                if (error.Result.ToString() == "Correo_Existente")
                                {
                                    await Application.Current.MainPage.DisplayAlert("Registro fallido", "Ya existe un usuario con ese correo electronico.!", "ok");
                                    IsLoading = false;
                                }
                                else
                                {
                                    var resultadoJson = await respuesta.Content.ReadAsStringAsync();
                                    var resultado = JsonConvert.DeserializeObject<Resultados>(resultadoJson);
                                    Ultimo_IdInsertado = resultado;
                                    InsertId_cliente = Ultimo_IdInsertado.InsertId;
                                    await Registrar_Billetera();
                                }

                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Registro fallido", "Se ha producido un fallo durante el registro", "ok");
                                IsLoading = false;
                            }
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Registro fallido", "El correo debe tener el formato correcto correo@correo.com", "ok");
                            IsLoading = false;
                        }


                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Registro fallido", "Llene todos los campos", "ok");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }

        }

        private async Task Envio_Email_Registro()
        {
            IsLoading = true;
            try
            {
                HttpCliente_ = new HttpClient();
                var URL = "https://berajotweb.com/tzedakin/api/correo_rg_gmail";
                var Email_RG = new
                {
                    from = "berajot@jorgehernandezr.dev",
                    to = Cliente.Correo.ToString(),
                    subject = "Bienvenido a Tzedaká",
                    text = "El club Tzedakin-Tzedaká te da la bienvenida."
                };

                var Json = JsonConvert.SerializeObject(Email_RG);
                var Contenido = new StringContent(Json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await HttpCliente_.PostAsync(URL, Contenido);
                if (respuesta.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Bienvenido a Tzedaka", "Se envio un correo electronico, puede buscar en correos no deseados.!", "ok");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }

            IsLoading = false;
        }
        private async Task Registrar_Billetera()
        {
            try
            {
                HttpCliente_ = new HttpClient();

                Billetera_Virtual nuevoBilletera = new Billetera_Virtual
                {
                    Id_cliente = InsertId_cliente,
                    Total = 0
                };

                var json = JsonConvert.SerializeObject(nuevoBilletera);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await HttpCliente_.PostAsync("https://berajotweb.com/tzedakin/api/billetera_virtual/", contenido);

                if (respuesta.IsSuccessStatusCode)
                {
                    await Envio_Email_Registro();
                    await Application.Current.MainPage.DisplayAlert("Registro exitoso", "Se ha registrado correctamente.! se le envio un correo", "ok");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Registro fallido", "Se ha producido un fallo durante el registro", "ok");
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }

            IsLoading = false;
        }

        public Billetera_Virtual Billetera
        {
            get { return _Billetera; }
            set
            {
                if (_Billetera != value)
                {
                    _Billetera = value;
                    OnPropertyChanged(nameof(Billetera));
                }
            }
        }


        public Clientes Cliente
        {
            get
            {
                return _Cliente;
            }
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
        public bool ActivarBtnRG
        {
            get => _activarBtnRG;
            set
            {
                _activarBtnRG = value;
                OnPropertyChanged(nameof(ActivarBtnRG));
            }
        }

        public bool AceptarTerminos
        {
            get => _aceptarTerminos;
            set
            {
                _aceptarTerminos = value;
                ActivarBtnRG = _aceptarTerminos;
                OnPropertyChanged(nameof(AceptarTerminos));
            }
        }

        public ObservableCollection<Ciudades> Ciudad
        {
            get { return Ciudades_; }
            set
            {
                if (Ciudades_ != value)
                {
                    Ciudades_ = value;
                    OnPropertyChanged(nameof(Ciudad));
                }
            }
        }

        public ObservableCollection<Pais> Pais
        {
            get { return Pais_; }
            set
            {
                if (Pais_ != value)
                {
                    Pais_ = value;
                    Mostrar_Ciudades();
                    OnPropertyChanged(nameof(Pais));
                }
            }
        }

        public Pais PaisSeleccionado
        {
            get { return PaisSeleccionado_; }
            set
            {
                if (PaisSeleccionado_ != value)
                {
                    PaisSeleccionado_ = value;
                    Mostrar_Ciudades();
                    OnPropertyChanged(nameof(PaisSeleccionado));
                }
            }
        }

        private async void Mostrar_Ciudades()
        {
            if (PaisSeleccionado != null)
            {
                await Get_Ciudades();
            }

        }

        public Resultados Ultimo_IdInsertado
        {
            get { return _insertId; }
            set
            {
                if (_insertId != value)
                {
                    _insertId = value;
                    OnPropertyChanged(nameof(Ultimo_IdInsertado));
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
