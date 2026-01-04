using AutoMapper;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Exceptions;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces.ExternalServices;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Services
{
    public class RequirementService : IRequirementService
    {
        private readonly IMapper _mapper;
        private readonly IeCartorioClient _ieCartorioClient;

        public RequirementService(IMapper mapper, IeCartorioClient ieCartorioClient)
        {
            _mapper = mapper;
            _ieCartorioClient = ieCartorioClient;
        }

        public async Task<IEnumerable<RequirementResponse>> GetAsync(string cerp)
        {
            if (string.IsNullOrEmpty(cerp))
                throw new BadRequestException("O cerp é obrigatório.");

            var result = await _ieCartorioClient.ExigenciaListarExigenciaAsync(cerp);
            if (result == null)
                throw new NotFoundException("Exigência não encontrada.");

            var resultMapped = _mapper.Map<IEnumerable<RequirementResponse>>(result);

            return resultMapped;
        }

        public async Task<IEnumerable<RequirementResponse>> GetByApplicantAsync(string document)
        {
            if (string.IsNullOrEmpty(document))
                throw new BadRequestException("O documento é obrigatório.");

            var result = await _ieCartorioClient.ExigenciaPesquisarExigenciasPorRequerenteAsync(document);
            if (result == null)
                throw new NotFoundException("Exigência não encontrada.");

            var resultMapped = _mapper.Map<IEnumerable<RequirementResponse>>(result);

            return resultMapped;
        }

        public async Task PostAsync(int idRequiremente, string message)
        {

            if (idRequiremente <= 0)
                throw new BadRequestException("A id da exigência é obrigatório.");

            if (string.IsNullOrEmpty(message))
                throw new BadRequestException("A mensagem é obrigatóio.");

            _ = await _ieCartorioClient.ExigenciaResponderExigenciaAsync(idRequiremente.ToString(), message);
        }
    }
}
