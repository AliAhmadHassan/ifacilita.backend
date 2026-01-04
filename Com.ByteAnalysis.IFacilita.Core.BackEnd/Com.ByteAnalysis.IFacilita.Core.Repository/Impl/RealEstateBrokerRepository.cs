using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class RealEstateBrokerRepository : IRealEstateBrokerRepository
    {
        IDatabaseSettings databaseSettings;

        public RealEstateBrokerRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_real_estate_broker", new { id });
            }
        }

        public RealEstateBroker Insert(RealEstateBroker entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    real_estate_registered_number = entity.RealEstateRegisteredNumber,
                    broker_registration_number = entity.BrokerRegistrationNumber
                };

                conn.Execute("spi_real_estate_broker", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public RealEstateBroker Update(RealEstateBroker entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    real_estate_registered_number = entity.RealEstateRegisteredNumber,
                    broker_registration_number = entity.BrokerRegistrationNumber
                };

                conn.Execute("spu_real_estate_broker", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<RealEstateBroker> FindAll()
        {
            IEnumerable<RealEstateBroker> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<RealEstateBroker>("sps_real_estate_broker");
            }
            return entities;
        }

        public RealEstateBroker FindById(int id)
        {
            RealEstateBroker entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.QueryFirstOrDefault<RealEstateBroker>("sps_real_estate_broker_by_pk", new { id });
            }
            return entity;
        }
		public List<RealEstateBroker> FindByRealEstateRegisteredNumber (Int64 RealEstateRegisteredNumber){throw new NotImplementedException();}
    }
}