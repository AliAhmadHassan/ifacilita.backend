using Com.ByteAnalysis.IFacilita.Core.Entity;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class TransactionFlowMap : EntityMap<TransactionFlow>
    {
        internal TransactionFlowMap()
        {
            Map(u => u.IdplatformSubWorkflow).ToColumn("idplatform_sub_workflow");
            Map(u => u.Idtransaction).ToColumn("idtransaction");
            Map(u => u.IdplatformWorkflow).ToColumn("idplatform_workflow");
            Map(u => u.Status).ToColumn("status");
            Map(u => u.StatusChanged).ToColumn("status_changed");
        }
    }
}
