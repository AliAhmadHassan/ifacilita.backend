using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class PatrimonyAcquirerTypeRepository : IPatrimonyAcquirerTypeRepository
    {
        IDatabaseSettings databaseSettings;

        public PatrimonyAcquirerTypeRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_patrimony_acquirer_type", new { id });
            }
        }

        public PatrimonyAcquirerType Insert(PatrimonyAcquirerType entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    description = entity.Description
                };

                entity.Id = conn.ExecuteScalar<int>("spi_patrimony_acquirer_type", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public PatrimonyAcquirerType Update(PatrimonyAcquirerType entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    id = entity.Id,
                    description = entity.Description
                };

                conn.Execute("spu_patrimony_acquirer_type", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<PatrimonyAcquirerType> FindAll()
        {
            IEnumerable<PatrimonyAcquirerType> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<PatrimonyAcquirerType>("sps_patrimony_acquirer_type");
            }
            return entities;
        }

        public PatrimonyAcquirerType FindById(int id)
        {
            PatrimonyAcquirerType entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.Query<PatrimonyAcquirerType>("sps_patrimony_acquirer_type_by_pk", param: new { id },
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return entity;
        }
    }
}