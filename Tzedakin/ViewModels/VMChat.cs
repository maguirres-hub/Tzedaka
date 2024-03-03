using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tzedaka.Model;
using Tzedaka.Views;
using Xamarin.Forms;

namespace Tzedaka.ViewModels
{
    public class VMChat : INotifyPropertyChanged
    {
        #region VARIABLES
        private Clientes _Cliente = new Clientes();
        public ObservableCollection<MensajeChat> _mensajes = new ObservableCollection<MensajeChat>();
        public string _textoMensaje;
        private Compras compra;
        public event PropertyChangedEventHandler PropertyChanged;
        public event CollectionChangeEventHandler CollectionChange;
        #endregion
        #region CONSTRUCTOR
        public VMChat(Compras compra)
        {
            if (Application.Current.Properties.ContainsKey("Cliente"))
            {
                Cliente = (Clientes)Application.Current.Properties["Cliente"];
                Application.Current.SavePropertiesAsync();
            }
            this.compra = compra;
            Cargar_Todo();
        }
        private async void Cargar_Todo()
        {
            await GetMensajes(compra.idCompra);
        }
        #endregion
        #region OBJETOS
        public Clientes Cliente
        {
            get { return _Cliente; }
            set { _Cliente = value; }
        }
        public string textoMensaje
        {
            get { return _textoMensaje; }
            set { _textoMensaje = value; OnPropertyChanged(nameof(textoMensaje)); }
        }
        public ObservableCollection<MensajeChat> Mensajes
        {
            get { return _mensajes; }
            set
            {
                if (_mensajes != value)
                {
                    _mensajes = value;
                    OnPropertyChanged(nameof(Mensajes));
                }
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region PROCESOS
        public async Task GetMensajes(int idCompra)
        {
            try
            {

                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(120);

                var url = Settings.Url + $"tzedakin/api/mensajes/{idCompra}";
                HttpResponseMessage respuesta = await client.GetAsync(url);


                if (respuesta.IsSuccessStatusCode)
                {

                    var JsonCompras = await respuesta.Content.ReadAsStringAsync();
                    var Produc = JsonConvert.DeserializeObject<ObservableCollection<MensajeChat>>(JsonCompras);
                    if (Produc.Count > 0)
                    {
                        Mensajes = new ObservableCollection<MensajeChat>(Produc);
                        foreach (var prod in Mensajes)
                        {
                            if (Cliente.Id_Cliente == prod.idUsuario) prod.propio = true;
                            else prod.propio = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }

        }
        public async Task AgregarMensaje()
        {
            try
            {

                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(120);

                var url = Settings.Url + $"tzedakin/api/mensajes/";
                MensajeChat comentarioTiendaEnvio = new MensajeChat
                {
                    idUsuario = Cliente.Id_Cliente,
                    fecha = DateTime.Now,
                    mensaje = textoMensaje,
                    idCompra = compra.idCompra
                };
                var jsonTienda = JsonConvert.SerializeObject(comentarioTiendaEnvio);
                var contenidoTienda = new StringContent(jsonTienda, Encoding.UTF8, "application/json");

                HttpResponseMessage respuestaTienda = await client.PostAsync(url, contenidoTienda);

                if (respuestaTienda.IsSuccessStatusCode)
                {
                    GetMensajes(compra.idCompra);
                    textoMensaje = "";
                }

            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }

        }
        #endregion
        #region COMANDOS
        public ICommand AgregarMensajeCommand => new Command(async () => await AgregarMensaje());
        #endregion
    }
}
