using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class PatrimonyTransmitterTypeService : IPatrimonyTransmitterTypeService
    {
        Repository.IPatrimonyTransmitterTypeRepository repository;

        public PatrimonyTransmitterTypeService(IPatrimonyTransmitterTypeRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<PatrimonyTransmitterType> FindAll()
        {
            return repository.FindAll();
        }

        public PatrimonyTransmitterType FindById(int id)
        {
            return repository.FindById(id);
        }

        public PatrimonyTransmitterType Insert(PatrimonyTransmitterType entity)
        {
            return repository.Insert(entity);
        }

        public PatrimonyTransmitterType Update(PatrimonyTransmitterType entity)
        {
            return repository.Update(entity);
        }
    }
}
