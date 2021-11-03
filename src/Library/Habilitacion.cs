using System;

namespace ClassLibrary
{
    /// <summary>
    /// Esta clase respresenta los datos basicos y necesarios de una Habilitación
    /// </summary>
    public class Habilitacion
    {
        /// <summary>
        /// Se indica el <value>descripcion</value> de la Habiltiacion
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Se indica el <value>nombreInsitucionHabilitada</value> de la Habiltiacion
        /// </summary>
        public string NombreInsitucionHabilitada { get; set; }

        /// <summary>
        /// Se indica la <value>fechaTramite</value> de la Habiltiacion
        /// </summary>
        public DateTime FechaTramite { get; set; }

        /// <summary>
        /// Se indica la <value>fechaVencimiento</value> de la Habiltiacion
        /// </summary>
        public DateTime FechaVencimiento { get; set; }

        /// <summary>
        /// Se indica si la Habilitacion está <value>vigente</value>
        /// </summary>
        public bool Vigente { get; set; }

        /// <summary>
        /// Crea una instancia de Habilitacion
        /// </summary>
        /// <param name="descripcion">Descripcion de la Habilitacion</param>
        /// <param name="nombreInsitucionHabilitada">Nombre de la institución que habilitó la habilitación</param>
        /// <param name="fechaTramite">Fecha de Tramite de la Habilitacion</param>
        /// <param name="fechaVencimiento">Fecha de Vencimiento de la Habilitacion</param>
        /// <param name="vigente">Indica si la Habilitacion está vigente</param>
        public Habilitacion(string descripcion, string nombreInsitucionHabilitada, DateTime fechaTramite, DateTime fechaVencimiento, bool vigente)
        {
            this.Descripcion = descripcion;
            this.NombreInsitucionHabilitada = nombreInsitucionHabilitada;
            this.FechaTramite = fechaTramite;
            this.FechaVencimiento = fechaVencimiento;
            this.Vigente = vigente;
        }
    }
}
