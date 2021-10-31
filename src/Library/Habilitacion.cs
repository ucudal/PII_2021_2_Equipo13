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
        public string descripcion { get; set; }

        /// <summary>
        /// Se indica el <value>nombreInsitucionHabilitada</value> de la Habiltiacion
        /// </summary>
        public string nombreInsitucionHabilitada { get; set; }

        /// <summary>
        /// Se indica la <value>fechaTramite</value> de la Habiltiacion
        /// </summary>
        public string fechaTramite { get; set; }

        /// <summary>
        /// Se indica la <value>fechaVencimiento</value> de la Habiltiacion
        /// </summary>
        public string fechaVencimiento { get; set; }

        /// <summary>
        /// Se indica el <value>estado</value> de la Habiltiacion
        /// </summary>
        public bool estado { get; set; }


        

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
