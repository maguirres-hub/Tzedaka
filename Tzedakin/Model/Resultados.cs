using Newtonsoft.Json;

namespace Tzedaka.Model
{
    public class Resultados
    {
        [JsonProperty("insertId")]
        public int InsertId { get; set; }
    }
}
