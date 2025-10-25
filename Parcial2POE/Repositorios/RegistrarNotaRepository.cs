using Parcial2POE.Clases;
using RepoDb;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Emerson Raúl Ventura Castillo. U20241017 Autoevaluación: 9.5

namespace Parcial2POE.Repositorios
{
    public class RegistrarNotaRepository
    {
        private readonly SqlConnection _connection;
        private readonly SqlTransaction _transaction;

        public RegistrarNotaRepository(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public RegistrarNota GetById(int id)
        {
            return _connection.Query<RegistrarNota>(where : e => e.Id == id, transaction: _transaction).FirstOrDefault();
        }

        public int Insert(RegistrarNota registrarNota)
        {
            return (int)_connection.Insert(registrarNota, transaction: _transaction);
        }

        public IEnumerable<RegistrarNota> GetAll()
        {
            return _connection.QueryAll<RegistrarNota>(transaction: _transaction);
        }

        public int Update(RegistrarNota registrarNota, IEnumerable<Field> campos)
        {
            return _connection.Update<RegistrarNota>(registrarNota, fields: campos, transaction: _transaction);
        }

        public IEnumerable<RegistrarNota> GetByEstado(bool estado)
        {
            return _connection.Query<RegistrarNota>(where: e => e.Activo == estado, transaction: _transaction);
        }
    }


}
