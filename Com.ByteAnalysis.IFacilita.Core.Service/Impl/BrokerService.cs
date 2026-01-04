using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class BrokerService : IBrokerService
    {
        Repository.IBrokerRepository repository;

        public BrokerService(IBrokerRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<Broker> FindAll()
        {
            return repository.FindAll();
        }

        public Broker FindById(int id)
        {
            return repository.FindById(id);
        }

        public Broker Insert(Broker entity)
        {
            return repository.Insert(entity);
        }

        public Broker Update(Broker entity)
        {
            return repository.Update(entity);
        }
    }
}
