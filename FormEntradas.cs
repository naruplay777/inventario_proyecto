using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace inventario_proyecto
{
    public partial class FormEntradas : Form
    {
        private DataTable dtProductos = new DataTable();
        private string connectionString = "Server=localhost;Database=heladeria;Uid=root;Pwd=andrewserver;";

        public FormEntradas()
        {
            InitializeComponent();
        }

        private void FormEntradas_Load(object sender, EventArgs e)
        {
            CargarProveedores();
            CargarProductos();
        }

        private void CargarProveedores()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT proveedor_id, nombre FROM proveedores";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cboProveedores.DataSource = dt;
                cboProveedores.DisplayMember = "nombre";
                cboProveedores.ValueMember = "proveedor_id";
            }
        }

        private void CargarProductos()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT producto_id, nombre FROM inventario";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                da.Fill(dtProductos);
                dgvProductos.DataSource = dtProductos;
            }
        }

        private void btnRegistrarEntrada_Click(object sender, EventArgs e)
        {
            if (cboProveedores.SelectedValue == null)
            {
                MessageBox.Show("Selecciona un proveedor.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int proveedorId = Convert.ToInt32(cboProveedores.SelectedValue);
            List<(int productoId, int cantidad, decimal precio)> entradas = new List<(int, int, decimal)>();

            foreach (DataGridViewRow row in dgvProductos.Rows)
            {
                if (row.Cells["cantidad"].Value != null && row.Cells["precio"].Value != null)
                {
                    int productoId = Convert.ToInt32(row.Cells["producto_id"].Value);
                    int cantidad = Convert.ToInt32(row.Cells["cantidad"].Value);
                    decimal precio = Convert.ToDecimal(row.Cells["precio"].Value);
                    entradas.Add((productoId, cantidad, precio));
                }
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                foreach (var entrada in entradas)
                {
                    string query = "INSERT INTO entradas (producto_id, proveedor_id, cantidad, precio, fecha) VALUES (@producto_id, @proveedor_id, @cantidad, @precio, NOW());";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@producto_id", entrada.productoId);
                    cmd.Parameters.AddWithValue("@proveedor_id", proveedorId);
                    cmd.Parameters.AddWithValue("@cantidad", entrada.cantidad);
                    cmd.Parameters.AddWithValue("@precio", entrada.precio);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Entradas registradas correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CargarProductos();
        }
    }
}
