using AutoMapper;
using Com.ByteAnalysis.IFacilita.DocuSign.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.DocuSign.Application.Models;
using Com.ByteAnalysis.IFacilita.DocuSign.Client.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client
{
    public class DocuSignClient : IDocuSignClient
    {
        private readonly IDocuSignApi<UserOutput> _docuSignApiUser;
        private readonly IDocuSignApi<EnvelopeOutput> _docuSignApiEnvelope;
        private readonly IDocuSignApi<EnvelopeGetOutput> _docuSignApiEnvelopeGet;
        private readonly IDocuSignApi<UserResponseOutput> _docuSignApiUserGet;

        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;

        public DocuSignClient(
            IConfiguration configuration,
            IDocuSignApi<UserOutput> docuSignApiUser,
            IDocuSignApi<EnvelopeOutput> docuSignApiEnvelope,
            IDocuSignApi<EnvelopeGetOutput> docuSignApiEnvelopeGet,
            IDocuSignApi<UserResponseOutput> docuSignApiUserGet
        )
        {
            _docuSignApiUser = docuSignApiUser;
            _docuSignApiEnvelope = docuSignApiEnvelope;
            _docuSignApiEnvelopeGet = docuSignApiEnvelopeGet;
            _docuSignApiUserGet = docuSignApiUserGet;

            _configuration = configuration;

            var configurationMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EventNotification, EventNotificationDocuSignInput>().ReverseMap();
                cfg.CreateMap<EnvelopeEventDocuSignInput, EnvelopeEvent>().ReverseMap();
                cfg.CreateMap<RecipientEventDocuSignInput, RecipientEvent>().ReverseMap();
            });

            _mapper = configurationMapper.CreateMapper();
        }

        public async Task<EnvelopeOutput> CreateEnvelopeDocuSingAsync(EnvelopeInput envelope)
        {
            var bodyEnvelope = new EnvelopeDocuSign();

            bodyEnvelope.EventNotification = _mapper.Map<EventNotification>(envelope.EnvelopeDocuSign.EventNotification);

            List<Document> documents = new List<Document>();
            foreach (var doc in envelope.EnvelopeDocuSign.Documents)
            {
                documents.Add(new Document()
                {
                    DocumentBase64 = doc.DocumentBase64,
                    DocumentId = doc.DocumentId,
                    FileExtension = doc.FileExtension,
                    Name = doc.Name
                });
            }

            Recipients recipients = new Recipients();
            foreach (var rec in envelope.EnvelopeDocuSign.Recipients)
            {
                List<Signer> signers = new List<Signer>();
                foreach (var sig in rec.Signers)
                {
                    Tabs tabs = new Tabs();
                    List<Tab> dateSignedTabs = new List<Tab>();
                    List<Tab> fullNameTabs = new List<Tab>();
                    List<SignHereTab> signHereTabs = new List<SignHereTab>();

                    if (sig.Tabs != null && sig.Tabs.DateSignedTabs != null)
                        foreach (var dtS in sig.Tabs.DateSignedTabs)
                        {
                            dateSignedTabs.Add(new Tab()
                            {
                                AnchorString = dtS.AnchorString,
                                AnchorYOffset = dtS.AnchorYOffset,
                                FontSize = dtS.FontSize,
                                Name = dtS.Name,
                                RecipientId = dtS.RecipientId,
                                TabLabel = dtS.TabLabel
                            });
                        }

                    if (sig.Tabs != null && sig.Tabs.FullNameTabs != null)
                        foreach (var dtS in sig.Tabs.FullNameTabs)
                        {
                            fullNameTabs.Add(new Tab()
                            {
                                AnchorString = dtS.AnchorString,
                                AnchorYOffset = dtS.AnchorYOffset,
                                FontSize = dtS.FontSize,
                                Name = dtS.Name,
                                RecipientId = dtS.RecipientId,
                                TabLabel = dtS.TabLabel
                            });
                        }

                    if (sig.Tabs != null && sig.Tabs.SignHereTabs != null)
                        foreach (var dtS in sig.Tabs.SignHereTabs)
                        {
                            signHereTabs.Add(new SignHereTab()
                            {
                                AnchorString = dtS.AnchorString,
                                AnchorYOffset = dtS.AnchorYOffset,
                                Name = dtS.Name,
                                RecipientId = dtS.RecipientId,
                                TabLabel = dtS.TabLabel,
                                AnchorUnits = dtS.AnchorUnits,
                                AnchorXOffset = dtS.AnchorXOffset,
                                Optional = dtS.Optional,
                                ScaleValue = dtS.ScaleValue
                            });
                        }

                    tabs.DateSignedTabs = dateSignedTabs.ToArray();
                    tabs.FullNameTabs = fullNameTabs.ToArray();
                    tabs.SignHereTabs = signHereTabs.ToArray();

                    signers.Add(new Signer() { Email = sig.Email, Name = sig.Name, RecipientId = sig.RecipientId, RoutingOrder = sig.RoutingOrder, Tabs = tabs });
                }

                recipients.Signers = signers.ToArray();
            }

            bodyEnvelope.Recipients = recipients;

            bodyEnvelope.Documents = documents.ToArray();
            bodyEnvelope.EmailSubject = envelope.EnvelopeDocuSign.EmailSubject;
            bodyEnvelope.Status = envelope.EnvelopeDocuSign.Status;

            var response = await _docuSignApiEnvelope.PostAsync(bodyEnvelope, _configuration["DocuSign:UrlControllers"], "accounts", _configuration["DocuSign:AccountId"], "envelopes");
            return response;
        }

        public async Task<Application.Models.UserOutput> CreateUserDocuSingAsync(UserInput users)
        {
            var bodyPost = new UserDocuSign();
            bodyPost.NewUsers.Add(new UserResponse() { Email = users.Users.Email, UserName = users.Users.UserName });

            var response = await _docuSignApiUser.PostAsync(bodyPost, _configuration["DocuSign:UrlControllers"], "accounts", _configuration["DocuSign:AccountId"], "users");
            return response;
        }

        public async Task<EnvelopeGetOutput> GetEnvelopeDocuSignAsync(string id)
        {
            var reponse = await _docuSignApiEnvelopeGet.GetAsync(_configuration["DocuSign:UrlControllers"], "accounts", _configuration["DocuSign:AccountId"], "envelopes", id);
            return reponse;
        }

        public async Task<UserResponseOutput> GetUserDocuSignAsync(string id)
        {
            var result = await _docuSignApiUserGet.GetAsync(_configuration["DocuSign:UrlControllers"], "accounts", _configuration["DocuSign:AccountId"], "users", id);
            return result;
        }
    }
}
