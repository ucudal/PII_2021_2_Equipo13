using System;

namespace PII_E13.ClassLibrary
{
   /// <summary>
   /// Esta es la interface de percisitencia de la aplicacion. su funcion es guardar y recuperar los datos de la aplicacion.
   /// </summary>
     public interface IPersistencia
     {
          /// <summary>
          /// Guarda el archivo en la ruta especificada.
          /// </summary>
          /// <param name="ruta">La ruta del archivo en el sistema.</param>
          /// <param name="informacion">La información a guardar en el archivo en un string.</param>
          void EscribirArchivo(string ruta, string informacion);

          /// <summary>
          /// Lee un archivo en el sistema.
          /// </summary>
          /// <param name="ruta">La ruta del archivo en el sistema.</param>
          /// <returns>Retorna la información contenida en un archivo en un string.</returns>
          string LeerArchivo(string ruta);
     }
}
