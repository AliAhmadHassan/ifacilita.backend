using Com.ByteAnalysis.IFacilita.Core.Entity;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class UserNaturgyMap : EntityMap<UserNaturgy>
    {

        internal UserNaturgyMap(){
            Map(u => u.Id).ToColumn("user_naturgy_data.id");
            Map(u => u.ChangeTitulary).ToColumn("user_naturgy_data.change_titulary");
            Map(u => u.PasswordNaturgy).ToColumn("user_naturgy_data.password_naturgy");
            Map(u => u.TreatedRobot).ToColumn("user_naturgy_data.treated_robot");
            Map(u => u.IfacilitaTreats).ToColumn("user_naturgy_data.ifacilita_treats");
            Map(u => u.GuidRobot).ToColumn("user_naturgy_data.guid_robot");
            Map(u => u.IdUser).ToColumn("user_naturgy_data.id_user");
        }
    }
}
