using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tzedaka.Model;
using Tzedaka.Views;
using Xamarin.Forms;

namespace Tzedaka.ViewModels
{
    class VMProductoCompra : INotifyPropertyChanged
    {
        #region VARIABLES
        int _Cantidad;
        string _Preciotexto;
        private ObservableCollection<ComentarioCompra> _comentarios = new ObservableCollection<ComentarioCompra>();
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isLoading;
        public Producto Parametrosrecibe { get; set; }
        #endregion
        #region CONSTRUCTOR
        public VMProductoCompra(Producto parametrosTrae)
        {
            _isLoading = false;
            Parametrosrecibe = parametrosTrae;
            Preciotexto = "$" + Parametrosrecibe.Precio;
            getComentarios(parametrosTrae.Id_Producto);
        }
        #endregion
        #region OBJETOS
        public ObservableCollection<ComentarioCompra> Comentarios
        {
            get { return _comentarios; }
            set
            {
                if (_comentarios != value)
                {
                    _comentarios = value;
                    OnPropertyChanged(nameof(Comentarios));
                }
            }
        }
        public string Preciotexto
        {
            get { return _Preciotexto; }
            set { _Preciotexto = value; }
        }
        public int Cantidad
        {
            get => _Cantidad;
            set
            {
                _Cantidad = value;
                OnPropertyChanged(nameof(Cantidad));
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
        #endregion
        #region PROCESOS
        public async void getComentarios(int idProducto)
        {
            try
            {
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(120);
                var url = Settings.Url + $"tzedakin/api/comentarios_producto/{idProducto}";
                HttpResponseMessage respuesta = await client.GetAsync(url);
                if (respuesta.IsSuccessStatusCode)
                {
                    var JsonCategoria = await respuesta.Content.ReadAsStringAsync();
                    var Cat = JsonConvert.DeserializeObject<ObservableCollection<ComentarioCompra>>(JsonCategoria);
                    if (Cat.Count > 0)
                    {
                        Comentarios = Cat;
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
        }
        public async Task Validarcompra()
        {
            //var funcion = new Ddetallecompras();
            //var listaXidproducto = await funcion.MostrarDcXidproducto(Parametrosrecibe.Idproducto);
            //if (listaXidproducto.Count > 0)
            //{
            //    await Editardc();
            //}
            //else
            //{
            //    await InsertarDc();
            //}
        }
        public async Task Editardc()
        {
            //if (Cantidad < 1)
            //{
            //    Cantidad = 1;
            //}
            //var funcion = new Ddetallecompras();
            //var parametros = new Mdetallecompras();
            //parametros.Cantidad = Cantidad.ToString();
            //parametros.Idproducto = Parametrosrecibe.Idproducto;
            //parametros.Preciocompra = Parametrosrecibe.Precio;
            //await funcion.Editardetalle(parametros);
            //await Volver();
        }
        public async Task InsertarDc()
        {
            //if (Cantidad == 0)
            //{
            //    Cantidad = 1;
            //}
            //var funcion = new Ddetallecompras();
            //var parametros = new Mdetallecompras();
            //parametros.Cantidad = Cantidad.ToString();
            //parametros.Idproducto = Parametrosrecibe.Idproducto;
            //parametros.Preciocompra = Parametrosrecibe.Precio;
            //double total = 0;
            //double preciocompra = Convert.ToDouble(Parametrosrecibe.Precio);
            //double cantidad = Convert.ToDouble(Cantidad);
            //total = cantidad * preciocompra;
            //parametros.Total = total.ToString();
            //await funcion.InsertarDc(parametros);
            //await Volver();
        }
        public async Task Volver()
        {
            //await Navigation.PopAsync();
        }
        private async Task ComprarProducto(Producto producto)
        {
            Clientes Comprador = (Clientes)Application.Current.Properties["Cliente"];
            if (producto.Stock < 1) await Application.Current.MainPage.DisplayAlert("Aviso", "No hay unidades disponibles del producto", "Ok");
            else
            {
                string modoPago = await Application.Current.MainPage.DisplayActionSheet("¿Desea comprar con saldo o con creditos?", "Cancelar", null, "Saldo", "Creditos");
                Billetera_Virtual saldoCliente = await SaldoByCliente(Comprador.Id_Cliente);
                switch (modoPago)
                {
                    case "Saldo":
                        if (producto.Precio <= saldoCliente.Total)
                        {
                            Billetera_Virtual saldoVendedor = await SaldoByCliente(producto.Id_Cliente);
                            DateTime fechaCompra = DateTime.Now;
                            int cant = 1;
                            await ActualizaSaldoCliente(Comprador.Id_Cliente, saldoCliente.Total - producto.Precio, fechaCompra, cant, -producto.Precio, "Compra de articulo", producto.Id_Producto, saldoCliente.creditos + (float)(producto.Precio * 0.1));
                            await ActualizaSaldoCliente(producto.Id_Cliente, saldoVendedor.Total + producto.Precio, fechaCompra, cant, producto.Precio, "Venta de articulo", producto.Id_Producto, saldoCliente.creditos);
                            await AgregaCompraTransaccion(Comprador.Id_Cliente, producto.Id_Cliente, cant, producto.Precio, fechaCompra, producto.Id_Producto);
                            await ActualizaHistorialSaldoCliente(fechaCompra, cant, (float)(producto.Precio * 0.1), Comprador.Id_Cliente, "Creditos por compra articulo", producto.Id_Producto);
                            await productoDisminuirStock(producto.Stock - 1, producto.Id_Producto);
                            await Application.Current.MainPage.Navigation.PopAsync();
                            await Application.Current.MainPage.DisplayAlert("Compra Exitosa", "Se ha realizado la compra exitosamente, Consulte el menu de mis compras para validar con el vendedor", "ok");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Aviso", "No posees fondos para comprar este producto. Recarga tu billetera", "Ok");
                        }
                        break;
                    case "Creditos":
                        if (producto.Precio <= saldoCliente.creditos)
                        {
                            Billetera_Virtual saldoVendedor = await SaldoByCliente(producto.Id_Cliente);
                            DateTime fechaCompra = DateTime.Now;
                            int cant = 1;
                            await ActualizaSaldoCliente(Comprador.Id_Cliente, saldoCliente.Total, fechaCompra, cant, -producto.Precio, "Compra de articulo con creditos", producto.Id_Producto, saldoCliente.creditos - producto.Precio);
                            await ActualizaSaldoCliente(producto.Id_Cliente, saldoVendedor.Total + producto.Precio, fechaCompra, cant, producto.Precio, "Venta de articulo", producto.Id_Producto, saldoCliente.creditos);
                            await AgregaCompraTransaccion(Comprador.Id_Cliente, producto.Id_Cliente, cant, producto.Precio, fechaCompra, producto.Id_Producto);
                            await productoDisminuirStock(producto.Stock - 1, producto.Id_Producto);
                            await Application.Current.MainPage.Navigation.PopAsync();
                            await Application.Current.MainPage.DisplayAlert("Compra Exitosa", "Se ha realizado la compra exitosamente, Consulte el menu de mis compras para validar con el vendedor", "ok");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Aviso", "No posees creditos suficientes para comprar este producto", "Ok");
                        }
                        break;
                }
            }
       
        }
        private async Task ActualizaSaldoCliente(int idCliente, float saldoNuevo, DateTime fecha, float cantidad, float variacion, string motivo, int idProducto,float creditos)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(120);
                Billetera_Virtual nuevaCarga = new Billetera_Virtual
                {
                    Total = saldoNuevo,
                    creditos = creditos
                };
                var json = JsonConvert.SerializeObject(nuevaCarga);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await client.PutAsync(Settings.Url + $"tzedakin/api/billetera_virtual/{idCliente}", contenido);

                if (respuesta.IsSuccessStatusCode)
                {
                    await ActualizaHistorialSaldoCliente(fecha, cantidad, variacion, idCliente, motivo, idProducto);
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
        }
        public async Task ActualizaHistorialSaldoCliente(DateTime fecha, float cantidad, float variacion, int idCliente, string motivo, int idProducto)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(120);

                Reporte_Billetera nuevaCarga = new Reporte_Billetera
                {
                    fecha = fecha.ToString("yyyy-MM-dd hh:mm:ss"),
                    cantidad = cantidad,
                    total = variacion,
                    id_cliente = idCliente,
                    motivo = motivo,
                    codigo_producto = idProducto,
                };
                var json = JsonConvert.SerializeObject(nuevaCarga);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await client.PostAsync(Settings.Url + $"tzedakin/api/reportes_billetera", contenido);

                if (respuesta.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Dinero Vendedor");

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
        }
        public async Task productoDisminuirStock(int cantidad, int idProducto)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(120);

                Producto nuevaCarga = new Producto
                {
                    Id_Producto = idProducto,
                    Stock = cantidad
                };
                var json = JsonConvert.SerializeObject(nuevaCarga);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await client.PutAsync(Settings.Url + $"tzedakin/api/productos_stock", contenido);

                if (respuesta.IsSuccessStatusCode)
                {
                    Debug.WriteLine("producto disminuye stock");

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
        }
        public async Task AgregaCompraTransaccion(int idComprador, int idVendedor, int cantidad, float precio, DateTime fecha, int idProducto)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(120);

                CompraTransaccion nuevaCarga = new CompraTransaccion
                {
                    fecha = fecha.ToString("yyyy-MM-dd hh:mm:ss"),
                    cantidad = cantidad,
                    precio = precio,
                    idComprador = idComprador,
                    idVendedor = idVendedor,
                    idProducto = idProducto,
                };
                var json = JsonConvert.SerializeObject(nuevaCarga);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await client.PostAsync(Settings.Url + $"tzedakin/api/compras_transacciones", contenido);

                if (respuesta.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Registro en compra exitoso");
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
        }
        private async Task<Billetera_Virtual> SaldoByCliente(int idCliente)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(120);
                var Url = Settings.Url + $"tzedakin/api/billetera_virtual/{idCliente}";
                HttpResponseMessage respuesta = await client.GetAsync(Url);
                if (respuesta.IsSuccessStatusCode)
                {
                    var Billeter = await respuesta.Content.ReadAsStringAsync();
                    var BilleteraConvert = JsonConvert.DeserializeObject<List<Billetera_Virtual>>(Billeter);
                    if (BilleteraConvert.Count > 0)
                    {
                       return BilleteraConvert[0];
                    }
                    else return null;
                }
                else throw new Exception("Error consultando datos");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
                return null;
            }
        }
        public void Auementar()
        {
            Cantidad += 1;
        }
        public void Disminuir()
        {
            if (Cantidad > 0)
            {
                Cantidad -= 1;
            }
        }
        private async Task IrTiendaCliente(int id_vendedor)
        {
            IsLoading = true;
            ViewModel_Tienda viewModel = new ViewModel_Tienda();
            await viewModel.GET_Productos_Cliente(id_vendedor);
            await viewModel.GET_Vendedor(id_vendedor);
            await viewModel.getComentarios(id_vendedor);
            await viewModel.GET_Tienda(id_vendedor);
            await Application.Current.MainPage.Navigation.PushAsync(new ViewTiendaCliente() { BindingContext = viewModel });
            IsLoading = false;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region COMANDOS
        public ICommand Volvercommand => new Command(async () => await Volver());
        public ICommand Aumentarcommand => new Command(Auementar);
        public ICommand Disminuircommand => new Command(Disminuir);
        public ICommand Insertarcommand => new Command(async () => await Validarcompra());
        public ICommand ComprarProductoCommand => new Command<Producto>(async (p) => await ComprarProducto(p));
        public ICommand IrTiendaClienteCommand => new Command<Producto>(async (p) => await IrTiendaCliente(p.Id_Cliente));
        #endregion
    }
}
