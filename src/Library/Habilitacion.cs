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

        public Habilitacion(string descripcion, string nombreInsitucionHabilitada, string fechaTramite, string fechaVencimiento, bool estado)
        {
            this.descripcion = descripcion;
            this.nombreInsitucionHabilitada = nombreInsitucionHabilitada;
            this.fechaTramite = fechaTramite;
            this.fechaVencimiento = fechaVencimiento;
            this.estado = estado;
        }

    }

}
