using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RepoDb;
using Microsoft.Data.SqlClient;
//Emerson Raúl Ventura Castillo. U20241017 Autoevaluación: 9.5


namespace Parcial2POE
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SqlServerBootstrap.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmRegistro());
        }
    }
}
