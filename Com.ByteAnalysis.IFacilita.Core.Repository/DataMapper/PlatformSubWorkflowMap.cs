using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class PlatformSubWorkflowMap: EntityMap<PlatformSubWorkflow>
    {
        internal PlatformSubWorkflowMap()
        {
            Map(u => u.Id).ToColumn("platform_sub_workflow.id");
            Map(u => u.Name).ToColumn("platform_sub_workflow.name");
            Map(u => u.Description).ToColumn("platform_sub_workflow.description");
            Map(u => u.IdPlatformWorkflow).ToColumn("platform_sub_workflow.idplatform_workflow");
        }
    }
}
