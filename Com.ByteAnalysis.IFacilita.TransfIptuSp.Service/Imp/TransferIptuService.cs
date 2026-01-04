using Com.ByteAnalysis.IFacilita.TransfIptuSp.Model;
using Com.ByteAnalysis.IFacilita.TransfIptuSp.Repository;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Com.ByteAnalysis.IFacilita.TransfIptuSp.Service.Imp
{
    public class TransferIptuService: ITransferIptuService
    {
        private readonly ITransferIptuRepository _repository;

        public TransferIptuService(ITransferIptuRepository repository)
        {
            _repository = repository;
        }

        public RequisitionModel Create(RequisitionModel entry) => _repository.Create(entry);

        public List<RequisitionModel> Get() => _repository.Get();

        public RequisitionModel Get(string id) => _repository.Get(id);

        public IEnumerable<RequisitionModel> GetPendings() => _repository.GetPendings();

        public void Remove(RequisitionModel entry) => _repository.Remove(entry);

        public void Remove(string id) => _repository.Remove(id);

        public void Update(string id, RequisitionModel entry)
        {
            try
            {
                _repository.Update(id, entry);

                if (!string.IsNullOrEmpty(entry.UrlCallback))
                {
                    try
                    {
                        using var clientHandler = new System.Net.Http.HttpClientHandler();
                        clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                        using var client = new HttpClient(clientHandler);
                        var response = client.GetAsync(string.Format(entry.UrlCallback,entry.Id)).Result;

                        entry.UrlCallbackResponse += $"O servidor {entry.UrlCallback} de callback retornou o status: {response.StatusCode.ToString()}, mensagem: {response.Content.ReadAsStringAsync().Result}";
                    }
                    catch (Exception ex)
                    {
                        entry.UrlCallbackResponse += ex.Message;
                    }
                    _repository.Update(id, entry);
                }
            }
            catch { }
        }
    }
}
