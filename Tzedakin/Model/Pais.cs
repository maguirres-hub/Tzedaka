using Newtonsoft.Json;

namespace Tzedaka.Model
{
    public class Pais
    {
        [JsonProperty("id_pais")]
        public int Id_Pais { get; set; }
        [JsonProperty("pais")]
        public string Nombre_Pais { get; set; }
    }
}
