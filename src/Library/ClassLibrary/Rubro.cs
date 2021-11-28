using System.Text.Json.Serialization;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Esta clase respresenta los datos basicos y necesarios de los rubros.
    /// </summary>
    public class Rubro
    {
        /// <summary>
        /// Se indica el nombre <value>rubro</value> del rubro
        /// </summary>
        public string Nombre { get; set; }

        [JsonConstructor]
        public Rubro()
        {
        }

        /// <summary>
        /// Se indica el nombre <value>rubro</value> del rubro
        /// </summary>
        /// <param name="nombre">rubro</param>        
        public Rubro(string nombre)
        {
            this.Nombre = nombre;
        }
    }
}