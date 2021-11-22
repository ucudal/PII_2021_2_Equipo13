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
    public class PostularseAOfertaHandler : HandlerBase
    {
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
        /// <param name="siguiente">El próximo "handler".</param>
        public PostularseAOfertaHandler(HandlerBase siguiente) : base(siguiente)
        {
            this.Busquedas = new Dictionary<string, InformacionPostulacion>();
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PostularseAOfertaHandler"/>. 
        /// Esta clase procesa la postulación a una oferta.
        /// </summary>
        /// <param name="siguiente">El próximo "handler".</param>
        /// <param name="intencion">La intención utilizada para identificar a este handler.</param>
        public PostularseAOfertaHandler(HandlerBase siguiente, string intencion) : base(siguiente, intencion)
        {
            this.Busquedas = new Dictionary<string, InformacionPostulacion>();
        }

        /// <summary>
        /// La clase procesa el mensaje y retorna true o no lo procesa y retorna false.
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        /// <param name="mensaje">El mensaje a procesar.</param>
        /// <param name="respuesta">La respuesta al mensaje procesado.</param>
        /// <returns>true si el mensaje fue procesado; false en caso contrario</returns>
        protected override bool ResolverInterno(Sesion sesion, IMensaje mensaje, out RespuestaTelegram respuesta)
        {
            respuesta = new RespuestaTelegram(string.Empty);
            if (!this.PuedeResolver(sesion))
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

            if (infoPostulacion.CategoriasDisponibles == null)
            {
                infoPostulacion.CategoriasDisponibles = new List<string>();
                foreach (Material material in Sistema.Instancia.Materiales)
                {
                    foreach (string categoria in material.Categorias)
                    {
                        if (!infoPostulacion.CategoriasDisponibles.Contains(categoria))
                        {
                            infoPostulacion.CategoriasDisponibles.Add(categoria);
                        }
                    }
                }
            }
            List<InlineKeyboardButton> botonesDeCategorias = TelegramBot.Instancia.ObtenerBotones(infoPostulacion.CategoriasDisponibles);
            List<InlineKeyboardButton> botonesDeOfertas = new List<InlineKeyboardButton>();
            List<List<InlineKeyboardButton>> tecladoFijoCategorias = new List<List<InlineKeyboardButton>>() {
                new List<InlineKeyboardButton>() {TelegramBot.Instancia.BotonAnterior, TelegramBot.Instancia.BotonSiguiente},
                new List<InlineKeyboardButton>() {TelegramBot.Instancia.BotonCancelar, TelegramBot.Instancia.BotonListo}
            };
            List<List<InlineKeyboardButton>> tecladoFijoOfertas = new List<List<InlineKeyboardButton>>() {
                new List<InlineKeyboardButton>() {TelegramBot.Instancia.BotonAnterior, TelegramBot.Instancia.BotonSiguiente},
                new List<InlineKeyboardButton>() {InlineKeyboardButton.WithCallbackData("Salir")}
            };

            switch (infoPostulacion.Estado)
            {

                case Estados.Etiquetas:
                    respuesta.Texto = "Por favor, indícanos detalladamente lo qué necesitas, dentro de un mensaje.";
                    infoPostulacion.Estado = Estados.Categorias;
                    return true;

                case Estados.Categorias:
                    foreach (Material material in Sistema.Instancia.Materiales)
                    {
                        if (mensaje.Texto.Contains(material.Nombre))
                        {
                            infoPostulacion.Categorias.AddRange(material.Categorias);
                            continue;
                        }
                        foreach (string categoria in material.Categorias)
                        {
                            if (mensaje.Texto.Contains(categoria))
                            {
                                infoPostulacion.Categorias.Add(categoria);
                            }
                        }
                    }
                    List<string> etiquetas = mensaje.Texto.Split(' ').ToList();
                    infoPostulacion.Etiquetas = etiquetas;
                    infoPostulacion.IndiceActual = 0;
                    respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeCategorias, infoPostulacion.IndiceActual, FILAS_CATEGORIAS, COLUMNAS_CATEGORIAS, tecladoFijoCategorias);
                    infoPostulacion.Estado = Estados.SeleccionandoCategorias;
                    respuesta.Texto = "Bien, ahora necesitamos que selecciones las categorías que creas adecuadas para los materiales que estás buscando.\n\nSelecciona \"Listo\" cuando quieras continuar la búsqueda, o \"Cancelar\" para detenerla.";
                    return true;

                case Estados.SeleccionandoCategorias:
                    switch (mensaje.Texto)
                    {
                        case "Siguiente":
                            if (!(botonesDeCategorias.Count <= infoPostulacion.IndiceActual + COLUMNAS_CATEGORIAS * FILAS_CATEGORIAS))
                            {
                                infoPostulacion.IndiceActual += COLUMNAS_CATEGORIAS * FILAS_CATEGORIAS;
                            }
                            respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeCategorias, infoPostulacion.IndiceActual, FILAS_CATEGORIAS, COLUMNAS_CATEGORIAS, tecladoFijoCategorias);
                            respuesta.Texto = String.Empty;
                            return true;

                        case "Anterior":
                            if (infoPostulacion.IndiceActual - COLUMNAS_CATEGORIAS * FILAS_CATEGORIAS < 0)
                            {
                                infoPostulacion.IndiceActual = 0;
                            }
                            else
                            {
                                infoPostulacion.IndiceActual -= COLUMNAS_CATEGORIAS * FILAS_CATEGORIAS;
                            }
                            respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeCategorias, infoPostulacion.IndiceActual, FILAS_CATEGORIAS, COLUMNAS_CATEGORIAS, tecladoFijoCategorias);
                            respuesta.Texto = String.Empty;
                            return true;

                        case "Listo":
                            infoPostulacion.IndiceActual = 0;
                            infoPostulacion.OfertasEncontradas = Buscador.Instancia.BuscarOfertas(Sistema.Instancia,
                                Sistema.Instancia.ObtenerEmprendedorPorId(mensaje.IdUsuario), infoPostulacion.Categorias,
                                infoPostulacion.Etiquetas);
                            StringBuilder stringBuilder = new StringBuilder();
                            stringBuilder.Append("Encontramos estas ofertas para ti:\n\n");
                            for (int i = infoPostulacion.IndiceActual; i < (infoPostulacion.IndiceActual + COLUMNAS_OFERTAS * FILAS_OFERTAS); i++)
                            {
                                try
                                {
                                    Oferta oferta = infoPostulacion.OfertasEncontradas[i];
                                    titulosOfertas.Append(oferta.Titulo);
                                    stringBuilder.Append($"{oferta.RedactarResumen()}\n\n");
                                }
                                catch (ArgumentOutOfRangeException e)
                                {
                                    System.Console.WriteLine($"[EXCEPCIÓN] {e.ToString()}");
                                    break;
                                }
                            }

                            botonesDeOfertas = TelegramBot.Instancia.ObtenerBotones(titulosOfertas);
                            infoPostulacion.Estado = Estados.SeleccionandoOferta;
                            respuesta.Texto = stringBuilder.ToString();
                            respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeOfertas, infoPostulacion.IndiceActual, FILAS_OFERTAS, COLUMNAS_OFERTAS, tecladoFijoOfertas);
                            return true;

                        case "Cancelar":
                            this.Cancelar(sesion);
                            return false;
                    }
                    if (!infoPostulacion.CategoriasDisponibles.Contains(mensaje.Texto))
                    {
                        respuesta.Texto = $"Lo sentimos, la categoría _\"{mensaje.Texto}\"_ todavía no está disponible.\n\nPor favor, selecciona una de las categorías listadas, _\"Listo\"_ cuando quieras continuar la búsqueda, o _\"Cancelar\"_ para detenerla.";
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
                    respuesta.Texto = $"Hemos añadido _\"{mensaje.Texto}\"_ a las categorías que utilizaremos para buscar la oferta.\n\nSelecciona _\"Listo\"_ cuando quieras continuar la búsqueda, o _\"Cancelar\"_ para detenerla.";
                    respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeCategorias, infoPostulacion.IndiceActual, FILAS_CATEGORIAS, COLUMNAS_CATEGORIAS, tecladoFijoCategorias);
                    respuesta.EditarMensaje = true;
                    return true;

                case Estados.SeleccionandoOferta:
                    botonesDeOfertas = TelegramBot.Instancia.ObtenerBotones(titulosOfertas);
                    switch (mensaje.Texto)
                    {
                        case "Siguiente":
                            if (botonesDeOfertas.Count <= infoPostulacion.IndiceActual + COLUMNAS_OFERTAS * FILAS_OFERTAS)
                            {
                                infoPostulacion.IndiceActual = botonesDeOfertas.Count - COLUMNAS_OFERTAS * FILAS_OFERTAS;
                            }
                            else
                            {
                                infoPostulacion.IndiceActual += COLUMNAS_OFERTAS * FILAS_OFERTAS;
                            }
                            respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeOfertas, infoPostulacion.IndiceActual, FILAS_OFERTAS, COLUMNAS_OFERTAS, tecladoFijoOfertas);
                            respuesta.Texto = String.Empty;
                            return true;

                        case "Anterior":
                            if (infoPostulacion.IndiceActual - COLUMNAS_OFERTAS * FILAS_OFERTAS < 0)
                            {
                                infoPostulacion.IndiceActual = 0;
                            }
                            else
                            {
                                infoPostulacion.IndiceActual -= COLUMNAS_OFERTAS * FILAS_OFERTAS;
                            }
                            respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeOfertas, infoPostulacion.IndiceActual, FILAS_OFERTAS, COLUMNAS_OFERTAS, tecladoFijoOfertas);
                            respuesta.Texto = String.Empty;
                            return true;

                        case "Cancelar":
                            this.Cancelar(sesion);
                            return false;
                    }

                    Oferta ofertaSeleccionada = infoPostulacion.OfertasEncontradas.Find(of => of.Titulo.Equals(mensaje.Texto));
                    respuesta.Texto = ofertaSeleccionada.Redactar();
                    InlineKeyboardButton botonPostular = new InlineKeyboardButton();
                    botonPostular.Text = "Postularme a esta oferta";
                    botonPostular.CallbackData = "Postularse";
                    InlineKeyboardButton botonVolver = new InlineKeyboardButton();
                    botonVolver.Text = "Ver más ofertas";
                    botonVolver.CallbackData = "Volver";
                    respuesta.TecladoTelegram = new InlineKeyboardMarkup(
                        new[] {
                            new [] { botonPostular },
                            new [] { botonVolver}
                        }
                    );
                    return true;

                case Estados.Detalle:

                    return true;

                case Estados.Postulando:

                    return true;
            }
            infoPostulacion = new InformacionPostulacion();
            return false;
        }

        /// <summary>
        /// La clase procesa el mensaje y retorna true o no lo procesa y retorna false.
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        /// <param name="callback">El callback a procesar.</param>
        /// <param name="respuesta">La respuesta al mensaje procesado.</param>
        /// <returns>true si el mensaje fue procesado; false en caso contrario</returns>
        protected override bool ResolverInterno(Sesion sesion, ICallBack callback, out RespuestaTelegram respuesta)
        {
            respuesta = new RespuestaTelegram(string.Empty);
            if (!this.PuedeResolver(sesion))
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

            if (infoPostulacion.CategoriasDisponibles == null)
            {
                infoPostulacion.CategoriasDisponibles = new List<string>();
                foreach (Material material in Sistema.Instancia.Materiales)
                {
                    foreach (string categoria in material.Categorias)
                    {
                        if (!infoPostulacion.CategoriasDisponibles.Contains(categoria))
                        {
                            infoPostulacion.CategoriasDisponibles.Add(categoria);
                        }
                    }
                }
            }
            List<InlineKeyboardButton> botonesDeCategorias = TelegramBot.Instancia.ObtenerBotones(infoPostulacion.CategoriasDisponibles);
            List<InlineKeyboardButton> botonesDeOfertas = new List<InlineKeyboardButton>();
            List<List<InlineKeyboardButton>> tecladoFijoCategorias = new List<List<InlineKeyboardButton>>() {
                new List<InlineKeyboardButton>() {TelegramBot.Instancia.BotonAnterior, TelegramBot.Instancia.BotonSiguiente},
                new List<InlineKeyboardButton>() {TelegramBot.Instancia.BotonCancelar, TelegramBot.Instancia.BotonListo}
            };
            List<List<InlineKeyboardButton>> tecladoFijoOfertas = new List<List<InlineKeyboardButton>>() {
                new List<InlineKeyboardButton>() {TelegramBot.Instancia.BotonAnterior, TelegramBot.Instancia.BotonSiguiente},
                new List<InlineKeyboardButton>() {InlineKeyboardButton.WithCallbackData("Salir")}
            };

            switch (infoPostulacion.Estado)
            {
                case Estados.SeleccionandoCategorias:
                    // Procesando paginado =========================================================================================================
                    switch (callback.Texto)
                    {
                        case "Siguiente":
                            if (!(botonesDeCategorias.Count <= infoPostulacion.IndiceActual + COLUMNAS_CATEGORIAS * FILAS_CATEGORIAS))
                            {
                                infoPostulacion.IndiceActual += COLUMNAS_CATEGORIAS * FILAS_CATEGORIAS;
                            }
                            respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeCategorias, infoPostulacion.IndiceActual, FILAS_CATEGORIAS, COLUMNAS_CATEGORIAS, tecladoFijoCategorias);
                            respuesta.Texto = String.Empty;
                            return true;

                        case "Anterior":
                            if (infoPostulacion.IndiceActual - COLUMNAS_CATEGORIAS * FILAS_CATEGORIAS < 0)
                            {
                                infoPostulacion.IndiceActual = 0;
                            }
                            else
                            {
                                infoPostulacion.IndiceActual -= COLUMNAS_CATEGORIAS * FILAS_CATEGORIAS;
                            }
                            respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeCategorias, infoPostulacion.IndiceActual, FILAS_CATEGORIAS, COLUMNAS_CATEGORIAS, tecladoFijoCategorias);
                            respuesta.Texto = String.Empty;
                            return true;

                        case "Listo":
                            infoPostulacion.IndiceActual = 0;
                            infoPostulacion.OfertasEncontradas = Buscador.Instancia.BuscarOfertas(Sistema.Instancia,
                                Sistema.Instancia.ObtenerEmprendedorPorId(callback.IdUsuario), infoPostulacion.Categorias,
                                infoPostulacion.Etiquetas);
                            StringBuilder stringBuilder = new StringBuilder();
                            stringBuilder.Append("Encontramos estas ofertas para ti:\n\n");
                            for (int i = infoPostulacion.IndiceActual; i < (infoPostulacion.IndiceActual + COLUMNAS_OFERTAS * FILAS_OFERTAS); i++)
                            {
                                try
                                {
                                    Oferta oferta = infoPostulacion.OfertasEncontradas[i];
                                    titulosOfertas.Append(oferta.Titulo);
                                    stringBuilder.Append($"{oferta.RedactarResumen()}\n\n");
                                }
                                catch (ArgumentOutOfRangeException e)
                                {
                                    System.Console.WriteLine($"[EXCEPCIÓN] {e.ToString()}");
                                    break;
                                }
                            }

                            botonesDeOfertas = TelegramBot.Instancia.ObtenerBotones(titulosOfertas);
                            infoPostulacion.Estado = Estados.SeleccionandoOferta;
                            respuesta.Texto = stringBuilder.ToString();
                            respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeOfertas, infoPostulacion.IndiceActual, FILAS_OFERTAS, COLUMNAS_OFERTAS, tecladoFijoOfertas);
                            return true;

                        case "Cancelar":
                            this.Cancelar(sesion);
                            return false;
                    }
                    // Procesando paginado =========================================================================================================


                    if (!infoPostulacion.CategoriasDisponibles.Contains(callback.Texto))
                    {
                        respuesta.Texto = $"Lo sentimos, la categoría _\"{callback.Texto}\"_ todavía no está disponible.\n\nPor favor, selecciona una de las categorías listadas, _\"Listo\"_ cuando quieras continuar la búsqueda, o _\"Cancelar\"_ para detenerla.";
                        respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeCategorias, infoPostulacion.IndiceActual, FILAS_CATEGORIAS, COLUMNAS_CATEGORIAS, tecladoFijoCategorias);
                        respuesta.EditarMensaje = true;
                        return true;
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
                    respuesta.Texto = $"Hemos añadido _\"{callback.Texto}\"_ a las categorías que utilizaremos para buscar la oferta.\n\nSelecciona _\"Listo\"_ cuando quieras continuar la búsqueda, o _\"Cancelar\"_ para detenerla.";
                    respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeCategorias, infoPostulacion.IndiceActual, FILAS_CATEGORIAS, COLUMNAS_CATEGORIAS, tecladoFijoCategorias);
                    respuesta.EditarMensaje = true;
                    return true;

                case Estados.SeleccionandoOferta:
                    botonesDeOfertas = TelegramBot.Instancia.ObtenerBotones(titulosOfertas);
                    // Procesando paginado =========================================================================================================
                    switch (callback.Texto)
                    {
                        case "Siguiente":
                            if (botonesDeOfertas.Count <= infoPostulacion.IndiceActual + COLUMNAS_OFERTAS * FILAS_OFERTAS)
                            {
                                infoPostulacion.IndiceActual = botonesDeOfertas.Count - COLUMNAS_OFERTAS * FILAS_OFERTAS;
                            }
                            else
                            {
                                infoPostulacion.IndiceActual += COLUMNAS_OFERTAS * FILAS_OFERTAS;
                            }
                            respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeOfertas, infoPostulacion.IndiceActual, FILAS_OFERTAS, COLUMNAS_OFERTAS, tecladoFijoOfertas);
                            respuesta.Texto = String.Empty;
                            return true;

                        case "Anterior":
                            if (infoPostulacion.IndiceActual - COLUMNAS_OFERTAS * FILAS_OFERTAS < 0)
                            {
                                infoPostulacion.IndiceActual = 0;
                            }
                            else
                            {
                                infoPostulacion.IndiceActual -= COLUMNAS_OFERTAS * FILAS_OFERTAS;
                            }
                            respuesta.TecladoTelegram = TelegramBot.Instancia.ObtenerKeyboard(botonesDeOfertas, infoPostulacion.IndiceActual, FILAS_OFERTAS, COLUMNAS_OFERTAS, tecladoFijoOfertas);
                            respuesta.Texto = String.Empty;
                            return true;

                        case "Cancelar":
                            this.Cancelar(sesion);
                            return false;
                    }
                    // Procesando paginado =========================================================================================================


                    Oferta ofertaSeleccionada = infoPostulacion.OfertasEncontradas.Find(of => of.Titulo.Equals(callback.Texto));
                    respuesta.Texto = ofertaSeleccionada.Redactar();
                    InlineKeyboardButton botonPostular = new InlineKeyboardButton();
                    botonPostular.Text = "Postularme a esta oferta";
                    botonPostular.CallbackData = "Postularse";
                    InlineKeyboardButton botonVolver = new InlineKeyboardButton();
                    botonVolver.Text = "Ver más ofertas";
                    botonVolver.CallbackData = "Volver";
                    respuesta.TecladoTelegram = new InlineKeyboardMarkup(
                        new[] {
                            new [] { botonPostular },
                            new [] { botonVolver}
                        }
                    );
                    return true;

                case Estados.Detalle:

                    return true;

                case Estados.Postulando:

                    return true;
            }
            infoPostulacion = new InformacionPostulacion();
            return false;
        }

        /// <summary>
        /// Retorna este "handler" al estado inicial. En los "handler" sin estado no hace nada. Los "handlers" que
        /// procesan varios mensajes cambiando de estado entre mensajes deben sobreescribir este método para volver al
        /// estado inicial.
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        public override void Cancelar(Sesion sesion)
        {
            this.CancelarInterno(sesion);
            if (this.Siguiente != null)
            {
                this.Siguiente.Cancelar(sesion);
            }
        }

        /// <summary>
        /// Este método puede ser sobreescrito en las clases sucesores que procesan varios mensajes cambiando de estado
        /// entre mensajes deben sobreescribir este método para volver al estado inicial. En la clase base no hace nada.
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        protected override void CancelarInterno(Sesion sesion)
        {
            this.Busquedas.Remove(sesion.IdUsuario);
        }

        /// <summary>
        /// Determina si este "handler" puede procesar el mensaje. En la clase base se utiliza procesado de lenguaje natural
        /// para comprobar que la intención identificada corresponda a la del "handler". Las clases sucesores pueden
        /// sobreescribir este método para proveer otro mecanismo para determina si procesan o no un mensaje.
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        /// <returns>true si el mensaje puede ser pocesado; false en caso contrario.</returns>
        protected override bool PuedeResolver(Sesion sesion)
        {
            try
            {
                Sistema.Instancia.ObtenerEmprendedorPorId(sesion.IdUsuario);
            }
            catch (KeyNotFoundException e)
            {
                System.Console.WriteLine($"[EXCEPCIÓN] {e.ToString()}");
                return false;
            }
            return sesion.PLN.UltimaIntencion.Nombre.Equals(this.Intencion) ||
                (this.Busquedas.ContainsKey(sesion.IdUsuario) && sesion.PLN.UltimaIntencion.Nombre.Equals("Default Fallback Intent")) ||
                (sesion.PLN.UltimaIntencion.ConfianzaDeteccion < 60);
        }

        /// <summary>
        /// Representación de los posibles estados de una postulación a oferta.
        /// </summary>
        private enum Estados
        {
            Etiquetas,
            Categorias,
            SeleccionandoCategorias,
            SeleccionandoOferta,
            Detalle,
            Postulando
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
            /// Lista de categorías disponibles para elegir.
            /// </summary>
            public List<string> CategoriasDisponibles { get; set; }
            /// <summary>
            /// Estado de la búsqueda de ofertas de un usuario.
            /// </summary>
            public Estados Estado { get; set; } = Estados.Etiquetas;
            /// <summary>
            /// Oferta seleccionada por un usuario entre la lista de ofertas encontradas.
            /// </summary>
            public Oferta ofertaSeleccionada { get; set; }
            /// <summary>
            /// Indice actual dentro de cualquier lista paginada.
            /// </summary>
            public int IndiceActual { get; set; } = 0;
        }
    }
}