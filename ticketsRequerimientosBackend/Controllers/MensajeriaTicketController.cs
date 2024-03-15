using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ticketsRequerimientosBackend.Models;

namespace ticketsRequerimientosBackend.Controllers
{
    [Route("api/MensajeriaTicket")]
    [ApiController]
    public class MensajeriaTicketController : ControllerBase
    {
        private readonly CMSSoftwarecontrolContext _context;
        private readonly IHubContext<MensajeHUB> _mensajeHub;
        public MensajeriaTicketController(CMSSoftwarecontrolContext context, IHubContext<MensajeHUB> mensajeHub)
        {
            _context = context;
            _mensajeHub = mensajeHub;
        }

        [HttpGet("ObtenerMensajesTicket/idRequerimiento/top")]
        public IActionResult ObtenerMensajeTicket([FromRoute] int idRequerimiento, [FromRoute] int top)
        {
            var Datos = (from mjt in _context.MensajeriaTicket
                        orderby mjt.Fechaemit descending
                        where mjt.IdRequerimiento.Equals(idRequerimiento) && mjt.Active.Equals("A")
                        select mjt).Take(top);
            return (Datos != null) ? Ok(Datos) : NotFound();
        }

        
        [HttpPost]
        [Route("GuardarMensajesTicket")]
        public async Task<IActionResult> GuardarMensajesTickets([FromBody] MensajeriaTicket model)
        {
            if (ModelState.IsValid)
            {
                _context.MensajeriaTicket.Add(model);
                var respuesta = from uspt in _context.UsuarioPortalTicket
                                where uspt.CodUser == model.Coduser
                                join img in _context.ImgFile on uspt.CodUser equals img.Codentidad
                                select new { imagen = img.Imagen, remitente = uspt.Usuario } ;
                await _mensajeHub.Clients.All.SendAsync("SendMessageHub", model, respuesta);
                return (await _context.SaveChangesAsync() > 0) ? Ok() : BadRequest("No se guardo");
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("BorrarMensajeTicket/{idMensaje}")]
        public async Task<IActionResult> BorrarTicket([FromRoute] int idMensaje)
        {
            var ticket = _context.MensajeriaTicket.FirstOrDefault(c => c.Idmensaje == idMensaje); // Suponiendo que deseas modificar el cliente con ID 1

            if (ticket != null)
            {
                ticket.Active = "F";
                return (await _context.SaveChangesAsync() > 0) ? Ok() : BadRequest();
            }
            return NotFound();
        }
    }
}
