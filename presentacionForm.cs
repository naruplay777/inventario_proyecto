using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace inventario_proyecto
{
    public partial class presentacionForm : Form
    {
        private DataTable dtPresentaciones = new DataTable();

        public presentacionForm()
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

        private void presentacionForm_Load(object sender, EventArgs e)
        {
            CargarPresentaciones();
            dgvPresentaciones.ClearSelection();
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
        }

        private void CargarPresentaciones()
        {
            dtPresentaciones.Clear();
            string query = "SELECT presentacion_id AS ID, nombre, descripcion, factor FROM presentaciones WHERE activo = 1";

            using (MySqlConnection conn = ConexionDB.GetConnection())
            {
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                da.Fill(dtPresentaciones);
            }

            dgvPresentaciones.DataSource = dtPresentaciones;
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtFactor.Clear();
            dgvPresentaciones.ClearSelection();
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
        }

        private void dgvPresentaciones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPresentaciones.Rows[e.RowIndex];
                txtNombre.Text = row.Cells["nombre"].Value.ToString();
                txtDescripcion.Text = row.Cells["descripcion"].Value.ToString();
                txtFactor.Text = row.Cells["factor"].Value.ToString();
                btnEditar.Enabled = true;
                btnEliminar.Enabled = true;

            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvPresentaciones.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una presentación para editar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int presentacionId = Convert.ToInt32(dgvPresentaciones.CurrentRow.Cells["ID"].Value);
            string query = "UPDATE presentaciones SET nombre = @nombre, descripcion = @descripcion, factor = @factor WHERE presentacion_id = @id";

            try
            {
                using (MySqlConnection conn = ConexionDB.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text.Trim());

                    // Verificar si factor es un número
                    if (float.TryParse(txtFactor.Text.Trim(), out float factor))
                    {
                        cmd.Parameters.AddWithValue("@factor", factor);
                    }
                    else
                    {
                        MessageBox.Show("El factor debe ser un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    cmd.Parameters.AddWithValue("@id", presentacionId);
                    cmd.ExecuteNonQuery();
                }

                LimpiarCampos();
                CargarPresentaciones();
                MessageBox.Show("Presentación actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar presentación: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvPresentaciones.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una presentacion para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(
                "¿Estás seguro de desactivar esta presentacion?",
                "Confirmar eliminación lógica",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                int presentacionID = Convert.ToInt32(dgvPresentaciones.CurrentRow.Cells["ID"].Value);
                string query = "UPDATE presentaciones SET activo = 0 WHERE presentacion_id = @id";

                try
                {
                    using (MySqlConnection conn = ConexionDB.GetConnection())
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", presentacionID);
                        cmd.ExecuteNonQuery();
                    }

                    LimpiarCampos();
                    CargarPresentaciones();
                    MessageBox.Show("Presentacion desactivada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar presentacion: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            string query = "INSERT INTO presentaciones (nombre, descripcion, factor) VALUES (@nombre, @descripcion, @factor)";

            try
            {
                using (MySqlConnection conn = ConexionDB.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text.Trim());
                    cmd.Parameters.AddWithValue("@factor", txtFactor.Text.Trim());
                    cmd.ExecuteNonQuery();
                }

                LimpiarCampos();
                CargarPresentaciones();
                MessageBox.Show("Categoría agregada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar categoría: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
