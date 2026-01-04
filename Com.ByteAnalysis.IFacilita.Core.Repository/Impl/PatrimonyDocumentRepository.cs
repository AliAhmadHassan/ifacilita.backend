using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class PatrimonyDocumentRepository : IPatrimonyDocumentRepository
    {
        IDatabaseSettings databaseSettings;

        public PatrimonyDocumentRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_patrimony_document", new { id });
            }
        }

        public PatrimonyDocument Insert(PatrimonyDocument entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    patrimony_municipal_registration = entity.PatrimonyMunicipalRegistration,
                    iddocument = entity.IdDocument
                };

                conn.Execute("spi_patrimony_document", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public PatrimonyDocument Update(PatrimonyDocument entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    patrimony_municipal_registration = entity.PatrimonyMunicipalRegistration,
                    iddocument = entity.IdDocument
                };

                conn.Execute("spu_patrimony_document", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<PatrimonyDocument> FindAll()
        {
            IEnumerable<PatrimonyDocument> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<PatrimonyDocument>("sps_patrimony_document");
            }
            return entities;
        }

        public PatrimonyDocument FindById(int id)
        {
            PatrimonyDocument entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.Query<PatrimonyDocument, Patrimony, Document, PatrimonyDocument>("sps_patrimony_document_by_pk", param: new { id }, map: (patrimonyDocument, patrimony, document)=>{
                       patrimonyDocument.Patrimony = patrimony;
                       patrimonyDocument.Document = document;
                       return patrimonyDocument;
                    }, 
                    splitOn: "patrimony.municipal_registration, document.id",
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return entity;
        }
		public List<PatrimonyDocument> FindByPatrimonyMunicipalRegistration (String PatrimonyMunicipalRegistration){throw new NotImplementedException();}
    }
}