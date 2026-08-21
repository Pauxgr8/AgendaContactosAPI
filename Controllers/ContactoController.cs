using AgendaContactos.Models;
using AgendaContactosAPI.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace AgendaContactosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactoController : ControllerBase
    {
        private readonly ContactoRepository _repository;

        public ContactoController(ContactoRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> InsertarContacto(Contacto contacto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                int id = await _repository.InsertarContactoAsync(contacto);

                return StatusCode(201, new
                {
                    mensaje = "Contacto creado correctamente",
                    ID = id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = ex.Message
                });
            }
        }
    }
}