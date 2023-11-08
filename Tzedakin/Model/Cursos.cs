using Newtonsoft.Json;

namespace Tzedaka.Model
{
    public class Cursos
    {
        [JsonProperty("id_curso")]
        public int Id_Curso { get; set; }
        [JsonProperty("nombre_curso")]
        public string Nombre_Curso { get; set; }
        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }
        [JsonProperty("url_video")]
        public string URl_Video { get; set; }
    }
}
