using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class PlatformWorkflowRepository : IPlatformWorkflowRepository
    {
        IDatabaseSettings databaseSettings;

        public PlatformWorkflowRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_platform_workflow", new { id });
            }
        }

        public PlatformWorkflow Insert(PlatformWorkflow entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    name = entity.Name,
                    description = entity.Description
                };

                entity.Id = conn.ExecuteScalar<int>("spi_platform_workflow", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public PlatformWorkflow Update(PlatformWorkflow entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    id = entity.Id,
                    name = entity.Name,
                    description = entity.Description
                };

                conn.Execute("spu_platform_workflow", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<PlatformWorkflow> FindAll()
        {
            IEnumerable<PlatformWorkflow> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<PlatformWorkflow>("sps_platform_workflow");
            }
            return entities;
        }

        public PlatformWorkflow FindById(int id)
        {
            PlatformWorkflow entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.QueryFirstOrDefault<PlatformWorkflow>("sps_platform_workflow_by_pk", new { id });
            }
            return entity;
        }
    }
}