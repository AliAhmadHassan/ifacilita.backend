using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class UserNaturgyService : IUserNaturgyService
    {
        Repository.IUserNaturgyRepository repository;

        public UserNaturgyService(IUserNaturgyRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }


        public IEnumerable<UserNaturgy> FindAll()
        {
            return repository.FindAll();
        }

        public UserNaturgy findByIdUser(int idUser)
        {
            return repository.findByIdUser(idUser);
        }

        public UserNaturgy FindById(int id)
        {
            return repository.FindById(id);
        }

        public IEnumerable<UserNaturgy> FindByTreatedRobot(bool treatedRobot)
        {
            return repository.FindByIdTreatedRobot(treatedRobot);
        }

        public UserNaturgy Insert(UserNaturgy entity)
        {
            return repository.Insert(entity);
        }

        public UserNaturgy Update(UserNaturgy entity)
        {
            return repository.Update(entity);
        }
    }
}
