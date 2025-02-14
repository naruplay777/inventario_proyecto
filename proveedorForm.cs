using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace inventario_proyecto
{
    public partial class proveedorForm : Form
    {
        private DataGridView dgvProveedores;
        private Button btnEditar;
        private Button btnEliminar;
        private Button btnLimpiar;
        private Button btnAgregar;
        private TextBox txtDireccion;
        private Label label2;
        private TextBox txtEmail;
        private Label label1;
        private TextBox txtNombre;
        private Label label3;
        private Label label4;
        private TextBox txtTelefono;
        private Panel panel1;
        private PictureBox pictureCerrar;
        private Label label5;
        private Button btnGenerarPDF;
        private DataTable dtProveedores = new DataTable();

        public proveedorForm()
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

        private void proveedorForm_Load(object sender, EventArgs e)
        {
            CargarProveedores();
            dgvProveedores.ClearSelection();
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
        }

        private void CargarProveedores()
        {
            dtProveedores.Clear();
            string query = "SELECT proveedor_id AS ID, nombre, direccion, telefono, email, fecha_registro FROM proveedores WHERE activo = 1";

            using (MySqlConnection conn = ConexionDB.GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    da.Fill(dtProveedores);
                    dgvProveedores.DataSource = dtProveedores;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar proveedores: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            dgvProveedores.ClearSelection();
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
        }

        private void dgvProveedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProveedores.Rows[e.RowIndex];
                txtNombre.Text = row.Cells["nombre"].Value.ToString();
                txtDireccion.Text = row.Cells["direccion"].Value.ToString();
                txtTelefono.Text = row.Cells["telefono"].Value.ToString();
                txtEmail.Text = row.Cells["email"].Value.ToString();
                btnEditar.Enabled = true;
                btnEliminar.Enabled = true;
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvProveedores.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un proveedor para editar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int proveedorId = Convert.ToInt32(dgvProveedores.CurrentRow.Cells["ID"].Value);
            string query = "UPDATE proveedores SET nombre = @nombre, direccion = @direccion, telefono = @telefono, email = @correo WHERE proveedor_id = @id";

            try
            {
                using (MySqlConnection conn = ConexionDB.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text.Trim());
                    cmd.Parameters.AddWithValue("@telefono", txtTelefono.Text.Trim());
                    cmd.Parameters.AddWithValue("@correo", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@id", proveedorId);
                    cmd.ExecuteNonQuery();
                }

                LimpiarCampos();
                CargarProveedores();
                MessageBox.Show("Proveedor actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar proveedor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProveedores.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un proveedor para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(
                "¿Estás seguro de desactivar este proveedor?",
                "Confirmar eliminación lógica",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                int proveedorId = Convert.ToInt32(dgvProveedores.CurrentRow.Cells["ID"].Value);
                string query = "UPDATE proveedores SET activo = 0 WHERE proveedor_id = @id";

                try
                {
                    using (MySqlConnection conn = ConexionDB.GetConnection())
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", proveedorId);
                        cmd.ExecuteNonQuery();
                    }

                    LimpiarCampos();
                    CargarProveedores();
                    MessageBox.Show("Proveedor desactivado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al desactivar proveedor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("El nombre del proveedor es obligatorio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = "INSERT INTO proveedores (nombre, direccion, telefono, email) VALUES (@nombre, @direccion, @telefono, @correo)";

            try
            {
                using (MySqlConnection conn = ConexionDB.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text.Trim());
                    cmd.Parameters.AddWithValue("@telefono", txtTelefono.Text.Trim());
                    cmd.Parameters.AddWithValue("@correo", txtEmail.Text.Trim());
                    cmd.ExecuteNonQuery();
                }

                LimpiarCampos();
                CargarProveedores();
                MessageBox.Show("Proveedor agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar proveedor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(proveedorForm));
            this.dgvProveedores = new System.Windows.Forms.DataGridView();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.txtDireccion = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTelefono = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureCerrar = new System.Windows.Forms.PictureBox();
            this.btnGenerarPDF = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProveedores)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureCerrar)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvProveedores
            // 
            this.dgvProveedores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProveedores.Location = new System.Drawing.Point(12, 218);
            this.dgvProveedores.Name = "dgvProveedores";
            this.dgvProveedores.Size = new System.Drawing.Size(410, 189);
            this.dgvProveedores.TabIndex = 0;
            this.dgvProveedores.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProveedores_CellClick);
            // 
            // btnEditar
            // 
            this.btnEditar.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnEditar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditar.FlatAppearance.BorderSize = 0;
            this.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditar.Location = new System.Drawing.Point(12, 182);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(75, 30);
            this.btnEditar.TabIndex = 1;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = false;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.BackColor = System.Drawing.Color.Red;
            this.btnEliminar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEliminar.FlatAppearance.BorderSize = 0;
            this.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEliminar.Location = new System.Drawing.Point(122, 182);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(75, 30);
            this.btnEliminar.TabIndex = 2;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = false;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnLimpiar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpiar.FlatAppearance.BorderSize = 0;
            this.btnLimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpiar.Location = new System.Drawing.Point(238, 182);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(75, 30);
            this.btnLimpiar.TabIndex = 3;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnAgregar
            // 
            this.btnAgregar.BackColor = System.Drawing.Color.LimeGreen;
            this.btnAgregar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAgregar.FlatAppearance.BorderSize = 0;
            this.btnAgregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgregar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregar.Location = new System.Drawing.Point(347, 182);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(75, 30);
            this.btnAgregar.TabIndex = 4;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = false;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // txtDireccion
            // 
            this.txtDireccion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDireccion.Location = new System.Drawing.Point(220, 67);
            this.txtDireccion.Name = "txtDireccion";
            this.txtDireccion.Size = new System.Drawing.Size(202, 20);
            this.txtDireccion.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Sylfaen", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(217, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 18);
            this.label2.TabIndex = 7;
            this.label2.Text = "Direccion";
            // 
            // txtEmail
            // 
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmail.Location = new System.Drawing.Point(12, 134);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(202, 20);
            this.txtEmail.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Sylfaen", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 18);
            this.label1.TabIndex = 9;
            this.label1.Text = "Email";
            // 
            // txtNombre
            // 
            this.txtNombre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNombre.Location = new System.Drawing.Point(12, 67);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(202, 20);
            this.txtNombre.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Sylfaen", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 18);
            this.label3.TabIndex = 11;
            this.label3.Text = "Nombre";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Sylfaen", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(217, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 18);
            this.label4.TabIndex = 13;
            this.label4.Text = "Telefono";
            // 
            // txtTelefono
            // 
            this.txtTelefono.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTelefono.Location = new System.Drawing.Point(220, 134);
            this.txtTelefono.Name = "txtTelefono";
            this.txtTelefono.Size = new System.Drawing.Size(202, 20);
            this.txtTelefono.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gold;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.pictureCerrar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(434, 26);
            this.panel1.TabIndex = 14;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelBarra_MouseDown);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Sylfaen", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(154, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 21);
            this.label5.TabIndex = 3;
            this.label5.Text = "Proveedores";
            // 
            // pictureCerrar
            // 
            this.pictureCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureCerrar.Image = ((System.Drawing.Image)(resources.GetObject("pictureCerrar.Image")));
            this.pictureCerrar.Location = new System.Drawing.Point(411, 3);
            this.pictureCerrar.Name = "pictureCerrar";
            this.pictureCerrar.Size = new System.Drawing.Size(20, 19);
            this.pictureCerrar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureCerrar.TabIndex = 2;
            this.pictureCerrar.TabStop = false;
            this.pictureCerrar.Click += new System.EventHandler(this.pictureCerrar_Click_1);
            // 
            // btnGenerarPDF
            // 
            this.btnGenerarPDF.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnGenerarPDF.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerarPDF.FlatAppearance.BorderSize = 0;
            this.btnGenerarPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerarPDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerarPDF.Location = new System.Drawing.Point(347, 424);
            this.btnGenerarPDF.Name = "btnGenerarPDF";
            this.btnGenerarPDF.Size = new System.Drawing.Size(75, 30);
            this.btnGenerarPDF.TabIndex = 15;
            this.btnGenerarPDF.Text = "PDF";
            this.btnGenerarPDF.UseVisualStyleBackColor = false;
            // 
            // proveedorForm
            // 
            this.ClientSize = new System.Drawing.Size(434, 466);
            this.Controls.Add(this.btnGenerarPDF);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtTelefono);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDireccion);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.dgvProveedores);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "proveedorForm";
            this.Load += new System.EventHandler(this.proveedorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProveedores)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureCerrar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void pictureCerrar_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
           
        }
        private void panelBarra_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
