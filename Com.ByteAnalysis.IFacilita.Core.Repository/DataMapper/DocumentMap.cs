using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class DocumentMap: EntityMap<Document>
    {
        internal DocumentMap()
        {
            Map(u => u.Id).ToColumn("document.id");
            Map(u => u.FilePath).ToColumn("document.file_path");
            Map(u => u.Created).ToColumn("document.created");
            Map(u => u.IdDocumentType).ToColumn("document.iddocument_type");
        }
    }
}
