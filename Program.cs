using System;
using System.Windows.Forms;

namespace inventario_proyecto
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new FormularioProductos(loginForm.RolUsuario));
                }
            }
        }
    }
}


