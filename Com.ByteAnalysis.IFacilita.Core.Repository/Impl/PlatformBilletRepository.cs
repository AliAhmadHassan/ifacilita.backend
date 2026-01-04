using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class PlatformBilletRepository : IPlatformBilletRepository
    {
        IDatabaseSettings databaseSettings;

        public PlatformBilletRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_platform_billet", new { id });
            }
        }

        public PlatformBillet Insert(PlatformBillet entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    our_number = entity.OurNumber,
                    value = entity.Value,
                    due_date = entity.DueDate,
                    created = entity.Created,
                    pay_day = entity.PayDay,
                    paid = entity.Paid,
                    idplatform_billet_bank_data = entity.IdPlatformBilletBankData,
                    iduser = entity.IdUser
                };

                conn.Execute("spi_platform_billet", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public PlatformBillet Update(PlatformBillet entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    our_number = entity.OurNumber,
                    value = entity.Value,
                    due_date = entity.DueDate,
                    created = entity.Created,
                    pay_day = entity.PayDay,
                    paid = entity.Paid,
                    idplatform_billet_bank_data = entity.IdPlatformBilletBankData,
                    iduser = entity.IdUser
                };

                conn.Execute("spu_platform_billet", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<PlatformBillet> FindAll()
        {
            IEnumerable<PlatformBillet> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<PlatformBillet>("sps_platform_billet");
            }
            return entities;
        }

        public PlatformBillet FindById(int id)
        {
            PlatformBillet entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.Query<PlatformBillet, PlatformBilletBankData, User, PlatformBillet>("sps_platform_billet_by_pk", param: new { id }, map: (platformBillet, platformBilletBankData, user)=>{
                       platformBillet.PlatformBilletBankData = platformBilletBankData;
                       platformBillet.User = user;
                       return platformBillet;
                    }, 
                    splitOn: "platform_billet_bank_data.id, user.id",
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return entity;
        }
		public List<PlatformBillet> FindByIdPlatformBilletBankData (Int32 IdPlatformBilletBankData){throw new NotImplementedException();}
    }
}