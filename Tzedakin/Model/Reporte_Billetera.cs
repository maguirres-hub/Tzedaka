using Newtonsoft.Json;

namespace Tzedaka.Model
{
    public class Reporte_Billetera
    {
        [JsonProperty("id_reporte_billetera")]
        public int id_reporte_billetera { get; set; }
        [JsonProperty("fecha")]
        public string fecha { get; set; }
        [JsonProperty("cantidad")]
        public float cantidad { get; set; }
        [JsonProperty("total")]
        public float total { get; set; }
        [JsonProperty("id_cliente")]
        public int id_cliente { get; set; }
        [JsonProperty("motivo")]
        public string motivo { get; set; }
        [JsonProperty("codigo_producto")]
        public int codigo_producto { get; set; }

    }
}
