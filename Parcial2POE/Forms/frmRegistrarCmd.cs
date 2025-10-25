using Parcial2POE.Clases;
using Parcial2POE.UoW;
using RepoDb;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parcial2POE.Forms
{
    public partial class frmRegistrarCmd : Form
    {
        private readonly string _connectionString;
        private readonly int _id;
        public frmRegistrarCmd(int id, string connectionString)
        {
            InitializeComponent();
            _id = id;
            _connectionString = connectionString;

  
        }

        private void frmRegistrarCmd_Load(object sender, EventArgs e)
        {
           
            txtCodigo.Enabled = false;

            using (var uow = new UnitOfwork(_connectionString))
            {
                
                var nota = uow.Notas.GetById(_id);

                if (nota == null)
                {
                    MessageBox.Show("Registro no encontrado.");
                    this.Close();
                    return;
                }

                // Cargar los datos en los controles
                txtCodigo.Text = nota.Codigo;
                txtNombre.Text = nota.Nombre;
                txtApellido.Text = nota.Apellido;
                txtLaboratorio.Text = nota.Laboratorio.ToString("0.00");
                txtParcial.Text = nota.Parcial.ToString("0.00");
                txtAsistencia.Text = nota.Asistencia.ToString("0.00");

               
            }
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

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text) ||
        string.IsNullOrWhiteSpace(txtNombre.Text) ||
        string.IsNullOrWhiteSpace(txtApellido.Text) ||
        string.IsNullOrWhiteSpace(txtLaboratorio.Text) ||
        string.IsNullOrWhiteSpace(txtParcial.Text) ||
        string.IsNullOrWhiteSpace(txtAsistencia.Text))
            {
                MessageBox.Show("Por favor, completa todos los campos.");
                return;
            }

            if (!TryNota(txtLaboratorio.Text, out var lab) ||
                !TryNota(txtParcial.Text, out var par) ||
                !TryNota(txtAsistencia.Text, out var asis))
            {
                MessageBox.Show("Cada nota debe ser decimal entre 0.00 y 10.00 ");
                return;
            }

            // Mayúsculas en textos
            var codigo = txtCodigo.Text.Trim().ToUpper();
            var nombre = txtNombre.Text.Trim().ToUpper();
            var apellido = txtApellido.Text.Trim().ToUpper();

            // Calcular nota final
            var notaFinal = CalcularNotaFinal(lab, par, asis);

            var entidad = new RegistrarNota
            {
                Id = _id,                 
                Codigo = codigo,
                Nombre = nombre,
                Apellido = apellido,
                Laboratorio = lab,
                Parcial = par,
                Asistencia = asis,
                NotaFinal = notaFinal,
                FechaModificacion = DateTime.Now
            };

            var camposActualizar = new List<Field>
            {
                new Field(nameof(RegistrarNota.Codigo)),
                new Field(nameof(RegistrarNota.Nombre)),
                new Field(nameof(RegistrarNota.Apellido)),
                new Field(nameof(RegistrarNota.Laboratorio)),
                new Field(nameof(RegistrarNota.Parcial)),
                new Field(nameof(RegistrarNota.Asistencia)),
                new Field(nameof(RegistrarNota.NotaFinal)),
                new Field(nameof(RegistrarNota.FechaModificacion))
            };

            using (var uow = new UnitOfwork(_connectionString))
            {
                uow.Notas.Update(entidad, camposActualizar);
                uow.Commit();
                MessageBox.Show("Registro actualizado correctamente.");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
