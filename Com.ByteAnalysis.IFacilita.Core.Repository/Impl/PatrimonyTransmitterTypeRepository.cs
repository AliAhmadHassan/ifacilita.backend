using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class PatrimonyTransmitterTypeRepository : IPatrimonyTransmitterTypeRepository
    {
        IDatabaseSettings databaseSettings;

        public PatrimonyTransmitterTypeRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_patrimony_transmitter_type", new { id });
            }
        }

        public PatrimonyTransmitterType Insert(PatrimonyTransmitterType entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    description = entity.Description
                };

                entity.Id = conn.ExecuteScalar<int>("spi_patrimony_transmitter_type", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public PatrimonyTransmitterType Update(PatrimonyTransmitterType entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    id = entity.Id,
                    description = entity.Description
                };

                conn.Execute("spu_patrimony_transmitter_type", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<PatrimonyTransmitterType> FindAll()
        {
            IEnumerable<PatrimonyTransmitterType> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<PatrimonyTransmitterType>("sps_patrimony_transmitter_type");
            }
            return entities;
        }

        public PatrimonyTransmitterType FindById(int id)
        {
            PatrimonyTransmitterType entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.Query<PatrimonyTransmitterType>("sps_patrimony_transmitter_type_by_pk", param: new { id },
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return entity;
        }
    }
}