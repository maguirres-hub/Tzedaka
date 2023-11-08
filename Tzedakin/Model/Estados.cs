using Newtonsoft.Json;

namespace Tzedaka.Model
{
    public class Estados
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("estado")]
        public string Estado { get; set; }

    }
}
