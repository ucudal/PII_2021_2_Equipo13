
namespace LibraryHandler
{

    public class VerOfertasHandler: HandlerBase 
    {
        public IHandler Handle(IMensaje message, out string response)
        {
            Sistema sistema = new Sistema();
            List<Oferta> ofertas = Buscador.Instancia.BuscarOfertas(sistema, sistema.ObtenerEmprendedorPorId(message.IdUsuario));
            string respuesta = "";
            ofertas.forEach(oferta => {
                respuesta += oferta.RedactarResumen() + "\n";
            });
            response = respuesta;
        }
    }

}