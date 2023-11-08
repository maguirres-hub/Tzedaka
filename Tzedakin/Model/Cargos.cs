using Newtonsoft.Json;

namespace Tzedaka.Model
{
    public class Cargos
    {
        [JsonProperty("id")]
        public int Id_Cargo { get; set; }

        [JsonProperty("cargo")]
        public string Cargo { get; set; }
    }
}
