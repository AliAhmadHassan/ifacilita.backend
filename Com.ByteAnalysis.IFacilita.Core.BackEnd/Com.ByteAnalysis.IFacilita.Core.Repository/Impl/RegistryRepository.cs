using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class RegistryRepository : IRegistryRepository
    {
        IDatabaseSettings databaseSettings;

        public RegistryRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_registry", new { id });
            }
        }

        public Registry Insert(Registry entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    name = entity.Name,
                    idaddress = entity.IdAddress
                };

                entity.Id = conn.ExecuteScalar<int>("spi_registry", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public Registry Update(Registry entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    id = entity.Id,
                    name = entity.Name,
                    idaddress = entity.IdAddress
                };

                conn.Execute("spu_registry", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<Registry> FindAll()
        {
            IEnumerable<Registry> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<Registry>("sps_registry");
            }
            return entities;
        }

        public Registry FindById(int id)
        {
            Registry entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.QueryFirstOrDefault<Registry>("sps_registry_by_pk", new { id });
            }
            return entity;
        }
		public List<Registry> FindByIdAddress (Int32 IdAddress){throw new NotImplementedException();}
    }
}