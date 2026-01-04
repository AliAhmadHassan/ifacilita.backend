using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Dapper;
using System.Data.SqlClient;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class HistoricRepository : IHistoricRepository
    {
        IDatabaseSettings databaseSettings;

        public HistoricRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using(SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Open();
                conn.Execute("spd_historic", new { id }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<Historic> FindAll()
        {
            IEnumerable<Historic> entities;
            using(SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Open();
                entities = conn.Query<Historic>("sps_historic", commandType: System.Data.CommandType.StoredProcedure);
            }

            return entities;
        }

        public Historic FindById(int id)
        {
            Historic entity;
            using(SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Open();
                entity = conn.Query<Historic, User, Transaction, Historic>("sps_historic_by_pk", param: new { id }, commandType: System.Data.CommandType.StoredProcedure,
                    map: (historic, user, transaction) =>
                    {
                        historic.User = user;
                        historic.Transaction = transaction;
                        return historic;
                    }, splitOn: "user.id, transaction.id").FirstOrDefault();
            }
            return entity;
        }

        public IEnumerable<Historic> FindByIdTransaction(int idTransaction)
        {
            IEnumerable<Historic> entities;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Open();
                entities = conn.Query<Historic, User, Transaction, Historic>("sps_historic_by_idtransaction", param: new { idTransaction }, commandType: System.Data.CommandType.StoredProcedure,
                    map: (historic, user, transaction) =>
                    {
                        historic.User = user;
                        historic.Transaction = transaction;
                        return historic;
                    }, splitOn: "user.id, transaction.id");
            }
            return entities;
        }


        public Historic Insert(Historic entity)
        {
            using(SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Open();
                entity.Id = conn.ExecuteScalar<int>("spi_historic", param: new
                {
                    description = entity.Description,
                    iduser = entity.IdUser,
                    created = entity.Created,
                    idtransaction = entity.IdTransaction,
                    topic = entity.Topic
                }, commandType: System.Data.CommandType.StoredProcedure);

            }
            return entity;
        }

        public Historic Update(Historic entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Open();
                conn.Execute("spu_historic", param: new
                {
                    id = entity.Id,
                    description = entity.Description,
                    iduser = entity.IdUser,
                    created = entity.Created,
                    idtransaction = entity.IdTransaction,
                    topic = entity.Topic
                }, commandType: System.Data.CommandType.StoredProcedure);

            }
            return entity;
        }
    }
}
