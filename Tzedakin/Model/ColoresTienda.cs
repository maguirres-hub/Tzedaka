using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tzedaka.Model
{
    public class ColoresTienda
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("color_fondo")]
        public string colorFondo { get; set; }
        [JsonProperty("color_letra_fondo")]
        public string colorLetraFondo { get; set; }
        [JsonProperty("color_producto")]
        public string colorProducto { get; set; }
        [JsonProperty("color_letra_producto")]
        public string colorLetraProducto { get; set; }
        [JsonProperty("color_comentario")]
        public string colorComentario { get; set; }
        [JsonProperty("color_letra_comentario")]
        public string colorLetraComentario { get; set; }
    }
}
