using AutoMapper;
using Com.ByteAnalysis.IFacilita.GuideRequest.Application.Exceptions;
using Com.ByteAnalysis.IFacilita.GuideRequest.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.GuideRequest.Application.Models;
using Com.ByteAnalysis.IFacilita.GuideRequest.Domain.Repositories;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.GuideRequest.Application.Services
{
    public class GuideRequestService : IGuideRequestService
    {
        private readonly IMapper _mapper;
        private readonly IGuideRequestRepository _repository;
        private readonly IValidator<GuideRequestInput> _validation;

        public GuideRequestService(IGuideRequestRepository repository, IMapper mapper, IValidator<GuideRequestInput> validation)
        {
            _mapper = mapper;
            _repository = repository;
            _validation = validation;
        }

        public async Task<GuideRequestInput> GetAsync(string guideId)
            => _mapper.Map<GuideRequestInput>(await _repository.GetAsync(guideId));

        public async Task<IEnumerable<GuideRequestInput>> GetAsync() 
            => _mapper.Map<IEnumerable<GuideRequestInput>>(await _repository.GetAsync());

        public async Task<IEnumerable<GuideRequestInput>> GetGuideRequestGuidePendingsAsync()
        {
            var result = await _repository.GetGuidePendingsAsync();
            var guides = _mapper.Map<List<GuideRequestInput>>(result);
            return guides;

        }

        public async Task<IEnumerable<GuideRequestInput>> GetGuideRequestPendingsAsync()
        {
            var result = await _repository.GetPendingsAsync();
            var guides = _mapper.Map<List<GuideRequestInput>>(result);
            return guides;
        }

        public async Task<GuideRequestInput> PostGuideRequestAsync(GuideRequestInput guide)
        {
            GuideRequestInput result = null;

            if (string.IsNullOrEmpty(guide.Id)) //Insert
            {
                var validationResult = _validation.Validate(guide);
                if (!validationResult.IsValid)
                {
                    var messageValidate = string.Join(',', validationResult.Errors.Select(x => x.ErrorMessage));
                    throw new GuideRequestInvalidException(messageValidate);
                }

                result = _mapper.Map<GuideRequestInput>(await _repository.CreateAsync(_mapper.Map<GuideRequest.Domain.Entities.GuideRequest>(guide)));
            }
            else //Update
            {
                var guideResult = await _repository.GetAsync(guide.Id);

                if (guideResult == null)
                    throw new NotFoundException("Guia de requisição não encontrada");

                await _repository.UpdateAsync(guide.Id, _mapper.Map<GuideRequest.Domain.Entities.GuideRequest>(guide));

                if (!string.IsNullOrEmpty(guideResult.UrlCallback))
                {
                    using var clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                    using var client = new HttpClient(clientHandler);
                    var response = await client.GetAsync(string.Format(guideResult.UrlCallback, guideResult.Id));

                    guideResult.UrlCallbackResponse += $"O servidor {guideResult.UrlCallback} de callback retornou o status: {response.StatusCode}, mensagem: { await response.Content.ReadAsStringAsync()}";
                    await _repository.UpdateAsync(guideResult.Id, _mapper.Map<GuideRequest.Domain.Entities.GuideRequest>(guide));
                }

                result = guide;
            }

            return result;
        }

        public async Task<bool> PutStatusGuidRequestAsync(string guideId, int status)
        {
            var guide = await _repository.GetAsync(guideId);

            if (guide == null)
                throw new NotFoundException("Guia de requisição não encontrada");

            guide.Status = status;

            await _repository.UpdateAsync(guideId, guide);

            return true;
        }
    }
}
