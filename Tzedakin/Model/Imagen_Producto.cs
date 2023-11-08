using Newtonsoft.Json;

namespace Tzedaka.Model
{
    public class Imagen_Producto
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("data")]
        public byte[] DatosImagen { get; set; }
    }
}
