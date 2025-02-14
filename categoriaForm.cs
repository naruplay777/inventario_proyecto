using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace inventario_proyecto
{
    public partial class categoriaForm : Form
    {
        private DataTable dtCategorias = new DataTable();

        public categoriaForm()
        {
            InitializeComponent();
        }

        // esto es para que el diseño se puda mover con mouse
        [DllImport("user32.Dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.Dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        // Clase para manejar la conexión a la base de datos
        private class ConexionDB
        {
            private static string connectionString = "Server=localhost;Database=heladeria;Uid=root;Pwd=andrewserver;";

            public static MySqlConnection GetConnection()
            {
                return new MySqlConnection(connectionString);
            }
        }

        private void categoriaForm_Load(object sender, EventArgs e)
        {
            CargarCategorias();
            dgvCategorias.ClearSelection();
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
        }

        private void CargarCategorias()
        {
            dtCategorias.Clear();
            string query = "SELECT categoria_id AS ID, nombre, descripcion FROM categorias WHERE activo = 1";

            using (MySqlConnection conn = ConexionDB.GetConnection())
            {
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                da.Fill(dtCategorias);
            }

            dgvCategorias.DataSource = dtCategorias;
        }


        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
            dgvCategorias.ClearSelection();
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
        }



        private void dgvCategorias_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCategorias.Rows[e.RowIndex];
                txtNombre.Text = row.Cells["nombre"].Value.ToString();
                txtDescripcion.Text = row.Cells["descripcion"].Value.ToString();
                btnEditar.Enabled = true;
                btnEliminar.Enabled = true;

            }
        }


        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvCategorias.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una categoría para editar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int categoriaId = Convert.ToInt32(dgvCategorias.CurrentRow.Cells["ID"].Value);
            string query = "UPDATE categorias SET nombre = @nombre, descripcion = @descripcion WHERE categoria_id = @id";

            try
            {
                using (MySqlConnection conn = ConexionDB.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text.Trim());
                    cmd.Parameters.AddWithValue("@id", categoriaId);
                    cmd.ExecuteNonQuery();
                }

                LimpiarCampos();
                CargarCategorias();
                MessageBox.Show("Categoría actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar categoría: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvCategorias.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una categoría para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(
                "¿Estás seguro de desactivar esta categoría?",
                "Confirmar eliminación lógica",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                int categoriaId = Convert.ToInt32(dgvCategorias.CurrentRow.Cells["ID"].Value);
                string query = "UPDATE categorias SET activo = 0 WHERE categoria_id = @id";

                try
                {
                    using (MySqlConnection conn = ConexionDB.GetConnection())
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", categoriaId);
                        cmd.ExecuteNonQuery();
                    }

                    LimpiarCampos();
                    CargarCategorias();
                    MessageBox.Show("Categoría desactivada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar categoría: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                MessageBox.Show("El nombre de la categoría es obligatorio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = "INSERT INTO categorias (nombre, descripcion) VALUES (@nombre, @descripcion)";

            try
            {
                using (MySqlConnection conn = ConexionDB.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text.Trim());
                    cmd.ExecuteNonQuery();
                }

                LimpiarCampos();
                CargarCategorias();
                MessageBox.Show("Categoría agregada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar categoría: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
        private void panelBarra_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}