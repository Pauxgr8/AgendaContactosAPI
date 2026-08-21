using System;
using System.ComponentModel.DataAnnotations;

namespace AgendaContactos.Models
{
    public class Contacto
    {
        public int ID { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        public string? Telefono { get; set; }

        public DateTime FechaRegistro { get; set; }

        public string UsuarioCreacion { get; set; }
    }
}