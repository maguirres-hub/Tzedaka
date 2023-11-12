using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tzedaka.Model;
using Tzedaka.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Tzedaka.ViewModels
{
    public class ViewModel_Tienda : INotifyPropertyChanged
    {
        HttpClient cliente;

        private Clientes _Cliente = new Clientes();
        private Clientes _Vendedor = new Clientes();
        private Producto _productos = new Producto();
        private PostProducto Post_Producto_ = new PostProducto();
        public Producto Producto_Seleccionado { get; set; }
        public Categorias Seleccion_Categoria { get; set; }
        private List<Categorias> Categoria_ = new List<Categorias>();

        private Reporte_Billetera Reporte_Billetera_ = new Reporte_Billetera();
        private Billetera_Virtual Cliente_Billetera_ = new Billetera_Virtual();
        private Billetera_Virtual Cliente_Billetera_Vendedor_ = new Billetera_Virtual();

        private ObservableCollection<Producto> productos_ = new ObservableCollection<Producto>();
        private ObservableCollection<Producto> MISProductos_ = new ObservableCollection<Producto>();
        private ObservableCollection<Producto> Items_ = new ObservableCollection<Producto>();

        private Producto SelectItem_;


        private ImageSource Ubicacion_Imagen_;
        public Command Btn_Cargar_Producs { get; set; }
        public Command Btn_Agregar_Producto { get; set; }
        public Command Btn_Cargar_Mis_Productos { get; set; }
        public Command Btn_Ir_Agregar_Producto { get; set; }
        public Command Btn_Ir_Mis_Producto { get; set; }
        public Command Seleccion_Comando { get; set; }
        public Command Btn_Eliminar { get; set; }
        public Command Btn_Envio_Compra { get; set; }
        public Command Btn_Seleccion_Imagen { get; set; }
        public Command Btn_Ir_Tienda_Cliente { get; set; }

        private bool _isLoading;
        private bool _isLoadingRegistro;
        private bool _isVisible;
        public string Direccion_Compra { get; set; }



        public ViewModel_Tienda()
        {
            _isLoading = false;
            if (Application.Current.Properties.ContainsKey("Cliente"))
            {
                Cliente = (Clientes)Application.Current.Properties["Cliente"];
                Application.Current.SavePropertiesAsync();
            }

            Btn_Agregar_Producto = new Command(() => Agregar_Producto());
            Btn_Seleccion_Imagen = new Command(() => Cargar_Imagen());
            Btn_Cargar_Producs = new Command(() => Cargar_Productos_Tienda());
            Btn_Ir_Mis_Producto = new Command(() => Ir_MisProductos());
            Btn_Ir_Tienda_Cliente = new Command<Producto>(Prod => Ir_TiendaCliente(Prod.Id_Cliente));
            Btn_Ir_Agregar_Producto = new Command(() => Agregar_Producto_Formulario());
            Btn_Cargar_Mis_Productos = new Command(() => Cargar_Productos_Cliente());
            Btn_Eliminar = new Command<Producto>(Prod => Eliminar_Producto(Prod.Id_Producto));
            Btn_Envio_Compra = new Command(() => Envio_Compra());
            Seleccion_Comando = new Command<Producto>(Prod => Comprar_Producto(Prod.Id_Cliente, Prod.Id_Producto, Prod.Precio));
            Cargar_Todo();
        }

        private async void Cargar_Todo()
        {
            await GET_Categorias();
            await GET_Productos_Tienda();
            await Get_Billetera();
            await Get_Reportes_Billetera();
        }

        private async void Envio_Compra()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ViewDireccionCompra());
        }

        private async void Cargar_Categorias()
        {
            await GET_Categorias();
        }

        private async void Cargar_Productos_Tienda()
        {
            await GET_Productos_Tienda();
        }

        private async void Cargar_Productos_Cliente()
        {
            await GET_Productos_Cliente(Cliente.Id_Cliente);
        }


        private async void Ir_MisProductos()
        {
            await Ir_MisProductos_Pagina();

        }
        private async void Ir_TiendaCliente(int id_vendedor)
        {
            await Ir_TiendaCliente_Pagina(id_vendedor);

        }
        private async Task Ir_MisProductos_Pagina()
        {

            ViewModel_Tienda viewModel = new ViewModel_Tienda();
            viewModel.Cargar_Productos_Cliente();
            await Application.Current.MainPage.Navigation.PushAsync(new ViewCarrito() { BindingContext = viewModel });
        }
        private async Task Ir_TiendaCliente_Pagina(int id_vendedor)
        {
            ViewModel_Tienda viewModel = new ViewModel_Tienda();
            await viewModel.GET_Productos_Cliente(id_vendedor);
            await viewModel.GET_Vendedor(id_vendedor);
            await Application.Current.MainPage.Navigation.PushAsync(new ViewTiendaCliente() { BindingContext = viewModel });
        }

        private async void Cargar_Imagen()
        {
            await SeleccionarImagenCelular();
        }

        private async void Agregar_Producto_Formulario()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ViewAgregarProducto());
            // await Post_Producto();
        }

        private async void Agregar_Producto()
        {
            await Post_Producto();
        }

        private async void Cargar_Billetera()
        {
            await Get_Billetera();
        }

        private async void Cargar_Reportes_Billetera()
        {
            await Get_Reportes_Billetera();
        }


        private async void Comprar_Producto(int id_vendedor, int id_producto, float precio)
        {
            Cargar_Billetera();
            Cargar_Reportes_Billetera();
            if (precio <= Cliente_Billetera.Total)
            {
                bool aceptar_compra = await Application.Current.MainPage.DisplayAlert("Compra", "¿Desea realizar la compra?", "Comprar", "Cancelar");
                if (aceptar_compra == true)
                {
                    await Actualizar_Billetera_Comprador(id_producto, precio);
                    await Actualizar_Billetera_Vendedor(id_vendedor, id_producto, precio);
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", "No posees fondos para comprar este producto. Recarga tu billetera", "Ok");
            }
        }

        private async void Eliminar_Producto(int id)
        {
            bool respuesta = await Application.Current.MainPage.DisplayAlert("Aviso", "¿Seguro que desea eliminar este producto?", "Si", "Cancelar");
            if (respuesta == true)
            {
                await Delete_Producto(id);
            }
        }




        private async Task SeleccionarImagenCelular()
        {

            try
            {
                IsLoading = true;


                var result = await MediaPicker.PickPhotoAsync();
                if (result != null)
                {

                    var ubicacion = result.FullPath.ToString();
                    Ubicacion_Imagen = ImageSource.FromFile(ubicacion);
                    Post_Producto_Byte.Img_Blob = File.ReadAllBytes(ubicacion);

                    using (var bitmap = SKBitmap.Decode(Post_Producto_Byte.Img_Blob))
                    {
                        // Escala la imagen a un ancho máximo de 800px y un alto máximo de 600px
                        var scaledBitmap = bitmap.Resize(new SKImageInfo(800, 600), SKFilterQuality.Medium);

                        using (var image = SKImage.FromBitmap(scaledBitmap))
                        {
                            // Codifica la imagen como JPEG con una calidad del 80%
                            var encodedData = image.Encode(SKEncodedImageFormat.Jpeg, 80);

                            // Convierte la imagen codificada a un arreglo de bytes
                            Post_Producto_Byte.Img_Blob = encodedData.ToArray();
                        }
                    }
                }
                IsLoading = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }
        public async Task Post_Producto()
        {
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);
                var url = Settings.Url + "tzedakin/api/productos/";

                IsLoadingRegistro = true;
                if (!string.IsNullOrEmpty(Producto_.Nombre) || !string.IsNullOrEmpty(Producto_.Descripcion) || Producto_.Precio != 0 || Producto_.Img_Blob != null)
                {
                    PostProducto producto = new PostProducto
                    {
                        Nombre = Producto_.Nombre,
                        Descripcion = Producto_.Descripcion,
                        Precio = Producto_.Precio,
                        Id_Categoria = Seleccion_Categoria.Id_Categoria,
                        Id_Estado = 1,
                        Img_Blob = Post_Producto_Byte.Img_Blob,
                        Id_Cliente = Cliente.Id_Cliente

                    };

                    Application.Current.Properties.Remove("Cliente");
                    await Application.Current.SavePropertiesAsync();

                    var Json = JsonConvert.SerializeObject(producto);
                    var Contenido = new StringContent(Json, Encoding.UTF8, "application/json");
                    HttpResponseMessage respuesta = await cliente.PostAsync(url, Contenido);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        Application.Current.Properties["Cliente"] = Cliente;
                        await Application.Current.SavePropertiesAsync();
                        Productos_.Clear();
                        Producto Borrar = new Producto
                        {
                            Nombre = "",
                            Descripcion = "",
                            Precio = 0,
                            Id_Categoria = 0,
                            Id_Estado = 0,
                            Img_Blob = null,
                            Id_Cliente = 0
                        };
                        Productos_.Add(Borrar);
                        await Application.Current.MainPage.DisplayAlert("Registro Exitoso.!", "Su producto fue creado, espere la autorizacion de un administrador para mostrarlo en la tienda.!", "ok");
                    }
                }
                else
                {
                    Application.Current.Properties["Cliente"] = Cliente;
                    await Application.Current.SavePropertiesAsync();
                    await Application.Current.MainPage.DisplayAlert("Registro fallido", "Debe llenar todos los campos", "ok");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }

            IsLoadingRegistro = false;
        }


        public async Task GET_Productos_Tienda()
        {

            try
            {
                IsLoading = true;

                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);

                var url = Settings.Url + "tzedakin/api/productos/";
                HttpResponseMessage respuesta = await cliente.GetAsync(url);


                if (respuesta.IsSuccessStatusCode)
                {

                    var JsonProductos = await respuesta.Content.ReadAsStringAsync();
                    var Produc = JsonConvert.DeserializeObject<ObservableCollection<Producto>>(JsonProductos);
                    if (Produc.Count > 0)
                    {
                        foreach (var prod in Produc)
                        {
                            if (prod.Id_Estado == 2)
                            {
                                Productos_ = new ObservableCollection<Producto>(Produc);
                                foreach (var prods in Productos_)
                                {
                                    if (prods.Id_Cliente == Cliente.Id_Cliente)
                                    {
                                        if (prods.Img_Blob != null && prods.Img_Blob.Type == "Buffer")
                                        {
                                            prods.Img_Blob.DatosImagen = Convert.FromBase64String(Encoding.UTF8.GetString(prods.Img_Blob.DatosImagen));
                                            prods.Img_Blob.Type = "image/jpeg"; // Or "image/jpeg" if applicable
                                            prods.Imagen_Ubicacion = ImageSource.FromStream(() => new MemoryStream(prods.Img_Blob.DatosImagen));
                                        }
                                        prods.IsVisible = false;
                                    }
                                    else
                                    {
                                        if (prods.Img_Blob != null && prods.Img_Blob.Type == "Buffer")
                                        {
                                            prods.Img_Blob.DatosImagen = Convert.FromBase64String(Encoding.UTF8.GetString(prods.Img_Blob.DatosImagen));
                                            prods.Img_Blob.Type = "image/jpeg"; // Or "image/jpeg" if applicable
                                            prods.Imagen_Ubicacion = ImageSource.FromStream(() => new MemoryStream(prods.Img_Blob.DatosImagen));
                                        }
                                        prods.IsVisible = true;
                                    }
                                }
                            }
                        }
                    }
                    Cargar_Billetera();
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                IsLoading = false;
            }
            IsLoading = false;

        }

        public async Task GET_Productos_Cliente(int idCliente)
        {
            IsLoading = true;
            try
            {

                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);

                var url = Settings.Url + $"tzedakin/api/productos_inner/{idCliente}";
                HttpResponseMessage respuesta = await cliente.GetAsync(url);


                if (respuesta.IsSuccessStatusCode)
                {

                    var JsonProductos = await respuesta.Content.ReadAsStringAsync();
                    var Produc = JsonConvert.DeserializeObject<ObservableCollection<Producto>>(JsonProductos);
                    if (Produc.Count > 0)
                    {
                        MISProductos = new ObservableCollection<Producto>(Produc);
                        foreach (var prod in MISProductos)
                        {
                            if (prod.Img_Blob != null && prod.Img_Blob.Type == "Buffer")
                            {
                                prod.Img_Blob.DatosImagen = Convert.FromBase64String(Encoding.UTF8.GetString(prod.Img_Blob.DatosImagen));
                                prod.Img_Blob.Type = "image/jpeg"; // Or "image/jpeg" if applicable
                                prod.Imagen_Ubicacion = ImageSource.FromStream(() => new MemoryStream(prod.Img_Blob.DatosImagen));
                            }
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Notificacion", "No tienes productos publicados", "Ok");
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
            IsLoading = false;
        }
        public async Task GET_Vendedor(int idVendedor)
        {
            IsLoading = true;
            try
            {

                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);

                var url = Settings.Url + $"tzedakin/api/clientes/{idVendedor}";
                HttpResponseMessage respuesta = await cliente.GetAsync(url);


                if (respuesta.IsSuccessStatusCode)
                {

                    var JsonVendedor = await respuesta.Content.ReadAsStringAsync();
                    _Vendedor = JsonConvert.DeserializeObject<List<Clientes>>(JsonVendedor)[0];
                }

            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
            IsLoading = false;
        }
        public async Task GET_Categorias()
        {
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);
                var url = Settings.Url + "tzedakin/api/categorias/";
                HttpResponseMessage respuesta = await cliente.GetAsync(url);
                if (respuesta.IsSuccessStatusCode)
                {
                    var JsonCategoria = await respuesta.Content.ReadAsStringAsync();
                    var Cat = JsonConvert.DeserializeObject<List<Categorias>>(JsonCategoria);
                    if (Cat.Count > 0)
                    {
                        Categoria = Cat;
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
        }



        private async Task Get_Billetera_Vendedor(int id_vendedor)
        {
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(60);
                var Url = Settings.Url + $"tzedakin/api/billetera_virtual/{id_vendedor}";

                HttpResponseMessage respuesta = await cliente.GetAsync(Url);
                if (respuesta.IsSuccessStatusCode)
                {
                    var Billeter = await respuesta.Content.ReadAsStringAsync();
                    var BilleteraConvert = JsonConvert.DeserializeObject<List<Billetera_Virtual>>(Billeter);
                    if (BilleteraConvert.Count > 0)
                    {
                        Cliente_Billetera_Vendedor.Total = BilleteraConvert[0].Total;

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
        }

        private async Task Get_Billetera()
        {
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);
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

        }
        private async Task Get_Reportes_Billetera()
        {
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);
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

        }



        public async Task Actualizar_Billetera_Comprador(int id, float precio)
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);


                float NuevoTotal = Cliente_Billetera.Total - precio;
                Billetera_Virtual nuevaCarga = new Billetera_Virtual
                {
                    Total = NuevoTotal
                };
                var json = JsonConvert.SerializeObject(nuevaCarga);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await cliente.PutAsync(Settings.Url + $"tzedakin/api/billetera_virtual/{Cliente.Id_Cliente}", contenido);

                if (respuesta.IsSuccessStatusCode)
                {
                    await Actualizar_Reporte_Billetera_Comprador(id, precio);
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

        public async Task Actualizar_Reporte_Billetera_Comprador(int id, float precio)
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);

                float NuevoTotal = Billetera_Reporte.total - precio;
                string Fecha_Compra = DateTime.Now.ToString("yyyy-MM-dd");
                Reporte_Billetera nuevaCarga = new Reporte_Billetera
                {
                    fecha = Fecha_Compra,
                    cantidad = precio,
                    total = NuevoTotal,
                    id_cliente = Cliente.Id_Cliente,
                    motivo = "Compra Producto",
                    codigo_producto = id,

                };
                var json = JsonConvert.SerializeObject(nuevaCarga);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await cliente.PostAsync(Settings.Url + $"tzedakin/api/reportes_billetera", contenido);

                if (respuesta.IsSuccessStatusCode)
                {
                    Cargar_Billetera();
                    Cargar_Reportes_Billetera();
                    await Application.Current.MainPage.DisplayAlert("Compra exitosa.!", "Hicistes una compra de un producto con el valor de " + precio + " se hizo un descuento en tu wallet revisa para mayor informacion, Recuerda comunicarte con el comprador y para mayor soporte escribe al correo tzedaka.tzadikim@gmail.com donde un adminsitrador te ayudara.!", "ok");

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Compra fallida", "Se ha producido un fallo durante la compra", "ok");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            IsLoading = false;
        }

        public async Task Actualizar_Billetera_Vendedor(int id_Vendedor, int id_producto, float precio)
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);

                await Get_Billetera_Vendedor(id_Vendedor);

                float NuevoTotal = Cliente_Billetera_Vendedor.Total + precio;
                Billetera_Virtual nuevaCarga = new Billetera_Virtual
                {
                    Total = NuevoTotal
                };
                var json = JsonConvert.SerializeObject(nuevaCarga);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await cliente.PutAsync(Settings.Url + $"tzedakin/api/billetera_virtual/{id_Vendedor}", contenido);

                if (respuesta.IsSuccessStatusCode)
                {
                    await Actualizar_Reporte_Billetera_Vendedor(id_Vendedor, id_producto, precio);
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

        public async Task Actualizar_Reporte_Billetera_Vendedor(int id_Vendedor, int id_producto, float precio)
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);

                float NuevoTotal = Billetera_Reporte.total + precio;
                string Fecha_Compra = DateTime.Now.ToString("yyyy-MM-dd");
                Reporte_Billetera nuevaCarga = new Reporte_Billetera
                {
                    fecha = Fecha_Compra,
                    cantidad = precio,
                    total = NuevoTotal,
                    id_cliente = id_Vendedor,
                    motivo = "Venta Producto",
                    codigo_producto = id_producto,

                };
                var json = JsonConvert.SerializeObject(nuevaCarga);
                var contenido = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = await cliente.PostAsync(Settings.Url + $"tzedakin/api/reportes_billetera", contenido);

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
            IsLoading = false;
        }

        private async Task Delete_Producto(int id)
        {
            IsLoading = true;
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(60);

                var Url = Settings.Url + $"tzedakin/api/productos/{id}";
                HttpResponseMessage respuesta = await cliente.DeleteAsync(Url);

                if (respuesta.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Producto Eliminado Correctamente", "Salir");
                    MISProductos_.Remove(SelectItem);
                    MISProductos_.Clear();
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
        public Clientes Vendedor
        {
            get { return _Vendedor; }
            set
            {
                if (_Vendedor != value)
                {
                    _Vendedor = value;
                    OnPropertyChanged(nameof(Vendedor));
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

        public bool IsLoadingRegistro
        {
            get => _isLoadingRegistro;
            set
            {
                _isLoadingRegistro = value;
                OnPropertyChanged(nameof(IsLoadingRegistro));
            }

        }
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }

        }



        public Producto Producto_
        {
            get { return _productos; }
            set
            {
                if (_productos != value)
                {
                    _productos = value;
                    OnPropertyChanged(nameof(Producto_));
                }
            }
        }

        public Producto SelectItem
        {
            get { return SelectItem_; }
            set
            {
                if (SelectItem_ != value)
                {
                    SelectItem_ = value;
                    OnPropertyChanged(nameof(SelectItem_));
                }
            }
        }

        public PostProducto Post_Producto_Byte
        {
            get { return Post_Producto_; }
            set
            {
                if (Post_Producto_ != value)
                {
                    Post_Producto_ = value;
                    OnPropertyChanged(nameof(Post_Producto_Byte));
                }
            }
        }

        public List<Categorias> Categoria
        {
            get { return Categoria_; }
            set
            {
                if (Categoria_ != value)
                {
                    Categoria_ = value;
                    OnPropertyChanged(nameof(Categoria));
                }
            }
        }

        public ImageSource Ubicacion_Imagen
        {
            get { return Ubicacion_Imagen_; }
            set
            {
                if (value != null)
                {
                    Ubicacion_Imagen_ = value;
                    OnPropertyChanged(nameof(Ubicacion_Imagen));
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

        public Billetera_Virtual Cliente_Billetera_Vendedor
        {
            get
            {
                return Cliente_Billetera_Vendedor_;
            }
            set
            {
                if (Cliente_Billetera_Vendedor_ != value)
                {
                    Cliente_Billetera_Vendedor_ = value;
                    OnPropertyChanged(nameof(Cliente_Billetera_Vendedor));
                }
            }
        }
        public ObservableCollection<Producto> Productos_
        {
            get { return productos_; }
            set
            {
                if (productos_ != value)
                {
                    productos_ = value;
                    OnPropertyChanged(nameof(Productos_));
                }
            }
        }
        public ObservableCollection<Producto> MISProductos
        {
            get { return MISProductos_; }
            set
            {
                if (MISProductos_ != value)
                {
                    MISProductos_ = value;
                    OnPropertyChanged(nameof(MISProductos));
                }
            }
        }
        public ObservableCollection<Producto> Items
        {
            get { return Items_; }
            set
            {
                if (Items_ != value)
                {
                    Items_ = value;
                    OnPropertyChanged(nameof(Items));
                }
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private class NotificationRequest
        {
            public string Titulo { get; set; }
            public string Deescripcion { get; set; }
            public string ReturnData { get; set; }
        }
    }
}
