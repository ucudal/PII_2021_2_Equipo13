using System;
using System.Threading.Tasks;
using Ucu.Poo.Locations.Client;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    ///  Patrones y principios utilizados en esta clase:
    /// Expert conocer la informacion de las ubicaciones a las cuales se desea calcular distancias u obtner coordenadas.
    /// </summary>
    public class AdaptadorLocacion : IAdaptadorLocacion
    {
        LocationApiClient Client = new LocationApiClient();

        /// <summary>
        /// Sirve para obtener la distancia entre dos ubicaciones.
        /// </summary>
        /// <param name="primaria">ubicacion primaria</param>
        /// <param name="secundaria">ubicacion secundaria</param>
        /// <returns></returns>
        public double ObtenerDistancia(IUbicacion primaria, IUbicacion secundaria)
        {
            Location locationA = new Location();
            locationA.AddresLine = primaria.Direccion;
            locationA.CountryRegion = primaria.Ciudad;

            Location locationB = new Location();
            locationB.AddresLine = secundaria.Direccion;
            locationB.CountryRegion = secundaria.Ciudad;

            var taskDistance = this.Client.GetDistanceAsync(locationA, locationB);
            taskDistance.Wait();
            Distance distance = taskDistance.Result;

            if (distance.Found)
            {
                return distance.TravelDistance;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}