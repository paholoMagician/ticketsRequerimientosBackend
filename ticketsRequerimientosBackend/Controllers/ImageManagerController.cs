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
            string fileModelpath = Path.Combine(Directory.GetCurrentDirectory(), "fileModel");
            string assetsPath = Path.Combine(fileModelpath, "Assets");
            string imagePath = Path.Combine(assetsPath, nombre);

            try
            {
                if (!Directory.Exists(assetsPath))
                {
                    Directory.CreateDirectory(assetsPath);
                }

                if (!Directory.Exists(imagePath) && request.Archivo is not null)
                {
                    Directory.CreateDirectory(imagePath);
                    // Obtén la ruta completa del archivo en lugar de solo la carpeta
                    string filePath = Path.Combine(imagePath, request.Archivo.FileName);
                    using FileStream newFile = System.IO.File.Create(filePath);
                    await request.Archivo.CopyToAsync(newFile);
                    await newFile.FlushAsync();
                }

                return Ok("La carpeta se ha creado");
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }
    }
}
