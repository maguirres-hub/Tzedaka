using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Tzedaka.Model
{
    public class Ciudades
    {
        [JsonProperty("id_ciudad")]
        public int Id_Ciudad { get; set; }
        [JsonProperty("ciudad")]
        public string Ciudad { get; set; }
        /* [JsonProperty("id_pais")]
         public int Id_Pais { get; set; }*/
    }
}
