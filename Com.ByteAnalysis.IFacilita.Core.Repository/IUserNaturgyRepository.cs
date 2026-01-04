using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface IUserNaturgyRepository : ICrudRepository<UserNaturgy, int>
    {
        IEnumerable<UserNaturgy> FindByIdTreatedRobot(bool treatedRobot);

        UserNaturgy findByIdUser(int idUser);
    }
}
