using System;
using System.Globalization;
using System.Web;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace Ucu.Poo.Locations.Client
{
    public class LocationApiClient
    {
        private const string BaseUrl = "https://pii-locationapi.azurewebsites.net";

        private HttpClient client = new HttpClient();

        private string DistanceUrl { get { return BaseUrl + "/distance"; } }

        private string LocationUrl { get { return BaseUrl + "/location"; } }

        private string MapUrl { get { return BaseUrl + "/map"; } }

        private string RouteUrl { get { return BaseUrl + "/route"; } }

        private Uri GetUri(string baseUrl, IDictionary<string, string> parameters)
        {
            return new Uri(string.Format("{0}?{1}",
                baseUrl,
                string.Join("&",
                    parameters.Select(kvp =>
                        string.Format("{0}={1}", kvp.Key, HttpUtility.UrlEncode(kvp.Value))))));
        }

        /// <summary>
        /// Obtiene las coordenadas de una dirección.
        /// </summary>
        /// <param name="address">La dirección.</param>
        /// <param name="city">La ciudad. Es opcional. El valor predeterminado es Montevideo.</param>
        /// <param name="department">El departamento, estado, provincia, etc. Es opcional. El valor predeterminado es `ontevideo.</param>
        /// <param name="country">El país. Es opcional. El valor predeterminado es Uruguay.</param>
        /// <returns>Las coordenadas de la dirección.</returns>
        public async Task<Location> GetLocationAsync(string address, string city = "Montevideo",
            string department = "Montevideo", string country = "Uruguay")
        {
            var parameters = new Dictionary<string, string>
            {
                { "address", address },
                { "city", city },
                { "department", department },
                { "country", country }
            };

            var requestUri = GetUri(LocationUrl, parameters);
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            Location result = JsonSerializer.Deserialize<Location>(content,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return result;
        }

        /// <summary>
        /// Obtiene la distancia entre dos coordenadas.
        /// </summary>
        /// <param name="from">La coordenada de origen.</param>
        /// <param name="to">La coordenada de destino.</param>
        /// <returns>La distancia entre las dos coordenadas.</returns>
        public async Task<Distance> GetDistanceAsync(Location from, Location to)
        {
            var parameters = new Dictionary<string, string>
            {
                { "fromLatitude", from.Latitude.ToString(CultureInfo.InvariantCulture) },
                { "fromLongitude", from.Longitude.ToString(CultureInfo.InvariantCulture) },
                { "toLatitude", to.Latitude.ToString(CultureInfo.InvariantCulture) },
                { "toLongitude", to.Longitude.ToString(CultureInfo.InvariantCulture) }
            };

            var requestUri = GetUri(DistanceUrl, parameters);
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            Distance result = JsonSerializer.Deserialize<Distance>(content,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return result;
        }

        /// <summary>
        /// Obtiene la distancia entre dos direcciones.
        /// </summary>
        /// <param name="from">La dirección de origen.</param>
        /// <param name="to">La dirección de destino.</param>
        /// <returns>La distancia entre las dos direcciones.</returns>
        public async Task<Distance> GetDistanceAsync(string from, string to)
        {
            var parameters = new Dictionary<string, string>
            {
                { "fromAddress", from },
                { "toAddress", to }
            };

            var requestUri = GetUri(DistanceUrl, parameters);
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            Distance result = JsonSerializer.Deserialize<Distance>(content,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return result;
        }

        /// <summary>
        /// Descarga una mapa de una coordenada.
        /// </summary>
        /// <param name="latitude">La latitud de la coordenada.</param>
        /// <param name="longitude">La longitud de la coordenada.</param>
        /// <param name="path">La ruta del archivo donde guardar el mapa. El formato es PNG.</param>
        /// <param name="zoomLevel">El nivel de zoom del mapa entre 1 y 20. Es opcional. El valor predeterminado es
        /// 15.</param>
        public async Task DownloadMapAsync(double latitude, double longitude, string path, int zoomLevel = 15)
        {
            var parameters = new Dictionary<string, string>
            {
                { "latitude", latitude.ToString(CultureInfo.InvariantCulture) },
                { "longitude", longitude.ToString(CultureInfo.InvariantCulture) },
                { "zoomLevel", zoomLevel.ToString(CultureInfo.InvariantCulture) }
            };

            var requestUri = GetUri(MapUrl, parameters);
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            using (var fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                await response.Content.CopyToAsync(fs);
                var stream = await response.Content.ReadAsStreamAsync();
            }
        }

        /// <summary>
        /// Un mapa con una ruta entre dos coordenadas.
        /// </summary>
        /// <param name="fromLatitude">La latitud de la coordenada de origen.</param>
        /// <param name="fromLongitude">La longitu de la coordenada de origen.</param>
        /// <param name="toLatitude">La latitud de la coordenada de destino.</param>
        /// <param name="toLongitude">La longitud de la coordenada de destino.</param>
        /// <param name="path">La ruta del archivo donde guardar el mapa. Es formato es PNG.</param>
        public async Task DownloadRouteAsync(double fromLatitude, double fromLongitude,
            double toLatitude, double toLongitude, string path)
        {
            var parameters = new Dictionary<string, string>
            {
                { "fromLatitude", fromLatitude.ToString(CultureInfo.InvariantCulture) },
                { "fromLongitude", fromLongitude.ToString(CultureInfo.InvariantCulture) },
                { "toLatitude", toLatitude.ToString(CultureInfo.InvariantCulture) },
                { "toLongitude", toLongitude.ToString(CultureInfo.InvariantCulture) }
            };

            var requestUri = GetUri(RouteUrl, parameters);
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            using (var fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                await response.Content.CopyToAsync(fs);
                var stream = await response.Content.ReadAsStreamAsync();
            }
        }
    }
}
