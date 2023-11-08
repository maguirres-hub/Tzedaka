using Newtonsoft.Json;
using System;

namespace Tzedaka.Model
{
    public class Pedidos
    {
        [JsonProperty("id_pedido")]
        public int Id_Pedido { get; set; }

        [JsonProperty("fecha_pedido")]
        public DateTime Fecha_Pedido { get; set; }

        [JsonProperty("id_estado")]
        public int Id_Estado { get; set; }

        [JsonProperty("id_cliente")]
        public int Id_Cliente { get; set; }

        [JsonProperty("id_pago")]
        public int Id_Pago { get; set; }
    }
}
