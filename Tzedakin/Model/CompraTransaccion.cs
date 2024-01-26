using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tzedaka.Model
{
    public class CompraTransaccion
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("fecha")]
        public string fecha { get; set; }
        [JsonProperty("cantidad")]
        public int cantidad { get; set; }
        [JsonProperty("precio")]
        public float precio { get; set; }
        [JsonProperty("idComprador")]
        public int idComprador { get; set; }
        [JsonProperty("idVendedor")]
        public int idVendedor { get; set; }
        [JsonProperty("idProducto")]
        public int idProducto { get; set; }
    }
}
