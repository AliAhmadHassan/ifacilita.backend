using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface IUserDocumentRepository: ICrudRepository<UserDocument, int>
    {
		List<UserDocument> FindByIdUser (Int32 IdUser);
    }
}
