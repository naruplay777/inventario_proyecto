using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace inventario_proyecto
{
    public partial class FormularioAgregarEditarProducto : Form
    {
        private DataTable dtProductos = new DataTable();

        public FormularioAgregarEditarProducto()
        {
            InitializeComponent();
        }

        private class ConexionDB
        {
            private static string connectionString = "Server=localhost;Database=heladeria;Uid=root;Pwd=andrewserver;";

            public static MySqlConnection GetConnection()
            {
                return new MySqlConnection(connectionString);
            }
        }

        public FormularioAgregarEditarProducto(Producto producto = null)
        {
            InitializeComponent();

        }

        private void FormularioAgregarEditarProducto_Load(object sender, EventArgs e)
        {
            CargarProductos();
            CargarCategorias();
            dgvProductos.ClearSelection();
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
        }



        private void CargarProductos()
        {
            dtProductos.Clear();
            string query = @"SELECT 
                        p.producto_id AS ID, 
                        p.nombre, 
                        p.descripcion, 
                        c.nombre AS categoria,  -- Aquí cambiamos categoria_id por su nombre
                        p.unidad_base, 
                        p.stock_minimo 
                    FROM productos p
                    JOIN categorias c ON p.categoria_id = c.categoria_id
                    WHERE p.activo = 1";

            using (MySqlConnection conn = ConexionDB.GetConnection())
            {
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                da.Fill(dtProductos);
            }

            dgvProductos.DataSource = dtProductos; // Aquí estaba el error, se asignaba dgvProductos a sí mismo
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtUnidad.Clear();
            txtStock.Clear();
            cmbCategoria.DataSource = null;
            cmbCategoria.Items.Clear();
            dgvProductos.ClearSelection();
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];

                txtNombre.Text = fila.Cells["nombre"].Value.ToString();
                txtDescripcion.Text = fila.Cells["descripcion"].Value.ToString();
                txtUnidad.Text = fila.Cells["unidad_base"].Value.ToString();
                txtStock.Text = fila.Cells["stock_minimo"].Value.ToString();

                // Obtener el nombre de la categoría desde el DataGridView
                string nombreCategoria = fila.Cells["categoria"].Value.ToString();

                // Buscar el índice del nombre en el ComboBox y seleccionarlo
                cmbCategoria.SelectedIndex = cmbCategoria.FindStringExact(nombreCategoria);

                btnEditar.Enabled = true;
                btnEliminar.Enabled = true;
            }
        }


        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un producto para editar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int productoId = Convert.ToInt32(dgvProductos.CurrentRow.Cells["ID"].Value);
            string query = "UPDATE productos SET nombre = @nombre, descripcion = @descripcion, categoria_id = @categoria_id, unidad_base = @unidad_base, stock_minimo = @stock_minimo  WHERE producto_id = @id";

            try
            {
                using (MySqlConnection conn = ConexionDB.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text.Trim());
                    cmd.Parameters.AddWithValue("@categoria_id", cmbCategoria.SelectedValue);
                    cmd.Parameters.AddWithValue("@unidad_base", txtUnidad.Text.Trim());

                    if (float.TryParse(txtStock.Text.Trim(), out float stock_minimo))
                    {
                        cmd.Parameters.AddWithValue("@stock_minimo", stock_minimo);
                    }
                    else
                    {
                        MessageBox.Show("El Stock Minimo debe ser un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }


                    cmd.Parameters.AddWithValue("@id", productoId);
                    cmd.ExecuteNonQuery();
                }

                LimpiarCampos();
                CargarProductos();
                MessageBox.Show("Producto actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar Producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            CargarCategorias();

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un Producto para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(
                "¿Estás seguro de desactivar esta producto?",
                "Confirmar eliminación lógica",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                int productoID = Convert.ToInt32(dgvProductos.CurrentRow.Cells["ID"].Value);
                string query = "UPDATE productos SET activo = 0 WHERE producto_id = @id";

                try
                {
                    using (MySqlConnection conn = ConexionDB.GetConnection())
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", productoID);
                        cmd.ExecuteNonQuery();
                    }

                    LimpiarCampos();
                    CargarProductos();
                    MessageBox.Show("Producto desactivada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                CargarCategorias();

            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre del Producto es obligatorio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = "INSERT INTO productos (nombre, descripcion, categoria_id, unidad_base, stock_minimo) VALUES (@nombre, @descripcion, @categoria_id, @unidad_base, @stock_minimo)";

            try
            {
                using (MySqlConnection conn = ConexionDB.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text.Trim());
                    cmd.Parameters.AddWithValue("@categoria_id", cmbCategoria.SelectedValue);
                    cmd.Parameters.AddWithValue("@unidad_base", txtUnidad.Text.Trim());

                    if (float.TryParse(txtStock.Text.Trim(), out float stock_minimo))
                    {
                        cmd.Parameters.AddWithValue("@stock_minimo", stock_minimo);
                    }
                    else
                    {
                        MessageBox.Show("El Stock Minimo debe ser un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }


                    cmd.ExecuteNonQuery();
                }

                LimpiarCampos();
                CargarProductos();
                MessageBox.Show("Producto agregada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void CargarCategorias()
        {
            string query = "SELECT categoria_id, nombre FROM categorias WHERE activo = 1";

            using (MySqlConnection conn = ConexionDB.GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dtCategorias = new DataTable();
                    da.Fill(dtCategorias);

                    cmbCategoria.DataSource = dtCategorias;
                    cmbCategoria.DisplayMember = "nombre";  // Lo que se mostrará en el ComboBox
                    cmbCategoria.ValueMember = "categoria_id";  // Lo que se guardará internamente
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar categorías: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Cancelar el formulario y cerrar
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

// DISEÑO---------------------------------------------------------------------------------

        private void pictureCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        [DllImport("user32.Dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.Dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }


    }
}
