using AutoMapper;
using Com.ByteAnalysis.IFacilita.DocuSign.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.DocuSign.Application.Models;
using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Entities;
using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Services
{
    public class EnvelopeDocuSignService : ServiceBase<EnvelopeDocuSign>, IEnvelopeDocuSignService
    {
        private readonly IDocuSignClient _docuSignClient;
        private readonly IRepositoryBase<EnvelopeDocuSign> _repository;
        private readonly IEnvelopeDocuSignRepository _envelopeDocuSignRepository;

        public EnvelopeDocuSignService(IEnvelopeDocuSignRepository envelopeDocuSignRepository, IRepositoryBase<EnvelopeDocuSign> repository, IDocuSignClient docuSignClient) : base(repository)
        {
            repository.SetNameCollection("envelopeDocuSign");
            _docuSignClient = docuSignClient;
            _repository = repository;
            _envelopeDocuSignRepository = envelopeDocuSignRepository;
        }

        public async Task<EnvelopeDocuSign> GetByEnvelopeIdAsync(string envelopeId) => await _envelopeDocuSignRepository.GetByEnvelopeIdAsync(envelopeId);

        public async Task<EnvelopeGetOutput> GetDocuSignAsync(string id)
        {
            var result = await _docuSignClient.GetEnvelopeDocuSignAsync(id);
            return result;
        }

        public async Task<EnvelopeOutput> PostDocuSign(EnvelopeInput envelope)
        {
            var result = await _docuSignClient.CreateEnvelopeDocuSingAsync(envelope);

            List<string> documents = new List<string>();
            envelope.EnvelopeDocuSign.Documents.ToList().ForEach(x => documents.Add(x.Name));

            List<SignsEnvelope> signs = new List<SignsEnvelope>();
            envelope.EnvelopeDocuSign.Recipients.ToList().ForEach(x => x.Signers.ToList().ForEach(y => signs.Add(new SignsEnvelope() { Email = y.Email, Name = y.Name })));

            var created = await _repository.CreateAsync(
                new EnvelopeDocuSign()
                {
                    EmailSubject = envelope.EnvelopeDocuSign.EmailSubject,
                    EnvelopeId = result.EnvelopeId,
                    Status = result.Status,
                    StatusDateTime = result.StatusDateTime,
                    Uri = result.Uri,
                    Documents = documents,
                    Signs = signs
                });

            return result;
        }
    }
}
