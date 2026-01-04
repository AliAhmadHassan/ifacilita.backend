using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class BrokerRepository : IBrokerRepository
    {
        IDatabaseSettings databaseSettings;

        public BrokerRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_broker", new { id });
            }
        }

        public Broker Insert(Broker entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    registration_number = entity.RegistrationNumber
                };

                conn.Execute("spi_broker", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public Broker Update(Broker entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    registration_number = entity.RegistrationNumber
                };

                conn.Execute("spu_broker", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<Broker> FindAll()
        {
            IEnumerable<Broker> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<Broker>("sps_broker");
            }
            return entities;
        }

        public Broker FindById(int id)
        {
            Broker entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.Query<Broker>("sps_broker_by_pk", param: new { id },
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return entity;
        }
    }
}