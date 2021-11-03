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
        public string FechaTramite { get; set; }

        /// <summary>
        /// Se indica la <value>fechaVencimiento</value> de la Habiltiacion
        /// </summary>
        public string FechaVencimiento { get; set; }

        /// <summary>
        /// Se indica el <value>estado</value> de la Habiltiacion
        /// </summary>
        public bool Estado { get; set; }


        
        /// <param name="descripcion">descripcion</param>
        /// <param name="nombreInsitucionHabilitada">nombreInsitucionHabilitada</param>
        /// <param name="fechaTramite">fechaTramite</param>
        /// <param name="fechaVencimiento">fechaVencimiento</param>
        /// <param name="estado">estado</param>        
        public Habilitacion(string descripcion, string nombreInsitucionHabilitada, string fechaTramite, string fechaVencimiento, bool estado)
        {
            this.Descripcion = descripcion;
            this.NombreInsitucionHabilitada = nombreInsitucionHabilitada;
            this.FechaTramite = fechaTramite;
            this.FechaVencimiento = fechaVencimiento;
            this.Estado = estado;
        }

    }

}
