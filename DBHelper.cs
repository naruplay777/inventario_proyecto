using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;

namespace inventario_proyecto
{
    public class DBHelper
    {
        private string connectionString;

        // Constructor que inicializa la cadena de conexión con un valor predeterminado
        public DBHelper()
        {
            this.connectionString = "Server=localhost;Database=inventario_heladeria;Uid=root;Pwd=andrewserver;";
        }

        // Constructor que permite pasar una cadena de conexión personalizada
        public DBHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Presentacion> ObtenerPresentaciones()
        {
            List<Presentacion> presentaciones = new List<Presentacion>();
            string query = "SELECT id_presentacion, descripcion_presentacion FROM presentaciones";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    connection.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            presentaciones.Add(new Presentacion
                            {
                                Id = reader.GetInt32("id_presentacion"),
                                Descripcion = reader.GetString("descripcion_presentacion")
                            });
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error al obtener presentaciones: {ex.Message}");
                }
            }
            return presentaciones;
        }
        public bool EliminarProducto(int productoId)
        {
            string query = "DELETE FROM productos WHERE id = @id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", productoId);

                    connection.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error al eliminar producto: {ex.Message}");
                    return false;
                }
            }
        }


        public bool InsertarPresentacion(Presentacion presentacion)
        {
            string query = "INSERT INTO presentaciones (descripcion_presentacion, costo_por_presentacion, producto_id) " +
                           "VALUES (@descripcion, @precio, @producto_id)";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@descripcion", presentacion.Descripcion);
                    cmd.Parameters.AddWithValue("@precio", presentacion.CostoPorPresentacion);
                    cmd.Parameters.AddWithValue("@producto_id", presentacion.ProductoId);

                    connection.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error al insertar presentación: {ex.Message}");
                    return false;
                }
            }
        }

        public Presentacion ObtenerPresentacionPorProducto(int productoId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT * FROM presentaciones WHERE producto_id = @producto_id LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@producto_id", productoId);

                    connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new Presentacion
                        {
                            Id = reader.GetInt32("id_presentacion"),
                            Descripcion = reader.GetString("descripcion_presentacion"),
                            CostoPorPresentacion = reader.GetDecimal("costo_por_presentacion"),
                            ProductoId = reader.GetInt32("producto_id")
                        };
                    }

                    return null;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error al obtener presentación: {ex.Message}");
                    return null;
                }
            }
        }

        public bool ActualizarPresentacion(Presentacion presentacion)
        {
            string query = "UPDATE presentaciones SET descripcion_presentacion = @descripcion, " +
                           "costo_por_presentacion = @costo WHERE id_presentacion = @id_presentacion";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id_presentacion", presentacion.Id);
                    cmd.Parameters.AddWithValue("@descripcion", presentacion.Descripcion);
                    cmd.Parameters.AddWithValue("@costo", presentacion.CostoPorPresentacion);

                    connection.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error al actualizar presentación: {ex.Message}");
                    return false;
                }
            }
        }
        public int ObtenerUltimoProductoId()
        {
            string query = "SELECT MAX(id_producto) FROM productos";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    object result = cmd.ExecuteScalar();

                    if (result != DBNull.Value)
                    {
                        return Convert.ToInt32(result); // Devuelve el último ID
                    }

                    return 0; // Si no hay productos, devuelve 0
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error al obtener el último ID de producto: {ex.Message}");
                    return 0; // En caso de error, devuelve 0
                }
            }
        }


    }
}