using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class HistoricService : IHistoricService
    {
        Repository.IHistoricRepository repository;

        public HistoricService(Repository.IHistoricRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<Historic> FindAll()
        {
            return this.repository.FindAll();
        }

        public Historic FindById(int id)
        {
            return this.repository.FindById(id);
        }

        public IEnumerable<Historic> FindByIdTransaction(int idTransaction)
        {
            return this.repository.FindByIdTransaction(idTransaction);
        }

        public Historic Insert(Historic entity)
        {
            return this.repository.Insert(entity);
        }

        public Historic Update(Historic entity)
        {
            return this.repository.Update(entity);
        }
    }
}
