using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class UserDocumentMap: EntityMap<UserDocument>
    {
        internal UserDocumentMap()
        {
            Map(u => u.IdUser).ToColumn("iduser");
            Map(u => u.IdDocument).ToColumn("iddocument");
        }
    }
}
