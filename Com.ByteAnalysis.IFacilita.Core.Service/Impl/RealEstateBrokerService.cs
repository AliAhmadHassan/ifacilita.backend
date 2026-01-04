using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class RealEstateBrokerService : IRealEstateBrokerService
    {
        Repository.IRealEstateBrokerRepository repository;

        public RealEstateBrokerService(IRealEstateBrokerRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<RealEstateBroker> FindAll()
        {
            return repository.FindAll();
        }

        public RealEstateBroker FindById(int id)
        {
            return repository.FindById(id);
        }

        public RealEstateBroker Insert(RealEstateBroker entity)
        {
            return repository.Insert(entity);
        }

        public RealEstateBroker Update(RealEstateBroker entity)
        {
            return repository.Update(entity);
        }
    }
}
