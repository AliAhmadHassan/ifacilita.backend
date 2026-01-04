using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Text;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class TransactionHiredMap : EntityMap<TransactionHired>
    {
        internal TransactionHiredMap()
        {
            Map(u => u.Id).ToColumn("id");
            Map(u => u.IdTransaction).ToColumn("idtransaction");
            Map(u => u.IdPlatformWorkflow).ToColumn("idplatform_workflow");
            Map(u => u.Completed).ToColumn("completed");
        }
    }
}
