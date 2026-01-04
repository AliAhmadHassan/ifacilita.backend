using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class PlatformSubWorkflowService : IPlatformSubWorkflowService
    {
        Repository.IPlatformSubWorkflowRepository repository;

        public PlatformSubWorkflowService(IPlatformSubWorkflowRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<PlatformSubWorkflow> FindAll()
        {
            return repository.FindAll();
        }

        public PlatformSubWorkflow FindById(int id)
        {
            return repository.FindById(id);
        }

        public PlatformSubWorkflow Insert(PlatformSubWorkflow entity)
        {
            return repository.Insert(entity);
        }

        public PlatformSubWorkflow Update(PlatformSubWorkflow entity)
        {
            return repository.Update(entity);
        }
    }
}
