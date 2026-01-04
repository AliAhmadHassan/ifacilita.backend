using Com.ByteAnalysis.IFacilita.SearchProtest.Model;
using Com.ByteAnalysis.IFacilita.SearchProtest.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.SearchProtest.Service.Impl
{
    public class SearchProtestService : ISearchProtestService
    {
        private readonly ISearchProtestRepository _repository;

        public SearchProtestService(ISearchProtestRepository repository)
        {
            _repository = repository;
        }

        public RequestSearchProtestModel Create(RequestSearchProtestModel entry) => _repository.Create(entry);

        public List<RequestSearchProtestModel> Get() => _repository.Get();

        public RequestSearchProtestModel Get(string id) => _repository.Get(id);

        public IEnumerable<RequestSearchProtestModel> GetPendings() => _repository.GetPendings();

        public void Remove(RequestSearchProtestModel entry) => _repository.Remove(entry);

        public void Remove(string id) => _repository.Remove(id);

        public void Update(string id, RequestSearchProtestModel entry)
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
                        var content = new StringContent(JsonConvert.SerializeObject(new { entry.Id, orderId = "", certiticateType = "SearchProtest" }), Encoding.UTF8, "application/json");
                        var response = client.PostAsync(entry.UrlCallback, content).Result;

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
