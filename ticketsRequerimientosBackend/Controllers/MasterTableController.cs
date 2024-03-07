using Microsoft.AspNetCore.Mvc;
using ticketsRequerimientosBackend.Models;

namespace ticketsRequerimientosBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterTableController : ControllerBase
    {

        private readonly CMSSoftwarecontrolContext _context;
        public MasterTableController(CMSSoftwarecontrolContext context)
        {
            _context = context;
        }

        [HttpGet("ObtenerMasterTable/{master}")]
        public IActionResult ObtenerMaster([FromRoute] string master)
        {
            var Datos = from mt in _context.MasterTable
                        where mt.Master == master
                        select mt;
            return (Datos != null) ? Ok(Datos) : NotFound();
        }
    }
}
