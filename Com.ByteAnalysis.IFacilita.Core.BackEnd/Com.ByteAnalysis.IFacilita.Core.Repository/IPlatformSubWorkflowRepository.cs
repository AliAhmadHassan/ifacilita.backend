using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface IPlatformSubWorkflowRepository: ICrudRepository<PlatformSubWorkflow, int>
    {
		List<PlatformSubWorkflow> FindByIdPlatformWorkflow (Int32 IdPlatformWorkflow);
    }
}
