using Newtonsoft.Json;

namespace Tzedaka.Model
{
    public class Billetera_Virtual
    {
        [JsonProperty("id_billetera")]
        public int Id_billetera { get; set; }
        [JsonProperty("id_cliente")]
        public int Id_cliente { get; set; }
        [JsonProperty("total")]
        public float Total { get; set; }
        [JsonProperty("creditos")]
        public float creditos { get; set; }
    }
}
