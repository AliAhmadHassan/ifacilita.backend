using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class AddressService : IAddressService
    {
        Repository.IAddressRepository repository;

        public AddressService(IAddressRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<Address> FindAll()
        {
            return repository.FindAll();
        }

        public Address FindById(int id)
        {
            return repository.FindById(id);
        }

        public Address Insert(Address entity)
        {
            return repository.Insert(entity);
        }

        public Address Update(Address entity)
        {
            return repository.Update(entity);
        }
    }
}
