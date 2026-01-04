using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class PatrimonyAcquirerTypeMap: EntityMap<PatrimonyAcquirerType>
    {
        internal PatrimonyAcquirerTypeMap()
        {
            Map(u => u.Id).ToColumn("patrimony_acquirer_type.id");
            Map(u => u.Description).ToColumn("patrimony_acquirer_type.description");
        }
    }
}
