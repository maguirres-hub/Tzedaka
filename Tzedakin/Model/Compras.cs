using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Tzedaka.Model
{
    public class Compras
    {
        [JsonProperty("id_compra")]
        public int idCompra { get; set; }
        [JsonProperty("id_producto")]
        public int Id_Producto { get; set; }
        [JsonProperty("nombre_producto")]
        public string NombreProducto { get; set; }
        [JsonProperty("estado")]
        public string Estado { get; set; }
        [JsonProperty("nombre_tienda")]
        public string NombreTienda { get; set; }
        [JsonProperty("direccion")]
        public string Direccion { get; set; }
        [JsonProperty("ciudad")]
        public string Ciudad { get; set; }
        [JsonProperty("pais")]
        public string Pais { get; set; }
        [JsonProperty("precio")]
        public int Precio { get; set; }
        [JsonProperty("img_blob")]
        public Imagen_Producto Img_Blob { get; set; }

        [JsonProperty("id_cliente")]
        public int Id_Cliente { get; set; }
        [JsonProperty("id_tienda")]
        public int Id_Tienda { get; set; }
        [JsonProperty("id_estado")]
        public int idEstado{ get; set; }
        [JsonProperty("fecha")]
        public string fecha { get; set; }

        public ImageSource Imagen_Ubicacion { get; set; }
        public string colorTextoEstado { get; set; }
    }
}
