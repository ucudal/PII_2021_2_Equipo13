using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PII_E13.ClassLibrary;

namespace PII_E13.HandlerLibrary
{
    /// <summary>
    /// Handler encargado de procesar la postulación de ofertas, desde el proceso de búsqueda hasta la selección final.
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
        protected override bool ResolverInterno(Sesion sesion, IMensaje mensaje, out IRespuesta respuesta)
        {
            respuesta = new Respuesta(mensaje);
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
            List<IBoton> botonesDeCategorias = new List<IBoton>();
            foreach (string opcion in infoPostulacion.CategoriasDisponibles)
            {
                botonesDeCategorias.Add(new Boton(opcion));
            }
            List<IBoton> botonesDeOfertas = new List<IBoton>();

            List<List<IBoton>> tecladoFijoCategorias = new List<List<IBoton>>()
            {
                new List<IBoton>() {TelegramBot.Instancia.BotonAnterior, TelegramBot.Instancia.BotonSiguiente},
                new List<IBoton>() {TelegramBot.Instancia.BotonCancelar, TelegramBot.Instancia.BotonListo}
            };

            List<List<IBoton>> tecladoFijoOfertas = new List<List<IBoton>>() {
                new List<IBoton>() {TelegramBot.Instancia.BotonAnterior, TelegramBot.Instancia.BotonSiguiente},
                new List<IBoton>() {new Boton("Salir")}
            };

            StringBuilder stringBuilder;

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
                    respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeCategorias, infoPostulacion.IndiceActual, FILAS_CATEGORIAS, COLUMNAS_CATEGORIAS, tecladoFijoCategorias);
                    infoPostulacion.Estado = Estados.SeleccionandoCategorias;
                    respuesta.Texto = "Bien, ahora necesitamos que selecciones las categorías que creas adecuadas para los materiales que estás buscando.\n\nSelecciona _\"Listo\"_ cuando quieras continuar la búsqueda, o _\"Cancelar\"_ para detenerla.";
                    return true;

                case Estados.SeleccionandoCategorias:
                    // Procesando paginado =========================================================================================================
                    switch (mensaje.Texto)
                    {
                        case "Siguiente":
                            if (!(botonesDeCategorias.Count <= infoPostulacion.IndiceActual + COLUMNAS_CATEGORIAS * FILAS_CATEGORIAS))
                            {
                                infoPostulacion.IndiceActual += COLUMNAS_CATEGORIAS * FILAS_CATEGORIAS;
                            }
                            respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeCategorias, infoPostulacion.IndiceActual, FILAS_CATEGORIAS, COLUMNAS_CATEGORIAS, tecladoFijoCategorias);
                            respuesta.Texto = String.Empty;
                            respuesta.EditarMensaje = true;
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
                            respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeCategorias, infoPostulacion.IndiceActual, FILAS_CATEGORIAS, COLUMNAS_CATEGORIAS, tecladoFijoCategorias);
                            respuesta.Texto = String.Empty;
                            respuesta.EditarMensaje = true;
                            return true;

                        case "Listo":
                            infoPostulacion.IndiceActual = 0;
                            infoPostulacion.OfertasEncontradas = Buscador.Instancia.BuscarOfertas(Sistema.Instancia,
                                Sistema.Instancia.ObtenerEmprendedorPorId(mensaje.IdUsuario), infoPostulacion.Categorias,
                                infoPostulacion.Etiquetas);

                            if (infoPostulacion.OfertasEncontradas.Count < 1)
                            {
                                respuesta.Texto = "Lo sentimos, parece que no tenemos ofertas disponibles de momento. Intenta buscar de nuevo más tarde.";
                                respuesta.Botones = new List<List<IBoton>>()
                                    {
                                        new List<IBoton>() {new Boton("Volver al menú")}
                                    };
                                this.Cancelar(sesion);
                                return true;
                            }
                            if (infoPostulacion.OfertasEncontradas.Count <= 3)
                            {
                                tecladoFijoOfertas = new List<List<IBoton>>()
                                {
                                    new List<IBoton>() {new Boton("Salir")}
                                };
                            }

                            stringBuilder = new StringBuilder();
                            stringBuilder.Append("Encontramos estas ofertas para ti:\n\n");
                            for (int i = infoPostulacion.IndiceActual; i < (infoPostulacion.IndiceActual + COLUMNAS_OFERTAS * FILAS_OFERTAS); i++)
                            {
                                try
                                {
                                    Oferta oferta = infoPostulacion.OfertasEncontradas[i];
                                    titulosOfertas.Add(oferta.Titulo);
                                    stringBuilder.Append($"{oferta.RedactarResumen()}\n\n");
                                }
                                catch (ArgumentOutOfRangeException e)
                                {
                                    break;
                                }
                            }

                            foreach (string tituloOferta in titulosOfertas)
                            {
                                botonesDeOfertas.Add(new Boton(tituloOferta));
                            }
                            respuesta.Texto = stringBuilder.ToString();
                            respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeOfertas, infoPostulacion.IndiceActual, FILAS_OFERTAS, COLUMNAS_OFERTAS, tecladoFijoOfertas);
                            infoPostulacion.Estado = Estados.SeleccionandoOferta;
                            return true;

                        case "Cancelar":
                            this.Cancelar(sesion);
                            return false;
                    }
                    // Procesando paginado =========================================================================================================

                    if (!infoPostulacion.CategoriasDisponibles.Contains(mensaje.Texto))
                    {
                        respuesta.Texto = $"Lo sentimos, la categoría _\"{mensaje.Texto}\"_ todavía no está disponible.\n\nPor favor, selecciona una de las categorías listadas, _\"Listo\"_ cuando quieras continuar la búsqueda, o _\"Cancelar\"_ para detenerla.";
                        respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeCategorias, infoPostulacion.IndiceActual, FILAS_CATEGORIAS, COLUMNAS_CATEGORIAS, tecladoFijoCategorias);
                        respuesta.EditarMensaje = true;
                        return true;
                    }

                    botonesDeCategorias.Remove(botonesDeCategorias.First(b => b.Texto == mensaje.Texto));
                    if (infoPostulacion.IndiceActual >= botonesDeCategorias.Count)
                    {
                        infoPostulacion.IndiceActual = botonesDeCategorias.Count - FILAS_CATEGORIAS * COLUMNAS_CATEGORIAS;
                    }
                    infoPostulacion.CategoriasDisponibles.Remove(mensaje.Texto);
                    if (infoPostulacion.CategoriasDisponibles.Count <= (FILAS_CATEGORIAS * COLUMNAS_CATEGORIAS))
                    {
                        tecladoFijoCategorias = new List<List<IBoton>>()
                        {
                            new List<IBoton>() { TelegramBot.Instancia.BotonCancelar, TelegramBot.Instancia.BotonListo }
                        };
                    }
                    infoPostulacion.Categorias.Add(mensaje.Texto);
                    respuesta.Texto = $"Hemos añadido _\"{mensaje.Texto}\"_ a las categorías que utilizaremos para buscar la oferta.\n\nSelecciona _\"Listo\"_ cuando quieras continuar la búsqueda, o _\"Cancelar\"_ para detenerla.";
                    respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeCategorias, infoPostulacion.IndiceActual, FILAS_CATEGORIAS, COLUMNAS_CATEGORIAS, tecladoFijoCategorias);
                    respuesta.EditarMensaje = true;
                    return true;

                case Estados.SeleccionandoOferta:
                    foreach (string tituloOferta in titulosOfertas)
                    {
                        botonesDeOfertas.Add(new Boton(tituloOferta));
                    }

                    // Procesando paginado =========================================================================================================
                    switch (mensaje.Texto)
                    {
                        case "Siguiente":
                            if (botonesDeOfertas.Count <= infoPostulacion.IndiceActual + COLUMNAS_OFERTAS * FILAS_OFERTAS)
                            {
                                infoPostulacion.IndiceActual = (botonesDeOfertas.Count - COLUMNAS_OFERTAS * FILAS_OFERTAS) > 0 ? (botonesDeOfertas.Count - COLUMNAS_OFERTAS * FILAS_OFERTAS) : 0;
                            }
                            else
                            {
                                infoPostulacion.IndiceActual += COLUMNAS_OFERTAS * FILAS_OFERTAS;
                            }

                            stringBuilder = new StringBuilder();
                            stringBuilder.Append("Encontramos estas ofertas para ti:\n\n");
                            for (int i = infoPostulacion.IndiceActual; i < (infoPostulacion.IndiceActual + COLUMNAS_OFERTAS * FILAS_OFERTAS); i++)
                            {
                                try
                                {
                                    Oferta oferta = infoPostulacion.OfertasEncontradas[i];
                                    titulosOfertas.Add(oferta.Titulo);
                                    stringBuilder.Append($"{oferta.RedactarResumen()}\n\n");
                                }
                                catch (ArgumentOutOfRangeException e)
                                {
                                    break;
                                }
                            }

                            respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeOfertas, infoPostulacion.IndiceActual, FILAS_OFERTAS, COLUMNAS_OFERTAS, tecladoFijoOfertas);
                            respuesta.Texto = stringBuilder.ToString();
                            respuesta.EditarMensaje = true;
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

                            stringBuilder = new StringBuilder();
                            stringBuilder.Append("Encontramos estas ofertas para ti:\n\n");
                            for (int i = infoPostulacion.IndiceActual; i < (infoPostulacion.IndiceActual + COLUMNAS_OFERTAS * FILAS_OFERTAS); i++)
                            {
                                try
                                {
                                    Oferta oferta = infoPostulacion.OfertasEncontradas[i];
                                    titulosOfertas.Add(oferta.Titulo);
                                    stringBuilder.Append($"{oferta.RedactarResumen()}\n\n");
                                }
                                catch (ArgumentOutOfRangeException e)
                                {
                                    break;
                                }
                            }

                            respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeOfertas, infoPostulacion.IndiceActual, FILAS_OFERTAS, COLUMNAS_OFERTAS, tecladoFijoOfertas);
                            respuesta.Texto = stringBuilder.ToString();
                            respuesta.EditarMensaje = true;
                            return true;

                        case "Salir":
                            this.Cancelar(sesion);
                            return false;
                    }

                    Oferta ofertaSeleccionada = infoPostulacion.OfertasEncontradas.Find(of => of.Titulo.Equals(mensaje.Texto));
                    respuesta.Texto = ofertaSeleccionada.Redactar();
                    IBoton botonPostular = new Boton("Postularme a esta oferta", ofertaSeleccionada.Titulo);
                    IBoton botonVolver = new Boton("Ver más ofertas", "Volver");
                    respuesta.Botones = new List<List<IBoton>>()
                    {
                        new List<IBoton>() { botonPostular },
                        new List<IBoton>() { botonVolver}
                    };
                    infoPostulacion.Estado = Estados.Detalle;
                    return true;

                case Estados.Detalle:
                    if (mensaje.Texto.Equals("Volver"))
                    {
                        stringBuilder = new StringBuilder();
                        stringBuilder.Append("Encontramos estas ofertas para ti:\n\n");
                        for (int i = infoPostulacion.IndiceActual; i < (infoPostulacion.IndiceActual + COLUMNAS_OFERTAS * FILAS_OFERTAS); i++)
                        {
                            try
                            {
                                Oferta oferta = infoPostulacion.OfertasEncontradas[i];
                                titulosOfertas.Add(oferta.Titulo.Replace("*", ""));
                                stringBuilder.Append($"{oferta.RedactarResumen()}\n\n");
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                break;
                            }
                        }

                        foreach (string tituloOferta in titulosOfertas)
                        {
                            botonesDeOfertas.Add(new Boton(tituloOferta));
                        }

                        if (infoPostulacion.OfertasEncontradas.Count < 1)
                        {
                            respuesta.Texto = "Lo sentimos, parece que no tenemos ofertas disponibles de momento. Intenta buscar de nuevo más tarde.";
                            respuesta.Botones = new List<List<IBoton>>()
                            {
                                new List<IBoton>() {new Boton("Volver al menú")}
                            };
                            this.Cancelar(sesion);
                            return true;
                        }
                        if (infoPostulacion.OfertasEncontradas.Count <= 3)
                        {
                            tecladoFijoOfertas = new List<List<IBoton>>()
                                {
                                    new List<IBoton>() {new Boton("Salir")}
                                };
                        }

                        infoPostulacion.Estado = Estados.SeleccionandoOferta;
                        respuesta.Texto = stringBuilder.ToString();
                        respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeOfertas, infoPostulacion.IndiceActual, FILAS_OFERTAS, COLUMNAS_OFERTAS, tecladoFijoOfertas);
                        return true;
                    }
                    else if (infoPostulacion.OfertasEncontradas.Any(of => of.Titulo.Equals(mensaje.Texto)))
                    {
                        Oferta ofertaPostulada = infoPostulacion.OfertasEncontradas.Find(of => of.Titulo.Equals(mensaje.Texto));
                        Sistema.Instancia.ObtenerEmprendedorPorId(mensaje.IdUsuario).PostularseAOferta(ofertaPostulada);
                        infoPostulacion.OfertasEncontradas.Remove(ofertaPostulada);

                        IBoton botonVerMas = new Boton("Ver más ofertas", "Volver");
                        IBoton botonMenu = new Boton("Volver al menú");
                        List<List<IBoton>> tecladoMenu = new List<List<IBoton>>() {
                                new List<IBoton>() { botonMenu },
                                new List<IBoton>() { botonVerMas}
                            };
                        respuesta.Botones = tecladoMenu;
                        System.Console.WriteLine($"[NUEVA POSTULACIÓN] - ID USUARIO: {mensaje.IdUsuario} - ID OFERTA: {ofertaPostulada.Id}");
                        respuesta.Texto = $"Felicidades, te has postulado a la oferta _\"{ofertaPostulada.Titulo}\"_ por _{Sistema.Instancia.ObtenerEmpresaPorId(ofertaPostulada.Empresa).Nombre}_ existosamente.";
                        infoPostulacion.Estado = Estados.Postulado;
                        return true;
                    }
                    else
                    {
                        this.Cancelar(sesion);
                        return false;
                    }

                case Estados.Postulado:
                    if (mensaje.Texto.Equals("Volver"))
                    {
                        stringBuilder = new StringBuilder();
                        stringBuilder.Append("Encontramos estas ofertas para ti:\n\n");
                        for (int i = infoPostulacion.IndiceActual; i < (infoPostulacion.IndiceActual + COLUMNAS_OFERTAS * FILAS_OFERTAS); i++)
                        {
                            try
                            {
                                Oferta oferta = infoPostulacion.OfertasEncontradas[i];
                                titulosOfertas.Add(oferta.Titulo.Replace("*", ""));
                                stringBuilder.Append($"{oferta.RedactarResumen()}\n\n");
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                break;
                            }
                        }

                        foreach (string tituloOferta in titulosOfertas)
                        {
                            botonesDeOfertas.Add(new Boton(tituloOferta));
                        }

                        if (infoPostulacion.OfertasEncontradas.Count < 1)
                        {
                            respuesta.Texto = "Lo sentimos, parece que no tenemos ofertas disponibles de momento. Intenta buscar de nuevo más tarde.";
                            respuesta.Botones = new List<List<IBoton>>()
                            {
                                new List<IBoton>() {new Boton("Volver al menú")}
                            };
                            this.Cancelar(sesion);
                            return true;
                        }
                        if (infoPostulacion.OfertasEncontradas.Count <= 3)
                        {
                            tecladoFijoOfertas = new List<List<IBoton>>()
                                {
                                    new List<IBoton>() {new Boton("Salir")}
                                };
                        }

                        infoPostulacion.Estado = Estados.SeleccionandoOferta;
                        respuesta.Texto = stringBuilder.ToString();
                        respuesta.Botones = this.ObtenerMatrizDeBotones(botonesDeOfertas, infoPostulacion.IndiceActual, FILAS_OFERTAS, COLUMNAS_OFERTAS, tecladoFijoOfertas);
                        return true;
                    }
                    this.Cancelar(sesion);
                    return false;
            }
            infoPostulacion = new InformacionPostulacion();
            return false;
        }

        /// <summary>
        /// Retorna este "handler" al estado inicial.
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
        /// Retorna este "handler" al estado inicial.
        /// </summary>
        /// <param name="sesion">La sesión en la cual se envió el mensaje.</param>
        protected override void CancelarInterno(Sesion sesion)
        {
            try
            {
                this.Busquedas.Remove(sesion.IdUsuario);
            }
            catch (Exception e) { }
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
                return false;
            }

            return sesion.PLN.UltimaIntencion.Nombre.Equals(this.Intencion) ||
                (
                    this.Busquedas.ContainsKey(sesion.IdUsuario) &&
                    (sesion.PLN.UltimaIntencion.Nombre.Equals("Default") || (sesion.PLN.UltimaIntencion.ConfianzaDeteccion < 80))
                );
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
            Postulado
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