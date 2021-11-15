
namespace LibraryHandler
{

    public class VerOfertasHandler : BaseHandler
    {
        public IHandler Handle(IMensaje message, out string response)
        {
            Sistema sistema = new Sistema();
            List<Oferta> ofertas = Buscador.Instancia.BuscarOfertas(sistema, sistema.ObtenerEmprendedorPorId(message));

        }
    }

}