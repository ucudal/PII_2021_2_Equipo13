namespace ClassLibrary
{
    public class Habilitacion
    {
        public string descripcion { get; set; }
        public string nombreInsitucionHabilitada { get; set; }
        public string fechaTramite { get; set; }
        public string fechaVencimiento { get; set; }
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
