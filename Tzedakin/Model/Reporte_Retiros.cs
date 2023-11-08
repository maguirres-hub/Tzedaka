using Newtonsoft.Json;

namespace Tzedaka.Model
{
    public class Reporte_Retiros
    {
        [JsonProperty("id_reporte_retiro")]
        public int Id_Retiro { get; set; }
        [JsonProperty("fecha")]
        public string Fecha { get; set; }
        [JsonProperty("cantidad")]
        public float Cantidad { get; set; }
        [JsonProperty("id_estado")]
        public int Id_Estado { get; set; }
        [JsonProperty("id_cliente")]
        public int Id_Cliente { get; set; }
        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }
    }
}
