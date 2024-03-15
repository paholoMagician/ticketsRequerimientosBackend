using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ticketsRequerimientosBackend.Authentication_Token;
using ticketsRequerimientosBackend.Models;

namespace ticketsRequerimientosBackend.Controllers
{
    //[Authorize(Policy = "All")]
    [Route("api/Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly CMSSoftwarecontrolContext _context;
        public LoginController(CMSSoftwarecontrolContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //[Authorize(Policy = "All")]
        [HttpPost]
        [Route("InicioSesion")]
        public IActionResult Login([FromBody] UserRequest userRequest)
        {
            Authenticator authenticator = new(_context);
            GenerateToken generateToken = new(_configuration);
            var user = authenticator.Authenticator_User(userRequest);
            if (user != null)
            {
                var imgUser = from im in _context.ImgFile
                              join uspt in _context.UsuarioPortalTicket on user.Usuario equals uspt.Usuario
                              join us in _context.Usuario on uspt.CodUser equals us.Coduser
                              where im.Codentidad == ("IMG-"+us.Coduser) && im.Tipo == "Perfil"
                              select im.Imagen;
                var token = generateToken.Generate(user);
                return Ok(new { token = token , imagen = imgUser});
            }
            else
            {
                return NotFound("Usuario no registrado");
            }
        }

    }
}
