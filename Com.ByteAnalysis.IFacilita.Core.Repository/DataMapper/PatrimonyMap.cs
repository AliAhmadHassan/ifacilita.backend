using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class PatrimonyMap: EntityMap<Patrimony>
    {
        internal PatrimonyMap()
        {
            Map(u => u.MunicipalRegistration).ToColumn("patrimony.municipal_registration");
            Map(u => u.Registration).ToColumn("patrimony.registration");
            Map(u => u.Bedrooms).ToColumn("patrimony.bedrooms");
            Map(u => u.MaidsRoom).ToColumn("patrimony.maids_room");
            Map(u => u.NumberOfCarSpaces).ToColumn("patrimony.number_of_car_spaces");
            Map(u => u.ForeiroProperty).ToColumn("patrimony.foreiro_property");
            Map(u => u.BathroomsExceptForMaids).ToColumn("patrimony.bathrooms_except_for_maids");
            Map(u => u.MaidBathroom).ToColumn("patrimony.maid_bathroom");
            Map(u => u.Balcony).ToColumn("patrimony.balcony");
            Map(u => u.FloorPosition).ToColumn("patrimony.floor_position");
            Map(u => u.IdAddress).ToColumn("patrimony.idaddress");
            Map(u => u.IdPatrimonyAcquirerType).ToColumn("patrimony.idpatrimony_acquirer_type");
            Map(u => u.IdPatrimonyTransmitterType).ToColumn("patrimony.idpatrimony_transmitter_type");

            Map(u => u.Elevator).ToColumn("patrimony.elevator");
            Map(u => u.RecreationArea).ToColumn("patrimony.recreation_area");
            Map(u => u.IdItbiRobot).ToColumn("patrimony.iditbi_robot");
            Map(u => u.StreetType).ToColumn("patrimony.street_type");

            Map(u => u.SqlIptu).ToColumn("patrimony.sql_iptu");
            Map(u => u.NotaryNumber).ToColumn("patrimony.notary_number");
            Map(u => u.Dependent).ToColumn("patrimony.dependent");
            Map(u => u.Id).ToColumn("patrimony.id");

            Map(u => u.FinancingType).ToColumn("patrimony.financing_type");
            Map(u => u.ValueFinancing).ToColumn("patrimony.value_financing");
            Map(u => u.TotalityTransfer).ToColumn("patrimony.total_transfer");
            Map(u => u.ProportionTransfer).ToColumn("patrimony.proportion_transfer");
            Map(u => u.PublicScripture).ToColumn("patrimony.public_scripture");
            Map(u => u.DateEventScripture).ToColumn("patrimony.date_event_scripture");
            Map(u => u.ScriptureNotesOffice).ToColumn("patrimony.scripture_notes_office");
            Map(u => u.ScriptureUf).ToColumn("patrimony.scripture_uf");
            Map(u => u.ScriptureCity).ToColumn("patrimony.scripture_city");
            Map(u => u.CondominiumDeclarationDebts).ToColumn("patrimony.condominium_declaration_debts");

        }
    }
}
