using System;
using System.Collections.Generic;
using System.IO;
using Recipes;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Clase encargada de mantener la información  escencial del sistema en archivos JSON.
    /// </summary>
    public class JsonPersistible : IPersistencia
    {

        public void EscribirArchivo(string ruta, string json)
        {
            if (!File.Exists(ruta))
            {
                File.CreateText(ruta);
                File.AppendAllText(ruta, json);
            }
            else
            {
                File.WriteAllText(ruta, json);
            }
        }

        public string LeerArchivo(string ruta)
        {
            return File.ReadAllText(ruta);
        }

    }
}