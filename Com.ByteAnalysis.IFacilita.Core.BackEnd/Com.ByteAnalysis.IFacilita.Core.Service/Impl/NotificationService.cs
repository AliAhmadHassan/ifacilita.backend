using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class NotificationService : INotificationService
    {
        Repository.INotificationRepository repository;

        public NotificationService(INotificationRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<Notification> FindAll()
        {
            return repository.FindAll();
        }

        public Notification FindById(int id)
        {
            return repository.FindById(id);
        }

        public Notification Insert(Notification entity)
        {
            return repository.Insert(entity);
        }

        public Notification Update(Notification entity)
        {
            return repository.Update(entity);
        }
    }
}
