using Newtonsoft.Json;

namespace Tzedaka.Model
{
    public class Categorias
    {
        [JsonProperty("id_categoria")]
        public int Id_Categoria { get; set; }
        [JsonProperty("categoria")]
        public string Categoria_Nombre { get; set; }
    }
}
