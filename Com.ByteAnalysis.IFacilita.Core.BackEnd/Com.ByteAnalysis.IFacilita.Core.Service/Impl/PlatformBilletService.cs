using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class PlatformBilletService : IPlatformBilletService
    {
        Repository.IPlatformBilletRepository repository;

        public PlatformBilletService(IPlatformBilletRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<PlatformBillet> FindAll()
        {
            return repository.FindAll();
        }

        public PlatformBillet FindById(int id)
        {
            return repository.FindById(id);
        }

        public PlatformBillet Insert(PlatformBillet entity)
        {
            return repository.Insert(entity);
        }

        public PlatformBillet Update(PlatformBillet entity)
        {
            return repository.Update(entity);
        }
    }
}
