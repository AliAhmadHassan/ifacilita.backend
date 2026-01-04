using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class PlatformWorkflowService : IPlatformWorkflowService
    {
        Repository.IPlatformWorkflowRepository repository;

        public PlatformWorkflowService(IPlatformWorkflowRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<PlatformWorkflow> FindAll()
        {
            return repository.FindAll();
        }

        public PlatformWorkflow FindById(int id)
        {
            return repository.FindById(id);
        }

        public PlatformWorkflow Insert(PlatformWorkflow entity)
        {
            return repository.Insert(entity);
        }

        public PlatformWorkflow Update(PlatformWorkflow entity)
        {
            return repository.Update(entity);
        }
    }
}
