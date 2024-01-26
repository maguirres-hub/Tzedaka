using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xamarin.Forms;

namespace Tzedaka.Model
{
    public class Producto
    {
        [JsonProperty("id_producto")]
        public int Id_Producto { get; set; }
        [JsonProperty("nombre")]
        public string Nombre { get; set; }
        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }
        [JsonProperty("precio")]
        public int Precio { get; set; }
        [JsonProperty("id_categoria")]
        public int Id_Categoria { get; set; }
        [JsonProperty("id_estado")]
        public int Id_Estado { get; set; }
        [JsonProperty("estado")]
        public string Estado { get; set; }

        [JsonProperty("categoria")]
        public string Categoria { get; set; }

        [JsonProperty("img_blob")]
        public Imagen_Producto Img_Blob { get; set; }
        [JsonProperty("stock")]
        public int Stock { get; set; }
        [JsonProperty("peso")]
        public float Peso { get; set; }
        [JsonProperty("puntuacion")]
        public float Puntuacion { get; set; }

        [JsonProperty("id_cliente")]
        public int Id_Cliente { get; set; }
        [JsonProperty("alto")]
        public float alto { get; set; }
        [JsonProperty("ancho")]
        public float ancho { get; set; }
        [JsonProperty("profundo")]
        public float profundo { get; set; }
        [JsonProperty("tipo_envio")]
        public string tipo_envio { get; set; }
        [JsonProperty("id_pais")]
        public int idPais { get; set; }

        public bool IsVisible { get; set; }

        public ImageSource Imagen_Ubicacion { get; set; }
    }
}
