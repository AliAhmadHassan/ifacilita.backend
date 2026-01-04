using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface IPatrimonyDocumentRepository: ICrudRepository<PatrimonyDocument, int>
    {
		List<PatrimonyDocument> FindByPatrimonyMunicipalRegistration (String PatrimonyMunicipalRegistration);
    }
}
