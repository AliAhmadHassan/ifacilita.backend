using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class DocumentRepository : IDocumentRepository
    {
        IDatabaseSettings databaseSettings;

        public DocumentRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_document", new { id });
            }
        }

        public Document Insert(Document entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    file_path = entity.FilePath,
                    created = entity.Created,
                    iddocument_type = entity.IdDocumentType
                };

                entity.Id = conn.ExecuteScalar<int>("spi_document", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public Document Update(Document entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    id = entity.Id,
                    file_path = entity.FilePath,
                    created = entity.Created,
                    iddocument_type = entity.IdDocumentType
                };

                conn.Execute("spu_document", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<Document> FindAll()
        {
            IEnumerable<Document> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<Document>("sps_document");
            }
            return entities;
        }

        public Document FindById(int id)
        {
            Document entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.QueryFirstOrDefault<Document>("sps_document_by_pk", new { id });
            }
            return entity;
        }
		public List<Document> FindByIdDocumentType (Int32 IdDocumentType){throw new NotImplementedException();}
    }
}