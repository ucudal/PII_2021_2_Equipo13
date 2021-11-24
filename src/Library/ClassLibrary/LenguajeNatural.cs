using Google.Cloud.Dialogflow.V2;

namespace PII_E13.ClassLibrary
{
    /// <summary>
    /// Representa un procesador de lenguaje natural que utiliza el servicio de Google Cloud "Dialogflow" para reconocer intenciones en texto.
    /// Se aplica el patrón Adapter para utilizar los servicios de las APIs de Google para procesado de lenguaje natural a través de esta clase,
    /// además aplicando Constructor al generar instancias de la clase <see cref="Intencion"/>, la cual representa a una intención hallada.
    /// </summary>
    public class LenguajeNatural
    {
        private const string PROYECTO_GCS = "lofty-hybrid-332616";
        private const string CODIGO_IDIOMA = "es-419";
        private readonly SessionsClient clienteDeSesiones = SessionsClient.Create();
        private SessionName sesion;

        /// <summary>
        /// Crea una instancia de un procesador de lenguaje natural para una sesión.
        /// </summary>
        /// <param name="idSesion">Id única de la <see cref="Sesion"/> dentro de la cual se está utilizando este procesador de lenguaje natural</param>
        public LenguajeNatural(string idSesion)
        {
            sesion = SessionName.FromProjectSession(PROYECTO_GCS, idSesion);
        }


        /// <summary>
        /// Genera una intención utilizando procesamiento de lenguaje natural (PLN) a partir de una cadena de texto.
        /// </summary>
        /// <param name="texto">La cadena de texto cuya intención se quiere encontrar con PLN.</param>
        /// <returns>Una instancia de <see cref="Intencion"/> con la información de la intención identificada.</returns>
        public Intencion ObtenerIntencion(string texto)
        {
            TextInput entradaDeTexto = new TextInput();
            entradaDeTexto.Text = texto;
            entradaDeTexto.LanguageCode = CODIGO_IDIOMA;

            QueryInput entradaDeConsultas = new QueryInput();
            entradaDeConsultas.Text = entradaDeTexto;

            DetectIntentResponse respuestaDeDeteccion = this.clienteDeSesiones.DetectIntent(this.sesion, entradaDeConsultas);

            QueryResult resultadoDeDeteccion = respuestaDeDeteccion.QueryResult;

            this.UltimaIntencion = new Intencion(resultadoDeDeteccion.Intent.DisplayName, (resultadoDeDeteccion.IntentDetectionConfidence * 100), texto);

            return this.UltimaIntencion;
        }

        /// <summary>
        /// Representa la última intención encontrada utilizando esta instancia de un procesador de lenguaje natural.
        /// </summary>
        /// <value>Una instancia de <see cref="Intencion"/> con la información de la última intención identificada.</value>
        public Intencion UltimaIntencion { get; private set; }
    }
}