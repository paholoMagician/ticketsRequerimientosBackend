using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
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
                return (await _context.SaveChangesAsync() > 0) ? Ok() : BadRequest("No se guardo");
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("ObtenerTicket/{id}")]
        public IActionResult ObtenerTicket([FromRoute] int id)
        {
            var Datos = from tr in _context.Ticketresolucion
                        join ct in _context.Cliente on tr.Idcliente equals ct.Codcliente
                        where tr.IdRequerimiento == id
                        select new
                        {
                            tr.IdRequerimiento,
                            tr.Idcliente,
                            ct.Ruc,
                            ct.Nombre,
                            ct.Telfpago,
                            ct.NombreMantenimiento,
                            ct.Telfclimanteni,
                            tr.UrlA,
                            tr.UrlB,
                            tr.Estado,
                            tr.MensajeDelProblema,
                            tr.MensajeResolucion,
                            tr.Obervacion,
                            tr.Iduser,
                            tr.Fechacrea
                        };
            return (Datos != null) ? Ok(Datos) : NotFound();
        }

        [HttpPut]
        [Route("ActualizarTicket/{id}")]
        public async Task<IActionResult> ActualizarTicket([FromRoute] int id, [FromBody] Ticketresolucion model)
        {
            if (id != model.IdRequerimiento)
            {
                return BadRequest();
            }
            _context.Entry(model).State = EntityState.Modified;
            return (await _context.SaveChangesAsync() > 0) ? Ok() : BadRequest();
        }

        [HttpPut]
        [Route("ActualizarTicketEstado/{id}/{estado}")]
        public async Task<IActionResult> ActualizarTicketEstado([FromRoute] int id, [FromRoute] int estado)
        {
            var ticket = _context.Ticketresolucion.FirstOrDefault(c => c.IdRequerimiento == id); // Suponiendo que deseas modificar el cliente con ID 1

            if (ticket != null)
            {
                ticket.Estado = estado;
                return (await _context.SaveChangesAsync() > 0) ? Ok() : BadRequest();
            }
            return NotFound();
        }

        [HttpDelete("BorrarTicket/{id}")]
        public IActionResult BorrarTicket([FromRoute] int id)
        {
            var delete = _context.Ticketresolucion
                          .Where(b => b.IdRequerimiento.Equals(id))
                          .ExecuteDelete();
            return (delete != 0) ? Ok() : BadRequest();
        }
    }
}
