using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class PatrimonyIptuMap:EntityMap<Entity.PatrimonyIptu>
    {
        internal PatrimonyIptuMap()
        {
            Map(u => u.Book).ToColumn("patrimony_iptu.book");
            Map(u => u.Craft).ToColumn("patrimony_iptu.craft");
            Map(u => u.Created).ToColumn("patrimony_iptu.created");
            Map(u => u.Date).ToColumn("patrimony_iptu.date");
            Map(u => u.GuideNumber).ToColumn("patrimony_iptu.guide_number");
            Map(u => u.Id).ToColumn("patrimony_iptu.id");
            Map(u => u.IdDocument).ToColumn("patrimony_iptu.id_document");
            Map(u => u.Paper).ToColumn("patrimony_iptu.paper");
            Map(u => u.PatrimonyMunicipalRegistration).ToColumn("patrimony_iptu.patrimony_municipal_registration");
            Map(u => u.RegistryNumber).ToColumn("patrimony_iptu.registry_number");
            Map(u => u.TaxValue).ToColumn("patrimony_iptu.tax_value");
            Map(u => u.TransactionValue).ToColumn("patrimony_iptu.transaction_value");
        }
    }
}
