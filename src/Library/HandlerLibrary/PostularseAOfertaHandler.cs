using System;
using System.Collections.Generic;
using System.Linq;
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
        public PostularseAOfertaHandler(HandlerBase siguiente) : base(siguiente)
        {
            this.Busquedas = new Dictionary<string, InformacionPostulacion>();
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
            if(!this.PuedeResolver(mensaje)){
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

            List<string> categorias = new List<string>();
            foreach(Material material in Sistema.Instancia.Materiales){
                categorias.AddRange(material.Categorias);
            }
            List<KeyboardButton> botonesDeCategorias = this.ObtenerBotones(categorias);

            switch(infoPostulacion.Estado){
                
                case Estados.Etiquetas:
                    respuesta.Texto = "Por favor, indícanos detalladamente lo qué necesitas, dentro de un mensaje.";
                    infoPostulacion.Estado = Estados.Categorias;
                    return true;

                case Estados.Categorias:
                    foreach(Material material in Sistema.Instancia.Materiales)
                    {
                        if(mensaje.Texto.Contains(material.Nombre)){
                            infoPostulacion.Categorias.AddRange(material.Categorias);
                            continue;
                        }
                        foreach(string categoria in material.Categorias){
                            if(mensaje.Texto.Contains(categoria)){
                                infoPostulacion.Categorias.Add(categoria);
                            }
                        }
                    }
                    List<string> etiquetas = mensaje.Texto.Split(' ').ToList();
                    infoPostulacion.Etiquetas = etiquetas;
                    infoPostulacion.IndiceEnCategorias = 0;
                    respuesta.TecladoTelegram = this.ObtenerKeyboard(botonesDeCategorias, infoPostulacion.IndiceEnCategorias);
                    infoPostulacion.Estado = Estados.SeleccionandoCategorias;
                    respuesta.Texto = "Bien, ahora necesitamos que selecciones las categorías que creas adecuadas para los materiales que estás buscando.\n\nSelecciona \"Listo\" cuando quieras continuar la búsqueda, o \"Cancelar\" para detenerla.";
                    return true;

                case Estados.SeleccionandoCategorias:
                    switch(mensaje.Texto){
                        case "Siguiente":
                            if(botonesDeCategorias.Count() <= infoPostulacion.IndiceEnCategorias + 8){
                                infoPostulacion.IndiceEnCategorias = botonesDeCategorias.Count() - 8;
                            } else {
                                infoPostulacion.IndiceEnCategorias += 8;
                            }
                            respuesta.TecladoTelegram = this.ObtenerKeyboard(botonesDeCategorias, infoPostulacion.IndiceEnCategorias);
                            respuesta.Texto = "Elige una categoría.\n\nSelecciona \"Listo\" cuando quieras continuar la búsqueda, o \"Cancelar\" para detenerla.";
                        return true;

                        case "Anterior":
                            if(infoPostulacion.IndiceEnCategorias - 8 < 0){
                                infoPostulacion.IndiceEnCategorias = 0;
                            } else {
                                infoPostulacion.IndiceEnCategorias -= 8;
                            }
                            respuesta.TecladoTelegram = this.ObtenerKeyboard(botonesDeCategorias, infoPostulacion.IndiceEnCategorias);
                            respuesta.Texto = "Elige una categoría.\n\nSelecciona \"Listo\" cuando quieras continuar la búsqueda, o \"Cancelar\" para detenerla.";
                        return true;

                        case "Listo":
                            infoPostulacion.OfertasEncontradas = Buscador.Instancia.BuscarOfertas(Sistema.Instancia, 
                                Sistema.Instancia.ObtenerEmprendedorPorId(mensaje.IdUsuario), infoPostulacion.Categorias, 
                                infoPostulacion.Etiquetas);
                            respuesta.Texto = "Encontramos estas ofertas para ti:\n\n";
                            
                            infoPostulacion.Estado = Estados.Visualizando;
                        return true;

                        case "Cancelar":
                            this.Cancelar();
                        return false;                            
                    }
                    infoPostulacion.Categorias.Add(mensaje.Texto);
                    respuesta.Texto = "Hemos añadido \"" + mensaje.Texto + "\" a las categorías que utilizaremos para buscar la oferta.\n\nSelecciona \"Listo\" cuando quieras continuar la búsqueda, o \"Cancelar\" para detenerla.";
                    return true;

                case Estados.Visualizando:

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
            try{
                Sistema.Instancia.ObtenerEmprendedorPorId(mensaje.IdUsuario);
            } catch(KeyNotFoundException e){
                return false;
            }
            // Cuando no hay palabras clave este método debe ser sobreescrito por las clases sucesoras y por lo tanto
            // este método no debería haberse invocado.
            if (this.Etiquetas == null || this.Etiquetas.Length == 0)
            {
                throw new InvalidOperationException("No hay palabras clave que puedan ser procesadas");
            }

            return this.Etiquetas.Any(s => mensaje.Texto.Equals(s, StringComparison.InvariantCultureIgnoreCase));
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
        /// Genera y retorna una lista de botones de Telegram a partir de una lista de opciones.
        /// </summary>
        /// <param name="opciones">La lista de opciones con las cuales crear los botones.</param>
        /// <returns>Una lista de <see cref="KeyboardButton"/> conteniendo botones con las opciones recibidas por parámetros.</returns>
        private List<KeyboardButton> ObtenerBotones(List<string> opciones)
        {
            List<KeyboardButton> botones = new List<KeyboardButton>();
            List<string> opcionesAuxiliar = new List<string>();
            foreach(string opcion in opciones)
            {
                if(!opcionesAuxiliar.Contains(opcion)){
                    botones.Add(new KeyboardButton(opcion));
                    opcionesAuxiliar.Add(opcion);
                }
            }
            return botones;
        }

        /// <summary>
        /// Genera y retorna un teclado de Telegram (<see cref="ReplyKeyboardMarkup"/>) con una lista de botones
        /// y un índice de la lista a partir del cual iniciar.
        /// </summary>
        /// <param name="botones">La lista de instancias de <see cref="KeyboardButton"/> con la cual se quiere generar un teclado.</param>
        /// <param name="indice">El índice de la lista desde el cual iniciar.</param>
        /// <returns></returns>
        private ReplyKeyboardMarkup ObtenerKeyboard(List<KeyboardButton> botones, int indice)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup();
            replyKeyboardMarkup = new(new []
            {
                botones.GetRange(indice, indice + 3).ToArray(),
                botones.GetRange(indice + 4, indice + 7).ToArray(),
                new KeyboardButton[] { "Anterior", "Siguiente" },
                new KeyboardButton[] { "Cancelar", "Listo" }
            })
            {
                ResizeKeyboard = true
            };
            return replyKeyboardMarkup;
        }
        /// <summary>
        /// Representación de los posibles estados de una postulación a oferta.
        /// </summary>
        private enum Estados{
            Etiquetas,
            Categorias,
            SeleccionandoCategorias,
            Visualizando,
            Detalle,
            Postulando
        }

        /// <summary>
        /// Clase privada contenedora de la información de una postulación a una oferta.
        /// </summary>
        private class InformacionPostulacion{
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
            public Estados Estado { get; set; } = Estados.Etiquetas;

            /// <summary>
            /// Oferta seleccionada por un usuario entre la lista de ofertas encontradas.
            /// </summary>
            public Oferta ofertaSeleccionada { get; set; }

            /// <summary>
            /// Indice actual dentro de la lista de categorías.
            /// </summary>
            public int IndiceEnCategorias { get; set; } = 0;
        }
    }
}