using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class DocumentTypeMap: EntityMap<DocumentType>
    {
        internal DocumentTypeMap()
        {
            Map(u => u.Id).ToColumn("document_type.id");
            Map(u => u.Name).ToColumn("document_type.name");
            Map(u => u.Description).ToColumn("document_type.description");
        }
    }
}
