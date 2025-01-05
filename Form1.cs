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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Puedes inicializar algo aquí si lo necesitas.
        }

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
    }
}