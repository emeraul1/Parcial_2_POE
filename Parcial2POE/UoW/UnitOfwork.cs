using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using RepoDb;
using Parcial2POE.Repositorios;

namespace Parcial2POE.UoW
{
    public class UnitOfwork : IUnitOfWork
    {
        private readonly SqlConnection _connection;
        private SqlTransaction _transaction;
        public RegistrarNotaRepository _RegistarNotaRepo;

        public UnitOfwork(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
    
        }

        public RegistrarNotaRepository Notas
        {
            get
            {
                if (_RegistarNotaRepo == null)
                {
                    _RegistarNotaRepo = new RegistrarNotaRepository(_connection, _transaction);
                }
                return _RegistarNotaRepo;
            }
        }

        public void Commit()
        {
            _transaction?.Commit();
            _transaction?.Dispose();
            _transaction = null;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }

    }
}
