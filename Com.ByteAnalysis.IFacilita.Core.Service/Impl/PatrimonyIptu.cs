using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class PatrimonyIptuService : IPatrimonyIptuService
    {
        Repository.IPatrimonyIptuRepository repository;

        public PatrimonyIptuService(Repository.IPatrimonyIptuRepository repository)
        {
            this.repository = repository;
        }
        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<PatrimonyIptu> FindAll()
        {
            return this.repository.FindAll();
        }

        public PatrimonyIptu FindById(int id)
        {
            return this.repository.FindById(id);
        }

        public PatrimonyIptu FindByPatrimonyMunicipalRegistration(string PatrimonyMunicipalRegistration)
        {
            return this.repository.FindByPatrimonyMunicipalRegistration(PatrimonyMunicipalRegistration);
        }

        public PatrimonyIptu Insert(PatrimonyIptu entity)
        {
            return this.repository.Insert(entity);
        }

        public PatrimonyIptu Update(PatrimonyIptu entity)
        {
            return this.repository.Update(entity);
        }
    }
}
