using Com.ByteAnalysis.IFacilita.Core.Entity;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

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
                    idaddress = entity.IdAddress,
                    idpatrimony_acquirer_type = entity.IdPatrimonyAcquirerType,
                    idpatrimony_transmitter_type = entity.IdPatrimonyTransmitterType,
                    elevator = entity.Elevator,
                    recreation_area = entity.RecreationArea,
                    iditbi_robot = entity.IdItbiRobot,
                    street_type = entity.StreetType,
                    sql_iptu = entity.SqlIptu,
                    notary_number = entity.NotaryNumber,
                    dependent = entity.Dependent,
                    financing_type = entity.FinancingType,
                    value_financing = entity.ValueFinancing,
                    total_transfer = entity.TotalityTransfer,
                    proportion_transfer = entity.ProportionTransfer,
                    public_scripture = entity.PublicScripture,
                    date_event_scripture = entity.DateEventScripture,
                    scripture_notes_office = entity.ScriptureNotesOffice,
                    scripture_uf = entity.ScriptureUf,
                    scripture_city = entity.ScriptureCity,
                    condominium_declaration_debts = entity.CondominiumDeclarationDebts
                };

                entity.Id = conn.ExecuteScalar<int>("spi_patrimony", values, commandType: System.Data.CommandType.StoredProcedure);
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
                    idaddress = entity.IdAddress,
                    idpatrimony_acquirer_type = entity.IdPatrimonyAcquirerType,
                    idpatrimony_transmitter_type = entity.IdPatrimonyTransmitterType,
                    elevator = entity.Elevator,
                    recreation_area = entity.RecreationArea,
                    iditbi_robot = entity.IdItbiRobot,
                    street_type = entity.StreetType,
                    sql_iptu = entity.SqlIptu,
                    notary_number = entity.NotaryNumber,
                    dependent = entity.Dependent,
                    id = entity.Id,

                    financing_type = entity.FinancingType,
                    value_financing = entity.ValueFinancing,
                    total_transfer = entity.TotalityTransfer,
                    proportion_transfer = entity.ProportionTransfer,
                    public_scripture = entity.PublicScripture,
                    date_event_scripture = entity.DateEventScripture,
                    scripture_notes_office = entity.ScriptureNotesOffice,
                    scripture_uf = entity.ScriptureUf,
                    scripture_city = entity.ScriptureCity,
                    condominium_declaration_debts = entity.CondominiumDeclarationDebts
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
                entity = conn.Query<Patrimony, Address, Patrimony>("sps_patrimony_by_pk", param: new { id = id }, map: (patrimony, address)=>{
                       patrimony.Address = address;
                       return patrimony;
                    }, 
                    splitOn: "address.id",
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return entity;
        }
		public List<Patrimony> FindByIdAddress (Int32 IdAddress){throw new NotImplementedException();}

    }
}