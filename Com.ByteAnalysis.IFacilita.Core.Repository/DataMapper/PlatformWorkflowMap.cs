using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class PlatformWorkflowMap: EntityMap<PlatformWorkflow>
    {
        internal PlatformWorkflowMap()
        {
            Map(u => u.Id).ToColumn("platform_workflow.id");
            Map(u => u.Name).ToColumn("platform_workflow.name");
            Map(u => u.Description).ToColumn("platform_workflow.description");
        }
    }
}
