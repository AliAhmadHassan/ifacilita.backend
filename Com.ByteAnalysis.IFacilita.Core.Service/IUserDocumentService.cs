using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service
{
    public interface IUserDocumentService: ICrudService<Entity.UserDocument, int>
    {
        Entity.UserDocument FindByIdUser(Int32 IdUser);
    }
}
