using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class PatrimonyRepository : IPatrimonyRepository
    {
        IDatabaseSettings databaseSettings;

        public PatrimonyRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_patrimony", new { id });
            }
        }

        public Patrimony Insert(Patrimony entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    municipal_registration = entity.MunicipalRegistration,
                    registration = entity.Registration,
                    bedrooms = entity.Bedrooms,
                    maids_room = entity.MaidsRoom,
                    number_of_car_spaces = entity.NumberOfCarSpaces,
                    foreiro_property = entity.ForeiroProperty,
                    bathrooms_except_for_maids = entity.BathroomsExceptForMaids,
                    maid_bathroom = entity.MaidBathroom,
                    balcony = entity.Balcony,
                    floor_position = entity.FloorPosition,
                    idaddress = entity.IdAddress
                };

                conn.Execute("spi_patrimony", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public Patrimony Update(Patrimony entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    municipal_registration = entity.MunicipalRegistration,
                    registration = entity.Registration,
                    bedrooms = entity.Bedrooms,
                    maids_room = entity.MaidsRoom,
                    number_of_car_spaces = entity.NumberOfCarSpaces,
                    foreiro_property = entity.ForeiroProperty,
                    bathrooms_except_for_maids = entity.BathroomsExceptForMaids,
                    maid_bathroom = entity.MaidBathroom,
                    balcony = entity.Balcony,
                    floor_position = entity.FloorPosition,
                    idaddress = entity.IdAddress
                };

                conn.Execute("spu_patrimony", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<Patrimony> FindAll()
        {
            IEnumerable<Patrimony> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<Patrimony>("sps_patrimony");
            }
            return entities;
        }

        public Patrimony FindById(int id)
        {
            Patrimony entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.QueryFirstOrDefault<Patrimony>("sps_patrimony_by_pk", new { id });
            }
            return entity;
        }
		public List<Patrimony> FindByIdAddress (Int32 IdAddress){throw new NotImplementedException();}
    }
}