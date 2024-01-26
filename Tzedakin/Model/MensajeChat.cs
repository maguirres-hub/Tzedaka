using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tzedaka.Model
{
    public class MensajeChat
    {
        [JsonProperty("id_compra")]
        public int idCompra { get; set; }
        [JsonProperty("fecha")]
        public DateTime fecha { get; set; }
        [JsonProperty("mensaje")]
        public string mensaje { get; set; }
        [JsonProperty("id_usuario")]
        public int idUsuario { get; set; }

        public bool propio { get; set; }
    }
}
