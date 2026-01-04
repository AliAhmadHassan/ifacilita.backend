using Com.ByteAnalysis.IFacilita.Core.Entity;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class RegistryRepository : IRegistryRepository
    {
        IDatabaseSettings databaseSettings;

        public RegistryRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_registry", new { id });
            }
        }

        public Registry Insert(Registry entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new
                {
                    name = entity.Name,
                    email_1 = entity.Email1,
                    email_2 = entity.Email2,
                    email_3 = entity.Email3,
                    idaddress = entity.IdAddress,
                    city = entity.City
                };

                entity.Id = conn.ExecuteScalar<int>("spi_registry", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public Registry Update(Registry entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new
                {
                    id = entity.Id,
                    name = entity.Name,
                    email_1 = entity.Email1,
                    email_2 = entity.Email2,
                    email_3 = entity.Email3,
                    idaddress = entity.IdAddress,
                    city = entity.City
                };

                conn.Execute("spu_registry", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<Registry> FindAll()
        {
            IEnumerable<Registry> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<Registry, Address, Registry>("sps_registry", map: (registry, address) =>
                {
                    registry.Address = address;
                    return registry;
                },
                    splitOn: "address.id");
            }
            return entities;
        }

        public Registry FindById(int id)
        {
            Registry entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.Query<Registry, Address, Registry>("sps_registry_by_pk", param: new { id }, map: (registry, address) =>
                {
                    registry.Address = address;
                    return registry;
                },
                    splitOn: "address.id",
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return entity;
        }

        public IEnumerable<Registry> FindCloser(int idtransaction)
        {
            IEnumerable<Registry> entities = null;

            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<Registry, Address, Registry>("sps_registry_closer", param: new { idtransaction }, map: (registry, address) =>
                {
                    registry.Address = address;
                    return registry;
                },
                    splitOn: "address.id",
                    commandType: System.Data.CommandType.StoredProcedure);
            }
            return entities;
        }

        public List<Registry> FindByIdAddress(Int32 IdAddress)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Registry> FindByCity(string city)
        {
            IEnumerable<Registry> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<Registry, Address, Registry>("sps_registry_by_city", param: new { city }, map: (registry, address) =>
                {
                    registry.Address = address;
                    return registry;
                },
                    splitOn: "address.id",
                    commandType: System.Data.CommandType.StoredProcedure);
            }
            return entities;
        }
    }
}