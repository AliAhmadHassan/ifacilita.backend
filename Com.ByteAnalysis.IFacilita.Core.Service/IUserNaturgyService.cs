using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service
{
    public interface IUserNaturgyService : ICrudService<Entity.UserNaturgy, int>
    {

        IEnumerable<Entity.UserNaturgy> FindByTreatedRobot(bool treatedRobot);

        UserNaturgy findByIdUser(int idUser);
    }
}
