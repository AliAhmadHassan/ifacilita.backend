using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class RealEstateService : IRealEstateService
    {
        Repository.IRealEstateRepository repository;

        public RealEstateService(IRealEstateRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<RealEstate> FindAll()
        {
            return repository.FindAll();
        }

        public RealEstate FindById(int id)
        {
            return repository.FindById(id);
        }

        public RealEstate Insert(RealEstate entity)
        {
            return repository.Insert(entity);
        }

        public RealEstate Update(RealEstate entity)
        {
            return repository.Update(entity);
        }
    }
}
