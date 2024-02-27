using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tzedaka.Model
{
    public class Tienda
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("nombre")]
        public string nombre { get; set; }
        [JsonProperty("direccion")]
        public string Direccion { get; set; }
        [JsonProperty("ciudad")]
        public string Ciudad { get; set; }
        [JsonProperty("pais")]
        public string Pais { get; set; }
        [JsonProperty("tipo_envio")]
        public string tipoEnvio { get; set; }
        [JsonProperty("id_cliente")]
        public int idCliente { get; set; }
        [JsonProperty("puntuacion")]
        public float puntuacion { get; set; }
        [JsonProperty("correo")]
        public string correo { get; set; }
        [JsonProperty("telefono")]
        public string telefono { get; set; }
        [JsonProperty("id_ciudad")]
        public int idCiudad { get; set; }
        [JsonProperty("id_pais")]

        public int idPais { get; set; }
        [JsonProperty("subscrito")]
        public int subscrito
        {
            get { return estaSubscrito ? 1 : 0; }
            set
            {
                estaSubscrito = value == 0 ? false : true;
                }
        }
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
        public bool estaSubscrito { get; set; }
        public bool tieneTienda { get; set; }

        public bool CamposLLenos()
        {
            foreach (var item in typeof(Tienda).GetProperties())
            {
                if (item.Name == "nombre" || item.Name == "Direccion" || item.Name == "tipoEnvio" || item.Name == "correo" || item.Name == "telefono")
                    if (item.GetValue(this) == null || string.IsNullOrEmpty(item.GetValue(this).ToString()))
                    {
                        return false;
                    }
            }
            return true;
        }
    }
}
