using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class PlatformSubWorkflowRepository : IPlatformSubWorkflowRepository
    {
        IDatabaseSettings databaseSettings;

        public PlatformSubWorkflowRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_platform_sub_workflow", new { id });
            }
        }

        public PlatformSubWorkflow Insert(PlatformSubWorkflow entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    name = entity.Name,
                    description = entity.Description,
                    idplatform_workflow = entity.IdPlatformWorkflow
                };

                entity.Id = conn.ExecuteScalar<int>("spi_platform_sub_workflow", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public PlatformSubWorkflow Update(PlatformSubWorkflow entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    id = entity.Id,
                    name = entity.Name,
                    description = entity.Description,
                    idplatform_workflow = entity.IdPlatformWorkflow
                };

                conn.Execute("spu_platform_sub_workflow", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<PlatformSubWorkflow> FindAll()
        {
            IEnumerable<PlatformSubWorkflow> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<PlatformSubWorkflow>("sps_platform_sub_workflow");
            }
            return entities;
        }

        public PlatformSubWorkflow FindById(int id)
        {
            PlatformSubWorkflow entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.Query<PlatformSubWorkflow, PlatformWorkflow, PlatformSubWorkflow>("sps_platform_sub_workflow_by_pk", param: new { id }, map: (platformSubWorkflow, platformWorkflow)=>{
                       platformSubWorkflow.PlatformWorkflow = platformWorkflow;
                       return platformSubWorkflow;
                    }, 
                    splitOn: "platform_workflow.id",
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return entity;
        }
		public List<PlatformSubWorkflow> FindByIdPlatformWorkflow (Int32 IdPlatformWorkflow){throw new NotImplementedException();}
    }
}