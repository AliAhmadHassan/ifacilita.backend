using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class CityRepository : ICityRepository
    {
        IDatabaseSettings databaseSettings;

        public CityRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_city", new { id });
            }
        }

        public City Insert(City entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    sig = entity.Sig,
                    description = entity.Description
                };

                conn.Execute("spi_city", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public City Update(City entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    sig = entity.Sig,
                    description = entity.Description
                };

                conn.Execute("spu_city", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<City> FindAll()
        {
            IEnumerable<City> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<City>("sps_city");
            }
            return entities;
        }

        public City FindById(int id)
        {
            City entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.QueryFirstOrDefault<City>("sps_city_by_pk", new { id });
            }
            return entity;
        }
    }
}