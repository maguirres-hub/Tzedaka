using Newtonsoft.Json;

namespace Tzedaka.Model
{
    public class Empleados
    {
        [JsonProperty("id_empleado")]
        public int Id_Empleado { get; set; }

        [JsonProperty("nombres")]
        public string Nombres { get; set; }

        [JsonProperty("apellidos")]
        public string Apellidos { get; set; }

        [JsonProperty("id_oficina")]
        public int Id_Oficina { get; set; }

        [JsonProperty("ciudad")]
        public string Ciudad { get; set; }

        [JsonProperty("id_cargo")]
        public int Id_Cargo { get; set; }

        [JsonProperty("cargo")]
        public string Cargo { get; set; }

        [JsonProperty("correo")]
        public string Correo { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("is_valid")]
        public int Is_Valid { get; set; }
    }
}
