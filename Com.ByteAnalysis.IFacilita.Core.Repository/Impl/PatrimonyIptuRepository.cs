using Com.ByteAnalysis.IFacilita.Core.Entity;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class PatrimonyIptuRepository : IPatrimonyIptuRepository
    {
        IDatabaseSettings databaseSettings;

        public PatrimonyIptuRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_patrimony_iptu", new { id });
            }
        }

        public IEnumerable<PatrimonyIptu> FindAll()
        {
            IEnumerable<PatrimonyIptu> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<PatrimonyIptu>("sps_patrimony_iptu");
            }
            return entities;
        }

        public PatrimonyIptu FindById(int id)
        {
            PatrimonyIptu entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.Query<PatrimonyIptu, Patrimony, PatrimonyIptu>("sps_patrimony_iptu_by_pk", param: new { id = id }, map: (patrimonyIptu, patrimony) => {
                    patrimonyIptu.Patrimony = patrimony;
                    return patrimonyIptu;
                },
                    splitOn: "patrimony.id",
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return entity;
        }

        public PatrimonyIptu FindByPatrimonyMunicipalRegistration(string PatrimonyMunicipalRegistration)
        {
            PatrimonyIptu entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.Query<PatrimonyIptu, Patrimony, PatrimonyIptu>("sps_patrimony_iptu_by_patrimony_municipal_registration", param: new { patrimony_municipal_registration = PatrimonyMunicipalRegistration }, map: (patrimonyIptu, patrimony) => {
                    patrimonyIptu.Patrimony = patrimony;
                    return patrimonyIptu;
                },
                    splitOn: "patrimony.id",
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return entity;
        }

        public PatrimonyIptu Insert(PatrimonyIptu entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new
                {
                    id_document = entity.IdDocument,
                    registry_number = entity.RegistryNumber,
                    craft = entity.Craft,
                    book = entity.Book,
                    paper = entity.Paper,
                    date = entity.Date,
                    guide_number = entity.GuideNumber,
                    created = entity.Created,
                    patrimony_municipal_registration = entity.PatrimonyMunicipalRegistration,
                    transaction_value = entity.TransactionValue,
                    tax_value = entity.TaxValue
                };

                conn.Execute("spi_patrimony_iptu", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public PatrimonyIptu Update(PatrimonyIptu entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new
                {
                    id = entity.Id,
                    id_document = entity.IdDocument,
                    registry_number = entity.RegistryNumber,
                    craft = entity.Craft,
                    book = entity.Book,
                    paper = entity.Paper,
                    date = entity.Date,
                    guide_number = entity.GuideNumber,
                    created = entity.Created,
                    patrimony_municipal_registration = entity.PatrimonyMunicipalRegistration,
                    transaction_value = entity.TransactionValue,
                    tax_value = entity.TaxValue
                };

                conn.Execute("spu_patrimony_iptu", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }
    }
}
