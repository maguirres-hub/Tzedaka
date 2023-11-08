using Newtonsoft.Json;

namespace Tzedaka.Model
{
    public class Clientes
    {
        [JsonProperty("id_cliente")]
        public int Id_Cliente { get; set; }

        [JsonProperty("nombres")]
        public string Nombres { get; set; }

        [JsonProperty("apellidos")]
        public string Apellidos { get; set; }

        [JsonProperty("telefono")]
        public string Telefono { get; set; }

        [JsonProperty("direccion")]
        public string Direccion { get; set; }
        [JsonProperty("correo")]
        public string Correo { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("id_pais")]
        public int Id_Pais { get; set; }
        [JsonProperty("pais")]
        public string Pais { get; set; }

        [JsonProperty("id_ciudad")]
        public int Id_Ciudad { get; set; }
        [JsonProperty("ciudad")]
        public string Ciudad { get; set; }

        [JsonProperty("fecha_registrocl")]
        public string Fecha_Regitrocl { get; set; }

        [JsonProperty("active")]
        public int Active { get; set; }

        [JsonProperty("id_subscripcion")]
        public int Id_Subscripcion { get; set; }

        [JsonProperty("is_valid")]
        public int Is_Valid { get; set; }
        [JsonProperty("bloque")]
        public int Bloque { get; set; }

        public string Nombre_Completo => "Donar a " + Nombres + " " + Apellidos;

    }
}
