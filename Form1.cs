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
using System.Runtime.InteropServices;
using iTextSharp.text;
using iTextSharp.text.pdf;



namespace inventario_proyecto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // esto es para que el diseño se puda mover con mouse
        [DllImport("user32.Dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.Dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        // Método para probar la conexión al hacer clic en un botón.

        private void btnTestConnection_Click_1(object sender, EventArgs e)
        {
            // Cadena de conexión: ajusta los valores según tu configuración.
            string connectionString = "Server=localhost;Database=inventario_heladeria;Uid=root;Pwd=andrewserver;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("¡Conexión exitosa a la base de datos!", "Conexión", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void formularioP_Click(object sender, EventArgs e)
        {

            // Crear una nueva instancia del Formulario1
            FormularioProductos formulario1 = new FormularioProductos();

            // Mostrar el Formulario1
            //formulario1.Show();

            // Cerrar Formulario2
            //this.Hide();

            AbrirFormButon(new FormularioProductos());
        }

        private void pictureCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureMaximizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            pictureMaximizar.Visible = false;
            pictureRetaurar.Visible = true;
        }

        private void pictureRetaurar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            pictureRetaurar.Visible = false;
            pictureMaximizar.Visible = true;
        }

        private void panelBarra_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void AbrirFormButon(object formbuton)
        {

            if (this.panelContenedor.Controls.Count > 0)
                this.panelContenedor.Controls.RemoveAt(0);
            Form fb = formbuton as Form;
            fb.TopLevel = false;
            fb.Dock = DockStyle.Fill;
            this.panelContenedor.Controls.Add(fb);
            this.panelContenedor.Tag = fb;
            fb.Show();


        }

        private void panelBarra_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //esto es para dar la hora y fecha en el diseño
            labelHora.Text = DateTime.Now.ToString("h:mm:ss");
            labelfecha.Text = DateTime.Now.ToShortDateString();
        }

        private void labelfecha_Click(object sender, EventArgs e)
        {

        }
    }
}