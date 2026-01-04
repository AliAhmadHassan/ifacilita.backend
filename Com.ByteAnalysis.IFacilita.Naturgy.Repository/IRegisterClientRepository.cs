using Com.ByteAnalysis.IFacilita.Naturgy.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Naturgy.Repository
{
    public interface IRegisterClientRepository
    {
        List<RegisterClient> Get();

        RegisterClient Get(string id);

        RegisterClient CreateOrUpdate(RegisterClient registerClient);

        void Remove(RegisterClient registerClient);

        void Remove(string id);
    }
}
