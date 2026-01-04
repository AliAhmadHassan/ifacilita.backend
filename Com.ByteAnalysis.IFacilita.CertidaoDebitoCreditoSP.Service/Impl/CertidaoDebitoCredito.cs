using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.CertidaoDebitoCreditoSP.Service.Impl
{
    public class CertidaoDebitoCredito : ICertidaoDebitoCreditoSPService
    {
        Repository.ICertidaoDebitoCreditoSPRepository repository;

        public CertidaoDebitoCredito(Repository.ICertidaoDebitoCreditoSPRepository repository)
        {
            this.repository = repository;
        }

        public List<Model.CertidaoDebitoCreditoSP> Get() => this.repository.Get();

        public Model.CertidaoDebitoCreditoSP Get(string id) => this.repository.Get(id);

        public void Remove(string id) => this.repository.Remove(id);

        public Model.CertidaoDebitoCreditoSP CreateOrUpdate(Model.CertidaoDebitoCreditoSP certidao) => this.repository.CreateOrUpdate(certidao);

        public Model.CertidaoDebitoCreditoSP GetUnprocessed() => repository.GetUnprocessed();

    }
}
