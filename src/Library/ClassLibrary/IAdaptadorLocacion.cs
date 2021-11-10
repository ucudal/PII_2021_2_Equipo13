using PII_E13.ClassLibrary;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Interfaz utilizada para desacoplar la implementación 
    /// de la clase AdaptadorLocacion utilizando el patron, adapter.
    /// logrando que la clase en la ubicacion no tenga que conocer la implementacion 
    /// de la clase AdaptadorLocacion quien se comunica con la libreria brindada por los profesores.
    /// </summary>
    public interface IAdaptadorLocacion
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="primaria"></param>
        /// <param name="secundaria"></param>
        /// <returns></returns>
        double ObtenerDistancia(Ubicacion primaria, Ubicacion secundaria);
    }
}