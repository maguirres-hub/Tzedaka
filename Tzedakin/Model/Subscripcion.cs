using Newtonsoft.Json;

namespace Tzedaka.Model
{
    public class Subscripcion
    {
        [JsonProperty("id_subscripcion")]
        public int Id_Subscripcion { get; set; }
        [JsonProperty("id_detalles_sub")]
        public int Id_Detalles_Sub { get; set; }
        [JsonProperty("id_cliente")]
        public int Id_Cliente { get; set; }
        [JsonProperty("fecha_inicio")]
        public string Fecha_Inicio { get; set; }
        [JsonProperty("fecha_final")]
        public string Fecha_Final { get; set; }
        [JsonProperty("bloque")]
        public int Bloque { get; set; }
        [JsonProperty("posicion_bloque")]
        public int Posicion_Bloque { get; set; }
        [JsonProperty("ultima_donacion")]
        public float Ultima_Donacion { get; set; }
        [JsonProperty("vueltas")]
        public int Vueltas { get; set; }
        [JsonProperty("donacion_activa")]
        public int Donacion_Activa { get; set; }
    }
}
