using System;
using System.Threading.Tasks;
using Ucu.Poo.Locations.Client;

namespace ClassLibrary
{
    /// <summary>
    ///  Patrones y principios utilizados en esta clase:
    /// Expert conocer la informacion de las ubicaciones a las cuales se desea calcular distancias u obtner coordenadas.
    /// </summary>
    public class GestorLocacion
    {
        LocationApiClient Client = new LocationApiClient();
        public async Task<double> ObtenerDistancia(Ubicacion primaria, Ubicacion secundaria)
        {
            Location locationA = new Location();
            locationA.AddresLine = primaria.Direccion;
            locationA.CountryRegion = primaria.Ciudad;

            Location locationB = new Location();
            locationB.AddresLine = secundaria.Direccion;
            locationB.CountryRegion = secundaria.Ciudad;

            Distance distance = await this.Client.GetDistanceAsync(locationA, locationB);

            if (distance.Found ) {
                return distance.TravelDistance;
            } else {
                throw new InvalidOperationException();
            }
        }

        public void ObtenerCoordenadas(string parametros)
        {
            throw new NotImplementedException();
        }

        public void ObtenerMapaDeRuta(Ubicacion primaria, Ubicacion secundaria)
        {
            throw new NotImplementedException();
        }

        public void ObtenerMapa(Ubicacion ubicacion)
        {
            throw new NotImplementedException();
        }
    }
}