using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class PatrimonyDocumentMap: EntityMap<PatrimonyDocument>
    {
        internal PatrimonyDocumentMap()
        {
            Map(u => u.PatrimonyMunicipalRegistration).ToColumn("patrimony_document.patrimony_municipal_registration");
            Map(u => u.IdDocument).ToColumn("patrimony_document.iddocument");
        }
    }
}
