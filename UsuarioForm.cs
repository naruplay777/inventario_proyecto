using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace inventario_proyecto
{
    public partial class UsuarioForm : Form
    {
        private Button btnEditar;
        private Button btnEliminar;
        private ComboBox cbRol;
        private Label label1;
        private DataGridView dgvUsuarios;
        private TextBox txtContrasena;
        private Label label2;
        private Label label3;
        private TextBox txtEmail;
        private Label label4;
        private TextBox txtNombre;
        private Button btnAgregar;
        private Panel panel1;
        private PictureBox pictureCerrar;
        private Label label5;
        private DataTable dtUsuarios = new DataTable();

        public UsuarioForm()
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

        private void UsuarioForm_Load(object sender, EventArgs e)
        {
            CargarUsuarios();
            ConfigurarComboBox();
            dgvUsuarios.ClearSelection();
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
        }

        private void ConfigurarComboBox()
        {
            cbRol.Items.Add("Admin");
            cbRol.Items.Add("Empleado");
            cbRol.SelectedIndex = -1;
        }

        private void CargarUsuarios()
        {
            dtUsuarios.Clear();
            string query = "SELECT usuario_id AS ID, nombre, email, rol, fecha_registro FROM usuarios";

            using (MySqlConnection conn = ConexionDB.GetConnection())
            {
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                da.Fill(dtUsuarios);
            }

            dgvUsuarios.DataSource = dtUsuarios;
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtEmail.Clear();
            txtContrasena.Clear();
            cbRol.SelectedIndex = -1;
            dgvUsuarios.ClearSelection();
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUsuarios.Rows[e.RowIndex];
                txtNombre.Text = row.Cells["nombre"].Value.ToString();
                txtEmail.Text = row.Cells["email"].Value.ToString();
                cbRol.Text = row.Cells["rol"].Value.ToString();
                btnEditar.Enabled = true;
                btnEliminar.Enabled = true;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtContrasena.Text) || cbRol.SelectedIndex == -1)
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = "INSERT INTO usuarios (nombre, email, contrasena, rol, fecha_registro) VALUES (@nombre, @correo, @contrasena, @rol, NOW())";

            try
            {
                using (MySqlConnection conn = ConexionDB.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@correo", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@contrasena", txtContrasena.Text.Trim());
                    cmd.Parameters.AddWithValue("@rol", cbRol.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();
                }

                LimpiarCampos();
                CargarUsuarios();
                MessageBox.Show("Usuario agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un usuario para editar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int usuarioId = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["ID"].Value);
            string query = "UPDATE usuarios SET nombre = @nombre, email = @correo, contrasena = @contrasena, rol = @rol WHERE usuario_id = @id";

            try
            {
                using (MySqlConnection conn = ConexionDB.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@correo", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@contrasena", txtContrasena.Text.Trim());
                    cmd.Parameters.AddWithValue("@rol", cbRol.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@id", usuarioId);
                    cmd.ExecuteNonQuery();
                }

                LimpiarCampos();
                CargarUsuarios();
                MessageBox.Show("Usuario actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un usuario para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(
                "¿Estás seguro de eliminar este usuario?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                int usuarioId = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["ID"].Value);
                string query = "DELETE FROM usuarios WHERE usuario_id = @id";

                try
                {
                    using (MySqlConnection conn = ConexionDB.GetConnection())
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", usuarioId);
                        cmd.ExecuteNonQuery();
                    }

                    LimpiarCampos();
                    CargarUsuarios();
                    MessageBox.Show("Usuario eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UsuarioForm));
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.cbRol = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvUsuarios = new System.Windows.Forms.DataGridView();
            this.txtContrasena = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureCerrar = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureCerrar)).BeginInit();
            this.SuspendLayout();
            // 
            // btnEditar
            // 
            this.btnEditar.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnEditar.FlatAppearance.BorderSize = 0;
            this.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditar.Location = new System.Drawing.Point(337, 96);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(75, 25);
            this.btnEditar.TabIndex = 0;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = false;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.BackColor = System.Drawing.Color.Red;
            this.btnEliminar.FlatAppearance.BorderSize = 0;
            this.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEliminar.Location = new System.Drawing.Point(427, 96);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(75, 25);
            this.btnEliminar.TabIndex = 1;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = false;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // cbRol
            // 
            this.cbRol.FormattingEnabled = true;
            this.cbRol.Location = new System.Drawing.Point(12, 96);
            this.cbRol.Name = "cbRol";
            this.cbRol.Size = new System.Drawing.Size(121, 21);
            this.cbRol.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Rol";
            // 
            // dgvUsuarios
            // 
            this.dgvUsuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsuarios.Location = new System.Drawing.Point(12, 125);
            this.dgvUsuarios.Name = "dgvUsuarios";
            this.dgvUsuarios.Size = new System.Drawing.Size(490, 160);
            this.dgvUsuarios.TabIndex = 4;
            this.dgvUsuarios.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsuarios_CellClick);
            // 
            // txtContrasena
            // 
            this.txtContrasena.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.txtContrasena.Location = new System.Drawing.Point(12, 42);
            this.txtContrasena.Name = "txtContrasena";
            this.txtContrasena.Size = new System.Drawing.Size(150, 20);
            this.txtContrasena.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;

            this.label2.Location = new System.Drawing.Point(12, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Contraseña";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(178, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Email";
            // 
            // txtEmail
            // 
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.txtEmail.Location = new System.Drawing.Point(181, 42);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(150, 20);
            this.txtEmail.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;

            this.label4.Location = new System.Drawing.Point(349, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Nombre";
            // 
            // txtNombre
            // 
            this.txtNombre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;


            this.txtNombre.Location = new System.Drawing.Point(352, 42);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(150, 20);
            this.txtNombre.TabIndex = 9;
            // 
            // btnAgregar
            // 
            this.btnAgregar.BackColor = System.Drawing.Color.LimeGreen;
            this.btnAgregar.FlatAppearance.BorderSize = 0;
            this.btnAgregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgregar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregar.Location = new System.Drawing.Point(243, 96);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(75, 25);
            this.btnAgregar.TabIndex = 11;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = false;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gold;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.pictureCerrar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(514, 27);
            this.panel1.TabIndex = 12;
            // 
            // pictureCerrar
            // 
            this.pictureCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureCerrar.Image = ((System.Drawing.Image)(resources.GetObject("pictureCerrar.Image")));
            this.pictureCerrar.Location = new System.Drawing.Point(491, 3);
            this.pictureCerrar.Name = "pictureCerrar";
            this.pictureCerrar.Size = new System.Drawing.Size(20, 19);
            this.pictureCerrar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureCerrar.TabIndex = 3;
            this.pictureCerrar.TabStop = false;
            this.pictureCerrar.Click += new System.EventHandler(this.pictureCerrar_Click);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Sylfaen", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(176, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(179, 25);
            this.label5.TabIndex = 4;
            this.label5.Text = "Registrar Usuarios";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UsuarioForm
            // 
            this.ClientSize = new System.Drawing.Size(514, 297);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtContrasena);
            this.Controls.Add(this.dgvUsuarios);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbRol);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnEditar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UsuarioForm";
            this.Load += new System.EventHandler(this.UsuarioForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureCerrar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void pictureCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

