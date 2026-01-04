using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface IPatrimonyIptuRepository: ICrudRepository<Entity.PatrimonyIptu, int>
    {
        Entity.PatrimonyIptu FindByPatrimonyMunicipalRegistration(String PatrimonyMunicipalRegistration);
    }
}
