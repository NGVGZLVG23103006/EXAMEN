using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using ExamenProgra.Models;
namespace ExamenProgra.Data
{
    public class EstudianteRepository
    {
        private readonly string _connectionString;
        public EstudianteRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Estudiante> ObtenerEstudiantes()
        {
            var estudiantes = new List<Estudiante>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = "SELECT ID, Nombre, Apellido, Edad, Correo FROM Estudiantes";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var estudiante = new Estudiante
                            {
                                ID = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2),
                                Edad = reader.GetInt32(3),
                                Correo = reader.GetString(4)
                            };
                            estudiantes.Add(estudiante);
                        }
                    }
                }
            }

            return estudiantes;
        }
    }
}
