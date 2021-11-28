using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Clase encargada de persistir información en archivos.
    /// </summary>
    public class PersistorDeJson : IPersistor
    {
        /// <summary>
        /// Guarda el contenido de un objeto en formato JSON en la ruta de destino.
        /// </summary>
        /// <param name="ruta">La ruta donde se guardará el archivo JSON.</param>
        /// <param name="objeto">El objeto a guardar en formato JSON.</param>
        public void Escribir<T>(string ruta, T objeto) where T : IIdentificable
        {
            if (File.Exists(ruta))
            {
                List<T> contenido = JsonSerializer.Deserialize<List<T>>(File.ReadAllText(ruta));
                int indice = contenido.FindIndex(objetoEnContenido => objetoEnContenido.Id == objeto.Id);
                if (indice != -1)
                {
                    contenido[indice] = objeto;
                }
                else
                {
                    contenido.Add(objeto);
                }
                File.WriteAllText(ruta, JsonSerializer.Serialize(contenido), encoding: System.Text.Encoding.UTF8);
            }
            else
            {
                List<T> contenido = new List<T>();
                contenido.Add(objeto);
                File.WriteAllText(ruta, JsonSerializer.Serialize(contenido), encoding: System.Text.Encoding.UTF8);
            }
        }

        /// <summary>
        /// Lee la información contenida en el archivo JSON ubicado en la ruta enviada por parámetros.
        /// </summary>
        /// <param name="ruta">La ruta hacia la fuente de información.</param>
        /// <returns>Retorna la información encontrada en forma de una cadena de caracteres.</returns>
        public T Leer<T>(string ruta) where T : new()
        {
            if (File.Exists(ruta))
            {
                return JsonSerializer.Deserialize<T>(File.ReadAllText(ruta));
            }
            else
            {
                System.Console.WriteLine(1);
                return new T();
            }
        }

    }
}