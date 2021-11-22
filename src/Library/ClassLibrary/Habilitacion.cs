using System;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Esta clase respresenta los datos basicos y necesarios de una Habilitación.
    /// </summary>
    public class Habilitacion: IJsonConvertible
    {
        /// <summary>
        /// Nombre de la habilitación.
        /// </summary>
        /// <value>Cadena de caracteres con el nombre de una habilitación.</value>
        public string Nombre { get; }

        /// <summary>
        /// Descripción de la habilitación.
        /// </summary>
        /// <value>Cadena de caracteres con la descripción de una habilitación.</value>
        public string Descripcion { get; }

        /// <summary>
        /// Nombre de la institución que expedió la habilitación.
        /// </summary>
        /// <value>Cadena de caracteres con el nombre de la institución que expedió una habilitación.</value>
        public string NombreInsitucionHabilitada { get; }

        /// <summary>
        /// Fecha del trámite de la habilitación.
        /// </summary>
        /// <value><see cref="DateTime"/> con la fecha del trámite de una habilitación.</value>
        public DateTime FechaTramite { get; }

        /// <summary>
        /// Fecha de vencimiento de la habilitación.
        /// </summary>
        /// <value><see cref="DateTime"/> con la fecha de vencimiento de una habilitación.</value>
        public DateTime FechaVencimiento { get; set; }

        /// <summary>
        /// Indica si la habilitación sigue vigente.
        /// </summary>
        /// <value>Booleano con valor <c>true</c> si la habilitación sigue vigente y <c>false</c> en caso contrario.</value>
        public bool Vigente { get; set; }

        /// <summary>
        /// Crea una instancia de Habilitacion.
        /// </summary>
        /// <param name="nombre">Nombre de la Habilitacion</param>
        /// <param name="descripcion">Descripcion de la Habilitacion.</param>
        /// <param name="nombreInsitucionHabilitada">Nombre de la institución que habilitó la Habilitacion.</param>
        /// <param name="fechaTramite">Fecha de Tramite de la Habilitacion.</param>
        /// <param name="fechaVencimiento">Fecha de Vencimiento de la Habilitacion.</param>
        /// <param name="vigente">Indica si la Habilitacion está vigente.</param>
        public Habilitacion(string nombre, string descripcion, string nombreInsitucionHabilitada, DateTime fechaTramite, DateTime fechaVencimiento, bool vigente)
        {
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.NombreInsitucionHabilitada = nombreInsitucionHabilitada;
            this.FechaTramite = fechaTramite;
            this.FechaVencimiento = fechaVencimiento;
            this.Vigente = vigente;
        }
    }
}
