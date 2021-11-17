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
            List<KeyboardButton> botonesDeCategorias = this.ObtenerBotones(Sistema.Instancia.Materiales);

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
                    respuesta.Texto = "Bien, ahora necesitamos que selecciones las categorías que creas adecuadas para los materiales que estás buscando.";
                    infoPostulacion.Estado = Estados.SeleccionandoCategorias;
                    return true;

                case Estados.SeleccionandoCategorias:
                    respuesta.Texto = "Por favor, selecciona las categorías que creas adecuadas para los materiales que estás buscando.";
                    if(mensaje.Texto.Equals("Listo")){
                        infoPostulacion.Estado = Estados.Buscando;
                    }
                    return true;

                case Estados.Buscando:

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

        private List<KeyboardButton> ObtenerBotones(List<Material> Materiales)
        {
            List<KeyboardButton> botones = new List<KeyboardButton>();
            List<string> categoriasAuxiliar = new List<string>();
            foreach(Material material in Materiales)
            {
                foreach(string categoria in material.Categorias)
                {
                    if(!categoriasAuxiliar.Contains(categoria)){
                        botones.Add(new KeyboardButton(categoria));
                        categoriasAuxiliar.Add(categoria);
                    }
                }
            }
            return botones;
        }

        private ReplyKeyboardMarkup ObtenerKeyboard(List<KeyboardButton> botones, int indice)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup();
            if(botones.Count <= indice){
                throw new ArgumentOutOfRangeException("Índice fuera de límites");
            } else if(botones.Count <= indice + 7 && botones.Count > indice + 4){
                replyKeyboardMarkup = new(new []
                {
                    botones.GetRange(indice, indice + 3).ToArray(),
                    botones.GetRange(indice + 4, botones.Count() - indice).ToArray(),
                    new KeyboardButton[] { "Anterior" },
                    new KeyboardButton[] { "Cancelar", "Listo" }
                })
                {
                    ResizeKeyboard = true
                };
            return replyKeyboardMarkup;
            } else if (botones.Count <= indice + 4){
                replyKeyboardMarkup = new(new []
                {
                    botones.GetRange(indice, botones.Count() - indice).ToArray(),
                    new KeyboardButton[] { "Anterior" },
                    new KeyboardButton[] { "Cancelar", "Listo" }
                })
                {
                    ResizeKeyboard = true
                };
            }
            else if(indice == 0) {
                replyKeyboardMarkup = new(new []
                {
                    botones.GetRange(indice, indice + 3).ToArray(),
                    botones.GetRange(indice + 4, indice + 7).ToArray(),
                    new KeyboardButton[] { "Siguiente" },
                    new KeyboardButton[] { "Cancelar", "Listo" }
                })
                {
                    ResizeKeyboard = true
                };
            } else {
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
            }
            return replyKeyboardMarkup;
        }
        /// <summary>
        /// Representación de los posibles estados de una postulación a oferta.
        /// </summary>
        private enum Estados{
            Etiquetas,
            Categorias,
            SeleccionandoCategorias,
            Buscando,
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
            public List<String> Etiquetas { get; set; }
            /// <summary>
            /// Lista de ofertas encontradas en la búsqueda de ofertas.
            /// </summary>
            public List<Oferta> OfertasEncontradas { get; set; }
            /// <summary>
            /// Lista de categorías que está usando un usuario para buscar una oferta.
            /// </summary>
            public List<String> Categorias { get; set; }
            /// <summary>
            /// Estado de la búsqueda de ofertas de un usuario.
            /// </summary>
            public Estados Estado { get; set; }

            /// <summary>
            /// Oferta seleccionada por un usuario entre la lista de ofertas encontradas.
            /// </summary>
            public Oferta ofertaSeleccionada { get; set; }

            /// <summary>
            /// Indice actual dentro de la lista de categorías.
            /// </summary>
            public int IndiceEnCategorias { get; set; }
        }
    }
}