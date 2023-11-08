using Newtonsoft.Json;

namespace Tzedaka.Model
{
    public class Detalles_Subscripcion
    {
        [JsonProperty("id_detalles_sub")]
        public int id_detalles_sub { get; set; }
        [JsonProperty("precio")]
        public float precio { get; set; }
    }
}
