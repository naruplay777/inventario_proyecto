using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace inventario_proyecto
{
    public partial class IngresoMercanciaForm : Form
    {
        private string connectionString = "Server=localhost;Database=heladeria;Uid=root;Pwd=andrewserver;"; // Reemplaza con tu cadena de conexión

        public IngresoMercanciaForm()
        {
            InitializeComponent();
            ConfigurarDataGridView();
            CargarProveedores();
            CargarProductos();
        }

        private class ConexionDB
        {
            private static string connectionString = "Server=localhost;Database=heladeria;Uid=root;Pwd=andrewserver;";

            public static MySqlConnection GetConnection()
            {
                return new MySqlConnection(connectionString);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvProductos.Columns.Add("producto_id", "ID Producto");
            dgvProductos.Columns.Add("nombre_producto", "Nombre Producto");
            dgvProductos.Columns.Add("cantidad", "Cantidad");
            dgvProductos.Columns.Add("precio_unitario", "Precio Unitario");
        }

        private void CargarProveedores()
        {
            using (MySqlConnection conn = ConexionDB.GetConnection())
            {
                string query = "SELECT proveedor_id, nombre FROM proveedores WHERE activo = 1";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cbProveedor.DataSource = dt;
                cbProveedor.DisplayMember = "nombre";
                cbProveedor.ValueMember = "proveedor_id";
            }
        }

        private void CargarProductos()
        {
            using (MySqlConnection conn = ConexionDB.GetConnection())
            {
                string query = "SELECT producto_id, nombre FROM productos WHERE activo = 1";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cbProducto.DataSource = dt;
                cbProducto.DisplayMember = "nombre";
                cbProducto.ValueMember = "producto_id";
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cbProducto.SelectedValue == null || string.IsNullOrWhiteSpace(txtCantidad.Text) || string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MessageBox.Show("Debe completar todos los campos antes de agregar un producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productoId = Convert.ToInt32(cbProducto.SelectedValue);
            string nombreProducto = cbProducto.Text;
            int cantidad = Convert.ToInt32(txtCantidad.Text);
            decimal precioUnitario = Convert.ToDecimal(txtPrecio.Text);

            dgvProductos.Rows.Add(productoId, nombreProducto, cantidad, precioUnitario);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvProductos.SelectedRows)
            {
                dgvProductos.Rows.Remove(row);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (cbProveedor.SelectedValue == null || dgvProductos.Rows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar un proveedor y agregar al menos un producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int proveedorId = Convert.ToInt32(cbProveedor.SelectedValue);
            int usuarioId = 1; // Reemplazar con el usuario logueado

            using (MySqlConnection conn = ConexionDB.GetConnection())
            {
                conn.Open();
                MySqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    foreach (DataGridViewRow row in dgvProductos.Rows)
                    {
                        int productoId = Convert.ToInt32(row.Cells["producto_id"].Value);
                        int cantidad = Convert.ToInt32(row.Cells["cantidad"].Value);
                        decimal precioUnitario = Convert.ToDecimal(row.Cells["precio_unitario"].Value);

                        string queryEntrada = "INSERT INTO entradas (producto_id, proveedor_id, usuario_id, cantidad, precio_unitario, fecha_entrada) VALUES (@productoId, @proveedorId, @usuarioId, @cantidad, @precio, NOW())";
                        MySqlCommand cmdEntrada = new MySqlCommand(queryEntrada, conn, transaction);
                        cmdEntrada.Parameters.AddWithValue("@productoId", productoId);
                        cmdEntrada.Parameters.AddWithValue("@proveedorId", proveedorId);
                        cmdEntrada.Parameters.AddWithValue("@usuarioId", usuarioId);
                        cmdEntrada.Parameters.AddWithValue("@cantidad", cantidad);
                        cmdEntrada.Parameters.AddWithValue("@precio", precioUnitario);
                        cmdEntrada.ExecuteNonQuery();

                        string queryInventario = "UPDATE inventario SET stock_actual = stock_actual + @cantidad WHERE producto_id = @productoId";
                        MySqlCommand cmdInventario = new MySqlCommand(queryInventario, conn, transaction);
                        cmdInventario.Parameters.AddWithValue("@cantidad", cantidad);
                        cmdInventario.Parameters.AddWithValue("@productoId", productoId);
                        cmdInventario.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Ingreso registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvProductos.Rows.Clear();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error al registrar el ingreso: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }




    }
}
