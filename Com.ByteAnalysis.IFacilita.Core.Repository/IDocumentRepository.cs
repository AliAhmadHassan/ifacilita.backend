using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface IDocumentRepository: ICrudRepository<Document, int>
    {
		List<Document> FindByIdDocumentType (Int32 IdDocumentType);
    }
}
