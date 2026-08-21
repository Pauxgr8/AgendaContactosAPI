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
        [HttpGet]
        public async Task<IActionResult> ObtenerContactos()
        {
            try
            {
                var contactos = await _repository.ObtenerContactosAsync();

                return Ok(contactos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerContactoPorId(int id)
        {
            try
            {
                var contacto =
                    await _repository.ObtenerContactoPorIdAsync(id);

                if (contacto == null)
                {
                    return NotFound(new
                    {
                        mensaje = "Contacto no encontrado."
                    });
                }

                return Ok(contacto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarContacto(
    int id,
    Contacto contacto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool resultado =
                    await _repository.ActualizarContactoAsync(
                        id,
                        contacto);

                if (!resultado)
                {
                    return NotFound(new
                    {
                        mensaje = "Contacto no encontrado."
                    });
                }

                return Ok(new
                {
                    mensaje = "Contacto actualizado correctamente."
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
        [HttpDelete("{id}")]

            public async Task<IActionResult> EliminarContacto(int id)
            {
                try
                {
                    bool resultado =
                        await _repository.EliminarContactoAsync(id);

                    if (!resultado)
                    {
                        return NotFound(new
                        {
                            mensaje = "Contacto no encontrado."
                        });
                    }

                    return Ok(new
                    {
                        mensaje = "Contacto eliminado correctamente."
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
