using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class PatrimonyService : IPatrimonyService
    {
        Repository.IPatrimonyRepository repository;

        public PatrimonyService(IPatrimonyRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<Patrimony> FindAll()
        {
            return repository.FindAll();
        }

        public Patrimony FindById(int id)
        {
            return repository.FindById(id);
        }

        public Patrimony Insert(Patrimony entity)
        {
            return repository.Insert(entity);
        }

        public Patrimony Update(Patrimony entity)
        {
            return repository.Update(entity);
        }
    }
}
