using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using RepoDb;
using Parcial2POE.UoW;
using Parcial2POE.Clases;
using Parcial2POE.Forms;
//Emerson Raúl Ventura Castillo. U20241017 Autoevaluación: 9.5
namespace Parcial2POE
{
    public partial class frmRegistro : Form
    {

        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["conexionBD"].ConnectionString;
        public frmRegistro()
        {
            InitializeComponent();
            
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmRegistro_Load(object sender, EventArgs e)
        {
            LimpiarControles();
            CargarGrid();

            //Automatiucamente en mayusculas
            txtCodigo.CharacterCasing = CharacterCasing.Upper;
            txtNombre.CharacterCasing = CharacterCasing.Upper;
            txtApellido.CharacterCasing = CharacterCasing.Upper;
        }

        private void CargarGrid()
        {
            using (var uow = new UnitOfwork(_connectionString))
            {
                var lista = uow.Notas.GetByEstado(true).ToList();
                dgvRegistro.DataSource = lista;
                dgvRegistro.Columns["Id"].Visible = false;
                dgvRegistro.Columns["Activo"].Visible = false;
                dgvRegistro.Columns["FechaCreacion"].Visible = false;
                dgvRegistro.Columns["FechaModificacion"].Visible = false;
                dgvRegistro.Refresh();
            }
        }

        private void LimpiarControles()
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtLaboratorio.Clear();
            txtParcial.Clear();
            txtAsistencia.Clear();
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Validar si solo es letra 
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Solo se permiten letras", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtApellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Validar si solo es letra 
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Solo se permiten letras", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SoloNumerosDecimales(KeyPressEventArgs e, TextBox txt)
        {
            // Validar que solo sean números decimales
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true; // Bloquea cualquier otra tecla
                MessageBox.Show("Solo se permiten números decimales", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Permitir solo un punto decimal
            if (e.KeyChar == '.' && txt.Text.Contains('.'))
            {
                e.Handled = true;
                MessageBox.Show("Solo se permite un punto decimal", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void txtLaboratorio_KeyPress(object sender, KeyPressEventArgs e)
        {
            SoloNumerosDecimales(e, (TextBox)sender);
        }

        private void txtParcial_KeyPress(object sender, KeyPressEventArgs e)
        {
            SoloNumerosDecimales(e, (TextBox)sender);
        }

        private void txtAsistencia_KeyPress(object sender, KeyPressEventArgs e)
        {
            SoloNumerosDecimales(e, (TextBox)sender);
        }
        private bool TryNota(string s, out decimal valor)
        {
            return decimal.TryParse(s, out valor) && valor >= 0m && valor <= 10m;
        }
       

        private decimal CalcularNotaFinal(decimal lab, decimal par, decimal asis)
        {
              return (lab * 0.5m) + (par * 0.4m) + (asis * 0.1m);
        }
            
        private void button1_click(object sender, EventArgs e)
        {
            // Validar campos vacíos
            if (string.IsNullOrWhiteSpace(txtCodigo.Text) ||
                string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtLaboratorio.Text) ||
                string.IsNullOrWhiteSpace(txtParcial.Text) ||
                string.IsNullOrWhiteSpace(txtAsistencia.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.");
                return;
            }

            // Validar rango 0..10
            if (!TryNota(txtLaboratorio.Text, out var lab) ||
                !TryNota(txtParcial.Text, out var par) ||
                !TryNota(txtAsistencia.Text, out var asis))
            {
                MessageBox.Show("Cada nota debe ser decimal entre 0.00 y 10.00 ");
                return;
            }

            var notaFinal = CalcularNotaFinal(lab, par, asis);
            // Convertir a mayusculas
            var codigo = txtCodigo.Text.Trim().ToUpper();
            var nombre = txtNombre.Text.Trim().ToUpper();
            var apellido = txtApellido.Text.Trim().ToUpper();

            var entidad = new RegistrarNota
            {
                Codigo = txtCodigo.Text.Trim(),
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Laboratorio = lab,
                Parcial = par,
                Asistencia = asis,
                NotaFinal = notaFinal,
                Activo = true,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now
            };

            using (var uow = new UnitOfwork(_connectionString))
            {
                uow.Notas.Insert(entidad);
                uow.Commit();
            }

            MessageBox.Show("Registro agregado correctamente.");
            LimpiarControles();
            CargarGrid();
        }

        private void dgvRegistro_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                var registro = (RegistrarNota)dgvRegistro.Rows[e.RowIndex].DataBoundItem;

                using (var frmActualizar = new frmRegistrarCmd(registro.Id, _connectionString))
                {
                    var result = frmActualizar.ShowDialog();
                    if(result == DialogResult.OK)
                    {
                        CargarGrid();
                    }
                }
            }
        }
    }
}
