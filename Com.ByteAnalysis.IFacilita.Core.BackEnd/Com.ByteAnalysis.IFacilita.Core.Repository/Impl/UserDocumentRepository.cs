using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class UserDocumentRepository : IUserDocumentRepository
    {
        IDatabaseSettings databaseSettings;

        public UserDocumentRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_user_document", new { id });
            }
        }

        public UserDocument Insert(UserDocument entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    iduser = entity.IdUser,
                    iddocument = entity.IdDocument
                };

                conn.Execute("spi_user_document", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public UserDocument Update(UserDocument entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    iduser = entity.IdUser,
                    iddocument = entity.IdDocument
                };

                conn.Execute("spu_user_document", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<UserDocument> FindAll()
        {
            IEnumerable<UserDocument> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<UserDocument>("sps_user_document");
            }
            return entities;
        }

        public UserDocument FindById(int id)
        {
            UserDocument entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.QueryFirstOrDefault<UserDocument>("sps_user_document_by_pk", new { id });
            }
            return entity;
        }
		public List<UserDocument> FindByIdUser (Int32 IdUser){throw new NotImplementedException();}
    }
}