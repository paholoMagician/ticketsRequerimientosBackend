using Microsoft.AspNetCore.Mvc;
using ticketsRequerimientosBackend.Models;

namespace ticketsRequerimientosBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageManagerController : ControllerBase
    {

        [HttpPost]
        [Route("crearCarpeta/{nombre}")]
        public async Task<IActionResult> CrearCarpeta([FromForm] IMGmodelClass request, [FromRoute] string nombre)
        {
            string fileModelpath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot", "storage");
            string imagePath = Path.Combine(fileModelpath, nombre);

            try
            {
                if (!Directory.Exists(fileModelpath))
                {
                    Directory.CreateDirectory(fileModelpath);
                }

                if (!Directory.Exists(imagePath) && request.Archivo is not null)
                {
                    Directory.CreateDirectory(imagePath);
                    string filePath = Path.Combine(imagePath, request.Archivo.FileName);
                    using FileStream newFile = System.IO.File.Create(filePath);
                    await request.Archivo.CopyToAsync(newFile);
                    await newFile.FlushAsync();
                }

                return Ok();

            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }
    }
}
