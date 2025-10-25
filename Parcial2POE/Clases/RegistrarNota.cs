using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepoDb;

namespace Parcial2POE.Clases
{
    public class RegistrarNota
    {
        
            public int Id { get; set; }
            public string Codigo { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public decimal Laboratorio { get; set; }
            public decimal Parcial { get; set; }
            public decimal Asistencia { get; set; }
            public decimal NotaFinal { get; set; }
            public DateTime FechaCreacion { get; set; }
            public DateTime FechaModificacion { get; set; }
            public bool Activo { get; set; }
    }
}
