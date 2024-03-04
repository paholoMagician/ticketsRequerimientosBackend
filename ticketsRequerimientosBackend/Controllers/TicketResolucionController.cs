using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ticketsRequerimientosBackend.Models;

namespace ticketsRequerimientosBackend.Controllers
{
    [Route("api/TicketResolucion")]
    [ApiController]
    public class TicketResolucionController : ControllerBase
    {

        private readonly CMSSoftwarecontrolContext _context;
        public TicketResolucionController(CMSSoftwarecontrolContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("GuardarTickets")]
        public async Task<IActionResult> GuardarTickets([FromBody] Ticketresolucion model)
        {
            if (ModelState.IsValid)
            {
                _context.Ticketresolucion.Add(model);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return Ok(model);
                }
                else
                {
                    return BadRequest("Datos incorrectos");
                }
            }
            else
            {
                return BadRequest("ERROR");
            }
        }

    }
}
