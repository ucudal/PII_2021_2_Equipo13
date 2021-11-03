namespace ClassLibrary
{
     /// <summary>
    /// Esta clase respresenta los datos basicos y necesarios de los rubros.
    /// </summary>
    public class Rubro
    {
        /// <summary>
        /// Se indica el nombre <value>rubro</value> del rubro
        /// </summary>
        public string Rubro { get; set; }

        /// <summary>
        /// Se indica el nombre <value>rubro</value> del rubro
        /// </summary>
        /// <param name="rubro">rubro</param>        
        public Rubro(string rubro)
        {
            this.rubro = rubro;
        }
    }
}