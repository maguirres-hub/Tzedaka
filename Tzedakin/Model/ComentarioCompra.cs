using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tzedaka.Model
{
    public class ComentarioCompra
    {
        [JsonProperty("idCliente")]
        public int idCliente { get; set; }
        [JsonProperty("idProducto")]
        public int idProducto { get; set; }
        [JsonProperty("idTienda")]
        public int idTienda { get; set; }
        [JsonProperty("fecha")]
        public DateTime fecha { get; set; }
        [JsonProperty("puntuacion")]
        public float puntuacion { get; set; }
        [JsonProperty("texto")]
        public string texto { get; set; }
        public string nombreCliente { get; set; }
    }
}
