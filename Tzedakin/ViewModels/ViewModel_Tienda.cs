using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
    public class ViewModel_Tienda : INotifyPropertyChanged
    {
        HttpClient cliente;

        private Clientes _Cliente = new Clientes();
        private Clientes _Vendedor = new Clientes();
        private Producto _productos = new Producto();
        private Tienda _tienda;
        private PostProducto Post_Producto_ = new PostProducto();
        public Producto Producto_Seleccionado { get; set; }
        private Categorias _Seleccion_Categoria = new Categorias();
        private List<Categorias> Categoria_ = new List<Categorias>();

        private Reporte_Billetera Reporte_Billetera_ = new Reporte_Billetera();
        private Billetera_Virtual Cliente_Billetera_ = new Billetera_Virtual();
        private Billetera_Virtual Cliente_Billetera_Vendedor_ = new Billetera_Virtual();

        private List<Producto> productosTemp;
        private ObservableCollection<Producto> productos_ = new ObservableCollection<Producto>();
        private ObservableCollection<Producto> MISProductos_ = new ObservableCollection<Producto>();
        private ObservableCollection<Producto> Items_ = new ObservableCollection<Producto>();
        private Ciudades CiudadSeleccionada_ = new Ciudades();
        private Pais PaisSeleccionado_ = new Pais();

        private ObservableCollection<Ciudades> Ciudades_ = new ObservableCollection<Ciudades>();
        private ObservableCollection<Pais> Pais_ = new ObservableCollection<Pais>();
        private ObservableCollection<ComentarioCompra> _comentarios = new ObservableCollection<ComentarioCompra>();
        public List<string> colores { get; set; }

        private string _colorFondo { get; set; }

        private string _colorLetraFondo { get; set; }

        private string _colorProducto { get; set; }

        private string _colorLetraProducto { get; set; }

        private string _colorComentario { get; set; }

        private string _colorLetraComentario { get; set; }


        private Producto SelectItem_;


        private ImageSource Ubicacion_Imagen_;
        public Command Btn_Cargar_Producs { get; set; }
        public Command Btn_Agregar_Producto { get; set; }
        public Command Btn_Agregar_Tienda { get; set; }
        public Command Btn_Cargar_Mis_Productos { get; set; }
        public Command Btn_Ir_Agregar_Producto { get; set; }
        public Command Btn_Ir_Editar_Producto { get; set; }
        public Command Btn_Ir_Agregar_Tienda { get; set; }
        public Command Btn_Ir_Mis_Producto { get; set; }
        public Command Btn_Ir_Producto_Compra { get; set; }
        public Command Btn_ComprarProducto { get; set; }
        public Command Btn_ComprarSubscripcion { get; set; }
        public Command Btn_Eliminar { get; set; }
        public Command Btn_Envio_Compra { get; set; }
        public Command Btn_Seleccion_Imagen { get; set; }
        public Command Btn_Ir_Tienda_Cliente { get; set; }
        public Command Btn_Ir_Mis_Compras { get; set; }
        public Command Btn_Ir_Mis_Ventas { get; set; }
        public Command Btn_ir_modificarDiseñoTienda { get; set; }
        public Command GuardarDiseñoTiendaCommand { get; set; }


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
            Btn_Agregar_Tienda = new Command(async () => await Post_Tienda());
            Btn_Seleccion_Imagen = new Command(() => Cargar_Imagen());
            Btn_Cargar_Producs = new Command(() => Cargar_Productos_Tienda());
            Btn_Ir_Mis_Producto = new Command(() => Ir_MisProductos());
            Btn_Ir_Producto_Compra = new Command<Producto>(p => Ir_ProductoCompra(p));
            Btn_Ir_Tienda_Cliente = new Command<Producto>(Prod => Ir_TiendaCliente(Prod.Id_Cliente));
            Btn_Ir_Mis_Compras = new Command(() => Ir_MisCompras());
            Btn_Ir_Mis_Ventas = new Command(async () => await Ir_MisVentas_Pagina());
            Btn_Ir_Agregar_Producto = new Command(() => Agregar_Producto_Formulario());
            Btn_Ir_Editar_Producto = new Command<Producto>(p => Editar_Producto_Formulario(p));
            Btn_Ir_Agregar_Tienda = new Command(() => Agregar_Tienda_Formulario());
            Btn_Cargar_Mis_Productos = new Command(() => Cargar_Productos_Cliente());
            Btn_Eliminar = new Command<Producto>(Prod => Eliminar_Producto(Prod.Id_Producto));
            Btn_Envio_Compra = new Command(() => Envio_Compra());
            Btn_ComprarProducto = new Command<Producto>(Prod => Comprar_Producto(Prod.Id_Cliente, Prod.Id_Producto, Prod.Precio));
            Btn_ComprarSubscripcion = new Command(Prod => Comprar_Subscripcion());
            Btn_ir_modificarDiseñoTienda = new Command(async Prod => await IrModificacionDiseñoTienda());
            GuardarDiseñoTiendaCommand = new Command(async Prod => await ModificarDiseñoTienda());
            Cargar_Todo();
            colores = new List<string>()
         {"AliceBlue",
                                                "AntiqueWhite",
                                                "Aqua",
                                                "Aquamarine",
                                                "Azure",
                                                "Beige",
                                                "Bisque",
                                                "Black",
                                                "BlanchedAlmond",
                                                "Blue",
                                                "BlueViolet",
                                                "Brown",
                                                "BurlyWood",
                                                "CadetBlue",
                                                "Chartreuse",
                                                "Chocolate",
                                                "Coral",
                                                "CornflowerBlue",
                                                "Cornsilk",
                                                "Crimson",
                                                "Cyan",
                                                "DarkBlue",
                                                "DarkCyan",
                                                "DarkGoldenrod",
                                                "DarkGray",
                                                "DarkGreen",
                                                "DarkKhaki",
                                                "DarkMagenta",
                                                "DarkOliveGreen",
                                                "DarkOrange",
                                                "DarkOrchid",
                                                "DarkRed",
                                                "DarkSalmon",
                                                "DarkSeaGreen",
                                                "DarkSlateBlue",
                                                "DarkSlateGray",
                                                "DarkTurquoise",
                                                "DarkViolet",
                                                "DeepPink",
                                                "DeepSkyBlue",
                                                "DimGray",
                                                "DodgerBlue",
                                                "Firebrick",
                                                "FloralWhite",
                                                "ForestGreen",
                                                "Fuschia",
                                                "Gainsboro",
                                                "GhostWhite",
                                                "Gold",
                                                "Goldenrod",
                                                "Gray",
                                                "Green",
                                                "GreenYellow",
                                                "Honeydew",
                                                "HotPink",
                                                "IndianRed",
                                                "Indigo",
                                                "Ivory",
                                                "Khaki",
                                                "Lavender",
                                                "LavenderBlush",
                                                "LawnGreen",
                                                "LemonChiffon",
                                                "LightBlue",
                                                "LightCoral",
                                                "LightCyan",
                                                "LightGoldenrodYellow",
                                                "LightGray",
                                                "LightGreen",
                                                "LightPink",
                                                "LightSalmon",
                                                "LightSeaGreen",
                                                "LightSkyBlue",
                                                "LightSlateGray",
                                                "LightSteelBlue",
                                                "LightYellow",
                                                "Lime",
                                                "LimeGreen",
                                                "Linen",
                                                "Magenta",
                                                "Maroon",
                                                "MediumAquamarine",
                                                "MediumBlue",
                                                "MediumOrchid",
                                                "MediumPurple",
                                                "MediumSeaGreen",
                                                "MediumSlateBlue",
                                                "MediumSpringGreen",
                                                "MediumTurquoise",
                                                "MediumVioletRed",
                                                "MidnightBlue",
                                                "MintCream",
                                                "MistyRose",
                                                "Moccasin",
                                                "NavajoWhite",
                                                "Navy",
                                                "OldLace",
                                                "Olive",
                                                "OliveDrab",
                                                "Orange",
                                                "OrangeRed",
                                                "Orchid",
                                                "PaleGoldenrod",
                                                "PaleGreen",
                                                "PaleTurquoise",
                                                "PaleVioletRed",
                                                "PapayaWhip",
                                                "PeachPuff",
                                                "Peru",
                                                "Pink",
                                                "Plum",
                                                "PowderBlue",
                                                "Purple",
                                                "Red",
                                                "RosyBrown",
                                                "RoyalBlue",
                                                "SaddleBrown",
                                                "Salmon",
                                                "SandyBrown",
                                                "SeaGreen",
                                                "SeaShell",
                                                "Sienna",
                                                "Silver",
                                                "SkyBlue",
                                                "SlateBlue",
                                                "SlateGray",
                                                "Snow",
                                                "SpringGreen",
                                                "SteelBlue",
                                                "Tan",
                                                "Teal",
                                                "Thistle",
                                                "Tomato",
                                                "Transparent",
                                                "Turquoise",
                                                "Violet",
                                                "Wheat",
                                                "White",
                                                "WhiteSmoke",
                                                "Yellow",
                                                "YellowGreen", };
        }

        public async void Cargar_Todo()
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
        private async void Cargar_Paises()
        {
            await Get_Paises();
        }
        private async Task Get_Ciudades()
        {
            IsLoading = true;
            try
            {

                HttpClient HttpCliente_ = new HttpClient();
                HttpCliente_.Timeout = TimeSpan.FromSeconds(120);
                var Url = Settings.Url + $"tzedakin/api/ciudades_inner_pais/{PaisSeleccionado.Id_Pais}";

                HttpResponseMessage respuesta = await HttpCliente_.GetAsync(Url);
                Debug.WriteLine(Url);
                if (respuesta.IsSuccessStatusCode)
                {
                    var ciudadesJson = await respuesta.Content.ReadAsStringAsync();
                    var ciudades = JsonConvert.DeserializeObject<ObservableCollection<Ciudades>>(ciudadesJson);
                    Debug.WriteLine(ciudadesJson.ToString());
                    Ciudad.Clear();
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
                HttpClient HttpCliente_ = new HttpClient();
                HttpResponseMessage respuesta = await HttpCliente_.GetAsync(Settings.Url + "tzedakin/api/paises");
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

        private async void Ir_MisProductos()
        {
            await Ir_MisProductos_Pagina();

        }
        private async void Ir_ProductoCompra(Producto p)
        {
            await Ir_ProductoCompra_Pagina(p);

        }
        private async void Ir_TiendaCliente(int id_vendedor)
        {
            await Ir_TiendaCliente_Pagina(id_vendedor);

        }
        private async void Ir_MisCompras()
        {
            await Ir_MisCompras_Pagina();

        }
        private async Task Ir_MisProductos_Pagina()
        {

            ViewModel_Tienda viewModel = new ViewModel_Tienda();
            await viewModel.GET_Tienda(Cliente.Id_Cliente);
            viewModel.Cargar_Productos_Cliente();
            await Application.Current.MainPage.Navigation.PushAsync(new ViewCarrito() { BindingContext = viewModel });
        }
        private async Task Ir_ProductoCompra_Pagina(Producto p)
        {

            await Application.Current.MainPage.Navigation.PushAsync(new ViewProductoCompra(p));
        }
        private async Task Ir_TiendaCliente_Pagina(int id_vendedor)
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
        private async Task Ir_MisCompras_Pagina()
        {
            VMMisCompras viewModel = new VMMisCompras();
            await Application.Current.MainPage.Navigation.PushAsync(new ViewMisCompras() { BindingContext = viewModel });
        }
        private async Task Ir_MisVentas_Pagina()
        {
            VMMisVentas viewModel = new VMMisVentas();
            await Application.Current.MainPage.Navigation.PushAsync(new ViewMisVentas() { BindingContext = viewModel });
        }

        private async void Cargar_Imagen()
        {
            await SeleccionarImagenCelular();
        }

        private async void Agregar_Producto_Formulario()
        {
            if (MISProductos.Count() < 3 || miTienda.estaSubscrito)
            {
                Ubicacion_Imagen = "";
                await Application.Current.MainPage.Navigation.PushAsync(new ViewAgregarProducto() { BindingContext = this });
            }
            else await Application.Current.MainPage.DisplayAlert("Aviso", "No puede agregar mas productos sin estar suscrito", "Ok");
            // await Post_Producto();
        }
        private async void Editar_Producto_Formulario(Producto productoEditar)
        {
            Producto_ = productoEditar;
            Post_Producto_Byte.Img_Blob = productoEditar.Img_Blob.DatosImagen;
            Ubicacion_Imagen = productoEditar.Imagen_Ubicacion;
            Seleccion_Categoria = new Categorias();
            Seleccion_Categoria.Id_Categoria = productoEditar.Id_Categoria;
            await Application.Current.MainPage.Navigation.PushAsync(new ViewAgregarProducto() { BindingContext = this });
            // await Post_Producto();
        }
        private async void Agregar_Tienda_Formulario()
        {
            await Get_Paises();
            if (miTienda.tieneTienda)
            {
                await Get_Ciudades(); CiudadSeleccionada = new Ciudades();
                PaisSeleccionado = new Pais();
                PaisSeleccionado.Id_Pais = miTienda.idPais;
                PaisSeleccionado.Nombre_Pais = miTienda.Pais;
                CiudadSeleccionada.Id_Ciudad = miTienda.idCiudad;
                CiudadSeleccionada.Ciudad = miTienda.Ciudad;

            }

            await Application.Current.MainPage.Navigation.PushAsync(new ViewAgregarTienda() { BindingContext = this });
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
        private async Task IrModificacionDiseñoTienda()
        {
            await GET_Productos_Cliente(Cliente.Id_Cliente);
            await GET_Vendedor(Cliente.Id_Cliente);
            await getComentarios(Cliente.Id_Cliente);
            await GET_Tienda(Cliente.Id_Cliente);
            await Application.Current.MainPage.Navigation.PushAsync(new ViewModificarDiseñoTienda() { BindingContext = this });
        }
        private async Task ModificarDiseñoTienda()
        {
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);
                var url = Settings.Url + "tzedakin/api/tienda_diseno/";

                if (ColorFondo != null)
                {
                    Tienda producto = new Tienda
                    {
                        id = miTienda.id,
                        colorFondo = ColorFondo,
                        colorLetraFondo = ColorLetraFondo,
                        colorProducto = ColorProducto,
                        colorLetraProducto = ColorLetraProducto,
                        colorComentario = ColorComentario,
                        colorLetraComentario = ColorLetraComentario
                    };

                    var Json = JsonConvert.SerializeObject(producto);
                    var Contenido = new StringContent(Json, Encoding.UTF8, "application/json");
                    HttpResponseMessage respuesta = await cliente.PutAsync(url, Contenido);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        await Application.Current.MainPage.Navigation.PopAsync();
                        await Application.Current.MainPage.DisplayAlert("Registro Exitoso.!", "Datos modificados con exito", "ok");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Registro fallido", "Debe llenar todos los campos", "ok");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
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
                    await Actualizar_Billetera_Comprador(id_producto, precio, "Compra producto");
                    await Actualizar_Billetera_Vendedor(id_vendedor, id_producto, precio);
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", "No posees fondos para comprar este producto. Recarga tu billetera", "Ok");
            }
        }
        private async void Comprar_Subscripcion()
        {
            try
            {

                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);

                var url = Settings.Url + $"tzedakin/api/detalles_subscripcion/2";
                HttpResponseMessage respuesta = await cliente.GetAsync(url);
                if (respuesta.IsSuccessStatusCode)
                {
                    var JsonProductos = await respuesta.Content.ReadAsStringAsync();
                    var Produc = JsonConvert.DeserializeObject<List<Detalles_Subscripcion>>(JsonProductos);
                    if (Produc.Count > 0)
                    {
                        float precioSubscripcion = Produc[0].precio;
                        if (Cliente_Billetera.Total < precioSubscripcion)
                            await Application.Current.MainPage.DisplayAlert("Aviso", "No tiene saldo suficiente el precio de la subscripcion es de USD." + precioSubscripcion + ", puede recargar ingresando a su Wallet", "Ok");
                        else
                        {
                            bool bandera = await Application.Current.MainPage.DisplayAlert("Aviso", "El precio de la subscripcion es de USD. " + precioSubscripcion + " Su saldo es de USD." + Cliente_Billetera.Total.ToString(), "Comprar", "Salir");
                            if (bandera == true)
                            {
                                var contenido = new StringContent("{" + '"' + "id" + '"' + ":" + miTienda.id + "}", Encoding.UTF8, "application/json");
                                url = Settings.Url + "tzedakin/api/tienda_subscripcion";
                                respuesta = await cliente.PutAsync(url, contenido);
                                if (respuesta.IsSuccessStatusCode == true)
                                {
                                    await ActualizaSaldoCliente(Cliente.Id_Cliente, Cliente_Billetera.Total - precioSubscripcion, DateTime.Now, 1, -precioSubscripcion, "Compra de segunda subscripción", 0, Cliente_Billetera.creditos);

                                    await GET_Tienda(Cliente.Id_Cliente);
                                    await Application.Current.MainPage.DisplayAlert("Compra Exitosa", "Su subscripcion se ha realizado con exito", "Ok");
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }
        private async Task ActualizaSaldoCliente(int idCliente, float saldoNuevo, DateTime fecha, float cantidad, float variacion, string motivo, int idProducto, float creditos)
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
                if (!string.IsNullOrEmpty(Producto_.Nombre) && !string.IsNullOrEmpty(Producto_.Descripcion) && Producto_.Precio != 0 && Post_Producto_Byte.Img_Blob != null && Producto_.Stock != 0 && Producto_.Peso != 0)
                {
                    PostProducto producto = new PostProducto
                    {
                        Id_Producto = Producto_.Id_Producto,
                        Nombre = Producto_.Nombre,
                        Descripcion = Producto_.Descripcion,
                        Precio = Producto_.Precio,
                        Id_Categoria = Seleccion_Categoria.Id_Categoria,
                        Id_Estado = 1,
                        Img_Blob = Post_Producto_Byte.Img_Blob,
                        Id_Cliente = miTienda.idCliente,
                        Peso = Producto_.Peso,
                        Stock = Producto_.Stock,
                        Puntuacion = 0,
                        alto = Producto_.alto,
                        ancho = Producto_.ancho,
                        profundo = Producto_.profundo
                    };

                    var Json = JsonConvert.SerializeObject(producto);
                    var Contenido = new StringContent(Json, Encoding.UTF8, "application/json");
                    HttpResponseMessage respuesta;
                    if (producto.Id_Producto == 0)
                        respuesta = await cliente.PostAsync(url, Contenido);
                    else
                        respuesta = await cliente.PutAsync(url, Contenido);
                    if (respuesta.IsSuccessStatusCode)
                    {
                        Producto_ = new Producto();
                        Cargar_Productos_Cliente();
                        Post_Producto_Byte.Img_Blob = null;
                        Ubicacion_Imagen = null;
                        Seleccion_Categoria = null;
                        await Application.Current.MainPage.Navigation.PopAsync();
                        await Application.Current.MainPage.DisplayAlert("Registro Exitoso.!", "Su producto fue creado con exito!, Espere la aprobacion del administrador para que los compradores puedan verlo", "ok");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Registro fallido", "Debe llenar todos los campos", "ok");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }

            IsLoadingRegistro = false;
        }

        public async Task Post_Tienda()
        {
            try
            {
                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);
                var url = Settings.Url + "tzedakin/api/tienda/";

                IsLoadingRegistro = true;
                if (miTienda.CamposLLenos() && CiudadSeleccionada != null && PaisSeleccionado != null)
                {
                    if (!miTienda.telefono.ToCharArray().All(Char.IsDigit))
                    {
                        await Application.Current.MainPage.DisplayAlert("Registro fallido", "El telefono debe ser numerico", "ok");
                        return;
                    }
                    miTienda.idCiudad = CiudadSeleccionada.Id_Ciudad;
                    miTienda.idPais = PaisSeleccionado.Id_Pais;
                    miTienda.idCliente = Cliente.Id_Cliente;
                    var Json = JsonConvert.SerializeObject(miTienda);
                    var Contenido = new StringContent(Json, Encoding.UTF8, "application/json");
                    HttpResponseMessage respuesta;
                    if (miTienda.id == 0)
                        respuesta = await cliente.PostAsync(url, Contenido);
                    else
                        respuesta = await cliente.PutAsync(url, Contenido);
                    if (respuesta.IsSuccessStatusCode)
                    {
                        Cargar_Productos_Cliente();
                        await GET_Tienda(Cliente.Id_Cliente);
                        await Application.Current.MainPage.Navigation.PopAsync();
                        await Application.Current.MainPage.Navigation.PopAsync();
                        await Application.Current.MainPage.DisplayAlert("Registro Exitoso.!", "Registro modificado con exito", "ok");
                    }
                }
                else
                {
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
                            Productos_ = new ObservableCollection<Producto>(Produc);
                            foreach (var prods in Productos_)
                            {
                                if (prods.Id_Cliente == Cliente.Id_Cliente || (prods.tipo_envio != "Internacional" && prods.idPais != Cliente.Id_Pais) || prods.Id_Estado == 1)
                                {
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
                            if (prod.Id_Estado == 1)
                            {
                                prod.IsVisible = false;
                            }
                            else
                            {

                                prod.IsVisible = true;
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
            IsLoading = false;
        }
        public async Task GET_Tienda(int idCliente)
        {
            IsLoading = true;
            try
            {

                cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(120);

                var url = Settings.Url + $"tzedakin/api/tienda/{idCliente}";
                HttpResponseMessage respuesta = await cliente.GetAsync(url);


                if (respuesta.IsSuccessStatusCode)
                {

                    var JsonProductos = await respuesta.Content.ReadAsStringAsync();
                    var Produc = JsonConvert.DeserializeObject<ObservableCollection<Tienda>>(JsonProductos);
                    if (Produc.Count > 0)
                    {
                        miTienda = Produc[0];
                        miTienda.tieneTienda = true;
                        CiudadSeleccionada = new Ciudades();
                        PaisSeleccionado = new Pais();
                        CiudadSeleccionada.Id_Ciudad = miTienda.idCiudad;
                        CiudadSeleccionada.Ciudad = miTienda.Ciudad;
                        PaisSeleccionado.Id_Pais = miTienda.idPais;
                        PaisSeleccionado.Nombre_Pais = miTienda.Pais;

                        ColorLetraFondo = miTienda.colorLetraFondo;
                        ColorFondo = miTienda.colorFondo;
                        ColorProducto = miTienda.colorProducto;
                        ColorLetraProducto = miTienda.colorLetraProducto;
                        ColorComentario = miTienda.colorComentario;
                        ColorLetraComentario = miTienda.colorLetraComentario;

                        OnPropertyChanged(nameof(miTienda));

                    }
                    else
                    {
                        miTienda = new Tienda();
                        miTienda.tieneTienda = false;
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

        public async Task getComentarios(int idCliente)
        {
            try
            {
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(120);
                var url = Settings.Url + $"tzedakin/api/comentarios_tienda/{idCliente}";
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



        public async Task Actualizar_Billetera_Comprador(int id, float precio, string motivo)
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
                    await Actualizar_Reporte_Billetera_Comprador(id, precio, motivo);
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
        public async Task<HttpResponseMessage> Actualizar_Reporte_Billetera_Comprador_funcion(int id, float precio, string motivo)
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
                motivo = motivo != null ? motivo : "Compra Producto",
                codigo_producto = id,

            };
            var json = JsonConvert.SerializeObject(nuevaCarga);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");
            return await cliente.PostAsync(Settings.Url + $"tzedakin/api/reportes_billetera", contenido);
        }
        public async Task Actualizar_Reporte_Billetera_Comprador(int id, float precio, string motivo)
        {
            IsLoading = true;
            try
            {

                HttpResponseMessage respuesta = await Actualizar_Reporte_Billetera_Comprador_funcion(id, precio, motivo);
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
                    Cargar_Productos_Cliente();
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
        public Categorias Seleccion_Categoria
        {
            get { return _Seleccion_Categoria; }
            set
            {
                if (_Seleccion_Categoria != value)
                {
                    _Seleccion_Categoria = value;
                    OnPropertyChanged(nameof(Seleccion_Categoria));
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
        public Ciudades CiudadSeleccionada
        {
            get { return CiudadSeleccionada_; }
            set
            {
                if (CiudadSeleccionada_ != value)
                {
                    CiudadSeleccionada_ = value;
                    OnPropertyChanged(nameof(CiudadSeleccionada));
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
        public bool IsLoadingRegistro
        {
            get => _isLoadingRegistro;
            set
            {
                _isLoadingRegistro = value;
                OnPropertyChanged(nameof(IsLoadingRegistro));
            }

        }
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
        public Tienda miTienda
        {
            get { return _tienda; }
            set
            {
                if (_tienda != value)
                {
                    _tienda = value;
                    OnPropertyChanged(nameof(miTienda));
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
        public string ColorProducto
        {
            get { return _colorProducto; }
            set
            {
                if (_colorProducto != value)
                {
                    _colorProducto = value;

                }
                OnPropertyChanged(nameof(ColorProducto));
            }
        }
        public string ColorLetraProducto
        {
            get { return _colorLetraProducto; }
            set
            {
                if (_colorLetraProducto != value)
                {
                    _colorLetraProducto = value;

                }
                OnPropertyChanged(nameof(ColorLetraProducto));
            }
        }
        public string ColorFondo
        {
            get { return _colorFondo; }
            set
            {
                if (_colorFondo != value)
                {
                    _colorFondo = value;

                }
                OnPropertyChanged(nameof(ColorFondo));
            }
        }
        public string ColorLetraFondo
        {
            get { return _colorLetraFondo; }
            set
            {
                if (_colorLetraFondo != value)
                {
                    _colorLetraFondo = value;

                }
                OnPropertyChanged(nameof(ColorLetraFondo));
            }
        }
        public string ColorComentario
        {
            get { return _colorComentario; }
            set
            {
                if (_colorComentario != value)
                {
                    _colorComentario = value;

                }
                OnPropertyChanged(nameof(ColorComentario));
            }
        }
        public string ColorLetraComentario
        {
            get { return _colorLetraComentario; }
            set
            {
                if (_colorLetraComentario != value)
                {
                    _colorLetraComentario = value;

                }
                OnPropertyChanged(nameof(ColorLetraComentario));
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
