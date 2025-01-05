using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace inventario_proyecto
{
    public class DBHelper
    {

        // Cadena de conexión a la base de datos
        private string connectionString = "Server=localhost;Database=inventario_heladeria;Uid=root;Pwd=andrewserver;";
        private MySqlConnection connection;

        public DBHelper()
        {
            connection = new MySqlConnection(connectionString);
        }

        // Función para conectar a la base de datos
        public void Conectar()
        {
            try
            {
                connection.Open();
                Console.WriteLine("Conexión exitosa a la base de datos.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar a la base de datos: {ex.Message}");
            }
        }

        // Función para desconectar de la base de datos
        public void Desconectar()
        {
            try
            {
                connection.Close();
                Console.WriteLine("Desconexión exitosa de la base de datos.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al desconectar de la base de datos: {ex.Message}");
            }
        }

        // Función para ejecutar una consulta SELECT
        public List<string> EjecutarConsulta(string query)
        {
            List<string> resultados = new List<string>();
            try
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    resultados.Add(reader.GetString(0));  // Obtiene el primer campo de cada fila
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al ejecutar la consulta: {ex.Message}");
            }
            return resultados;
        }

        // Función para ejecutar comandos INSERT, UPDATE, DELETE
        public void EjecutarComando(string comando)
        {
            try
            {
                MySqlCommand command = new MySqlCommand(comando, connection);
                command.ExecuteNonQuery();
                Console.WriteLine("Comando ejecutado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al ejecutar el comando: {ex.Message}");
            }
        }

        // Función para obtener la conexión (en caso de que se necesite para algo específico)
        public MySqlConnection ObtenerConexion()
        {
            return connection;
        }

    }
}
