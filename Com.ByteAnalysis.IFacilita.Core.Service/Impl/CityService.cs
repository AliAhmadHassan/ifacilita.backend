using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class CityService : ICityService
    {
        Repository.ICityRepository repository;

        public CityService(ICityRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<City> FindAll()
        {
            return repository.FindAll();
        }

        public City FindById(int id)
        {
            return repository.FindById(id);
        }

        public City Insert(City entity)
        {
            return repository.Insert(entity);
        }

        public City Update(City entity)
        {
            return repository.Update(entity);
        }
    }
}
