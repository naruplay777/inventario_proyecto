using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace inventario_proyecto
{
    public partial class LoginForm : Form
    {
        public string RolUsuario { get; private set; }

        public LoginForm()
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string contrasena = txtContrasena.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(contrasena))
            {
                MessageBox.Show("Por favor, ingrese su nombre y contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = "SELECT rol FROM usuarios WHERE nombre = @nombre AND contrasena = @contrasena";

            using (MySqlConnection conn = ConexionDB.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@contrasena", contrasena);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    RolUsuario = result.ToString();
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Nombre o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.txtContrasena = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureCerrar = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureMinimizar = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureCerrar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureMinimizar)).BeginInit();
            this.SuspendLayout();
            // 
            // txtNombre
            // 
            this.txtNombre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNombre.ForeColor = System.Drawing.Color.DimGray;
            this.txtNombre.Location = new System.Drawing.Point(274, 72);
            this.txtNombre.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(357, 20);
            this.txtNombre.TabIndex = 0;
            this.txtNombre.Text = "Usuario";
            this.txtNombre.Enter += new System.EventHandler(this.txtNombre_Enter);
            this.txtNombre.Leave += new System.EventHandler(this.txtNombre_Leave);
            // 
            // txtContrasena
            // 
            this.txtContrasena.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtContrasena.ForeColor = System.Drawing.Color.DimGray;
            this.txtContrasena.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtContrasena.Location = new System.Drawing.Point(274, 135);
            this.txtContrasena.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtContrasena.Name = "txtContrasena";
            this.txtContrasena.Size = new System.Drawing.Size(357, 20);
            this.txtContrasena.TabIndex = 1;
            this.txtContrasena.Text = "Contraseña";
            this.txtContrasena.Enter += new System.EventHandler(this.txtContraseña_Enter);
            this.txtContrasena.Leave += new System.EventHandler(this.txtContraseña_Leave);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.LightGray;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Location = new System.Drawing.Point(274, 193);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(357, 30);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "ACCEDER";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gold;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(219, 248);
            this.panel1.TabIndex = 5;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(219, 248);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // pictureCerrar
            // 
            this.pictureCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureCerrar.Image = ((System.Drawing.Image)(resources.GetObject("pictureCerrar.Image")));
            this.pictureCerrar.Location = new System.Drawing.Point(663, 0);
            this.pictureCerrar.Name = "pictureCerrar";
            this.pictureCerrar.Size = new System.Drawing.Size(20, 19);
            this.pictureCerrar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureCerrar.TabIndex = 6;
            this.pictureCerrar.TabStop = false;
            this.pictureCerrar.Click += new System.EventHandler(this.pictureCerrar_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(403, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 31);
            this.label3.TabIndex = 7;
            this.label3.Text = "LOGIN";
            // 
            // pictureMinimizar
            // 
            this.pictureMinimizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureMinimizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureMinimizar.Image = ((System.Drawing.Image)(resources.GetObject("pictureMinimizar.Image")));
            this.pictureMinimizar.Location = new System.Drawing.Point(637, 0);
            this.pictureMinimizar.Name = "pictureMinimizar";
            this.pictureMinimizar.Size = new System.Drawing.Size(20, 19);
            this.pictureMinimizar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureMinimizar.TabIndex = 8;
            this.pictureMinimizar.TabStop = false;
            this.pictureMinimizar.Click += new System.EventHandler(this.pictureMinimizar_Click);
            // 
            // LoginForm
            // 
            this.ClientSize = new System.Drawing.Size(682, 248);
            this.Controls.Add(this.pictureMinimizar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureCerrar);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtContrasena);
            this.Controls.Add(this.txtNombre);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureCerrar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureMinimizar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private Button btnLogin;
        private Panel panel1;
        private PictureBox pictureBox1;
        private PictureBox pictureCerrar;
        private Label label3;
        private TextBox txtNombre;
        private PictureBox pictureMinimizar;
        private TextBox txtContrasena;

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
        private void txtNombre_Enter(object sender, EventArgs e)
        {
            if (txtNombre.Text == "Usuario")
            {
                txtNombre.Text = "";
                txtNombre.ForeColor = Color.Black;

            }
        }

        private void txtNombre_Leave(object sender, EventArgs e)
        {
            if (txtNombre.Text == "")
            {
                txtNombre.Text = "Usuario";
                txtNombre.ForeColor = Color.Black;
            }
        }

        private void txtContraseña_Enter(object sender, EventArgs e)
        {
            if (txtContrasena.Text == "Contraseña")
            {

                txtContrasena.Text = "";
                txtContrasena.ForeColor = Color.Black;
                txtContrasena.UseSystemPasswordChar = true;

            }
        }

        private void txtContraseña_Leave(object sender, EventArgs e)
        {
            if (txtContrasena.Text == "")
            {
                txtContrasena.Text = "Contraseña";
                txtContrasena.ForeColor = Color.Black;
                txtContrasena.UseSystemPasswordChar = true;
            }
        }

        private void pictureCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}


