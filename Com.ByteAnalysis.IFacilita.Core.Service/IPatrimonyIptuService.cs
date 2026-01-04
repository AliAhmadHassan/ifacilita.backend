using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service
{
    public interface IPatrimonyIptuService: ICrudService<Entity.PatrimonyIptu, int>
    {
        Entity.PatrimonyIptu FindByPatrimonyMunicipalRegistration(String PatrimonyMunicipalRegistration);
    }
}
