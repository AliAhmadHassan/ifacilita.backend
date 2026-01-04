using Com.ByteAnalysis.IFacilita.Naturgy.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Naturgy.Service.Impl
{
    public class RegisterClientService : IRegisterClientService
    {
        Repository.IRegisterClientRepository repository;

        public RegisterClientService(Repository.IRegisterClientRepository repository)
        {
            this.repository = repository;
        }

        public RegisterClient CreateOrUpdate(RegisterClient registerClient)
        {
            registerClient.StatusModified = DateTime.Now;
            registerClient = this.repository.CreateOrUpdate(registerClient);
            return registerClient;
        }

        public List<RegisterClient> Get() => this.repository.Get();


        public RegisterClient Get(string id) => this.repository.Get(id);

        public void Remove(RegisterClient registerClient) => this.repository.Remove(registerClient);

        public void Remove(string id) => this.repository.Remove(id);
    }
}
