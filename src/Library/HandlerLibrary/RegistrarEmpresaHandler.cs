using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PII_E13.ClassLibrary;
using Telegram.Bot.Types.ReplyMarkups;

namespace PII_E13.HandlerLibrary
{
    /// <summary>
    /// Clase base para implementar el patrón Chain of Responsibility. En ese patrón se pasa un mensaje a través de una
    /// cadena de "handlers" que pueden procesar o no el mensaje. Cada "handler" decide si procesa el mensaje, o si se lo
    /// pasa al siguiente. Esta clase base implmementa la responsabilidad de recibir el mensaje y pasarlo al siguiente
    /// "handler" en caso que el mensaje no sea procesado. La responsabilidad de decidir si el mensaje se procesa o no, y
    /// de procesarlo, se delega a las clases sucesoras de esta clase base.
    /// </summary>
    public class RegistrarEmpresaHandler : HandlerBase
    {
        private StringBuilder stringBuilder;
        private string accionPrevia;
        private const int COLUMNAS_CATEGORIAS = 3;
        private const int FILAS_CATEGORIAS = 2;
        private const int COLUMNAS_OFERTAS = 1;
        private const int FILAS_OFERTAS = 3;

        /// <summary>
        /// Diccionario utilizado para contener todas las búsquedas que se están realizando por los usuarios.
        /// Se identifica al usuario por su id en una plataforma y se guarda una instancia de <see cref="InformacionPostulacion"/>.
        /// </summary>
        /// <value>Diccionario de instancias de <see cref="InformacionPostulacion"/> identificadas por ids de usuarios en string</value>
        private Dictionary<string, InformacionPostulacion> Busquedas { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PostularseAOfertaHandler"/>. 
        /// Esta clase procesa la postulación a una oferta.
        /// </summary>
        public RegistrarEmpresaHandler(HandlerBase siguiente) : base(siguiente)
        {
            this.Busquedas = new Dictionary<string, InformacionPostulacion>();
            this.stringBuilder = new StringBuilder();
            stringBuilder.Append("Datos sobre tu empresa: \n\n");

        }

        /// <summary>
        /// La clase procesa el mensaje y retorna true o no lo procesa y retorna false.
        /// </summary>
        /// <param name="mensaje">El mensaje a procesar.</param>
        /// <param name="respuesta">La respuesta al mensaje procesado.</param>
        /// <returns>true si el mensaje fue procesado; false en caso contrario</returns>
        protected override bool ResolverInterno(IMensaje mensaje, out RespuestaTelegram respuesta)
        {
            respuesta = new RespuestaTelegram(string.Empty);
            if (!this.PuedeResolver(mensaje))
            {
                return false;
            }

            InformacionPostulacion infoPostulacion = new InformacionPostulacion();
            if (this.Busquedas.ContainsKey(mensaje.IdUsuario))
            {
                infoPostulacion = this.Busquedas[mensaje.IdUsuario];
            }
            else
            {
                this.Busquedas.Add(mensaje.IdUsuario, infoPostulacion);
            }
            List<string> titulosOfertas = new List<string>();
            List<string> opciones = new List<string>();
            opciones.Add("Nombre");
            opciones.Add("Direccion");
            opciones.Add("Rubro");
            opciones.Add("Ciudad");

            if (infoPostulacion.CategoriasDisponibles == null)
            {
                infoPostulacion.CategoriasDisponibles = new List<string>();

                foreach (string opcion in opciones)
                {
                    if (!infoPostulacion.CategoriasDisponibles.Contains(opcion))
                    {
                        infoPostulacion.CategoriasDisponibles.Add(opcion);
                    }
                }

            }
            List<InlineKeyboardButton> botonesDeCategorias = TelegramBot.Instancia.ObtenerBotones(infoPostulacion.CategoriasDisponibles);
            List<InlineKeyboardButton> botonesDeOfertas = new List<InlineKeyboardButton>();
            List<List<InlineKeyboardButton>> tecladoFijoCategorias = new List<List<InlineKeyboardButton>>() {
                new List<InlineKeyboardButton>() {TelegramBot.Instancia.BotonCancelar, TelegramBot.Instancia.BotonListo}
            };
            List<List<InlineKeyboardButton>> tecladoFijoOfertas = new List<List<InlineKeyboardButton>>() {
                new List<InlineKeyboardButton>() {InlineKeyboardButton.WithCallbackData("Salir")}
            };

            switch (infoPostulacion.Estado)
            {

                case Estados.Inicio:
                    respuesta.Texto = "Por favor, indícanos detalladamente lo qué necesitas, dentro de un mensaje.";
                    infoPostulacion.Estado = Estados.Categorias;
                    return true;

                case Estados.Categorias:
                    // for each con los botones para el registro
                    foreach (string opcion in opciones)
                    {
                        if (!infoPostulacion.CategoriasDisponibles.Contains(opcion))
                        {
                            infoPostulacion.CategoriasDisponibles.Add(opcion);
                        }
                    }

                    List<string> etiquetas = mensaje.Texto.Split(' ').ToList();
                    infoPostulacion.Etiquetas = etiquetas;
                    infoPostulacion.IndiceActual = 0;
                    respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeCategorias, infoPostulacion.IndiceActual, FILAS_CATEGORIAS, COLUMNAS_CATEGORIAS, tecladoFijoCategorias);
                    infoPostulacion.Estado = Estados.DatosEmpresa;
                    StringBuilder st = new StringBuilder();
                    st.Append("############   REGISTRO EMPRESA   ############");
                    st.Append("\nBien, ahora necesitamos que selecciones los datos que quiere ir ingresando.\n\nSelecciona \"Listo\" cuando quieras continuar el registro, o \"Cancelar\" para detenerlo.");
                    respuesta.Texto = st.ToString();
                    return true;

                case Estados.DatosEmpresa:
                    switch (mensaje.Texto)
                    {
                        case "Listo":

                            stringBuilder.Append("Datos sobre tu empresa: \n\n");


                            botonesDeOfertas = TelegramBot.Instancia.ObtenerBotones(titulosOfertas);
                            respuesta.Texto = this.stringBuilder.ToString();
                            respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeOfertas, infoPostulacion.IndiceActual, FILAS_OFERTAS, COLUMNAS_OFERTAS, tecladoFijoOfertas);
                            return true;

                        case "Cancelar":
                            this.Cancelar();
                            return false;
                    }
                    if (!infoPostulacion.CategoriasDisponibles.Contains(mensaje.Texto))
                    {
                        respuesta.Texto = $"Se ingresó el dato _\"{mensaje.Texto}\"_ en el campo *{this.accionPrevia}*";
                        this.stringBuilder.Append("\n" + this.accionPrevia + ":   " + mensaje.Texto);
                        respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeCategorias, infoPostulacion.IndiceActual, FILAS_CATEGORIAS, COLUMNAS_CATEGORIAS, tecladoFijoCategorias);
                        respuesta.EditarMensaje = true;
                        return true;
                    }

                    botonesDeCategorias.Remove(botonesDeCategorias.First(b => b.Text == mensaje.Texto));
                    if (infoPostulacion.IndiceActual >= botonesDeCategorias.Count)
                    {
                        infoPostulacion.IndiceActual = botonesDeCategorias.Count - FILAS_CATEGORIAS * COLUMNAS_CATEGORIAS;
                    }
                    infoPostulacion.CategoriasDisponibles.Remove(mensaje.Texto);
                    if (infoPostulacion.CategoriasDisponibles.Count <= (FILAS_CATEGORIAS * COLUMNAS_CATEGORIAS))
                    {
                        tecladoFijoCategorias = new List<List<InlineKeyboardButton>>()
                        {
                            new List<InlineKeyboardButton>() { TelegramBot.Instancia.BotonCancelar, TelegramBot.Instancia.BotonListo }
                        };
                    }
                    infoPostulacion.Categorias.Add(mensaje.Texto);
                    respuesta.Texto = $"A continuacion se habilito el campo _\"{mensaje.Texto}\"_ para su ingreso.\n\nSelecciona _\"Listo\"_ cuando quieras finalizar el registro, o _\"Cancelar\"_ para detenerlo.";
                    respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeCategorias, infoPostulacion.IndiceActual, FILAS_CATEGORIAS, COLUMNAS_CATEGORIAS, tecladoFijoCategorias);
                    respuesta.EditarMensaje = true;
                    return true;

                
            }
            infoPostulacion = new InformacionPostulacion();
            return false;
        }

        /// <summary>
        /// La clase procesa el mensaje y retorna true o no lo procesa y retorna false.
        /// </summary>
        /// <param name="callback">El callback a procesar.</param>
        /// <param name="respuesta">La respuesta al mensaje procesado.</param>
        /// <returns>true si el mensaje fue procesado; false en caso contrario</returns>
        protected override bool ResolverInterno(ICallBack callback, out RespuestaTelegram respuesta)
        {
            respuesta = new RespuestaTelegram(string.Empty);
            if (!this.PuedeResolver(callback))
            {
                return false;
            }

            InformacionPostulacion infoPostulacion = new InformacionPostulacion();
            if (this.Busquedas.ContainsKey(callback.IdUsuario))
            {
                infoPostulacion = this.Busquedas[callback.IdUsuario];
            }
            else
            {
                this.Busquedas.Add(callback.IdUsuario, infoPostulacion);
            }
            List<string> titulosOfertas = new List<string>();
           
            List<InlineKeyboardButton> botonesDeCategorias = TelegramBot.Instancia.ObtenerBotones(infoPostulacion.CategoriasDisponibles);
            List<InlineKeyboardButton> botonesDeOfertas = new List<InlineKeyboardButton>();
            List<List<InlineKeyboardButton>> tecladoFijoCategorias = new List<List<InlineKeyboardButton>>() {
                new List<InlineKeyboardButton>() {TelegramBot.Instancia.BotonCancelar, TelegramBot.Instancia.BotonListo}
            };
            List<List<InlineKeyboardButton>> tecladoFijoOfertas = new List<List<InlineKeyboardButton>>() {
                new List<InlineKeyboardButton>() {InlineKeyboardButton.WithCallbackData("Salir")}
            };

            switch (infoPostulacion.Estado)
            {
                case Estados.DatosEmpresa:
                    switch (callback.Texto)
                    {
                        case "Listo":
                            botonesDeOfertas = TelegramBot.Instancia.ObtenerBotones(titulosOfertas);
                            respuesta.Texto = this.stringBuilder.ToString();
                            respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeOfertas, infoPostulacion.IndiceActual, FILAS_OFERTAS, COLUMNAS_OFERTAS, tecladoFijoOfertas);
                            return true;

                        case "Cancelar":
                            this.Cancelar();
                            return false;
                    }
                
                    botonesDeCategorias.Remove(botonesDeCategorias.First(b => b.Text == callback.Texto));
                    if (infoPostulacion.IndiceActual >= botonesDeCategorias.Count)
                    {
                        infoPostulacion.IndiceActual = botonesDeCategorias.Count - FILAS_CATEGORIAS * COLUMNAS_CATEGORIAS;
                    }
                    infoPostulacion.CategoriasDisponibles.Remove(callback.Texto);
                    if (infoPostulacion.CategoriasDisponibles.Count <= (FILAS_CATEGORIAS * COLUMNAS_CATEGORIAS))
                    {
                        tecladoFijoCategorias = new List<List<InlineKeyboardButton>>()
                        {
                            new List<InlineKeyboardButton>() { TelegramBot.Instancia.BotonCancelar, TelegramBot.Instancia.BotonListo }
                        };
                    }
                    infoPostulacion.Categorias.Add(callback.Texto);
                    respuesta.Texto = $"A continuacion se habilito el campo _\"{callback.Texto}\"_ para su ingreso.\n\nSelecciona _\"Listo\"_ cuando quieras continuar la búsqueda, o _\"Cancelar\"_ para detenerla.";
                    this.accionPrevia = callback.Texto;
                    respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeCategorias, infoPostulacion.IndiceActual, FILAS_CATEGORIAS, COLUMNAS_CATEGORIAS, tecladoFijoCategorias);
                    respuesta.EditarMensaje = true;
                    return true;

               
            }
            infoPostulacion = new InformacionPostulacion();
            return false;
        }

        /// <summary>
        /// Este método puede ser sobreescrito en las clases sucesores que procesan varios mensajes cambiando de estado
        /// entre mensajes deben sobreescribir este método para volver al estado inicial. En la clase base no hace nada.
        /// </summary>
        protected void CancelarInterno(string idUsuario)
        {
            this.Busquedas.Remove(idUsuario);
        }

        /// <summary>
        /// Determina si este "handler" puede procesar el mensaje. En la clase base se utiliza el array
        /// <see cref="HandlerBase.Etiquetas"/> para buscar el texto en el mensaje ignorando mayúsculas y minúsculas. Las
        /// clases sucesores pueden sobreescribir este método para proveer otro mecanismo para determina si procesan o no
        /// un mensaje.
        /// </summary>
        /// <param name="mensaje">El mensaje a procesar.</param>
        /// <returns>true si el mensaje puede ser pocesado; false en caso contrario.</returns>
        protected override bool PuedeResolver(IMensaje mensaje)
        {
            try
            {
                Sistema.Instancia.ObtenerEmprendedorPorId(mensaje.IdUsuario);
            }
            catch (KeyNotFoundException e)
            {
                return false;
            }
            // Cuando no hay palabras clave este método debe ser sobreescrito por las clases sucesoras y por lo tanto
            // este método no debería haberse invocado.
            /*
            if (this.Etiquetas == null || this.Etiquetas.Length == 0)
            {
                throw new InvalidOperationException("No hay palabras clave que puedan ser procesadas");
            }
            */

            return true;
            //return this.Etiquetas.Any(s => mensaje.Texto.Equals(s, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Determina si este "handler" puede procesar el mensaje. En la clase base se utiliza el array
        /// <see cref="HandlerBase.Etiquetas"/> para buscar el texto en el mensaje ignorando mayúsculas y minúsculas. Las
        /// clases sucesores pueden sobreescribir este método para proveer otro mecanismo para determina si procesan o no
        /// un mensaje.
        /// </summary>
        /// <param name="callback">El callback a procesar.</param>
        /// <returns>true si el mensaje puede ser pocesado; false en caso contrario.</returns>
        protected override bool PuedeResolver(ICallBack callback)
        {
            try
            {
                Sistema.Instancia.ObtenerEmprendedorPorId(callback.IdUsuario);
                InformacionPostulacion infoPostulacion = this.Busquedas[callback.IdUsuario];
                if (infoPostulacion.Estado != Estados.DatosEmpresa)
                {
                    return false;
                }
            }
            catch (KeyNotFoundException e)
            {
                return false;
            }

            // Cuando no hay palabras clave este método debe ser sobreescrito por las clases sucesoras y por lo tanto
            // este método no debería haberse invocado.
            /*
            if (this.Etiquetas == null || this.Etiquetas.Length == 0)
            {
                throw new InvalidOperationException("No hay palabras clave que puedan ser procesadas");
            }
            */

            return true;
            //return this.Etiquetas.Any(s => mensaje.Texto.Equals(s, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Retorna este "handler" al estado inicial. En los "handler" sin estado no hace nada. Los "handlers" que
        /// procesan varios mensajes cambiando de estado entre mensajes deben sobreescribir este método para volver al
        /// estado inicial.
        /// </summary>
        public void Cancelar(string idUsuario)
        {
            this.CancelarInterno(idUsuario);
            if (this.Siguiente != null)
            {
                this.Siguiente.Cancelar();
            }
        }

        /// <summary>
        /// Representación de los posibles estados de una postulación a oferta.
        /// </summary>
        private enum Estados
        {
            Inicio,
            Categorias,
            DatosEmpresa
           
        }

        /// <summary>
        /// Clase privada contenedora de la información de una postulación a una oferta.
        /// </summary>
        private class InformacionPostulacion
        {
            /// <summary>
            /// Lista de etiquetas que está usando un usuario para buscar una oferta.
            /// </summary>
            public List<string> Etiquetas { get; set; } = new List<string>();
            /// <summary>
            /// Lista de ofertas encontradas en la búsqueda de ofertas.
            /// </summary>
            public List<Oferta> OfertasEncontradas { get; set; } = new List<Oferta>();
            /// <summary>
            /// Lista de categorías que está usando un usuario para buscar una oferta.
            /// </summary>
            public List<string> Categorias { get; set; } = new List<string>();
            /// <summary>
            /// Estado de la búsqueda de ofertas de un usuario.
            /// </summary>
            public Estados Estado { get; set; } = Estados.Inicio;

            /// <summary>
            /// Oferta seleccionada por un usuario entre la lista de ofertas encontradas.
            /// </summary>
            public Oferta ofertaSeleccionada { get; set; }

            /// <summary>
            /// Indice actual dentro de la lista de categorías.
            /// </summary>
            public int IndiceActual { get; set; } = 0;

            public List<string> CategoriasDisponibles { get; set; }
        }
    }
}