using ticketsRequerimientosBackend.Models;

namespace ticketsRequerimientosBackend.Authentication_Token
{
    public class Authenticator
    {
        private CMSSoftwarecontrolContext _context;
        public Authenticator(CMSSoftwarecontrolContext context)
        {
            _context = context;
        }

        public UsuarioPortalTicket? Authenticator_User(UserRequest userRequest)
        {
            var usuario = _context.UsuarioPortalTicket.Where(x => x.Usuario == userRequest.Usuario && x.Password == userRequest.Password).FirstOrDefault();
            if (usuario != null)
            {
                return usuario;
            }
            else
            {
                return null;
            }
        }
    }
}
