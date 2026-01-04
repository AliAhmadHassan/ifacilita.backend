using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class DocumentTypeRepository : IDocumentTypeRepository
    {
        IDatabaseSettings databaseSettings;

        public DocumentTypeRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_document_type", new { id });
            }
        }

        public DocumentType Insert(DocumentType entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    name = entity.Name,
                    description = entity.Description
                };

                entity.Id = conn.ExecuteScalar<int>("spi_document_type", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public DocumentType Update(DocumentType entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    id = entity.Id,
                    name = entity.Name,
                    description = entity.Description
                };

                conn.Execute("spu_document_type", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<DocumentType> FindAll()
        {
            IEnumerable<DocumentType> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<DocumentType>("sps_document_type");
            }
            return entities;
        }

        public DocumentType FindById(int id)
        {
            DocumentType entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.Query<DocumentType>("sps_document_type_by_pk", param: new { id },
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return entity;
        }
    }
}