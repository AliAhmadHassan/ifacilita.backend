using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class HistoricMap : EntityMap<Entity.Historic>
    {
        internal HistoricMap()
        {
            Map(u => u.Id).ToColumn("historic.id");
            Map(u => u.Description).ToColumn("historic.description");
            Map(u => u.Created).ToColumn("historic.created");
            Map(u => u.IdTransaction).ToColumn("historic.idtransaction");
            Map(u => u.IdUser).ToColumn("historic.iduser");
            Map(u => u.Topic).ToColumn("historic.topic");
        }
    }
}
