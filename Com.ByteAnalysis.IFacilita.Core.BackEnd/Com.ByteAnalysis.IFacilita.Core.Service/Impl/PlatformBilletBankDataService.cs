using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class PlatformBilletBankDataService : IPlatformBilletBankDataService
    {
        Repository.IPlatformBilletBankDataRepository repository;

        public PlatformBilletBankDataService(IPlatformBilletBankDataRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<PlatformBilletBankData> FindAll()
        {
            return repository.FindAll();
        }

        public PlatformBilletBankData FindById(int id)
        {
            return repository.FindById(id);
        }

        public PlatformBilletBankData Insert(PlatformBilletBankData entity)
        {
            return repository.Insert(entity);
        }

        public PlatformBilletBankData Update(PlatformBilletBankData entity)
        {
            return repository.Update(entity);
        }
    }
}
