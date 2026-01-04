using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface ISendInviteRepository: ICrudRepository<SendInvite, int>
    {
		List<SendInvite> FindByRealEstateRegisteredNumber (Int64 RealEstateRegisteredNumber);
		List<SendInvite> FindByIdUser (Int32 IdUser);
    }
}
