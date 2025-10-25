using Parcial2POE.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepoDb;

namespace Parcial2POE.UoW
{
    public  interface IUnitOfWork : IDisposable
    {
        RegistrarNotaRepository Notas { get; }    
        void Commit();

    }
}
