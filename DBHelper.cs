using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;
using System.Windows.Forms;

namespace inventario_proyecto
{
    public class DBHelper
    {
        private string connectionString;

        // Constructor que inicializa la cadena de conexión con un valor predeterminado
        public DBHelper()
        {
            this.connectionString = "Server=localhost;Database=inventarioproyecto;Uid=root;Pwd=frankelvis20052004;";
        }

        // Constructor que permite pasar una cadena de conexión personalizada
        public DBHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // Crear un nuevo producto
        public bool InsertarProducto(Producto producto)
        {
            string query = "INSERT INTO productos (nombre_producto, id_categoria) VALUES (@nombre, @categoria_id)";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@categoria_id", producto.CategoriaId);

                    connection.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error al insertar producto: {ex.Message}");
                    return false;
                }
            }
        }

        // Obtener todos los productos con detalles
        public List<Producto> ObtenerProductos()
        {
            string query = "SELECT p.id_producto, p.nombre_producto, p.id_categoria, c.nombre_categoria, " +
                           "pr.unidad_medida, p.stock_actual, p.stock_minimo " +
                           "FROM productos p " +
                           "JOIN categorias c ON p.id_categoria = c.id_categoria " +
                           "LEFT JOIN presentaciones pr ON p.id_producto = pr.id_producto";

            List<Producto> productos = new List<Producto>();
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
                            productos.Add(new Producto
                            {
                                Id = reader.GetInt32("id_producto"),
                                Nombre = reader.GetString("nombre_producto"),
                                CategoriaId = reader.GetInt32("id_categoria"),
                                CategoriaNombre = reader.GetString("nombre_categoria"),
                                UnidadMedida = reader.IsDBNull(reader.GetOrdinal("unidad_medida")) ? string.Empty : reader.GetString("unidad_medida"),
                                StockActual = reader.GetDecimal("stock_actual"),
                                StockMinimo = reader.GetDecimal("stock_minimo")
                            });
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error al obtener productos: {ex.Message}");
                }
            }
            return productos;
        }



        // Obtener todas las categorías
        public List<Categoria> ObtenerCategorias()
        {
            List<Categoria> listaCategorias = new List<Categoria>();
            string query = "SELECT id_categoria, nombre_categoria FROM categorias";

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
                            listaCategorias.Add(new Categoria
                            {
                                Id = reader.GetInt32("id_categoria"),
                                Nombre = reader.GetString("nombre_categoria")
                            });
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error al obtener categorías: {ex.Message}");
                }
            }
            return listaCategorias;
        }

        // Obtener todas las presentaciones
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
        // Actualizar un producto
        public bool ActualizarProducto(Producto producto)
        {
            string query = "UPDATE productos SET nombre_producto = @nombre, " +
                           "id_categoria = @categoria_id, unidad_medida = @unidad_medida, " +
                           "stock_actual = @stock, stock_minimo = @stock_minimo " +
                           "WHERE id_producto = @id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", producto.Id); // id_producto
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre); // nombre_producto
                    cmd.Parameters.AddWithValue("@categoria_id", producto.CategoriaId); // id_categoria
                    cmd.Parameters.AddWithValue("@unidad_medida", producto.UnidadMedida); // unidad_medida
                    cmd.Parameters.AddWithValue("@stock", producto.StockActual); // stock_actual
                    cmd.Parameters.AddWithValue("@stock_minimo", producto.StockMinimo); // stock_minimo

                    connection.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error al actualizar producto: {ex.Message}");
                    return false;
                }
            }
        }
        // Eliminar un producto
        public bool EliminarProducto(int idProducto)
        {
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();
                using (MySqlTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        // Eliminar relaciones en la tabla 'presentaciones' primero
                        string queryEliminarRelacionados = "DELETE FROM presentaciones WHERE id_producto = @Id";
                        using (MySqlCommand comandoRelacionados = new MySqlCommand(queryEliminarRelacionados, conexion, transaccion))
                        {
                            comandoRelacionados.Parameters.AddWithValue("@Id", idProducto);
                            comandoRelacionados.ExecuteNonQuery();
                        }

                        // Luego, eliminar el producto en la tabla 'productos'
                        string queryEliminarProducto = "DELETE FROM productos WHERE id_producto = @Id";
                        using (MySqlCommand comandoProducto = new MySqlCommand(queryEliminarProducto, conexion, transaccion))
                        {
                            comandoProducto.Parameters.AddWithValue("@Id", idProducto);
                            comandoProducto.ExecuteNonQuery();
                        }

                        transaccion.Commit();
                        return true;
                    }
                    catch (MySqlException ex)
                    {
                        transaccion.Rollback();

                        // Manejo de errores específicos, como claves foráneas
                        if (ex.Number == 1451) // Código para restricción de clave foránea
                        {
                            MessageBox.Show("No se puede eliminar este producto porque está relacionado con otras tablas.");
                        }
                        else
                        {
                            MessageBox.Show("Error al eliminar el producto: " + ex.Message);
                        }
                        return false;
                    }
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