using AgendaContactos.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

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
        public async Task<List<Contacto>> ObtenerContactosAsync()
        {
            List<Contacto> contactos = new List<Contacto>();

            using SqlConnection conexion =
                new SqlConnection(_connectionString);

            using SqlCommand comando =
                new SqlCommand("sp_ObtenerContactos", conexion);

            comando.CommandType = CommandType.StoredProcedure;

            await conexion.OpenAsync();

            using SqlDataReader reader =
                await comando.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Contacto contacto = new Contacto
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    Nombre = reader["Nombre"].ToString() ?? "",
                    Correo = reader["Correo"].ToString() ?? "",

                    Telefono = reader["Telefono"] == DBNull.Value
                        ? null
                        : reader["Telefono"].ToString(),

                    FechaRegistro =
                        Convert.ToDateTime(reader["FechaRegistro"])
                };

                contactos.Add(contacto);
            }

            return contactos;
        }

        public async Task<Contacto?> ObtenerContactoPorIdAsync(int id)
        {
            using SqlConnection conexion =
                new SqlConnection(_connectionString);

            using SqlCommand comando =
                new SqlCommand("sp_ObtenerContactoPorID", conexion);

            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@ID", id);

            await conexion.OpenAsync();

            using SqlDataReader reader =
                await comando.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Contacto
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    Nombre = reader["Nombre"].ToString() ?? "",
                    Correo = reader["Correo"].ToString() ?? "",

                    Telefono = reader["Telefono"] == DBNull.Value
                        ? null
                        : reader["Telefono"].ToString(),

                    FechaRegistro =
                        Convert.ToDateTime(reader["FechaRegistro"])
                };
            }

            return null;
        }

        public async Task<bool> ActualizarContactoAsync(
    int id,
    Contacto contacto)
        {
            using SqlConnection conexion =
                new SqlConnection(_connectionString);

            using SqlCommand comando =
                new SqlCommand("sp_ActualizarContacto", conexion);

            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@ID", id);
            comando.Parameters.AddWithValue("@Nombre", contacto.Nombre);
            comando.Parameters.AddWithValue("@Correo", contacto.Correo);

            comando.Parameters.AddWithValue(
                "@Telefono",
                (object?)contacto.Telefono ?? DBNull.Value);

            comando.Parameters.AddWithValue(
                "@usuario_modificacion",
                contacto.UsuarioCreacion);

            await conexion.OpenAsync();

            await comando.ExecuteNonQueryAsync();

            return true;
        }

        public async Task<bool> EliminarContactoAsync(int id)
        {
            using SqlConnection conexion =
                new SqlConnection(_connectionString);

            using SqlCommand comando =
                new SqlCommand("sp_EliminarContacto", conexion);

            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@ID", id);

            await conexion.OpenAsync();

            await comando.ExecuteNonQueryAsync();

            return true;
        }
    }
}