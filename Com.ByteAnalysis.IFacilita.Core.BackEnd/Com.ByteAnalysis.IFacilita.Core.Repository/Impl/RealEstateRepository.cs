using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class RealEstateRepository : IRealEstateRepository
    {
        IDatabaseSettings databaseSettings;

        public RealEstateRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_real_estate", new { id });
            }
        }

        public RealEstate Insert(RealEstate entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    registered_number = entity.RegisteredNumber,
                    corporate_name = entity.CorporateName
                };

                conn.Execute("spi_real_estate", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public RealEstate Update(RealEstate entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    registered_number = entity.RegisteredNumber,
                    corporate_name = entity.CorporateName
                };

                conn.Execute("spu_real_estate", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<RealEstate> FindAll()
        {
            IEnumerable<RealEstate> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<RealEstate>("sps_real_estate");
            }
            return entities;
        }

        public RealEstate FindById(int id)
        {
            RealEstate entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.QueryFirstOrDefault<RealEstate>("sps_real_estate_by_pk", new { id });
            }
            return entity;
        }
    }
}