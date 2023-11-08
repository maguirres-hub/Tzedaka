using Newtonsoft.Json;

namespace Tzedaka.Model
{
    public class Solicitudes
    {
        [JsonProperty("id_cliente")]
        public int id_cliente { get; set; }
        [JsonProperty("nombres")]
        public string Nombres { get; set; }

        [JsonProperty("apellidos")]
        public string Apellidos { get; set; }
        [JsonProperty("id_solicitud")]
        public string Id_Solicitud { get; set; }

        [JsonProperty("activado")]
        public int Activado { get; set; }


    }
}
