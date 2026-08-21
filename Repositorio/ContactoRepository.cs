using AgendaContactos.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AgendaContactosAPI.Repositorio
{
    public class ContactoRepository
    {
        private readonly string _connectionString;

        public ContactoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "No se encontró la cadena de conexión DefaultConnetion.");
        }

        public async Task<int> InsertarContactoAsync(Contacto contacto)
        {
            using SqlConnection conexion = new SqlConnection(_connectionString);

            using SqlCommand comando = new SqlCommand(
                "sp_InsertarContacto", conexion);

            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue(
                "@Nombre", contacto.Nombre);

            comando.Parameters.AddWithValue(
                "@Correo", contacto.Correo);

            comando.Parameters.AddWithValue(
                "@Telefono",
                (object?)contacto.Telefono ?? DBNull.Value);

            comando.Parameters.AddWithValue(
                "@usuario_creacion", contacto.UsuarioCreacion);

            await conexion.OpenAsync();

            object? resultado = await comando.ExecuteScalarAsync();

            return Convert.ToInt32(resultado);
        }
    }
}