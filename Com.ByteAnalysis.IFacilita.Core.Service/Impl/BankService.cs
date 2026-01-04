using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class BankService : IBankService
    {
        Repository.IBankRepository repository;

        public BankService(IBankRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<Bank> FindAll()
        {
            return repository.FindAll();
        }

        public Bank FindById(int id)
        {
            return repository.FindById(id);
        }

        public Bank Insert(Bank entity)
        {
            return repository.Insert(entity);
        }

        public Bank Update(Bank entity)
        {
            return repository.Update(entity);
        }
    }
}
