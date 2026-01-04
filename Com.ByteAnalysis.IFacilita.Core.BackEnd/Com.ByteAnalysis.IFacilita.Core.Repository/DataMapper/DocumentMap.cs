using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class DocumentMap: EntityMap<Document>
    {
        internal DocumentMap()
        {
            Map(u => u.Id).ToColumn("id");
            Map(u => u.FilePath).ToColumn("file_path");
            Map(u => u.Created).ToColumn("created");
            Map(u => u.IdDocumentType).ToColumn("iddocument_type");
        }
    }
}
