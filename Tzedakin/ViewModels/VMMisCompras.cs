using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tzedaka.Model;
using Tzedaka.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Tzedaka.ViewModels
{
    public class VMMisCompras : INotifyPropertyChanged
    {
        #region VARIABLES
        private Clientes _Cliente = new Clientes();
        private ObservableCollection<Compras> compras_ = new ObservableCollection<Compras>();
        private Compras compraReseña_ = new Compras();
        private ComentarioCompra _comentarioTienda = new ComentarioCompra();
        private ComentarioCompra _comentarioProducto = new ComentarioCompra();

        public event PropertyChangedEventHandler PropertyChanged;

        public Compras Parametrosrecibe { get; set; }
        #endregion
        #region CONSTRUCTOR
        public VMMisCompras()
        {
            if (Application.Current.Properties.ContainsKey("Cliente"))
            {
                Cliente = (Clientes)Application.Current.Properties["Cliente"];
                Application.Current.SavePropertiesAsync();
            }
            Cargar_Todo();
        }
        private async void Cargar_Todo()
        {
            await Compras_Cliente(Cliente.Id_Cliente);
        }
        #endregion
        #region OBJETOS
        public Clientes Cliente
        {
            get { return _Cliente; }
            set { _Cliente = value; }
        }
        public ComentarioCompra ComentarioTienda
        {
            get { return _comentarioTienda; }
            set { _comentarioTienda = value; }
        }
        public ComentarioCompra ComentarioProducto
        {
            get { return _comentarioProducto; }
            set { _comentarioProducto = value; }
        }
        public ObservableCollection<Compras> Compras_
        {
            get { return compras_; }
            set
            {
                if (compras_ != value)
                {
                    compras_ = value;
                    OnPropertyChanged(nameof(Compras_));
                }
            }
        }
        public Compras CompraReseña
        {
            get { return compraReseña_; }
            set
            {
                if (compraReseña_ != value)
                {
                    compraReseña_ = value;
                    OnPropertyChanged(nameof(CompraReseña));
                }
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region PROCESOS
        public async Task Compras_Cliente(int idCliente)
        {
            try
            {

                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(120);

                var url = Settings.Url + $"tzedakin/api/productos_comprados/{idCliente}";
                HttpResponseMessage respuesta = await client.GetAsync(url);


                if (respuesta.IsSuccessStatusCode)
                {

                    var JsonCompras = await respuesta.Content.ReadAsStringAsync();
                    var Produc = JsonConvert.DeserializeObject<ObservableCollection<Compras>>(JsonCompras);
                    if (Produc.Count > 0)
                    {
                        Compras_ = new ObservableCollection<Compras>(Produc);
                        foreach (var prod in compras_)
                        {
                            if (prod.Img_Blob != null && prod.Img_Blob.Type == "Buffer")
                            {
                                prod.Img_Blob.DatosImagen = Convert.FromBase64String(Encoding.UTF8.GetString(prod.Img_Blob.DatosImagen));
                                prod.Img_Blob.Type = "image/jpeg"; // Or "image/jpeg" if applicable
                                prod.Imagen_Ubicacion = ImageSource.FromStream(() => new MemoryStream(prod.Img_Blob.DatosImagen));
                            }
                            if (prod.idEstado == 1) prod.colorTextoEstado = "#DD9A19";
                            else if (prod.idEstado == 2) prod.colorTextoEstado = "#008000";
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Notificacion", "No tienes compras publicados", "Ok");
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }

        }
        private async Task IrReseñaCompra(Compras compra)
        {
            VMMisCompras viewModel = new VMMisCompras();
            viewModel.CompraReseña = compra;
            await Application.Current.MainPage.Navigation.PushAsync(new ViewReseñaCompra() { BindingContext = viewModel });
        }
        private async Task IrChat(Compras compra)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ViewChat(compra));
        }
        public async Task AgregarComentario()
        {
            try
            {
                if (ComentarioTienda.puntuacion == 0 || _comentarioProducto.puntuacion == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Notificacion", "Debe agregar la calificación", "Ok");
                }
                else
                {
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromSeconds(120);

                    var url = Settings.Url + $"tzedakin/api/comentarios_compra/";
                    string Fecha = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    ComentarioCompra comentarioTiendaEnvio = new ComentarioCompra
                    {
                        idCliente = Cliente.Id_Cliente,
                        idProducto = 0,
                        idTienda = CompraReseña.Id_Tienda,
                        fecha = DateTime.Now,
                        puntuacion = ComentarioTienda.puntuacion,
                        texto = ComentarioTienda.texto
                    };
                    ComentarioCompra comentarioProductoEnvio = new ComentarioCompra
                    {
                        idCliente = Cliente.Id_Cliente,
                        idProducto = CompraReseña.Id_Producto,
                        idTienda = 0,
                        fecha = DateTime.Now,
                        puntuacion = ComentarioProducto.puntuacion,
                        texto = ComentarioProducto.texto
                    };
                    var jsonTienda = JsonConvert.SerializeObject(comentarioTiendaEnvio);
                    var contenidoTienda = new StringContent(jsonTienda, Encoding.UTF8, "application/json");
                    var jsonProducto = JsonConvert.SerializeObject(comentarioProductoEnvio);
                    var contenidoProducto = new StringContent(jsonProducto, Encoding.UTF8, "application/json");
                    HttpResponseMessage respuestaTienda = await client.PostAsync(url, contenidoTienda);
                    HttpResponseMessage respuestaProducto = await client.PostAsync(url, contenidoProducto);

                    if (respuestaTienda.IsSuccessStatusCode && respuestaProducto.IsSuccessStatusCode)
                    {
                        await Application.Current.MainPage.DisplayAlert("Notificacion", "Su comentario ha sido guardado", "Ok");
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }

        }
        public void CambiarPuntuacionTienda(int puntaje)
        {
            ComentarioTienda.puntuacion = puntaje;
        }
        #endregion
        #region COMANDOS
        public ICommand IrReseñaCompraCommand => new Command<Compras>(async (c) => await IrReseñaCompra(c));
        public ICommand IrChatCommand => new Command<Compras>(async (c) => await IrChat(c));
        public ICommand AgregarComentarioCommand => new Command(async () => await AgregarComentario());
        #endregion
    }
}
