using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class TransactionPaymentFormMap: EntityMap<Entity.TransactionPaymentForm>
    {
        internal TransactionPaymentFormMap()
        {
            Map(u => u.Id).ToColumn("id");
            Map(u => u.IdTransaction).ToColumn("idtransaction");
            Map(u => u.Plain).ToColumn("plan");
            Map(u => u.Value).ToColumn("value");
        }
    }
}
