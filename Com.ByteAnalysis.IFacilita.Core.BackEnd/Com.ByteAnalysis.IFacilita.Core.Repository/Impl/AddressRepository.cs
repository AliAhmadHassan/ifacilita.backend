using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class AddressRepository : IAddressRepository
    {
        IDatabaseSettings databaseSettings;

        public AddressRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_address", new { id });
            }
        }

        public Address Insert(Address entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    street = entity.Street,
                    number = entity.Number,
                    complement = entity.Complement,
                    district = entity.District,
                    zip_code = entity.ZipCode,
                    city_sig = entity.CitySig
                };

                entity.Id = conn.ExecuteScalar<int>("spi_address", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public Address Update(Address entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    id = entity.Id,
                    street = entity.Street,
                    number = entity.Number,
                    complement = entity.Complement,
                    district = entity.District,
                    zip_code = entity.ZipCode,
                    city_sig = entity.CitySig
                };

                conn.Execute("spu_address", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<Address> FindAll()
        {
            IEnumerable<Address> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<Address>("sps_address");
            }
            return entities;
        }

        public Address FindById(int id)
        {
            Address entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.QueryFirstOrDefault<Address>("sps_address_by_pk", new { id });
            }
            return entity;
        }
		public List<Address> FindByCitySig (String CitySig){throw new NotImplementedException();}
    }
}